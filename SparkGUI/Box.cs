using System;
using System.Collections.Generic;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SparkGUI
{
    class Box : Widget, Orientable
    {
        public struct Schema ()
        {
            public Margin Margin;
            public float MinWidth = 100;
            public float MinHeight = 50;
            public Orientation Orientation = Orientation.VERTICAL;
            public float Spacing = 0;
            public Color4 BgColor = new(1f, 1f, 1f, 0f);
            public Widget[] Children;
        }
        
        public Box(Schema schema)
        {
            ContentBounds = new(0, 0, schema.MinWidth, schema.MinHeight);
            BgColor = schema.BgColor;
            Orientation = schema.Orientation;
            Spacing = schema.Spacing;
            Margin = schema.Margin;
            
            foreach (var w in schema.Children)
            {
                AddChild(w);
            }
            
            _remSpacing();
        }

        public Box(out Box binding, Schema schema) : this(schema)
        {
            binding = this;
        }

        public Color4 BgColor;
        private List<Widget> children = [];
        // private int _spacing = 0;
        public float Spacing { get; private set; }
        public Orientation Orientation { get; set; }
        override public Vector2 Position {
            set
            {
                _childrenSize = 0;
                ContentBounds.PlaceAt(
                    value.X + Margin.Start,
                    value.Y + Margin.Top
                );
                Status = Status.POSITIONED;

                foreach (var c in children) {
                    _placeChild(c);
                }

                _remSpacing();
            } 
            get => new(
                ContentBounds.X1 - Margin.Start,
                ContentBounds.Y1 - Margin.Top
            );
        }

        private float _childrenSize;

        public override void Render()
        {
            base.Render();
            
            if (BgColor.A != 0)
            {
                Core.DrawTrianglesFan(
                    BgColor,
                    ContentBounds.FlattenAs3D()
                );
            }
            
            foreach (var wid in children)
            {
                wid.Render();
            }
        }
    
        public override bool HandleClick(MouseButtonEventArgs e)
        {
            bool capture = false;
            
            foreach (var c in children)
            {
                capture |= c.HandleClick(e);
            }
            
            if (capture)
            {
                return true;
            }
            
            if (e.Button == MouseButton.Left
                && e.Action == InputAction.Press
                && ContentBounds.Contains(Core._gameWindow.MousePosition))
            {
                return true;
            }

            return false;
        }
        
        public override bool HandleMotion(MouseMoveEventArgs e)
        {
            bool capture = false;
            
            foreach (var c in children)
            {
                capture |= c.HandleMotion(e);
            }
            
            if (capture)
            {
                return true;
            }
            
            if (ContentBounds.Contains(Core._gameWindow.MousePosition))
            {
                return true;
            }
    
            return false;
        }
        
        void AddChild(Widget widget)
        {
            _addSpacing();
            _placeChild(widget);
            _remSpacing();

            children.Add(widget);
        }
        
        private void _placeChild(Widget widget)
        {
            var sp = Position;
            Vector2 newPos;
            
            switch (Orientation)
            {
                case Orientation.VERTICAL:
                    newPos = new Vector2(
                        sp.X + Margin.Start,
                        sp.Y + Margin.Top + _childrenSize
                    );
                    widget.Position = newPos;

                    float childOrt = widget.Width;
                    _childrenSize += widget.Height + Spacing;

                    ContentBounds = new(
                        ContentBounds.X1, ContentBounds.Y1,
                        width: Math.Max(childOrt, ContentBounds.Width),
                        height: Math.Max(_childrenSize, ContentBounds.Height)
                    );

                    break;
                case Orientation.HORIZONTAL:
                    newPos = new Vector2(
                        sp.X + Margin.Start + _childrenSize,
                        sp.Y + Margin.Top
                    );
                    widget.Position = newPos;

                    childOrt = widget.Height;
                    _childrenSize += widget.Width + Spacing;

                    ContentBounds = new(
                        ContentBounds.X1, ContentBounds.Y1,
                        Math.Max(_childrenSize, ContentBounds.Width),
                        Math.Max(childOrt, ContentBounds.Height)
                    );

                    break;
            }
        }
        
        // HACK: убрать отступ за последним виджетом
        void _remSpacing()
        {
            _childrenSize -= Spacing;
    
            switch (Orientation) {
                case Orientation.VERTICAL:
                    ContentBounds.Y2 -= Spacing;
                    break;
                case Orientation.HORIZONTAL:
                    ContentBounds.X2 -= Spacing;
                    break;
            }
        }
    
        // HACK: добавить отступ за последним виджетом
        void _addSpacing()
        {
            _childrenSize += Spacing;
    
            switch (Orientation) {
                case Orientation.VERTICAL:
                    ContentBounds.Y2 += Spacing;
                    break;
                case Orientation.HORIZONTAL:
                    ContentBounds.X2 += Spacing;
                    break;
            }
        }
    }
}
