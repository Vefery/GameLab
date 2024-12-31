using System.Linq;

using OpenTK.Mathematics;
using Silk.NET.OpenGL;

using AvaloniaGame.Utils;

namespace AvaloniaGame.GameLogic
{
    public class ExitDoor : GameObject, IRenderable
    {
        public Mesh mesh { get; private set; }
        private GameObject player;
        private AudioObject AudioDoor = new AudioObject();
        public ExitDoor(GL gl) : base(gl)
        {
            mesh = new Mesh(gl, "avares://AvaloniaGame/Assets/" + "Models/ExitDoor.model", "avares://AvaloniaGame/Assets/" + "Textures/Exit.png");
            mesh.texId = 4;
            player = MainLogic.gameObjects.OfType<Player>().Single();
            AudioDoor.LoadAudio("Door");
        }

        public override void Update(float deltaTime)
        {
            if (Vector3.Distance(position, player.position) < 2)
            {
                AudioDoor.PlayAudio(looped: false);
                MainLogic.finishFlag = true;
            }
        }

        public override void Start(GL _)
        {
            ;
        }
    }
}
