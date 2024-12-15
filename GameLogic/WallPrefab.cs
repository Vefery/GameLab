using MazeGame.GameLogic.Collider;
using OpenTK.Mathematics;

namespace MazeGame.GameLogic
{
    public class WallPrefab : GameObject, IRenderable
    {
        public Mesh mesh {  get; private set; }
        public BoxCollider collision { get; private set; }
        public WallPrefab()
        {
            mesh = new Mesh(MainWindow.assetsPath + "Models/Wall.model");
            collision = new BoxCollider(new List<Vector3>(mesh.verticesPos));
        }
        public override void Start()
        {
            this.collision.updateGlobalCollision(this.position, this.radianRotation);
        }
    }
}
