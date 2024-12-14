using OpenTK.Mathematics;
using System;

namespace SparkGUI
{
    class Rect
    {
        public float X1 { get; set; }
        public float Y1 { get; set; }
        public float X2 { get; set; }
        public float Y2 { get; set; }
        public float Width { get => X2 - X1; }
        public float Height { get => Y2 - Y1; }
        
        public Rect(float x, float y, float width, float height) {
            
            X1 = x;
            Y1 = y;
            X2 = x + width;
            Y2 = y + height;
        }
        
        Rect() {
            X1 = 0;
            Y1 = 0;
            X2 = 0;
            Y2 = 0;
        }

        public Rect(Rect clonee)
            : this(clonee.X1, clonee.Y1, clonee.Width, clonee.Height) {}

        public bool Contains(float x, float y) {
            return X1 <= x && x <= X2 &&
                   Y1 <= y && y <= Y2;
        }
        
        public bool Contains(Vector2 vec) {
            return Contains(vec.X, vec.Y);
        }
        
        public float[] FlattenAs3D()
        {
            return [
                X1, Y1, 0,
                X1, Y2, 0,
                X2, Y2, 0,
                X2, Y1, 0,
            ];
        }
        
        public void PlaceAt(float x, float y) {
            float dx = x - X1;
            float dy = y - Y1;
            X1 = x;
            X2 += dx;
            Y1 = y;
            Y2 += dy;
        }
    };
}
