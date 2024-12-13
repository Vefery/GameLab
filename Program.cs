﻿using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using MazeGame.Utils;
using MazeGame.GameLogic;

class MainWindow : GameWindow
{
    private const int frameRate = 60; // Target frame rate
    private SpotlightShader _shaderProgram;

    /* Параметры освещения */
    private Spotlight _spotLightParam; // Параметры настройки освещения. Менаются в зависимости от положения камеры. 
    private OpenTK.Mathematics.Vector3 _globalAmbient;
    private Material Gold;

   
    private Camera _camera;

    public static string assetsPath = AppDomain.CurrentDomain.BaseDirectory + "/../../../Assets/";

    private Matrix4 projectionMatrix, viewMatrix, modelMatrix, norm_matrix;

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
        _shaderProgram = new SpotlightShader(assetsPath + "Shaders/Spotlight.vert", assetsPath + "Shaders/Spotlight.frag");

        // Инициализация камеры
        _camera = new Camera(new Vector3(0.0f, 5.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
        MainLogic.gameObjects.Add(_camera);

        // Устанавливаем цвет очистки
        GL.ClearColor(0f, 0f, 0f, 1.0f);
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
        _shaderProgram.SetMatrix4("view", viewMatrix);
        _shaderProgram.SetMatrix4("projection", projectionMatrix);

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
        
        norm_matrix = (modelMatrix*viewMatrix).Inverted();
        norm_matrix.Transpose();

        /* Настройки параметров освещения */
        _globalAmbient = new Vector3(0.3f, 0.3f, 0.3f);
        _spotLightParam.position = Vector3.Zero;
        _spotLightParam.direction = -Vector3.UnitZ;


        _spotLightParam.cutoffAngle = MathF.Cos(((float) Math.PI / 180.0f)*15.5f);
        _spotLightParam.outerCutoff = MathF.Cos(((float)Math.PI / 180.0f) * 45.0f);
        _spotLightParam.intensity = 1.0f;
        _spotLightParam.constant = 1.0f;
        _spotLightParam.linear = 0.09f;
        _spotLightParam.quadratic = 0.032f;
        _spotLightParam.color = new Vector3(1.0f, 1.0f, 1.0f);
      
        /* Вкинем материал */
        Gold.ambient = new Vector3(0.24725f, 0.1995f, 0.0745f);
        Gold.diffuse = new Vector3 (0.75164f, 0.60648f, 0.22648f);
        Gold.specular = new Vector3(0.62828f, 0.5558f, 0.36607f);
        Gold.shininess = 51.2f;


        _shaderProgram.SetMaterial("material", Gold);
        _shaderProgram.SetSpotlight("spotLight", _spotLightParam);
        _shaderProgram.SetVector3("globalAmbient", _globalAmbient);
        _shaderProgram.SetVector3("viewPos",_spotLightParam.position);
        _shaderProgram.SetMatrix4("model", modelMatrix);
        _shaderProgram.SetMatrix4("norm_matrix",norm_matrix);

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
