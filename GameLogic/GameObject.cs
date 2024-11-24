using OpenTK.Mathematics;

namespace MazeGame.GameLogic
{
    public abstract class GameObject
    {
        public Vector3 position {  get; set; }
        public Vector3 eulerRotation { get; set; }

        public virtual void Update(float deltaTime) { }

        public GameObject()
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
