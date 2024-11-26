# Bind устройств с протоколом ELRS
Есть два способа связывания дрона и пульта:  
- **Bind по кнопке**  
- **Bind фразу**

Обязательным предусловием является то что на дроне и аппаратуре должно быть:  
- **одинаковый** протокол (ELRS)  
- **одинаковой версии** (2.х.х или 3.х.х)  
- **одинаковой частоты** (2.4ГГц или 915 мГц)  

Если версии не совпадают, значит надо апгрейдить или передатчик на аппаратуре до версии на приемнике дрона, или приемник дрона до версии на передатчике аппаратуры.  
Процесс апгрейда выходит за рамки этой статьи.  
Можно прочитать, например, [на этом сайте](https://expresslrs.ru/Manuals/Firmware/Transmitters/Flashing-internal-tx/).  
Или в видео от Joshua Bardwell: [Easiest Way To Flash and Bind ExpressLRS](https://www.youtube.com/watch?v=MFFUsN9ZHSU)

Если не совпадают частоты, сбиндить дрон и пульт **нельзя**.

## Тип подключения ELRS приемника к полетнику
ELRS приемник может быть подключен к полетному контроллеру двумя способами: через **UART** порт или **SPI**. От этого зависит, каким образом биндить дрон.  
Проверить способ подключения можно подключив дрон к Betaflight.  
[Далее читать здесь](18_Подключение_приемника.md)

## [Проверка версии ELRS](20_Версия_ELRS.md)

## Bind по кнопке.  
 - Переводим дрон в режим Bind. Способ входа в этот режим может зависит от способа подключения приемника к полетному контроллеру (см. ниже).  
 - Выполняем операцию Bind на аппаратуре. Способ  выполнения зависит от аппаратуры (см. ниже).  
 
Недостаток такого способа в том, что таким образом можно связать одну аппаратуру только с одним дроном. Последующая перепривязка разрывает прошлую связь.

[Перевод дрона в режим Bind (UART)](40_Режим_Bind_дрона_с_UART.md)  

[Перевод дрона в режим Bind (SPI)](50_Режим_Bind_дрона_с_SPI.md)  

[Перевод пульта на EdgeTX в режим Bind на примере RadioMaster Pocket](60_Режим_Bind_пульта_EdgeTX.md)  

[Перевод пульта BETAFPV LiteRadio 3 в режим Bind](62_Режим_Bind_пульта_Literadio3.md)

## Указание Bind фразы.  
В ELRS модуль и аппаратур(ы) и дрона или дронов указывается одно и тоже ключевое слово (Bind фраза). И тогда все аппаратуры и дроны с этим словом могут работать без перепривязки.  

[Ввод Bind фразы на дроне с приемником на UART](44_Bind_фраза_дрона_с_UART.md)  

[Ввод Bind фразы на дроне с приемником на SPI](54_Bind_фраза_дрона_с_SPI.md)  

[Ввод Bind фразы на пульте с EdgeTX](56_Bind_фраза_пульта_EdgeTX.md)  

[Ввод Bind фразы на пульте BETAFPV LiteRadio 3](58_Bind_фраза_пульта_LiteRadio.md)  

## Полезные статьи и видео на тему Bind
[ExpressLRS.RU: биндинг](https://expresslrs.ru/Manuals/Binding/)

[Radiomaster Pocket - обзор, разбор, пейр. YouTube: Petrokey](https://www.youtube.com/watch?v=xYzz5JtX9GE)

[ExpressLRS - соединить Betafpv Cetus, LiteRadio3 RadioMaster tx16s. версии, совместимость, настройка. YouTube: Petrokey](https://www.youtube.com/watch?v=cM5g9BC9sQY)

[Betafpv Cetus X пэйр пульта бинд кнопкой. YouTube: Petrokey](https://www.youtube.com/watch?v=CByA9YKPEJI)

[Я забыл BIND-фразу ELRS! Что делать? YouTube: 
ZhukoRama FPVlog (ZRFPV)](https://www.youtube.com/watch?v=c6mdZVzCn58)

[Как привязать пульт к квадрокоптеру BETAFPV. YouTube: Змей Горыныч](https://www.youtube.com/watch?v=fwcmUY4qMXs)

[Як це все підключити? З'єднуємо пульт та FPV дрон і налаштовуємо відеозв'язок. ELRS і Crossfire. (укр.). YouTube: Є-Дрон](https://www.youtube.com/watch?v=US8rYxZ1YHw)

[BETAFPV Meteor 65 Pro ELRS - How To Set Up and Bind (SPI Version). YouTube: Volaertus](https://www.youtube.com/watch?v=T3NA_eTy63k)

[Подключить Betafpv LiteRadio3 к UART ExpressLRS (Meteor75). YouTube Petrokey](https://www.youtube.com/watch?v=r3wsgmIChx0)
