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

## ELRS приемник по SPI протоколу
Если ELRS приемник подключен к полетнику , он не имеет собственного WiFi сайта. Все настройки осуществляются через CLI в Betaflight Configurator.

### номер модели для Model Match

```get expresslrs_model_id```
Значение от 0-255. 255 означает Model Match отключен.

```set expresslrs_model_id = N```

### Packet Rate
``` get expresslrs_rate_index```

На странице [How to Bind with F4 Betaflight FC (SPI ExpressLRS Receiver)](https://support.betafpv.com/hc/en-us/articles/4403742839705-How-to-Bind-with-F4-Betaflight-FC-SPI-ExpressLRS-Receiver) сайта BETAFPV указано:  
- 500Hz = 0  
- 250Hz = 1  
- 150Hz = 2  
- 50Hz = 3

Ставить значение обычно не нужно. Оно устанавливается автоматически при успешном бинде.

### Установка Bind фразы
Устаревший способ. Обычно уже есть поле ввода.


```set expresslrs_uid = [ Your UID bytes ]```
где UID - это хеш  
Сначала нужно превратить Bind фразу в коды и потом ввести ее.
Для этого идем на страницу [UID Byte Generator сайта https://www.expresslrs.org.](https://www.expresslrs.org/hardware/spi-receivers/?h=uid#uid-byte-generator).  
Там в поле Binding Phrase вводим свое слово. Ниже появятся нужные коды и команда, которую надо ввести в CLI.




## Turtle mode settings
``
set crashflip_motor_percent = 0  
set crashflip_expo = 35
``
Подробности в видео [My Whoop Won't Flip Over! Fixing Turtle Mode? - FPV Questions. YouTube: Joshua Bardwell Livestream Clips](https://www.youtube.com/watch?v=U4AvhJiqLPM)
