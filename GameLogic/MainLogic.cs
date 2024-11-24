using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MazeGame.GameLogic
{
    public static class MainLogic
    {
        public static event Action? OnAwakeGlobal;
        public static event Action? OnStartGlobal;
        public static event Action<float>? OnUpdateGlobal;

        public static List<GameObject> gameObjects = [];
        public static List<GameObject> renderables = [];
        public static KeyboardState? keyboardState;

        public static void InitializeScene()
        {
            gameObjects.Add(new Maze());

            OnAwakeGlobal?.Invoke();
            OnStartGlobal?.Invoke();

            renderables = gameObjects.Where(o => o is IRenderable).ToList();
        }
        public static void RaiseUpdate(float deltaTime)
        {
            OnUpdateGlobal?.Invoke(deltaTime);
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