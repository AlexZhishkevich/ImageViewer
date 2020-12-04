# SOLID principles

Это аббревиатура пяти базовых правил разработки ПО, которые задают траекторию, по которой нужно следовать при написании программ, чтобы их проще было масштабировать и поддерживать. Они получили известность благодаря программисту Роберту Мартину.

## S – Single Responsibility (Принцип единственной ответственности)

Класс программы должен отвечать за конкретный функционал, который в идеале лаконично отражён в его названии. При соблюдении этого условия "класс имеет одну и только одну причину для изменения" (c) Роберт Мартин.  
Маркёрами для пересмотра структуры класса с целью соответствия принципу SRP могут служить следующие ситуации:
* объект класса имеет много "разношёрстных" методов
* доменная логика концентрируется только в одном классе;
* изменение свойств или методов класса влечёт за собой каскад необходимых изменений в других классах, где это не подразумевалось
* нет возможности перенести класс в другую часть приложения без множества ссылок на необходимые для него зависимости

Также стоит отметить, что объединение ответственностей является общепринятой практикой и в этом нет ничего плохого, до тех пор пока это легко обслуживать. Следование принципу единственной ответственности зависит от функций программного продукта и является труднейшим при проектировании приложений.


## O – Open-Closed (Принцип открытости-закрытости)

Данный принцип звучит так: «программные сущности (классы, модули, функции и т. п.) должны быть открыты для расширения, но закрыты для изменения» (с) Бертран Мейер.   
То есть программные сущности (не только классы, но и модули приложения) должны быть:
* открыты для расширения: поведение сущности может быть расширено путём создания новых типов сущностей
* закрыты для изменения: в результате расширения поведения сущности, не должны вноситься изменения в код, который эту сущность использует.

Есть некоторые различия в трактовке данного принципа: авто заявлял о том, что однажды разработанная реализация класса в дальнейшем требует только исправления ошибок, а новые или изменённые функции требуют создания нового класса. Этот новый класс может переиспользовать код исходного класса через механизм наследования, причём определение интерфейса может измениться.

Современное видение принципа заключается в том, что спецификации интерфейсов могут быть переиспользованы через наследование от "абстрактного интерфейса", но реализации изменяться не должны. То есть расширение функционала должно быть внедрено за счёт новой реализации "дочернего" интерфейса без переопределения "родителя".

## L — Liskov Substitution (Принцип подстановки Барбары Лисков)

Автор кратко сформулировала свой принцип следующим образом:

Пусть **q(x)** является свойством, верным относительно объектов **x** некоторого типа **T**. Тогда **q(y)** также должно быть верным для объектов **y** типа **S**, где **S** является подтипом типа **T**.

Роберт С. Мартин определил этот принцип так:

Функции, которые используют базовый тип, должны иметь возможность использовать подтипы базового типа, не зная об этом.

Простыми словами можно это принцип сформулировать так: родительский класс не должен знать, с какой именно реализацией абстракции он работает.

## I — Interface Segregation (Принцип разделения интерфейсов)

Принцип разделения интерфейсов говорит о том, что слишком «толстые» интерфейсы необходимо разделять на более маленькие и специфические, чтобы клиенты маленьких интерфейсов знали только о методах, которые необходимы им в работе для реализации интерфейса. В итоге, при изменении метода интерфейса не должны меняться клиенты, которые этот метод не используют. 

## D — Dependency Inversion (Принцип инверсии зависимостей)

Данный принцип призывает строить основной функционал приложения, основываясь на высокоуровневых абстракциях без привязки к деталям реализации, предполагая лёгкую заменяемость компонентов. И наоборот, детали реализации должны зависеть от абстракции.