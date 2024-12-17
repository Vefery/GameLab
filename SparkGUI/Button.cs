using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SparkGUI
{
    class Button : Widget
    {
        public struct Schema
        {
            public Margin Margin;
            public string Text = "";
            public Color4 TextColor = new(0.6f, 0.6f, 0.6f, 1);
            public Color4 BgColor = new(0.6f, 0.6f, 0.6f, 1);
            public float MinWidth = 100;
            public float MinHeight = 50;
            public System.Action<MouseButtonEventArgs> ClickedCallback;

            public Schema() {}
        }
        
        public Button(Schema schema)
        {
            Margin = schema.Margin;
            _label = new Label(new(){
                BgColor = new Color4(0f, 0f, 0f, 0f),
                Text = schema.Text,
                TextColor = schema.TextColor,
            });
            BgColor = schema.BgColor;
            ContentBounds = new(0, 0, schema.MinWidth, schema.MinHeight);
            ClickedEvent += schema.ClickedCallback;
        }
    
        public Button(out Button binding, Schema schema) : this(schema)
        {
            binding = this;
        }
    
        public Color4 BgColor;
        public event System.Action<MouseButtonEventArgs> ClickedEvent;
        private Label _label;
        private bool _hovered = false;

        public override Vector2 Position {
            get => base.Position;
            set {
                base.Position = value;
                float x = (ContentBounds.Width -_label.Width)/2;
                float y = (ContentBounds.Height -_label.Height)/2;

                if (x < 0 || y < 0)
                {
                    throw new Exception("WARNING: Label is bigger than it's parent button");
                }
                _label.Position = new Vector2(
                    ContentBounds.X1 + x,
                    ContentBounds.Y1 + y
                );
            }
        }

        public override bool HandleClick(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left
                && e.Action == InputAction.Press
                && ContentBounds.Contains(Core._gameWindow.MousePosition))
            {
                ClickedEvent?.Invoke(e);
                return true;
            }
            return false;
        }
        
        public override bool HandleMotion(MouseMoveEventArgs e)
        {
            if (ContentBounds.Contains(e.Position))
            {
                _hovered = true;
                return true;
            } else {
                _hovered = false;
                return false;
            }
        }

        public override void Render()
        {
            base.Render();
            
            var Bg = BgColor;
            if (_hovered) {
                Bg.R -= 0.2f;
                Bg.B -= 0.2f;
            }

            Core.DrawTrianglesFan(
                Bg,
                ContentBounds.FlattenAs3D()
            );

            _label.Render();
        }
    }
}
