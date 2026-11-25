<#
.SYNOPSIS
    Converts animated GIFs in docs/ to looping MP4 videos and removes empty bars.

.DESCRIPTION
    For each GIF the script:
      1. Crops transparent borders. MP4 has no alpha channel, so transparent
         areas would turn into solid (usually white) bars.
      2. Checks for solid black or white bars baked into the frame. They are
         reported as a warning; pass -CropSolidBars to crop them too.
      3. Encodes H.264 MP4 (yuv420p, faststart, no audio) next to the GIF.
      4. With -UpdateLinks, replaces ![alt](Name.gif) in the Markdown pages of
         the same folder with an autoplaying, looping, muted <video> tag.

    The script does not delete GIFs. Remove a GIF with `git rm` only after no
    page links to it.

.PARAMETER Path
    One or more GIF files.

.PARAMETER FfmpegDir
    Folder with ffmpeg and ffprobe. Default: $env:FFMPEG_DIR, then PATH.

.PARAMETER Crf
    x264 quality (lower is better and larger). Default 23.

.PARAMETER CropSolidBars
    Also crop solid black/white bars that are opaque in the GIF.

.PARAMETER UpdateLinks
    Rewrite image links to the GIF in .md files of the GIF's folder.

.EXAMPLE
    pwsh scripts/Convert-GifToMp4.ps1 docs/30_Polety/Series_1.gif -UpdateLinks
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory, ValueFromPipeline, Position = 0)]
    [string[]]$Path,
    [string]$FfmpegDir = $env:FFMPEG_DIR,
    [int]$Crf = 23,
    [switch]$CropSolidBars,
    [switch]$UpdateLinks
)

begin {
    $ErrorActionPreference = 'Stop'

    function Resolve-Tool([string]$name) {
        if ($FfmpegDir) {
            $candidate = Get-ChildItem -LiteralPath $FfmpegDir -Filter "$name*" -File |
                Where-Object { $_.BaseName -eq $name } | Select-Object -First 1
            if ($candidate) { return $candidate.FullName }
        }
        $cmd = Get-Command $name -ErrorAction SilentlyContinue
        if ($cmd) { return $cmd.Source }
        throw "$name not found. Pass -FfmpegDir or set FFMPEG_DIR."
    }

    $ffmpeg = Resolve-Tool 'ffmpeg'
    $ffprobe = Resolve-Tool 'ffprobe'

    # Union of the non-empty area over all frames, as "w:h:x:y", or $null.
    function Get-CropArea([string]$file, [string]$filter) {
        $log = & $ffmpeg -hide_banner -i $file -vf "$filter,cropdetect=limit=16:round=2:reset=0" -f null - 2>&1 |
            Out-String
        $found = [regex]::Matches($log, 'crop=(\d+:\d+:\d+:\d+)')
        if ($found.Count -eq 0) { return $null }
        return $found[$found.Count - 1].Groups[1].Value
    }

    function Get-Size([string]$file) {
        $wh = (& $ffprobe -v error -select_streams v:0 -show_entries stream=width,height -of csv=s=x:p=0 $file).Trim()
        $w, $h = $wh -split 'x'
        return [pscustomobject]@{ Width = [int]$w; Height = [int]$h }
    }

    # Intersection of two crop areas ("w:h:x:y").
    function Join-Crop([string]$a, [string]$b) {
        if (-not $a) { return $b }
        if (-not $b) { return $a }
        $p = $a -split ':' | ForEach-Object { [int]$_ }
        $q = $b -split ':' | ForEach-Object { [int]$_ }
        $x = [math]::Max($p[2], $q[2]); $y = [math]::Max($p[3], $q[3])
        $r = [math]::Min($p[2] + $p[0], $q[2] + $q[0]); $btm = [math]::Min($p[3] + $p[1], $q[3] + $q[1])
        return '{0}:{1}:{2}:{3}' -f ($r - $x), ($btm - $y), $x, $y
    }
}

