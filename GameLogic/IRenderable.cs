using OpenTK.Mathematics;

namespace MazeGame.GameLogic
{
    public interface IRenderable
    {
        Mesh mesh { get; }
        public Vector3 position { get; }
        public Vector3 eulerRotation { get; }
    }
}