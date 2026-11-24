# **Anti Gravity**

**Anti Gravity** — это настройка в PID-профиле, которая **временно усиливает интегральную составляющую (I), а начиная с Betaflight 4.3 — ещё и пропорциональную (P)** во время быстрых изменений газа. Она служит для **устранения клевков и наклонов коптера на резкие “прыжки” газа** (throttle punches) и поддержания устойчивости положения во время этих манёвров.

📌 Реальная механика:

* При резком увеличении или сбросе газа инерция дрона вызывает кратковременные «провалы» и нестабильность, а стандартный I-терм реагирует слишком медленно. Anti Gravity даёт *временный “буст”* I-терму, чтобы быстрее удерживать угол коптера при таких изменениях. Это особенно заметно во фристайле (Powerloops) и при резком наборе высоты.
* В Betaflight **4.2+ эффект стал независим от базового I**, то есть anti gravity gain сам определяет силу этого буста.
* В Betaflight **4.3** добавлено усиление P при резком сбросе газа.
* В Betaflight **4.4** алгоритм переписан: параметры режима и порога удалены, шкала gain изменилась (см. таблицу ниже).

---

## Где находится и как это настраивается

В Betaflight Configurator / Betaflight App:  
**PID Tuning → PID Profile Settings → PID Controller Settings → Anti Gravity**.

Параметры в CLI зависят от версии прошивки:

| Параметр | Betaflight 4.2–4.3 | Betaflight 4.4 и новее |
|---|---|---|
| `anti_gravity_gain` | 0–30000, по умолчанию 3500 (в Configurator — 3.5) | 0–250, по умолчанию 80 |
| `anti_gravity_mode` | SMOOTH (по умолчанию) или STEP | удалён |
| `anti_gravity_threshold` | 20–1000, по умолчанию 250 — порог скорости изменения газа, после которого Anti Gravity включается | удалён |
| `anti_gravity_cutoff_hz` | — | 2–50, по умолчанию 5 |
| `anti_gravity_p_gain` | — | 0–250, по умолчанию 100 — сила усиления P |

* **anti_gravity_gain** — сила усиления во время резких изменений газа.  
  Чем выше значение → тем сильнее удержание положения при throttle punches. Слишком высокое значение может привести к осцилляциям (колебаниям) и “тяжёлой” реакции.

Значения из разных версий **нельзя переносить напрямую**: в 4.4 изменилась не только шкала, но и сам алгоритм.

---

## Практика настройки

1. **Начни со значения по умолчанию** для своей версии прошивки.  
2. Лети, делай быстрые “throttle punches” (газ в пол вверх/вниз).  
3. Если коптер **наклоняется носом вверх/вниз на резких изменениях газа** — увеличь Anti Gravity.  
4. Следи за поведением при обычном газе — слишком высокий Anti Gravity может дать “тяжёлую” реакцию.  
5. Сохраняй и тестируй в воздухе короткими пробами.

---

## Статьи и руководства для чтения

[PID Tuning Guide _ Betaflight](https://www.betaflight.com/docs/wiki/guides/current/PID-Tuning-Guide)

[Betaflight 4.2 Tuning Notes _ Betaflight](https://www.betaflight.com/docs/wiki/tuning/4-2-Tuning-Notes)

[Betaflight 4.3 Tuning Notes _ Betaflight](https://www.betaflight.com/docs/wiki/tuning/4-3-Tuning-Notes)

[Betaflight Modes Explained and How to Setup - Oscar Liang](https://oscarliang.com/betaflight-modes/#ANTI-GRAVITY)

[Betaflight FPV Drone Tuning In 10 Simple Steps - Oscar Liang](https://oscarliang.com/fpv-drone-tuning/#Step-6-%E2%80%93-Anti-Gravity-and-I-Term-Relax)

---

## Видео по теме

[Betaflight 4.4 Anti Gravity REVIEW and Tuning](https://www.youtube.com/watch?v=Mb-Q52UINts)

[Betaflight 4.3 – AntiGravity Improvements Explained](https://www.youtube.com/watch?v=Xg6C-YalQm4)

[BETAFLIGHT ANTI GRAVITY (basic explanation)](https://www.youtube.com/watch?v=G63xIVN-Wro)

[Betaflight anti-gravity gain: что это и как настроить (старая версия, но принцип тот же)](https://www.youtube.com/watch?v=SmSWZFjXBGM)

## Anti Gravity для **1S whoop** (65–75 мм)

**минимум или почти выкл.**  
1S вупы лёгкие, инерция маленькая — Anti Gravity легко переусилить.

### Рекомендованные значения (Betaflight 4.2–4.3)

Значения ниже указаны так, как их показывает Configurator. В CLI они умножаются на 1000 (1.5 → `1500`).

**База (стартовая точка):**

* Anti Gravity Gain: **1.0–2.0**  
* `anti_gravity_mode`: **SMOOTH**

**Если при резком газе whoop клюёт носом / заваливается:**

* повышай до **2.5–3.0**

**Если появляются признаки перекомпенсации:**

* дёрганье по pitch при газе  
* “липкий” отклик  
  → снижай до **0.5–1.0** или **выключай (0)**

### Betaflight 4.4 и новее

Точного пересчёта старых значений нет. Начни со значения по умолчанию (`80`) и снижай его шагами по 10–20, если появляются признаки перекомпенсации из списка выше.

---

### Типовые сетапы (Betaflight 4.2–4.3, значения Configurator)

**65 мм / 0702 / 19000–23000 KV / indoor**

* Anti Gravity Gain: **0.5–1.0**  
* часто **0** работает лучше

**65–75 мм / 0802 / 20000–22000 KV / outdoor**

* Anti Gravity Gain: **1.5–2.5**

**75 мм heavy whoop (камера + VTX + защита)**

* Anti Gravity Gain: **2.0–3.0**

---

### Важные замечания именно для 1S

* Anti Gravity **не лечит плохой I-term**  
* Если whoop “плавает” → сначала **I gain**, потом Anti Gravity  
* На 1S **лучше недокрутить**, чем перекрутить  
* Высокий Anti Gravity = больше шума в PID loop

---

### CLI (пример)

Betaflight 4.2–4.3 (1.5 в Configurator):

```bash
set anti_gravity_gain = 1500
set anti_gravity_mode = SMOOTH
save
```

Betaflight 4.4 и новее:

```bash
set anti_gravity_gain = 60
save
```

---

## Практический тест

1. Висение  
2. Резко газ в пол → резко убрать  
3. Смотри:

   * клюёт → ↑ Anti Gravity  
   * дёргается → ↓ Anti Gravity

## Побочный эффект
Происходит корректировка интегральной составляющей PID при изменении газа — как бы задержка на отклик.  
Эффект такой, что ты газ бросил, а дрон не падает.
