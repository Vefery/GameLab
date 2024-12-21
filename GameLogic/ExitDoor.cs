using MazeGame.GameLogic.Collider;
using MazeGame.Utils;
using OpenTK.Mathematics;

namespace MazeGame.GameLogic
{
    public class ExitDoor : GameObject, IRenderable
    {
        public Mesh mesh { get; private set; }
        private GameObject player;
        private AudioObject AudioDoor = new AudioObject();
        public ExitDoor()
        {
            mesh = new Mesh(MainWindow.assetsPath + "Models/ExitDoor.model", MainWindow.assetsPath + "Textures/Exit.png");
            mesh.texId = 4;
            player = MainLogic.gameObjects.OfType<Player>().Single();
            string assetsPath = AppDomain.CurrentDomain.BaseDirectory + "/../../../Assets/";
            AudioDoor.LoadAudio(assetsPath, "Door");
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            if (Vector3.Distance(position, player.position) < 2)
            {
                AudioDoor.PlayAudio(looped:false);
                MainLogic.finishFlag = true;
            }  
        }
    }
}
