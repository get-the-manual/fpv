# Вопросы, проблемы связанные с видео

## Много помех при полетах в помещении
Нужно на дрон и шлем ставить клевер- антенну. При отражении сигнала от стены он меняет поляризацию и практически не дает помех на приемную антенну.

## В OSD и Betaflight не отображается ток и напряжение
Нужно проверить, чтобы в Betaflight на вкладке Power в обоих выпадающих списках стояло `Onboard ADC` (Встроенный АЦП).  
[I Don't Have Any Voltage In Betaflight! Help! - FPV Troubleshooting. YouTube:L Joshua Bardwell Livestream Clips](https://www.youtube.com/watch?v=X0aFJ_YZqi4)

## Помехи в изображении шлема
**Вопрос**: В шлеме BETAFPV VR03 появляются помехи.  Записал DVR, а на нем идеальное изображение без помех.  
Связка: Foxeer f405, VTX Zeus nano. Отдельно VTX с камерой и другим полетником нормально работают. Как такое может быть?

**Решение**: Переключил камеру с PAL на NTSC и помехи ушли

## Видео в очках плавно смещается в сторону
[Analog Video Scrolls To The Right? How Do I Fix It? Bad Camera?. YouTube: Joshua Bardwell Livestream Clips](https://www.youtube.com/watch?v=CbkhxywTZZs)

## OSD в изображении "моргает"
"Строб" OSD может быть:  
- если не совпадает система PAL/NTSC на камере и в настройках OSD Betaflight.  
- Хреново залились шрифты в OSD. Для этого нужно с включенной баткой залить другой шрифт, потом перезалить обратно свой любимый.