process {
    foreach ($item in $Path) {
        $gif = (Resolve-Path -LiteralPath $item).Path
        if ([IO.Path]::GetExtension($gif) -ne '.gif') { Write-Warning "Skip (not a GIF): $item"; continue }
        $dir = Split-Path $gif -Parent
        $name = [IO.Path]::GetFileNameWithoutExtension($gif)
        $mp4 = Join-Path $dir "$name.mp4"
        $size = Get-Size $gif
        $full = '{0}:{1}:0:0' -f $size.Width, $size.Height

        # 1. Transparent borders.
        $pixFmt = (& $ffprobe -v error -select_streams v:0 -show_entries stream=pix_fmt -of csv=p=0 $gif).Trim()
        $crop = $null
        if ($pixFmt -match '^(bgra|rgba|argb|abgr|yuva|ya)') { $crop = Get-CropArea $gif 'alphaextract' }

        # 2. Solid black / white bars (composited over black).
        $base = if ($crop) { "crop=$crop" } else { 'null' }
        $black = Get-CropArea $gif "$base,format=rgb24"
        $white = Get-CropArea $gif "$base,format=rgb24,negate"
        $solid = Join-Crop $black $white
        $afterAlpha = if ($crop) { $crop -replace '^(\d+:\d+):\d+:\d+$', '$1:0:0' } else { $full }
        if ($solid -and $solid -ne $afterAlpha) {
            if ($CropSolidBars) {
                $s = $solid -split ':' | ForEach-Object { [int]$_ }
                if ($crop) {
                    $c = $crop -split ':' | ForEach-Object { [int]$_ }
                    $crop = '{0}:{1}:{2}:{3}' -f $s[0], $s[1], ($c[2] + $s[2]), ($c[3] + $s[3])
                } else { $crop = $solid }
            } else {
                Write-Warning "${name}: possible solid bars, content area $solid (after alpha crop). Check a frame; rerun with -CropSolidBars if they are bars."
            }
        }

        # 3. Encode.
        $vf = (@($(if ($crop) { "crop=$crop" }), 'scale=trunc(iw/2)*2:trunc(ih/2)*2') | Where-Object { $_ }) -join ','
        & $ffmpeg -v error -y -i $gif -vf $vf -c:v libx264 -crf $Crf -preset slow -pix_fmt yuv420p -movflags +faststart -an $mp4
        if ($LASTEXITCODE -ne 0) { Write-Error "ffmpeg failed: $gif"; continue }
        $out = Get-Size $mp4

        # 4. Markdown links in the same folder.
        $updated = @()
        if ($UpdateLinks) {
            $pattern = '!\[([^\]]*)\]\(' + [regex]::Escape("$name.gif") + '\)'
            foreach ($md in Get-ChildItem -LiteralPath $dir -Filter '*.md' -File) {
                $bytes = [IO.File]::ReadAllBytes($md.FullName)
                $hasBom = $bytes.Length -ge 3 -and $bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF
                $text = [IO.File]::ReadAllText($md.FullName)
                if ($text -notmatch $pattern) { continue }
                # A page's URL is ".../Page/", so same-folder media is one level up;
                # index.md is served at the folder URL itself.
                $src = if ($md.BaseName -in 'index', 'README') { "$name.mp4" } else { "../$name.mp4" }
                $text = [regex]::Replace($text, $pattern, {
                        param($m)
                        $title = if ($m.Groups[1].Value) { ' title="{0}"' -f [Net.WebUtility]::HtmlEncode($m.Groups[1].Value) } else { '' }
                        '<video src="{0}" autoplay loop muted playsinline width="{1}" height="{2}"{3}></video>' -f $src, $out.Width, $out.Height, $title
                    })
                [IO.File]::WriteAllText($md.FullName, $text, [Text.UTF8Encoding]::new($hasBom))
                $updated += $md.Name
            }
        }

        [pscustomobject]@{
            Gif      = Split-Path $gif -Leaf
            GifKB = [int]((Get-Item $gif).Length / 1KB)
            Crop  = if ($crop) { $crop } else { 'none' }
            Size  = '{0}x{1}' -f $out.Width, $out.Height
            Mp4KB = [int]((Get-Item $mp4).Length / 1KB)
            Pages = $updated -join ', '
        }
    }
}
