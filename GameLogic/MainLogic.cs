using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MazeGame.GameLogic
{
    public static class MainLogic
    {
        public static List<GameObject> gameObjects = [];
        public static List<IRenderable> renderables { get { return gameObjects.OfType<IRenderable>().ToList(); } }
        public static KeyboardState keyboardState;
        public static MouseState mouseState;

        public static void InitializeScene()
        {
            gameObjects.Add(new Maze());
            //Instantiate<Room>(Vector3.Zero);
        }
        public static void CallUpdate(float deltaTime)
        {
            foreach (var gameObject in gameObjects)
                gameObject.Update(deltaTime);
        }
        public static T Instantiate<T>(Vector3 position, Vector3 rotation) where T : GameObject, new()
        {
            T newObject = new();
            newObject.position = position;
            newObject.eulerRotation = rotation;
            gameObjects.Add(newObject);
            return newObject;
        }
        public static T Instantiate<T>(Vector3 position) where T : GameObject, new()
        {
            T newObject = new();
            newObject.position = position;
            newObject.eulerRotation = Vector3.Zero;
            gameObjects.Add(newObject);
            return newObject;
        }
    }
}