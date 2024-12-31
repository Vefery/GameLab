using OpenTK.Mathematics;
using Silk.NET.OpenGL;

namespace AvaloniaGame.GameLogic
{
    public abstract class GameObject
    {
        public Vector3 position {  get; set; }
        public Vector3 eulerRotation { get; set; }
        public Vector3 radianRotation { get => new Vector3(MathHelper.DegreesToRadians(eulerRotation.X), MathHelper.DegreesToRadians(eulerRotation.Y), MathHelper.DegreesToRadians(eulerRotation.Z));  }

        public abstract void Update(float deltaTime);
        public abstract void Start(GL gl);

        public GameObject(GL gl)
        {
            eulerRotation = Vector3.Zero;
        }
        public GameObject(Vector3 position)
        {
            eulerRotation = Vector3.Zero;
            this.position = position;
        }
    }
}
