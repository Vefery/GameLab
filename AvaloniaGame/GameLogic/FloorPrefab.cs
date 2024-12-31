using System.Collections.Generic;

using OpenTK.Mathematics;
using Silk.NET.OpenGL;

using AvaloniaGame.GameLogic.Collider;

namespace AvaloniaGame.GameLogic
{
    public class FloorPrefab : GameObject, IRenderable
    {
        public Mesh mesh { get; private set; }
        public BoxCollider collision { get; private set; }
        public FloorPrefab(GL gl) : base(gl)
        {
            mesh = new Mesh(gl, "avares://AvaloniaGame/Assets/" + "Models/Floor.model", "avares://AvaloniaGame/Assets/" + "Textures/Floor.png");
            mesh.texId = 1;
            collision = new BoxCollider(new List<Vector3>(mesh.verticesPos));
        }
        public override void Start(GL gl)
        {
            this.collision.updateGlobalCollision(this.position, this.radianRotation);
        }

        public override void Update(float deltaTime)
        {
            ;
        }
    }
}
