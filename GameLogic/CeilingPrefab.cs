using MazeGame.GameLogic.Collider;
using OpenTK.Mathematics;

namespace MazeGame.GameLogic
{
    public class CeilingPrefab : GameObject, IRenderable
    {
        public Mesh mesh { get; private set; }
        public BoxCollider collision { get; private set; }
        public CeilingPrefab()
        {
            mesh = new Mesh(MainWindow.assetsPath + "Models/Ceiling.model", MainWindow.assetsPath + "Textures/Ceiling.png");
            mesh.texId = 3;
            collision = new BoxCollider(new List<Vector3>(mesh.verticesPos));
        }
        public override void Start()
        {
            this.collision.updateGlobalCollision(this.position, this.radianRotation);
        }
    }
}
