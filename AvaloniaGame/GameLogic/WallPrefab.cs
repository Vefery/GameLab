using System.Collections.Generic;

using OpenTK.Mathematics;
using Silk.NET.OpenGL;

using AvaloniaGame.Views;
using AvaloniaGame.GameLogic.Collider;

namespace AvaloniaGame.GameLogic
{
    public class WallPrefab : GameObject, IRenderable
    {
        public Mesh mesh {  get; private set; }
        public BoxCollider collision { get; private set; }
        public WallPrefab(GL gl) : base(gl)
        {
            mesh = new Mesh(gl, MainWindow.assetsPath + "Models/Wall.model", MainWindow.assetsPath + "Textures/Wall.png");
            mesh.texId = 2;
            collision = new BoxCollider(new List<Vector3>(mesh.verticesPos));
        }

        public override void Update(float deltaTime)
        {
            ;
        }

        public override void Start(GL gl)
        {
            collision.updateGlobalCollision(this.position, this.radianRotation);
        }
    }
}
