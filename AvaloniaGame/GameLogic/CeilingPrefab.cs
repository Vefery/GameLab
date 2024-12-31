using System.Collections.Generic;

using Silk.NET.OpenGL;
using OpenTK.Mathematics;

using AvaloniaGame.GameLogic.Collider;

namespace AvaloniaGame.GameLogic
{
    public class CeilingPrefab : GameObject, IRenderable
    {
        public Mesh mesh { get; private set; }
        public BoxCollider collision { get; private set; }
        public CeilingPrefab(GL gl) : base(gl)
        {
            mesh = new Mesh(gl, "avares://AvaloniaGame/Assets/" + "Models/Ceiling.model", "avares://AvaloniaGame/Assets/" + "Textures/Ceiling.png");
            mesh.texId = 3;
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
