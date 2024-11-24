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
   * - [ ] Считывание моделек из .obj
   * - [ ] Отрисовка фрагментным шейдером
   * - [ ] Сделать источник света (конкретно spotlight для фонарика игроку) 
2. - [x] Сделать модельки окружения
3. - [ ] Реализовать цикл игровой логики
4. - [x] Сделать алгоритм генерации лабиринта
5. - [ ] Реализовать просчет коллизий (будут только параллелепипедные коллайдеры)
6. - [ ] Реализовать контроллер игрока (для начала перемещение)
7. - [ ] Найти текстурки для моделек пола и стен (я думаю пиксельные подойдут)
8. - [ ] Сделать какое-нибудь базовое меню с выходом и выбором сложности
9. - [ ] Если останутся силы и время можно и звуки шагов и эмбиент прикрутить

## Архитектура игры
Предлагаю взять архитектуру из Unity и не париться:<br>
* Каждый объект почкуется от базового класса GameObject
* GameObject у нас будет иметь:
    * Абсолютные координаты
    * Абстракстные методы Awake, Start, Update (Возможно обойдемся и без Awake)
* При создании и запуске сцены у всех объектов вызываются Awake, Start
* Цикл игровой логики (каждый кадр вызывается) такой:
    1. Просчет коллизий
    2. Регистрация input'а (клавы и мыши у нас)
    3. Update у всех объектов (там логика самих объектов выполняется)
    4. Рендер графики
* Для цикла скорее всего удобнее хранить все объекты в одном списке и бегать по нему, вызывая функции
* Для специальных объектов (с коллизией например) наверное удобно будет сделать интерфейсы и через List.Select выбирать их из общего списка для выполнения логики конкретно с ними