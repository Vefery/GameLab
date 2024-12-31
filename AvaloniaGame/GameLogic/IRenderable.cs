using OpenTK.Mathematics;

namespace AvaloniaGame.GameLogic
{
    public interface IRenderable
    {
        Mesh mesh { get; }
        public Vector3 position { get; }
        public Vector3 radianRotation { get; }
    }
}
