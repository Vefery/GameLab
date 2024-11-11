using Avalonia.OpenGL.Controls;
using AvaloniaGame.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaGame.GameLogic
{
    public class MainLogicLoop
    {
        public static event Action? OnAwakeGlobal;
        public static event Action? OnStartGlobal;
        public static event Action<float>? OnUpdateGlobal;

        private const int frameRate = 60; // Target frame rate
        private readonly TimeSpan FrameTime = TimeSpan.FromSeconds(1.0 / frameRate);
        private DateTime _lastUpdateTime;
        private OpenGLClass _glControl;

        public MainLogicLoop(OpenGLClass glControl)
        {
            _glControl = glControl;
            OnAwakeGlobal?.Invoke();
            OnStartGlobal?.Invoke();

            GameLoop();
        }

        async void GameLoop()
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
    }
}
