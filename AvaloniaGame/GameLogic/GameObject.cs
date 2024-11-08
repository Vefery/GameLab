using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaGame.GameLogic
{
    public abstract class GameObject
    {
        public Vector3 position;

        public abstract void Awake();
        public abstract void Start();
        public abstract void Update();

        public GameObject()
        {
            MainLogicLoop.OnAwakeGlobal += Awake;
            MainLogicLoop.OnStartGlobal += Start;
            MainLogicLoop.OnUpdateGlobal += Update;
        }
        public GameObject(Vector3 position)
        {
            this.position = position;
            MainLogicLoop.OnAwakeGlobal += Awake;
            MainLogicLoop.OnStartGlobal += Start;
            MainLogicLoop.OnUpdateGlobal += Update;
        }
    }
}
