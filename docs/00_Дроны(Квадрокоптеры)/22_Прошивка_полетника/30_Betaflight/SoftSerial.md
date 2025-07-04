# SoftSerial

**Главная рекомендация**: It is always better to use a hardware UART if one is available.

## Статьи и видео
[Руководство: как использовать Betaflight SoftSerial](https://rcdetails.info/rukovodstvo-kak-ispolzovat-betaflight-softserial/)

[Tutorial: Betaflight Softserial. YouTube: Mirko FPV](https://www.youtube.com/watch?v=7b_ltNHvuvk)

[Setting Up Softserial In Betaflight! New Command In 4.5! - FPV Questions. YouTube: Joshua Bardwell Livestream Clips](https://www.youtube.com/watch?v=Yx27d-7zNJY)

[SoftSerial - Betaflight](https://betaflight.com/docs/wiki/guides/current/softserial)

[Tutorial: How to Setup Betaflight Softserial (oscarliang)](https://oscarliang.com/betaflight-soft-serial/)

## Схема подключения аналогового VTX на SoftSerial
Если хочешь как на сайте бэты для полетника BETAFPV F4 1S 12A AIO, в порт s5, то надо:  
- Активировать softserial на вкладке Конфигурации.  
- Убедиться что softserial действительно заведен на эту площадку (вроде как да. см resource для полетника)  
- во вкладке порты нужно на появившемся softserial выставить в последней колонке "VTX (TBS Smart Audio)"  

**Предупреждение:** SoftSerial грузит CPU, если есть свободные порты, лучше их использовать


## SoftSerial грузит CPU
Уменьшаем частоту Gyro и PID Loop с 8kHz на 4kHz на закладке Configuration.  
[Not Enough UART Ports? No Worries - Betaflight SOFTSERIAL To The (GPS) Rescue. YouTube: KremerFPV](https://www.youtube.com/watch?v=C7zYdPz-KtY&t=279s)

## Назначение SoftSerial в ресурсах
В BF 4.4 он называется:   
SERIAL_RX 11  
SERIAL_TX 11  
SERIAL_RX 12  
SERIAL_TX 12  

В BF 4.5:  
SOFTSERIAL_TX 1   
SOFTSERIAL_RX 1   
SOFTSERIAL_TX 2   
SOFTSERIAL_RX 2   

посмотреть можно командой 
```
resource 
resource show
```
