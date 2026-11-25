"""Optimize PNG and JPEG images in place without changing names or formats.

Requirements (Python 3.10+):
    pip install pillow numpy pyoxipng imagequant

Usage:
    python scripts/optimize_images.py [--apply] [--report FILE] [PATH ...]

PATH is an image file or a folder (searched recursively). Without PATH the
script takes every PNG/JPEG under docs/. Without --apply it is a dry run:
it prints what would change and writes nothing.

Rules:
    PNG : lossless recompression (oxipng). Files over 300 KB also try a
          256-colour palette (libimagequant); the palette is kept only if it
          is at least 40 % smaller than the lossless result and PSNR stays
          at or above 38 dB.
    JPEG: re-encode at quality 85 (progressive, EXIF orientation applied,
          metadata dropped, ICC profile kept); kept only if PSNR >= 38 dB.
    A file is rewritten only when it gets at least 10 % smaller.
    No image is resized.

Running the script again on already optimized files changes nothing.
"""
import argparse
import io
import sys
from pathlib import Path

import imagequant
import numpy as np
import oxipng
from PIL import Image, ImageOps

MIN_GAIN = 0.10
QUANT_MIN_SIZE = 300 * 1024
QUANT_EXTRA_GAIN = 0.40
MIN_PSNR = 38.0
JPEG_QUALITY = 85
EXTENSIONS = {".png", ".jpg", ".jpeg"}


def psnr(a: Image.Image, b: Image.Image) -> float:
    mode = "RGBA" if "A" in a.getbands() or "A" in b.getbands() else "RGB"
    x = np.asarray(a.convert(mode), dtype=np.float64)
    y = np.asarray(b.convert(mode), dtype=np.float64)
    if mode == "RGBA":
        # Ignore the colour of fully transparent pixels.
        mask = (x[..., 3] > 0) | (y[..., 3] > 0)
        diff = (x - y)[mask]
    else:
        diff = x - y
    mse = float(np.mean(diff ** 2)) if diff.size else 0.0
    return 99.0 if mse == 0 else 10 * np.log10(255 ** 2 / mse)


def lossless_png(data: bytes) -> bytes:
    return oxipng.optimize_from_memory(data, level=3, strip=oxipng.StripChunks.safe())


def optimize(data: bytes):
    """Return (format, method, psnr, best_bytes) for one image."""
    img = Image.open(io.BytesIO(data))
    fmt = img.format
    img.load()
    best, how, q = data, "keep", 99.0

    if fmt == "PNG":
        ll = lossless_png(data)
        if len(ll) < len(best):
            best, how = ll, "lossless"
        if len(data) > QUANT_MIN_SIZE:
            src = img.convert("RGBA")
            pal = imagequant.quantize_pil_image(src, dithering_level=1.0, max_colors=256)
            buf = io.BytesIO()
            pal.save(buf, "PNG")
            qd = lossless_png(buf.getvalue())
            p = psnr(src, Image.open(io.BytesIO(qd)))
            if p >= MIN_PSNR and len(qd) <= len(best) * (1 - QUANT_EXTRA_GAIN):
                best, how, q = qd, "palette", p
    elif fmt == "JPEG":
        src = ImageOps.exif_transpose(img)
        if src.mode not in ("RGB", "L"):
            src = src.convert("RGB")
        buf = io.BytesIO()
        kw = {"quality": JPEG_QUALITY, "optimize": True, "progressive": True}
        if img.info.get("icc_profile"):
            kw["icc_profile"] = img.info["icc_profile"]
        src.save(buf, "JPEG", **kw)
        p = psnr(src, Image.open(io.BytesIO(buf.getvalue())))
        if p >= MIN_PSNR and len(buf.getvalue()) < len(best):
            best, how, q = buf.getvalue(), f"jpeg{JPEG_QUALITY}", p

    if len(best) > len(data) * (1 - MIN_GAIN):
        best, how, q = data, "keep", 99.0
    return fmt, how, q, best


def collect(paths: list[str]) -> list[Path]:
    files: list[Path] = []
    for p in map(Path, paths or ["docs"]):
        if p.is_dir():
            files += sorted(f for f in p.rglob("*") if f.suffix.lower() in EXTENSIONS)
        elif p.is_file():
            files.append(p)
        else:
            sys.exit(f"Not found: {p}")
    return files


def main() -> None:
    ap = argparse.ArgumentParser(description=__doc__.split("\n", 1)[0])
    ap.add_argument("paths", nargs="*", help="image files or folders (default: docs)")
    ap.add_argument("--apply", action="store_true", help="rewrite files (default: dry run)")
    ap.add_argument("--report", help="write a TSV report to this file")
    args = ap.parse_args()

    rows = []
    total_before = total_after = changed = 0
    for f in collect(args.paths):
        data = f.read_bytes()
        try:
            fmt, how, q, best = optimize(data)
        except Exception as e:  # noqa: BLE001
            print(f"ERROR  {f}: {e}", file=sys.stderr)
            continue
        total_before += len(data)
        total_after += len(best)
        rows.append(f"{f.as_posix()}\t{fmt}\t{len(data)}\t{len(best)}\t{how}\t{q:.1f}")
        if how != "keep":
            changed += 1
            print(f"{how:9} {len(data) / 1024:8.0f} KB -> {len(best) / 1024:6.0f} KB"
                  f"  PSNR {q:4.1f}  {f.as_posix()}")
            if args.apply:
                f.write_bytes(best)

    if args.report:
        Path(args.report).write_text("\n".join(rows) + "\n", encoding="utf-8")
    mode = "applied" if args.apply else "dry run, nothing written"
    print(f"{len(rows)} files, {changed} smaller: {total_before / 2**20:.1f} MB -> "
          f"{total_after / 2**20:.1f} MB ({mode})")


if __name__ == "__main__":
    main()
