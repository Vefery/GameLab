using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace SparkGUI
{
    // TODO: поле выделенной области?
    // оно пригодилось бы для виджетов, которые
    // хотят располагаться посередине или в конце некоторой
    // выделенной области
    struct Margin
    {
        public int End = 0;
        public int Top = 0;
        public int Start = 0;
        public int Bottom = 0;

        public Margin()
        {
        }
    };

    enum Status
    {
        CREATED,
        POSITIONED,
    }
    
    abstract class Widget
    {
        internal Status Status = Status.CREATED;
        
        virtual public Vector2 Position {
            set
            {
                ContentBounds.PlaceAt(
                    value.X + Margin.Start,
                    value.Y + Margin.Top
                );
                Status = Status.POSITIONED;
            } 
            get => new Vector2(
                ContentBounds.X1 - Margin.Start,
                ContentBounds.Y1 - Margin.Top
            );
        }
        public float Height {
            get => ContentBounds.Height + Margin.Top + Margin.Bottom;
        }
        public float Width {
            get => ContentBounds.Width + Margin.Start + Margin.End;
        }
        public Margin Margin { set; get; }

        protected Rect ContentBounds { get; set; } = new(0, 0, 100, 100);

        // возвращает true если нужно захватить нажатие
        public abstract bool HandleClick(MouseButtonEventArgs e);
        
        // только одна проверка, но всё равно рекомендуется вызывать
        // этот метод в дочерних классах 
        public virtual void Render() {
            if (Status != Status.POSITIONED) {
                throw new System.Exception(
                    "Попытка отрисовать неразмещённый виджет"
                );
            }
        }
    };
}
