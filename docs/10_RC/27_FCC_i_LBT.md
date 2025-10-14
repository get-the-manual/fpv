# FCC и EU LBT

## На какой версии стандарта связи должен быть пульт? FCC и LBT?
Краткий ответ: Одно из отличий, LBT имеет ограничение по мощности в 100мВт. У FCC таких ограничений нет.  
Если дрон из Китая, то лучше брать пульт с FCC.   
А вообще можно перешить пульт с одной на другую, если не ту возьмешь.  
[Updating Internal ExpressLRS Module](https://oscarliang.com/setup-radiomaster-pocket/#Updating-Internal-ExpressLRS-Module)  

**Производитель аппаратуры RadioMaster заявляет**, что LBT ограничения только программные. Перепрошивка на FCC снимает все ограничения.  
Оригинальная цитата:  
LBT is no different from FCC hardware, only equipped with different software firmware.  
LBT is only suitable for EU regulations, with a limited power of 100mW and must use the low performance LBT protocol (Listen before talk).  
Outside the EU, you can use the fully functional FCC version without any restrictions, and of course, you can easily replace the firmware and switch between LBT and FCC versions at any time.

## Что такое FCC и EU LBT?
`FCC (Federal Communications Commission)` – это стандарт, регулируемый Федеральной комиссией связи США. Настоящий стандарт устанавливает правила и нормы использования радиочастот в Соединенных Штатах. FCC версия прошивки приемника предназначена для рынка США и других регионов, где действуют аналогичные стандарты.

`EU LBT (Listen Before Talk)` – это стандарт, принятый в Европейском Союзе. Эта технология включает механизм, заставляющий приемник проверять наличие другого радиосигнала на частоте перед тем, как начать передачу. Такой подход обеспечивает меньшее препятствие и безопасное использование радиоволн, особенно в случаях, когда воздушное пространство переполнено другими сигналами.

## Выбор между FCC и EU LBT
Выбор между версиями FCC и EU LBT зависит от региона, где вы планируете использовать оборудование. Это связано не только с требованиями законодательства, но и техническими особенностями каждого стандарта. Например, приемники и передатчики с прошивкой EU LBT могут эффективно не работать за пределами Европы из-за разницы в радиочастотных нормах.

## Прошивка приемников и передатчиков
Процесс обновления прошивки в приемниках – это важная процедура, позволяющая пользователям адаптировать оборудование под конкретные стандарты. Следует отметить, что прошивка приемника должна соответствовать прошивке передатчика. К примеру, приемник с EU LBT прошивкой не будет совместим с передатчиком на прошивке FCC.

[Ссылка на источник](https://modelistam.com.ua/standarty-fcc-lbt-a-294/)

При прошивки можно указать `FCC` или `LBT`. Подробнее можно прочитать в этой статье в разделе [Updating Internal ExpressLRS Module](https://oscarliang.com/setup-radiomaster-pocket/#Updating-Internal-ExpressLRS-Module)