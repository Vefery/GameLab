using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SparkGUI
{
    class Label : Widget
    {
        public struct Schema ()
        {
            public Margin Margin;
            public float MinWidth = 100;
            public float MinHeight = 50;
            public Color4 TextColor = new(0f, 0f, 0f, 1f);
            public Color4 BgColor = new(1f, 1f, 1f, 0f);
            public string Text = "";
        }
        
        public Label(Schema schema)
        {
            Margin = schema.Margin;
            float width = Math.Max(
                Stb.EasyFont.Width(schema.Text) * 2f,
                schema.MinWidth
            );
            float height = Math.Max(
                Stb.EasyFont.Height(schema.Text) * 2f,
                schema.MinHeight
            );

            ContentBounds = new(0, 0, width, height);
            TextColor = schema.TextColor;
            BgColor = schema.BgColor;
            Text = schema.Text;
        }

        public Label(out Label binding, Schema schema) : this(schema)
        {
            binding = this;
        }
        
        Color4 BgColor { get; }
        Color4 TextColor { get; }
        string Text { get; }

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
            
            if (TextColor.A == 0)
            {
                return;
            }

            float x0 = (Width - Stb.EasyFont.Width(Text) * 2) / 2;
            float y0 = (Height - Stb.EasyFont.Height(Text) * 2) / 2;

            float[] buf = new float[9999];
            var numQuads = Stb.EasyFont.Print(
                0,
                0,
                Text,
                null,
                buf
            );

            Core.DrawQuads(
                TextColor,
                new Span<float>(buf, 0, (int)numQuads * 4 * 3),
                ContentBounds.X1 + x0,
                ContentBounds.Y1 + y0,
                scale: 2
            );
        }
        
        public override bool HandleClick(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left
                && e.Action == InputAction.Press
                && ContentBounds.Contains(Core._gameWindow.MousePosition))
            {
                return true;
            }
            return false;
        }
    }
}
