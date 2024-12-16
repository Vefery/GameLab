using MazeGame.GameLogic.Collider;
using OpenTK.Mathematics;

namespace MazeGame.GameLogic
{
    public class FloorPrefab : GameObject, IRenderable
    {
        public Mesh mesh { get; private set; }
        public BoxCollider collision { get; private set; }
        public FloorPrefab()
        {
            mesh = new Mesh(MainWindow.assetsPath + "Models/Floor.model", MainWindow.assetsPath + "Textures/Floor.png");
            mesh.texId = 1;
            collision = new BoxCollider(new List<Vector3>(mesh.verticesPos));
        }
        public override void Start()
        {
            this.collision.updateGlobalCollision(this.position, this.radianRotation);
        }
    }
}
