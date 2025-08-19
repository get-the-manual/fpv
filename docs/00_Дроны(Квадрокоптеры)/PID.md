# Все о PID, фильтрах и их настройке

[О PID регуляторе простым языком. YouTube: microhobby](https://www.youtube.com/watch?v=NbEhtZlSa6A)

## Краткая инструкция 1
Когда я поставил на Мобулу 6 RCINPOWER GTS V3 0802 вместо родных, то она летала от силы минуту и высаживала батарею в 0. При этом сильно грелся полëтник.  
После этого почитал какой параметрах в пидах и фильтрах за что отвечает и для начала уменьшил все значения в 4-5 раз от исходного (в сторон более вялой работы). После чего греться сильно перестал, но летал дрон как мешок.  
Ну и затем смотрел как какой параметр влияет и по своим ощущениям "комфорта" крутил их. Если бы комп подключался к дрону без вынимания батарейки, то весь процесс занял был минут 30-40.

## Краткая инструкция 2
Поставить 1 из слайдов пидов, допустим I на низкое значение 0,6. Полетать секунд 30, просто качая его по осям. Потом переставить на 0,8 и так до 1,2 вроде. В процессе записывая всё на Blackbox и слушая моторы, а так же проверяя насколько они горячие

Первый ползунок на 0.2 двигать до 1.4  
Самый нижний МАСТЕР от 0.8 до 1.4 по 0.2  
I двигаем от 0.3 до 1.2 по 0.3  
FF Stick response от 0.5 до 1 . По 0.1  

После всего anti-gravity gain увеличивать, при полете по прямой с пиками газа, уменьшает нырки носа 

## Поведение дрона при заряженной/разряженной батарее
На вкладке PID Tunning есть переключатель `Vbat Sag Compensation`.  
Эта настройка корректирует пиды в зависимости степени разряженности батареи.  
При включенной настройке поведение дрона будет одинаковым при  разряженной и заряженная батке.

## Статьи
[Что такое PID, на что влияет и как настроить - Все о квадрокоптерах - PROFPV.RU](https://profpv.ru/chto-takoe-pid-na-chto-vliyaet-i-kak-nastroit/)

[How to Tune FPV Drone Filters & PID with Blackbox (PIDToolBox & Blackbox Explorer Guide) - oscarliang.com](https://oscarliang.com/pid-filter-tuning-blackbox/)

## PID для чайников от Mustfly
[О настройке PID простыми словами. Часть 1](https://www.youtube.com/watch?v=aLAsaDUWzuc)  
[Часть 2. Дружище, ты в порядке?](https://www.youtube.com/watch?v=YZBem_4jWSQ)  
[Часть 3. Настраиваем PID контроллер](https://www.youtube.com/watch?v=zGu1mwwVEm8)  
[Базовая настройка PID в Betaflight](https://www.youtube.com/watch?v=KkFfeIvJPjI)

## Налаштування PID от Aves Lab (укр.)
[Налаштування PID регулятора для FPV дронів в Betaflight. Теоретична частина. YouTube: Aves Lab](https://www.youtube.com/watch?v=NlqPHb28eaw)  

[Практичні поради з налаштування PID регулятора Betaflight для FPV дронів. YouTube: Aves Lab](https://www.youtube.com/watch?v=76FeOTWqC_Y)

## Другие видео
[Betaflight - теория PID и Фильтров. Youube: fpv_am](https://www.youtube.com/watch?v=YjYo7p7Nu9o)

[О том что такое PID и рейты, как настроить ПИДы? YouTube: WARPfly](https://www.youtube.com/watch?v=Rnytz89bVss)

[КАК НАСТРОИТЬ FPV ДРОН - PID и ФИЛЬТРЫ для НОВИЧКОВ - Betaflight 4.4. YouTube: recopter](https://www.youtube.com/watch?v=X5IRArDcGx8)

[Налаштування фільтрів FPV в Betaflight від #SocialDroneUA (укр.)](https://www.youtube.com/watch?v=Wlc4EoptGGk)

[How does PID controller work? | Simple Explaination on Quadcopter. YouTube: Pratik Phadte](https://www.youtube.com/watch?v=dMRDzicSvXk)

[Please PID Tune your FPV Drone (easy tutorial). YouTube: nils vo](https://www.youtube.com/watch?v=pgyTmRJ-hik)
