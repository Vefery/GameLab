using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using LoopFunc = System.Func<System.DateTime, bool>;

// Основной файл библиотеки.
namespace SparkGUI
{
    // функция, которая должна выполняться в цикле
    // public delegate bool LoopFunc(DateTime lastTick);

    static class Core
    {
        private static Dictionary<int, LoopFunc> _loopFuncs = new();
        private static int _lastFuncID = 0;
        private static DateTime _lastTick = DateTime.Now;

        private static MazeGame.Utils.Shader _solid;
        private static int _vertexBufferObject;
        private static int _vertexArrayObject;
        private static int _elementBufferObject;
        internal static GameWindow _gameWindow;

        // должна быть вызвана пользователем библиотеке перед использованием
        // любых функций из библиотеки
        static public void Init(GameWindow win)
        {
            _vertexBufferObject = GL.GenBuffer();
            _vertexArrayObject = GL.GenVertexArray();
            _elementBufferObject = GL.GenBuffer();
            _gameWindow = win;

            var projectRoot = "../../../";
            _solid = new(projectRoot + "Assets/Shaders/solid.vert", projectRoot + "Assets/Shaders/solid.frag");
            _solid.Use();
        }
        
        static internal void DrawTrianglesFan(Color4 color, ReadOnlySpan<float> vertices)
        {
            if (vertices.Length % 3 != 0)
            {
                throw new ArgumentException("vertices.Length % 3 != 0");
            }
        
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                vertices.Length * sizeof(float),
                vertices.ToArray(),
                BufferUsageHint.DynamicDraw
            );

            GL.BindVertexArray(_vertexArrayObject);
            var loc = _solid.GetAttribLocation("aPosition");
            GL.VertexAttribPointer(
                loc, 3,
                VertexAttribPointerType.Float,
                false,
                3 * sizeof(float),
                0
            );
            GL.EnableVertexAttribArray(loc);
            _solid.Use();
            _solid.SetVector4("aColor", (Vector4)color);

            var proj = Matrix4.CreateOrthographicOffCenter(
                0, _gameWindow.ClientSize.X,
                _gameWindow.ClientSize.Y, 0,
                -1, 1
            );
            _solid.SetMatrix4("aProj", proj);

            GL.DrawArrays(PrimitiveType.TriangleFan, 0, vertices.Length / 3);
        }
        
        static internal void DrawQuads(
            Color4 color,
            ReadOnlySpan<float> vertices,
            float x0, float y0,
            float scale = 1)
        {
            if (vertices.Length % 12 != 0)
            {
                throw new ArgumentException("vertices.Length % 12 != 0");
            }
        
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                vertices.Length * sizeof(float),
                vertices.ToArray(),
                BufferUsageHint.DynamicDraw
            );

            GL.BindVertexArray(_vertexArrayObject);
            var loc = _solid.GetAttribLocation("aPosition");
            GL.VertexAttribPointer(
                loc, 3,
                VertexAttribPointerType.Float,
                false,
                3 * sizeof(float),
                0
            );

            uint[] indices = new uint[vertices.Length / 2];
            for(uint i = 0; i < vertices.Length/12; i += 1)
            {
                indices[i*6 + 0] = 0 + i*4;
                indices[i*6 + 1] = 1 + i*4;
                indices[i*6 + 2] = 2 + i*4;
                indices[i*6 + 3] = 0 + i*4;
                indices[i*6 + 4] = 2 + i*4;
                indices[i*6 + 5] = 3 + i*4;
            }

            GL.BindBuffer(
                BufferTarget.ElementArrayBuffer,
                _elementBufferObject
            );
            GL.BufferData(
                BufferTarget.ElementArrayBuffer,
                indices.Length * sizeof(uint),
                indices,
                BufferUsageHint.StaticDraw
            );

            GL.EnableVertexAttribArray(loc);
            _solid.Use();
            _solid.SetVector4("aColor", (Vector4)color);

            var proj = Matrix4.Identity;
            proj *= Matrix4.CreateScale(scale);
            proj *= Matrix4.CreateTranslation(x0, y0, 0);
            proj *= Matrix4.CreateOrthographicOffCenter(
                0, _gameWindow.ClientSize.X,
                _gameWindow.ClientSize.Y, 0,
                -1, 1
            );
            _solid.SetMatrix4("aProj", proj);

            GL.DrawElements(
                PrimitiveType.Triangles,
                indices.Length,
                DrawElementsType.UnsignedInt,
                0
            );
        }
        
        public static void Tick()
        {
            // спарк - двумерная библиотека
            GL.Disable(EnableCap.DepthTest);
            // должна ли функция продолжать вызываться
            var toRemove = new List<int>(32);
            bool cont;
            foreach (var a in _loopFuncs)
            {
                cont = a.Value(_lastTick);
                if (!cont)
                {
                    toRemove.Add(a.Key);
                }
            }
            foreach (var i in toRemove)
            {
                _loopFuncs.Remove(i);
            }
            // ... но во внешнем коде нужна проверка на глубину
            GL.Enable(EnableCap.DepthTest);
        }
        
        // TODO: исправить "дырки" в присваивании ID
        internal static int LoopAdd(LoopFunc func)
        {
            Console.WriteLine($"Added {_lastFuncID}");
            int res = _lastFuncID;
            _lastFuncID++;
            _loopFuncs[res] = func;

            return res;
        }
        
        internal static void LoopRemove(int id)
        {
            Console.WriteLine($"Removed  {id}");
            _loopFuncs.Remove(id);
        }
        
        internal static void AddMouse(System.Action<MouseButtonEventArgs> func) {
            _gameWindow.MouseUp += func;
        }
    }
}
