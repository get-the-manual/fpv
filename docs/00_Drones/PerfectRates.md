# Настройка рейтов по научному

Материал ниже основан на видео [Perfect FPV Rates with Science? YouTube: nils vo](https://www.youtube.com/watch?v=ssBoPK95vus)

По схожему принципу создан проект `RateFinder`, суть которого заключается в анализе стиля полета в симуляторе и расчете оптимальных рейтов.

## RateFinder

RateFinder — это бесплатная программа для Windows, которая:  
- Подключается к вашей аппаратуре через USB.  
- Записывает ваши действия стиками с частотой около 1 кГц во время выполнения калибровочных полетов в VelociDrone.  
- Измеряет три показателя на основе ваших данных: радиус точности центра (CPR), максимальную скорость срабатывания (CMV) и распределение в среднем диапазоне (MDI).  
- Рекомендует персонализированные Actual/Betaflight рейты, основанные на вашем стиле.

Главный сайт проекта: [RateFinder — Wildtype Drones](https://www.wildtypedrones.com/ratefinder)

Репозиторий с описанием и дистрибутивом программы [jaydeelocs_WTRateFinder](https://github.com/jaydeelocs/WTRateFinder)

Видеоинструкция: [RateFinder v1.0 — The First FPV Rate Tuning Tool That Watches How You Fly. YouTube: Joshua Locsin](https://www.youtube.com/watch?v=rF5qzlTjtDE)


[Статья на www.reddit.com](https://www.reddit.com/r/fpv/comments/1s80osr/i_built_a_free_tool_that_analyzes_your_stick/)

## Видео от `nils vo`

[Perfect FPV Rates with Science? YouTube: nils vo](https://www.youtube.com/watch?v=ssBoPK95vus)

**Перевод**

**(00:00–00:35)** Рейты (rates) выглядят похоже, но дают разное поведение. Неправильные рейты ограничивают прогресс — они должны соответствовать стилю полёта, а не просто «нравиться».

**(00:35–01:24)** Рейты — это преобразование движения стика в угловую скорость (deg/s). Важно не название параметров (expo и т.д.), а форма кривой: она определяет чувствительность.

**(01:24–02:17)** Даже при expo=0 кривая может быть нелинейной, и наоборот. Смотри только на график. Оптимум — баланс между экспоненциальной и линейной кривой.

**(02:36–04:05)** Полёт делится на диапазоны:

  * precision: ~0–50 deg/s (точные движения)  
  * normal: ~50–300 (обычный полёт)  
  * tricks: ~600–800+ (флипы/роллы)  
  * между ними есть «мёртвая зона», которую лучше минимизировать  

**(04:25–04:44)** Чем ниже максимальный rate, тем выше общая точность. Ставь максимум как можно ниже, но достаточный для трюков.

**(05:07–06:22)** Оптимизация:

  * увеличить долю precision + normal диапазонов  
  * уменьшить dead range  
  * регулировать center sensitivity и expo для формы кривой  

**(06:22–06:43)** Резкие изгибы кривой дают «неконсистентное» управление (разная чувствительность на близких значениях). Кривая должна быть плавной.

**(08:28–09:08)** Правильные рейты → равномерное использование диапазона (график распределения). Неправильные → пилот «зависает» в мёртвой зоне.

**(09:44–11:23)** Разные стили требуют разных рейтов:

  * freestyle → мягкий центр + отдельный диапазон для трюков  
  * racing → почти линейные рейты, без выраженных зон точности/трюков  

**(12:28–14:05)** Рейты не только подстраиваются под стиль, но и меняют его. Небольшие изменения (center, expo, max rate) сильно влияют на контроль и точность.



## Transcript:
(00:00) These two lines are two different FPV rates and they may look quite similar to an untrained eye, but they actually fly very different from each other. And picking the wrong one for your personal flying style can be very limiting to your progress because rates are really not just personal preference, but instead they need to reflect your flying style.

(00:19) So, it kind of is personal preference indirectly if you set up your rates correctly. But if you try to force a flying style that doesn't match your rates, you're going to have a very hard time. So, in this video, I'm going to show you how to find out what your personal flying style actually is and how to set up your rates to match that flying style.

(00:35) So, the first step is to understand what rates actually are and how they affect your flying. So, rates are basically just some fancy math that convert your stick input into a rotation speed of your drone. Or in other words, it dedicates a specific range of your stick input to a specific type of turn. So, with these rates, it maps this portion of your stick travel to the 0 to 50 turning speed.

(00:57) So that's about seven boxes dedicated for 0 to 50. And then maps this stick travel to 50 to 100. So about three boxes for 50 to 100. And then it maps about two boxes to 100 to 150. And about 1 and a half boxes to 150 to 200. And it basically goes like this up all the way up until 700. So these rates are actually quite exponential even though this expo value is set to zero which is super misleading.

(01:24) And let's take a look at the other rates. So these rates have an expo of one, but they're actually super linear. So as you can see, you have about three and a half boxes for 0 to 50 and another three and a half boxes for 50 to 100 and another 3 and 1/2 boxes for 100 to 150 and another 3 and 1/2 boxes to 150 to 200.

(01:45) So these rates are super linear even though they have an expo value of one. So really just forget about what these numbers are called and what they are and really just look at this curve and see where it lands on this graph because that's all that really matters. And these rates are kind of the two extremes.

(02:01) And what you're probably looking for is something in between. So this red line is my rate which is kind of a mix between the two. It's right between the two in the beginning and then it kind of continues on like the more linear rate in the end. And this kind of balance is exactly what you're looking for.

(02:17) But let me show you with some practical examples how this really translates to your flying because you probably have no idea what 100°/s of rotation actually looks like and what kind of tricks that applies to. So I'm going to take you to my simulator where I added two features that make showing this very easy and that can also help you find your perfect rates.

(02:36) So to really make sense of what I just said, you first need to understand your personal flying style. So this little number in the middle of the screen is the current turning speed in degrees per second. So if I'm flying here, it's ranging from like 50 to 150, something like that. And to understand your flying style, you can basically split your rates into four separate parts.

(02:59) The first is your precision range. So let's find out what my precision range is. So let's say I want to hit this small little window. Let's do it again. I think I saw like a 25 there. I think I even hit a hit a 50 51s. So, it's somewhere between 0 and 50. That's my precision range. Then next up is my normal flying range.

(03:29) So, let's say I'm just uh freestyling along here. And this is somewhere between 50 and 250, maybe 300. So just normal corners, stuff like that. Let's do like an orbit as well. The orbit is like 150 to 200 something. So I'd say my normal flying range is somewhere between 50 and 300. Then there's your trick range, which for me is 700.

(04:05) So that's your like max roll, max flip. And that number is really personal preference. So if you like doing a lot of really fast tricks like Rubik's cubes and flips, rolls, that kind of stuff, you might be looking somewhere at the 900 to,00 range. But for most freestyle pilots, this is going to be somewhere in the 600 to 800 range.

(04:25) And one thing you need to understand about your max rate is, the lower you go, the more precision you have across the entire range of your raids. So you really want to set this as low as possible while still allowing everything that you want to do because going higher than you need it to be will limit your precision in the rest of the range.

(04:44) So for me, we got the precision range, which is 0 to 50. then my normal flying range which is about 50 to 300 and my trick range which is 700. And the space between my normal flying range and the trick range is kind of the dead space that I don't really use. So with these numbers in mind, let's go back to the graph and I'll show you exactly how to use these numbers to optimize your rates.

(05:07) And the key to optimizing your rates is to make the precision range and the normal flying range as big as possible and minimizing the space for the dead range because that's just wasted. So, let's start by cranking the expo to one, which moves everything to the right like this. And then increase the center sensitivity to make the normal flying range as big as possible.

(05:29) Because currently my normal flying range is only like this. So let's make it very big like that. So now I got all the space for my normal flying and precision flying and just this much space is wasted which is already a lot better. But also if I leave it like this my precision range is pretty small. And to increase it I need to change the shape of this curve a little bit.

(05:57) So with a center of 350 I got about three and a half boxes of precision range which is pretty small. And to increase it, I'm just going to decrease the center sensitivity a bit. So with 250, I got about five boxes, which is a lot more reasonable. But by doing that, I also decrease the sensitivity up here. So to bring that back a little bit, I'm going to decrease the expo to flatten this range out a bit.

(06:22) Because every time you cross over this bendy part in the range, it's going to feel super inconsistent. Because let's say you are correcting from 150° to 200, you have about two and a half boxes. But if you want to correct from 250 to 300, you only have one box. So it's going to feel super inconsistent because your rate is changing so much in a very short amount of time.

(06:43) So you really want to avoid crossing over the bendy part. And to do that, I'm going to take a bit of expo out to just reduce how bendy this is. So at 0.7, this looks a lot more gradual, which is going to feel a lot nicer and more consistent. So with these rates now, I got a fairly good precision range down here with about 4 and a half boxes.

(07:03) Then I got a pretty big normal flying range up until 300. Then I got a relatively small dead range. And of course, I got my max rate or my trick range up here at 700 still. So let's get back on the sim and try these out. 250, 700, and 0.7. And let's try flying like this. And I've never flown these rates before, so this is first time trying them.

(07:28) And let's see how it feels. [screaming] Nice. It's a bit different than my rates, so it's going to take a couple minutes to get used to. >> [screaming] [screaming] >> And this This is what it should look like once you set up your rates correctly for your flying style. And there's no getting used to them first because they're just going to feel right for your flying style.

(08:28) And another tool I added to the sim to find out if your rates are set up correctly is a little graph in the rates tab, which looks like this. And it basically shows you what part of your range you're using the most. And this is exactly what it looks like if your rates are set up correctly. >> [snorts] >> It's going to be a fairly even spread across the entire range.

(08:50) And here's the the dead range where I'm not really flying, which is good. And then here, a little tick where I'm doing my max flips and max rolls. But let me now show you what happens if you try to force a different flying style that is not really compatible with these rates. And to do that, I'm going to take these racing.

(09:08) Let's do Let's do this one. So in these rates, I got a 6.4. And let's look at the distribution. So this is what it looks like if you force a different flying style on rates that is not compatible. You can see I'm spending a lot of time in this dead range, which I really don't want to do because this is the expo range, which is super inconsistent.

(09:44) And you can see I'm spending basically an equal amount of time over the entire range. So let's set up some racing rates that are more compatible with this flying style. So since I was spending basically an equal amount of time over the entire range, I'm just going to make my rates very linear. So let's try this. It's very linear.

(10:02) It's going to be pretty twitchy in the middle, but I think that's fine for racing. And let's just uh try this 450 700.7. So, before on the freestyle rates, I got a time of 6.4, I think. And let's try on these rates. Oh, this is really twitchy. [screaming] On the on the second lap, I already matched my time. And I've never flown these race before, BY THE WAY.

(10:43) >> [screaming] >> ALREADY BEAT IT BY 0.3. [screaming] [screaming] OH, it beat my PB. So, this is what the distribution is like. And uh but they are feeling super twitchy in the middle. So, I'm going to decrease this a little bit. Let's try 400. And I'm also not really using this max range too much. So, I'm going to decrease this a bit to 600. And let's try these instead.

(11:23) [screaming] Oh, beat my PB again. And I even beat Freddy. Nice. Let's look at the graph now. Okay. I'm hitting max range quite a bit, but I think that's fine. And this is looking a lot more flat now. And it also feels much better. So these are the two rates that I just set up. The light one is the racing rate and the dark one is the freestyle rate.

(11:51) And as you can see, they are quite a bit different where the freestyle one has a lot more expo at the end because I need to get up to that trick range. And the freestyle one also is a lot more soft in the middle here. And the racing rate goes up pretty linear all the way up until the max range because when you're racing, you're not really doing tricks.

(12:08) So, you don't need a trick range and you're also not really flying precise. So, you don't really need a precision range either. And instead, it's all just normal flying range, which should be pretty linear. And that's exactly what this racing rate is. And I haven't flown either of these rates before making this video, and I didn't need any time to get used to them.

(12:28) So, as I said, rates are really not personal preference, but instead, they need to match your flying style. And if you set up your rates to match your flying style, then it just feels right at home and you don't need to get used to them. And on these racing rates, I beat my previous personal best by like half a second, which is a ton.

(12:43) And I've only flown these for like 3 minutes. So setting up your rates correctly can make a huge difference. And this doesn't only apply for racing, but also freestyle. So, if you have rates like these, which are very soft in the middle, and you try to do like a trippy spin or an orbit on these, then you might find it pretty hard because those tricks are usually in the 150 to 300 range.

(13:05) And with rates like these, you don't really have a lot of resolution here. So, what you should be looking for is sacrificing some of that early range and decreasing some of that dead range up here to make space for resolution here in this trippy spin and orbit range. And that's going to make a very big difference.

(13:19) And also you should try decreasing your max rate as much as possible because this gives you a very big accuracy boost over the entire range. And even going from say 800 to 700 is going to make a huge difference, much more than you might expect. And also rates not only need to match your flying style, but they also dictate your flying style.

(13:38) So once you change your rates, you might find yourself flying differently than before. And once your flying style changed, you might want to do this exercise again and fine-tune your rates a bit more to match your new updated flying style. And these changes are probably going to be pretty small, like going from 250 center stick to 220 to get a bit more accuracy in the middle or maybe changing your max rate by a couple clicks or your expo to make the end of the curve a bit flatter, stuff like that.

(14:05) So really, really small changes. So yeah, that's how you set up your rates with science. And don't underestimate how much of a difference even a small change of your curve can make, especially if you are flying freestyle on the beta flight default rates because those make trippy spins for example really hard because you don't really have a lot of resolution in this 150 to 250 range where a lot of these tricks like trippy spins and orbits fall.

(14:29) And optimizing rates can make a huge difference