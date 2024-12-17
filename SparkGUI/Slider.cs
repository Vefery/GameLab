using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SparkGUI
{
    class Slider : Widget
    {
        public class Schema ()
        {
            public Margin Margin;
            
            public Color4 BgColor = new(0.6f, 0.6f, 0.6f, 1f);
            public Color4 FgColor = new(0, 0.6f, 0, 1f);
            public float MinWidth = 100;
            public float MinHeight = 50;
            public Action<float> ValueCallback;
        }
    
        public Slider(Schema schema)
        {
            Margin = schema.Margin;
            BgColor = schema.BgColor;
            FgColor = schema.FgColor;
            ContentBounds = new(0, 0, schema.MinWidth, schema.MinHeight);
            ValueEvent += schema.ValueCallback;
        }
    
        public Slider(out Slider binding, Schema schema) : this(schema)
        {
            binding = this;
        }
        
        public Color4 BgColor;
        public Color4 FgColor;
        public event Action<float> ValueEvent;
        public float Value { get; private set; }
        private int? _loopID = null;
        private bool _hovered = false;

        public override bool HandleClick(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left
                && e.Action == InputAction.Press
                && ContentBounds.Contains(Core._gameWindow.MousePosition))
            {
                bool sliderUpdate(DateTime _)
                {
                    var pos = Core._gameWindow.MousePosition;
                    var x = Math.Clamp(pos.X, ContentBounds.X1, ContentBounds.X2);

                    Value = (x - ContentBounds.X1) / ContentBounds.Width;
                    ValueEvent?.Invoke(Value);
                    return true;
                }

                _loopID = Core.LoopAdd(sliderUpdate);
                return true;
            }
            else if (e.Button == MouseButton.Left && e.Action == InputAction.Release)
            {
                if (_loopID != null)
                {
                    Core.LoopRemove(_loopID.Value);
                    _loopID = null;
                }
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
                Bg.R += 0.1f;
            }
            Core.DrawTrianglesFan(
                Bg,
                ContentBounds.FlattenAs3D()
            );

            var valueBar = new Rect(ContentBounds);
            valueBar.X2 = valueBar.X1 + valueBar.Width * Value;
            Core.DrawTrianglesFan(
                FgColor,
                valueBar.FlattenAs3D()
            );
        }
    }
}
