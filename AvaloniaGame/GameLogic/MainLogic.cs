using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using AvaloniaGame.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaGame.GameLogic
{
    public static class MainLogic
    {
        public static event Action? OnAwakeGlobal;
        public static event Action? OnStartGlobal;
        public static event Action<float>? OnUpdateGlobal;

        private const int frameRate = 60; // Target frame rate
        private static readonly TimeSpan FrameTime = TimeSpan.FromSeconds(1.0 / frameRate);
        private static DateTime _lastUpdateTime;
        private static OpenGLClass _glControl;
        public static List<GameObject> gameObjects = [];
        public static List<GameObject> renderables = [];

        public static void StartWork(OpenGLClass glControl)
        {
            _glControl = glControl;

            gameObjects.Add(new Maze());

            OnAwakeGlobal?.Invoke();
            OnStartGlobal?.Invoke();

            renderables = gameObjects.Where(o => o is IRenderable).ToList();
            //colliders = gameObjects.Where(o => o is ICollider).ToList();

            GameLoop();
        }
        async static void GameLoop()
        {
            _lastUpdateTime = DateTime.Now;

            while (true)
            {
                var now = DateTime.Now;
                var elapsed = now - _lastUpdateTime;

                if (elapsed >= FrameTime)
                {
                    float deltaTime = (float)elapsed.TotalSeconds;
                    OnUpdateGlobal?.Invoke(deltaTime);
                    _glControl.RenderFrame();
                    _lastUpdateTime = now;
                }

                await Task.Delay(1);
            }
        }
        public static T Instantiate<T>(Vector3 position, Vector3 rotation) where T : GameObject, new()
        {
            T newObject = new();
            newObject.position = position;
            newObject.eulerRotation = rotation;
            gameObjects.Add(newObject);
            return newObject;
        }
        public static T Instantiate<T>(Vector3 position) where T : GameObject, new()
        {
            T newObject = new();
            newObject.position = position;
            newObject.eulerRotation = Vector3.Zero;
            gameObjects.Add(newObject);
            return newObject;
        }
    }
}
