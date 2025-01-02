using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK.Mathematics;
using Silk.NET.OpenGL;

using AvaloniaGame.Utils;
using AvaloniaGame.Views;
using Avalonia.Controls;
using System.IO;

namespace AvaloniaGame.GameLogic
{
    public static class MainLogic
    {
        public static GL gl {set; private get;}
        public static event Action? OnFinished;
        public static List<GameObject> gameObjects = [];
        public static List<IRenderable> renderables
        {
            get
            {
                return gameObjects.OfType<IRenderable>().ToList();
            }
        }
        // public static KeyboardState keyboardState;
        // public static MouseState mouseState;
        public static bool finishFlag = false;
        public static int difficulty = 0;
        public static Control control;

        public static void InitializeScene()
        {
            gameObjects.Add(new Maze(gl));
        }
        public static Player InitializePlayer()
        {
            Player _player = new Player(gl, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1, 5, 1), 3, 4, 0.4f);
            control.KeyDown += _player.KeyDownHandler;
            control.KeyUp += _player.KeyUpHandler;
            control.PointerMoved += _player.PointerMovedHandler;
            gameObjects.Add(_player);

            return _player;
        }
        public static Player ReloadLevel()
        {
            
            gameObjects.Clear();

            var player = InitializePlayer();
            InitializeScene();
            return player;
        }
        public static void CallUpdate(float deltaTime)
        {
            if (finishFlag)
            {
                OnFinished?.Invoke();
                finishFlag = false;
                return;
            }
            foreach (var gameObject in gameObjects)
                gameObject.Update(deltaTime);
        }

        public static void OnCloseCleanUp(Object? sender, WindowClosingEventArgs e)
        {
            // ѕо хорошему надо удал€ть временные файлы, но они используютс€ libvlc и залочены
            //Directory.Delete("Temp", true);
        }

        public static T Register<T>(T gameObject, Vector3 position, Vector3 rotation)
        where T: GameObject
        {
            gameObject.position = position;
            gameObject.eulerRotation = rotation;
            gameObjects.Add(gameObject);
            gameObject.Start(gl);
            return gameObject;
        }

        public static T Register<T>(T gameObject, Vector3 position)
        where T: GameObject
        {
            Register(gameObject, position, Vector3.Zero);
            return gameObject;
        }
    }
}
