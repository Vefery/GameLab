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
        public static event Action? OnUpdateGlobal;

        public MainLogicLoop()
        {
            OnAwakeGlobal?.Invoke();
            OnStartGlobal?.Invoke();

            GameLoop();
        }

        public void GameLoop()
        {
            while (true)
            {
                // Тут логика
            }
        }
    }
}
