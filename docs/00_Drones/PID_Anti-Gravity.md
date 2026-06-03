# **Anti Gravity**

**Anti Gravity** — это настройка в PID-профиле, которая **временно усиливает интегральную составляющую (I), а в новых версиях Betaflight — ещё и пропорциональную (P)** во время быстрых изменений газа. Она служит для **устранения рыскания/наклонов коптера на резкие “прыжки” газа** (throttle punches) и поддержания устойчивости положения во время этих манёвров. 

📌 Реальная механика:

* Когда ты резко меняешь газ (вверх/вниз), стандартный I-терм может реагировать слишком медленно. Anti Gravity даёт *временный “буст”* этому I-терму, чтобы быстрее удерживать угол/позицию коптера при таких изменениях.  
* В Betaflight **4.2+ эффект стал независим от базового I**, то есть anti gravity gain сам определяет силу этого буста.

---

## Где находится и как это настраивается

В Betaflight Configurator / Betaflight App:  
**PID Tuning → PID Profile Settings → PID Controller Settings → Anti Gravity**. 

Настройки:

* **anti_gravity_gain (Anti Gravity Gain)** — сила усиления I/P во время резких изменений газа.  
  Чем выше значение → тем сильнее усиливается удержание положения при throttle punches.  
* В новых версиях также есть **anti_gravity_mode** (обычно SMOOTH) для более плавного эффекта. 

Типовые диапазоны:

* 3–5 (для классического 5″ фристайла) — мягко помогает.  
* Большее значение = более жёсткая реакция на газ и стабилизация.

---

## Практика настройки

1. **Начни с низкого значения** (например 0–3).  
2. Лети, делай быстрые “throttle punches” (газ в пол вверх/вниз).  
3. Если коптер **наклоняется носом вверх/вниз на резких изменениях газа** — увеличь Anti Gravity.  
4. Следи за поведением при обычном газе — слишком высокий Anti Gravity может дать “тяжёлую” реакцию.  
5. Сохраняй и тестируй в воздухе короткими пробами. 

---

## Статьи и руководства для чтения

[PID Tuning Guide _ Betaflight](https://www.betaflight.com/docs/wiki/guides/current/PID-Tuning-Guide)

[Betaflight 4.2 Tuning Notes _ Betaflight](https://www.betaflight.com/docs/wiki/tuning/4-2-Tuning-Notes)

[Betaflight Modes Explained and How to Setup - Oscar Liang](https://oscarliang.com/betaflight-modes/#ANTI-GRAVITY)

[Betaflight FPV Drone Tuning In 10 Simple Steps - Oscar Liang](https://oscarliang.com/fpv-drone-tuning/#Step-6-%E2%80%93-Anti-Gravity-and-I-Term-Relax)


---

## Видео по теме

[Betaflight 4.4 Anti Gravity REVIEW and Tuning](https://www.youtube.com/watch?v=Mb-Q52UINts&utm_source=chatgpt.com)

[Betaflight 4.3 – AntiGravity Improvements Explained](https://www.youtube.com/watch?v=Xg6C-YalQm4&utm_source=chatgpt.com)

[BETAFLIGHT ANTI GRAVITY (basic explanation)](https://www.youtube.com/watch?v=G63xIVN-Wro&utm_source=chatgpt.com)

[Anti Gravity Overview in Betaflight (older but concept same)](https://www.youtube.com/watch?v=SmSWZFjXBGM&utm_source=chatgpt.com)


## Anti Gravity для **1S whoop** (65–75 мм)

**минимум или почти выкл.**  
1S вупы лёгкие, инерция маленькая — Anti Gravity легко переусилить.

### Рекомендованные значения (Betaflight 4.3–4.5)

**База (стартовая точка):**

* `anti_gravity_gain`: **1.0–2.0**  
* `anti_gravity_mode`: **SMOOTH**

**Если при резком газе whoop клюёт носом / заваливается:**

* повышай до **2.5–3.0**

**Если появляются признаки перекомпенсации:**

* дёрганье по pitch при газе  
* “липкий” отклик  
  → снижай до **0.5–1.0** или **выключай (0)**

---

### Типовые сетапы

**65 мм / 0702 / 19000–23000 KV / indoor**

* `anti_gravity_gain`: **0.5–1.0**  
* часто **0** работает лучше

**65–75 мм / 0802 / 20000–22000 KV / outdoor**

* `anti_gravity_gain`: **1.5–2.5**

**75 мм heavy whoop (камера + VTX + защита)**

* `anti_gravity_gain`: **2.0–3.0**

---

### Важные замечания именно для 1S

* Anti Gravity **не лечит плохой I-term**  
* Если whoop “плавает” → сначала **I gain**, потом Anti Gravity  
* На 1S **лучше недокрутить**, чем перекрутить  
* Высокий Anti Gravity = больше шума в PID loop

---

### CLI (пример)

```bash  
set anti_gravity_gain = 1.5  
set anti_gravity_mode = SMOOTH  
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
Происходит корректировка интегральной составляющая PID газа - как бы задержка на отклик.  
Эффект такой, что ты газ бросил, а дрон не падает.
