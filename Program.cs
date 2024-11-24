using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;
using MazeGame.Utils;
using MazeGame.GameLogic;
class MainWindow : GameWindow
{
    private const int frameRate = 60; // Target frame rate
    private ShaderProgram _shaderProgram;
    private Mesh _cube, _sphere;
    private Camera _camera;

    public static string assetsPath = AppDomain.CurrentDomain.BaseDirectory + "\\..\\..\\..\\Assets\\";

    private Matrix4 projectionMatrix, viewMatrix, modelMatrix;

    public MainWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) { }

    protected override void OnLoad()
    {
        base.OnLoad();

        CursorState = CursorState.Grabbed;

        InitializeGraphics();
        MainLogic.InitializeScene();
    }

    private void InitializeGraphics()
    {
        // Устанавливаем параметры OpenGL
        GL.Enable(EnableCap.DepthTest);

        // Загрузка шейдера
        _shaderProgram = new ShaderProgram(assetsPath + "Shaders\\Vertex.glsl", assetsPath + "Shaders\\Fragment.glsl");

        // Инициализация камеры
        _camera = new Camera(new Vector3(0.0f, 5.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
        MainLogic.gameObjects.Add(_camera);

        // Устанавливаем цвет очистки
        GL.ClearColor(0.2f, 0.2f, 0.5f, 1.0f);
    }
    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);

        projectionMatrix = _camera.getProjectionMatrix((float)ClientSize.X / ClientSize.Y);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        // Это не трограть. Это должно быть до отрисоки кадра. 
        _shaderProgram.Use();

        // Устанавливаем матрицы камеры
        viewMatrix = _camera.GetMatrix();

        // Не трограть 
        _shaderProgram.SetUniform("view", viewMatrix);
        _shaderProgram.SetUniform("projection", projectionMatrix);

        foreach (var obj in MainLogic.renderables)
            DrawObject(obj.mesh, obj.position, obj.radianRotation, Vector3.One);

        SwapBuffers();
    }
    private void DrawObject(Mesh mesh, Vector3 position, Vector3 eulerRotation, Vector3 scale)
    {
        modelMatrix = Matrix4.Identity;
        modelMatrix = modelMatrix * Matrix4.CreateRotationX(eulerRotation.X);
        modelMatrix = modelMatrix * Matrix4.CreateRotationY(eulerRotation.Y);
        modelMatrix = modelMatrix * Matrix4.CreateRotationZ(eulerRotation.Z);
        modelMatrix = modelMatrix * Matrix4.CreateTranslation(position);
        modelMatrix = modelMatrix * Matrix4.CreateScale(scale);


        _shaderProgram.SetUniform("model", modelMatrix);
        mesh.draw();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        // HandleCollision();
        MainLogic.keyboardState = KeyboardState;
        MainLogic.mouseState = MouseState;
        MainLogic.CallUpdate((float)args.Time);
    }

    protected override void OnUnload()
    {
        base.OnUnload();

        // Очистка ресурсов
        //_shaderProgram.Dispose();
        //_cube.Dispose();
        //_pyramid.Dispose();
        //_sphere.Dispose();
    }

    public static void Main(string[] args)
    {
        var t = AppDomain.CurrentDomain.BaseDirectory + "/Assets/Models";
        var gameWindowSettings = new GameWindowSettings
        {
            //RenderFrequency = 60.0,
            UpdateFrequency = frameRate
        };
        
        var nativeWindowSettings = new NativeWindowSettings
        {
            ClientSize = new Vector2i(1280, 720),
            Title = "3D Scene with OpenTK"
        };

        using var window = new MainWindow(gameWindowSettings, nativeWindowSettings);
        window.Run();
    }
}
