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
        public Vector3 eulerRotation;

        public virtual void Awake() { }
        public virtual void Start() { }
        public virtual void Update(float deltaTime) { }

        public GameObject()
        {
            eulerRotation = Vector3.Zero;
            MainLogic.OnAwakeGlobal += Awake;
            MainLogic.OnStartGlobal += Start;
            MainLogic.OnUpdateGlobal += Update;
        }
        public GameObject(Vector3 position)
        {
            eulerRotation = Vector3.Zero;
            this.position = position;
            MainLogic.OnAwakeGlobal += Awake;
            MainLogic.OnStartGlobal += Start;
            MainLogic.OnUpdateGlobal += Update;
        }
    }
}
