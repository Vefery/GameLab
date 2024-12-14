namespace SparkGUI {
    enum Orientation {
        VERTICAL,
        HORIZONTAL,
    };
    // интерфейс виджета, имеющего направление
    // например контейнер может распологать в себе виджеты
    // горизонтально или вертикально
    interface Orientable {
        public Orientation Orientation { get; set; }
    };
}
