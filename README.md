## Важная инфа
Данил попытался использовать встроенный OpenGL из Авалонии, не смог, тильтанул и перенёс весь проект в OpenTK (Что-то типа OpenGL + glfw но на C#). Всё что мы потеряли - возможность удобно создать меню игры. В остальном всё так же

## Описание игры
Навигация в лабиринте от первого лица. Цель - найти выход. Лабиринт генирируется рандомно для каждого запуска. Присутствуют разные настройки сложности, которые определяют размер лабиринта.<br> Лабиринт выглядит примерно так:
<br> ![image](https://github.com/user-attachments/assets/8515d752-9187-4904-85d0-aec06846c7bd)
<br> Сам геймплей будет выглядеть примерно так (скрин из Unity, без текстурок)
<br> ![image](https://github.com/user-attachments/assets/342f0594-c0bb-440c-83c6-3aa364992756)


# Возможности игрока
* Ходить
* Для начала хотя бы ходить, дальше посмотрим

## Задачи
1. - [ ] Реализовать 3D графику в OpenGL
   * - [x] Считывание моделек из .obj
   * - [ ] Отрисовка шейдером (поставлю галочку как только шейдер перестанет быть просто монотонным цветом)
   * - [ ] Сделать источник света (конкретно spotlight для фонарика игроку и направленный для дебага) 
2. - [x] Сделать модельки окружения
3. - [x] Реализовать цикл игровой логики
4. - [x] Сделать алгоритм генерации лабиринта
5. - [ ] Реализовать просчет коллизий (между игроком и полом/стенами. Должно быть не сложно)
6. - [ ] Реализовать контроллер игрока (перемещение камеры уже есть. По-хорошему засунуть класс камеры в класс игрока)
7. - [ ] Найти текстурки и натянуть их на модели (я думаю пиксельные подойдут)
8. - [ ] Сделать какое-нибудь базовое меню с выходом и выбором сложности. Тут теперь два стула:
        * Остаться в чистом OpenTK и делать UI с 0 средствами OpenGL
        * Портировать проект обратно в Авалонию вместе с OpenTK (в гугле лежит ровно 2 репозитория с примерами)
9. - [ ] Если останутся силы и время можно и звуки шагов и эмбиент прикрутить
10. - [ ] Желательно переделать вращение камеры с помощью кватернионов. Сейчас там просто через углы 

## Архитектура игры
Предлагаю взять чуть подправленную архитектуру из Unity и не париться:<br>
* Каждый объект почкуется от базового класса GameObject
* GameObject у нас будет иметь:
    * Абсолютные координаты
    * Поворот в Эйлере и радианах
    * Абстракстные методы Start, Update
* Цикл игровой логики (каждый кадр вызывается) такой:
    1. Просчет коллизий
    2. Регистрация input'а (клавы и мыши у нас)
    3. Update у всех объектов (там логика самих объектов выполняется)
    4. Рендер графики

## Маленькая документация для тех, кто что-то начнет делать
* Все ресурсы лежат в папке Assets. Путь к ней лежит в `MainWindow.assetsPath`
* Статический класс `MainLogic` управляет логикой всей игры. God object короче
* Все объекты хранятся в статическом списке `MainLogic.gameObjects`
* Интерфейсы `IRenderable` и `ICollider` определяют объекты которые нужно отрисовать и просчитать в коллизии соответственно
* Объекты создавать через `MainLogic.Instantiate<T>()`, а не `new T()`
* Новый тип объекта наследовать от `GameObject`. В конструкторе все инициализации. Для начальной логики переопределить `Start()`. Для логики каждый кадр переопределить `Update()`
