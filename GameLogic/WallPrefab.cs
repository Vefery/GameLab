namespace MazeGame.GameLogic
{
    public class WallPrefab : GameObject, IRenderable
    {
        public Mesh mesh {  get; private set; }
        public WallPrefab()
        {
            mesh = new Mesh(MainWindow.assetsPath + "Models/Wall.model");
        }
    }
}
