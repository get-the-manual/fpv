# Полезные команды Betaflight CLI
`diff` - получить текущие настройки, не включая те что по умолчанию  
`dump all` - получить **полные** текущие настройки включая те что по умолчанию  
`bl` - перейти в режим `Boot Loader / DFU`  
`status` - статус устройств и состояние полетника

`resource` - получить список ресурсов, то есть назначение функций на выводы микросхемы  
`resource show` - получить список всех площадок и их использование. См. видео [Setting Up Softserial In Betaflight! New Command In 4.5! - FPV Questions](https://www.youtube.com/watch?v=Yx27d-7zNJY)  

## Устройство для передачи OSD
Команда `osd_displayport_device` в Betaflight используется для настройки устройства, через которое передаются данные OSD (On-Screen Display) по DisplayPort. 
Считать значение можно запустив команду:  
`get osd_displayport_device`  
Ответ из полетника на аналоговом VTX:  
```
# get osd_displayport_device
osd_displayport_device = AUTO
Allowed values: NONE, AUTO, MAX7456, MSP, FRSKYOSD
```

Возможные значения:   
- `MAX7456` - аналоговый OSD, работающий через чип MAX7456. Это стандартный чип для FPV-систем с аналоговым видео (PAL/NTSC)    
- `MSP` - MSP-порт для отправки OSD-данных на цифровые системы или через UART.   
- `FRSKYOSD` - OSD системы FrSky, передающее данные через телеметрию FrSky SmartPort.  
- `auto` – Betaflight автоматически выбирает устройство DisplayPort.  
- `dji` – Принудительно использовать DJI FPV Air Unit или Caddx Vista.  
- `hdzero` – Принудительно использовать HDZero VTX.  
- `walksnail` – Принудительно использовать Walksnail Avatar VTX.  
- `none` – Отключает передачу данных OSD через DisplayPort.  

Если нужно явно указать порт, используется команда:  
`set osd_displayport_device = НУЖНОЕ_ЗНАЧЕНИЕ`

Прошивая полетник с аналоговым VTX на Betaflight 4.5.x можно случайно указать в опциях оба вида OSD: `OSD (Analog)` и `OSD (HD)`. И тогда на экране не будет OSD, потому что по умолчанию будет направляться в HD.  
Чтобы вернуть на аналог надо запустить следующие команды:
```
set osd_displayport_device = MAX7456
set vcd_video_system = AUTO
save
```
Объяснение тут [FAQ / No OSD in Betaflight 4.5](https://hackmd.io/@nerdCopter/r1JbnG0Q0)  

## Turtle mode settings
``
set crashflip_motor_percent = 0  
set crashflip_expo = 35
``
Подробности в видео [My Whoop Won't Flip Over! Fixing Turtle Mode? - FPV Questions. YouTube: Joshua Bardwell Livestream Clips](https://www.youtube.com/watch?v=U4AvhJiqLPM)
