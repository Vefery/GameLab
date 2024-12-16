using MazeGame.Utils;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MazeGame.GameLogic
{
    public static class MainLogic
    {
        public static event Action? OnFinished;
        public static List<GameObject> gameObjects = [];
        public static List<IRenderable> renderables { get { return gameObjects.OfType<IRenderable>().ToList(); } }
        public static KeyboardState keyboardState;
        public static MouseState mouseState;
        public static bool finishFlag = false;

        public static void InitializeScene()
        {
            gameObjects.Add(new Maze());
        }
        public static Player InitializePlayer()
        {
            Player _player = new Player(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1, 5, 1), 3, 4, 0.4f);
            gameObjects.Add(_player);

            return _player;
        }
        public static Player ReloadLevel()
        {
            gameObjects.Clear();

            var player = InitializePlayer();
            InitializeScene();
            return player;
        }
        public static void CallUpdate(float deltaTime)
        {
            if (finishFlag)
            {
                OnFinished?.Invoke();
                finishFlag = false;
                return;
            }
            foreach (var gameObject in gameObjects)
                gameObject.Update(deltaTime);
        }
        public static T Instantiate<T>(Vector3 position, Vector3 rotation) where T : GameObject, new()
        {
            T newObject = new();
            newObject.position = position;
            newObject.eulerRotation = rotation;
            gameObjects.Add(newObject);
            newObject.Start();
            return newObject;
        }
        public static T Instantiate<T>(Vector3 position) where T : GameObject, new()
        {
            T newObject = new();
            newObject.position = position;
            newObject.eulerRotation = Vector3.Zero;
            gameObjects.Add(newObject);
            newObject.Start();
            return newObject;
        }
    }
}