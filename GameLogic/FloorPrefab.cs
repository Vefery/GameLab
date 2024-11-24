namespace MazeGame.GameLogic
{
    public class FloorPrefab : GameObject, IRenderable
    {
        public Mesh mesh { get; private set; }
        public FloorPrefab()
        {
            mesh = new Mesh(MainWindow.assetsPath + "Models\\Floor.model");
        }
    }
}
