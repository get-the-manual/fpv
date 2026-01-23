# Прошивка Betaflight
![](BetaflightLogo.png)

Betaflight - это открытое программное обеспечение (встроенное ПО) контроллера полета, используемое для настройки и полетов на различных аппаратах, (квадрокоптерах, гексакоптеров, октокоптеров) под управлением полетных контроллеров, работающих под прошивкой Betaflight.

[Официальный сайт](https://betaflight.com/)  
[Страница закачки Betaflight Configurator (на все ОС, включая Android)](https://github.com/betaflight/betaflight-configurator/releases/)  
[SpeedyBee - альтернатива Betaflight Configurator для Android](https://play.google.com/store/apps/details?id=com.runcam.android.runcambf&hl=ru&gl=US)

## Установка на Linux
```
sudo apt install betaflight-configurator  
sudo pacman -S betaflight-configurator
```

## Настройка дрона в Betaflight

**Кратко** от `Maxim Testov`:  

Заходишь в [веб конфигуратор](https://app.betaflight.com/) и ставишь под таргет своего полетника 4.5.2(последняя стабильная версия), только нужно не забыть выставить настройки справа, где протокол моторов, какой видеопередатчик.  
Далее все настраиваешь:  
- положение полетника
- юарты где у тебя RX, где VTX
- порядок моторы, направление вращения
- пиды можно попробовать накатить через пресеты для тинивупов (karate, justice)
- рейты свои ставишь
- ну и нужно не забыть проверить что во вкладке приемника у тебя правильно выставлен протокол приемника и полоски двигаются в зависимости от того чем ты на аппе двигаешь
- настраиваешь aux(тумблеры свои) на нужные действия (арм, черепаха...)
- залетаешь нужную таблицу для видеопередатчика (скорее всего есть на сайте производителя)
- настраиваешь OSD

Если с версией 4.5.2 что-то идёт не так, например видеопередатчик во вкладке vtx в состоянии не готов (not ready), то можно накатить предыдущую версию.


[Настройка FPV дрона в Betaflight для новичков + прошивка ESC BiHeli_S, BlueJay. YouTube: Recopter](https://www.youtube.com/watch?v=yJxMRLE3dVI)  

[КАК СОБРАТЬ ДРОН С FPV КАМЕРОЙ ЗА $89!. YouTube: recopter](https://www.youtube.com/watch?v=t7Dv1oOI_Qk)  
В видео есть пошаговая настройка дрона в Betaflight

[Типичная настройка дрона после сборки на примере 5" фристайлового квадрокоптера. YouTube: PropWash Service](https://www.youtube.com/watch?v=gLDMeevq410)

[Собрал и Полетел! Как настроить FPV дрон за минуты (на примере SpeedyBee Bee35). YouTube: DRONOFLY FPV](https://www.youtube.com/watch?v=-c042AORi24) 

[Налаштування квадрокоптера в бетафлай за 15хв (укр.) YouTube: Nazar Kovalenko](https://www.youtube.com/watch?v=JR5qjRWxhkQ)  

## Рекомендованные пресеты от Lesha Rodin
Начни например с `Justice`, главное галочки протыкай нормально, не ставь `spicy tune` поставь в `normal` билд и `normal tune`.   
Если плохо полетит попробуй `karate2024`  
Прошить `bluejay 48khz`  
Подобрать рейты себе по вкусу, рекомендую рейты `Min Chan Kim`, но у `Justice` в пресете тоже похожие по форме в целом.

## Blackbox Explorer
[BetaFlight Blackbox Explorer - детальна інструкція. YouTube: Aves Lab (укр.)](https://www.youtube.com/watch?v=FhQDbtbXL5Y)

## Серия видео `Betaflight Won't Arm` от Joshua Bardwell
[Betaflight Won't Arm](https://www.youtube.com/playlist?list=PLwoDb7WF6c8n8SCsG7mtIgQUgsuWxaiiO)

## Серия видео `Продвинутый Betaflight` от My Hobby Log
[Продвинутый Betaflight - Знакомство с Blackbox](https://www.youtube.com/watch?v=GphFE2Lt8SU)  
[Продвинутый Betaflight - Акро (Acro) и Рейты (Rates)](https://www.youtube.com/watch?v=xnpsr-AiDBo)  
[Продвинутый Betaflight - вибрации и фильтры (Notch, LPF)](https://www.youtube.com/watch?v=L7qY19ynXFk)  
[Продвинутый Betaflight - что такое ПИД регулятор](https://www.youtube.com/watch?v=m6YgSluarmA)  
[Продвинутый Betaflight - ГАЙД по настройке ПИД регулятора (ПИДов)](https://www.youtube.com/watch?v=OuZSiozHMt4)

## Видео по прошивке 2025.12
[НОВЫЕ ФУНКЦИИ BETAFLIGHT 2025.12.1. YouTube: recopter](https://www.youtube.com/watch?v=xA2UxVpMBd4)

[Betaflight 2025.12.1 Tutorial: Getting Position Hold Working. YouTube: Pineapple 22](https://www.youtube.com/watch?v=Wk2uNRD8kNc)

[The Secret to Perfect Alt Hold & GPS Rescue: It’s Not What You Think! | Betaflight 2025.12. YouTube: Pineapple 22](https://www.youtube.com/watch?v=vw1KBUagZHk)

[New Assisted Flip Crash Mode in Betaflight 2025.12.1 | Complete Setup. YouTube: Pineapple 22](https://www.youtube.com/watch?v=1OOqIkibCcQ)
