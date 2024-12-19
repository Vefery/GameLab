using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using MazeGame.Utils;
using MazeGame.GameLogic;
using StbImageSharp;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SparkGUI;

class MainWindow : GameWindow
{
    private const int frameRate = 60; // Target frame rate
    private SpotlightShader _shaderProgram;

    /* Параметры освещения */
    private Spotlight _spotLightParam; // Параметры настройки освещения. Менаются в зависимости от положения камеры. 
    private Vector3 _globalAmbient;
    private Material Material;
    private Player _player;

    public static string assetsPath = AppDomain.CurrentDomain.BaseDirectory + "/../../../Assets/";

    private Matrix4 projectionMatrix, viewMatrix, modelMatrix, norm_matrix;

    public MainWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) { }

    private SparkGUI.Toplevel toplevel;

    protected override void OnLoad()
    {
        base.OnLoad();

        CursorState = CursorState.Grabbed;

        InitializeGraphics();
        InitializeAudio();
        _player = MainLogic.InitializePlayer();
        MainLogic.InitializeScene();
        MainLogic.OnFinished += HandleGameFinish;

        // инициализация Spark должна происходить до любых остальных
        // обращений к этой библиотеке
        SparkGUI.Core.Init(this);

        var sideBox = new SparkGUI.Box(new () {
            BgColor = new(0, 0.7f, 0.5f, 1f),
            Spacing = 10,
            Children = [
                new SparkGUI.Label(new (){
                    Margin = new () {
                        Top = 10,
                        Start = 10,
                        End = 10,
                    },
                    MinWidth = 150,
                    TextColor = new(0f, 0f, 0f, 1f),
                    BgColor = new(1f, 1f, 1f, 1f),
                    Text = "Difficulty:",
                }),
                new SparkGUI.Button(new (){
                    Margin = new () {
                        Start = 30,
                        End = 10,
                    },
                    MinWidth = 150,
                    TextColor = new(0.9f, 0.9f, 0.9f, 1f),
                    BgColor = new(0.3f, 0f, 0.5f, 1f),
                    Text = "Easy",
                    ClickedCallback = _ => {
                        MainLogic.difficulty = 0;
                        MainLogic.finishFlag = true;
                    }
                }),
                new SparkGUI.Button(new (){
                    Margin = new () {
                        Start = 30,
                        End = 10,
                    },
                    MinWidth = 150,
                    TextColor = new(0.9f, 0.9f, 0.9f, 1f),
                    BgColor = new(0.3f, 0f, 0.5f, 1f),
                    Text = "Medium",
                    ClickedCallback = _ => {
                        MainLogic.difficulty = 1;
                        MainLogic.finishFlag = true;
                    }
                }),
                new SparkGUI.Button(new (){
                    Margin = new () {
                        Start = 30,
                        End = 10,
                    },
                    MinWidth = 150,
                    TextColor = new(0.9f, 0.9f, 0.9f, 1f),
                    BgColor = new(0.3f, 0f, 0.5f, 1f),
                    Text = "Hard",
                    ClickedCallback = _ => {
                        MainLogic.difficulty = 2;
                        MainLogic.finishFlag = true;
                    }
                }),
                new SparkGUI.Button(new (){
                    Margin = new () {
                        Top = 30,
                        Start = 10,
                        End = 10,
                        Bottom = 10,
                    },
                    MinWidth = 170,
                    TextColor = new(0.9f, 0.9f, 0.9f, 1f),
                    BgColor = new(0.3f, 0f, 0.5f, 1f),
                    Text = "Quit",
                    ClickedCallback = _ => {
                        Close();
                    }
                }),
                
            ]
        });
        toplevel = new SparkGUI.Toplevel(sideBox);
        toplevel.Active = false;
    }
    private void HandleGameFinish()
    {
        _player = MainLogic.ReloadLevel();
    }
    private void InitializeGraphics()
    {
        // Устанавливаем параметры OpenGL
        GL.Enable(EnableCap.DepthTest);

        // Загрузка шейдера
        _shaderProgram = new SpotlightShader(assetsPath + "Shaders/Spotlight.vert", assetsPath + "Shaders/Spotlight.frag");

        // Устанавливаем цвет очистки
        GL.ClearColor(0.6f, 0.6f, 0.6f, 1.0f);

        StbImage.stbi_set_flip_vertically_on_load(1); // приводит текстуры к формату opengl
        InitializeTexturePool();
    }

    private void InitializeAudio()
    {
        AudioPlayer.LoadAudio(assetsPath);
        AudioDoor.LoadAudio(assetsPath);
        AudioAmbient.PlayAmbient(assetsPath);
        AudioEvents.LoadAudio(assetsPath);
    }

    private void InitializeTexturePool()
    {
        int textureId;
        ImageResult image;
        // 1 - Пол
        GL.GenTextures(1, out textureId);
        GL.BindTexture(TextureTarget.Texture2D, textureId);
        image = ImageResult.FromStream(File.OpenRead(MainWindow.assetsPath + "Textures/Floor.png"), ColorComponents.RedGreenBlue);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, image.Width, image.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, image.Data);
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        SetupTextureParams();
        
        // 2 - Стены
        GL.GenTextures(1, out textureId);
        GL.BindTexture(TextureTarget.Texture2D, textureId);
        image = ImageResult.FromStream(File.OpenRead(MainWindow.assetsPath + "Textures/Wall.png"), ColorComponents.RedGreenBlue);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, image.Width, image.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, image.Data);
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        SetupTextureParams();

        // 3 - Потолок
        GL.GenTextures(1, out textureId);
        GL.BindTexture(TextureTarget.Texture2D, textureId);
        image = ImageResult.FromStream(File.OpenRead(MainWindow.assetsPath + "Textures/Ceiling.png"), ColorComponents.RedGreenBlue);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, image.Width, image.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, image.Data);
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        SetupTextureParams();

        // 4 - Дверь
        GL.GenTextures(1, out textureId);
        GL.BindTexture(TextureTarget.Texture2D, textureId);
        image = ImageResult.FromStream(File.OpenRead(MainWindow.assetsPath + "Textures/Exit.png"), ColorComponents.RedGreenBlue);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, image.Width, image.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, image.Data);
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        SetupTextureParams();
    }
    private void SetupTextureParams()
    {
        // Set texture parameters
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.GenerateMipmap, (int)All.False);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
    }
    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);

        projectionMatrix = _player.camera.getProjectionMatrix((float)ClientSize.X / ClientSize.Y);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        // Это не трограть. Это должно быть до отрисоки кадра. 
        _shaderProgram.Use();

        // Устанавливаем матрицы камеры
        viewMatrix = _player.camera.GetMatrix();

        // Не трограть
        _shaderProgram.SetMatrix4("view", viewMatrix);
        _shaderProgram.SetMatrix4("projection", projectionMatrix);

        foreach (var obj in MainLogic.renderables)
            DrawObject(obj.mesh, obj.position, obj.radianRotation, Vector3.One);

        // Итерация Spark должна запускаться после остальных
        SparkGUI.Core.Tick();
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
        _spotLightParam.constant = 1.0f;
        _spotLightParam.linear = 0.09f;
        _spotLightParam.quadratic = 0.032f;
        _spotLightParam.color = new Vector3(1.0f, 1.0f, 0.7f);

        /* Вкинем материал */
        SetupMaterial(mesh.texId);


        _shaderProgram.SetMaterial("material", Material);
        _shaderProgram.SetSpotlight("spotLight", _spotLightParam);
        _shaderProgram.SetVector3("globalAmbient", _globalAmbient);
        _shaderProgram.SetVector3("viewPos",_spotLightParam.position);
        _shaderProgram.SetMatrix4("model", modelMatrix);
        _shaderProgram.SetMatrix4("norm_matrix",norm_matrix);

        GL.ActiveTexture(TextureUnit.Texture0); // Activate texture unit 0
        GL.BindTexture(TextureTarget.Texture2D, mesh.texId);
        _shaderProgram.SetInt("u_texture", 0);

        mesh.draw();
    }

    private void SetupMaterial(int id)
    {
        if (id == 1)
        {
            Material.ambient = new Vector3(0.2f, 0.2f, 0.2f);
            Material.diffuse = new Vector3(1f, 1f, 1f);
            Material.specular = new Vector3(0f, 0f, 0f);
            Material.shininess = 1.2f;
        }
        else if (id == 2)
        {
            Material.ambient = new Vector3(0.2f, 0.2f, 0.2f);
            Material.diffuse = new Vector3(1f, 1f, 1f);
            Material.specular = new Vector3(0.3f, 0.3f, 0.3f);
            Material.shininess = 10.2f;
        }
        else if (id == 3)
        {
            Material.ambient = new Vector3(0.2f, 0.2f, 0.2f);
            Material.diffuse = new Vector3(1f, 1f, 1f);
            Material.specular = new Vector3(0.4f, 0.4f, 0.4f);
            Material.shininess = 20.2f;
        }
        else if (id == 4)
        {
            Material.ambient = new Vector3(0.25f, 0.25f, 0.25f);
            Material.diffuse = new Vector3(0.7f, 0.7f, 0.7f);
            Material.specular = new Vector3(0.774597f, 0.774597f, 0.774597f);
            Material.shininess = 76.8f;
        }
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        // HandleCollision();
        if (!toplevel.Active)
        {
            MainLogic.keyboardState = KeyboardState;
            MainLogic.mouseState = MouseState;
            MainLogic.CallUpdate((float)args.Time);
        }

        var input = KeyboardState;
        if (input.IsKeyReleased(Keys.Escape))
        {
            toplevel.Active = !toplevel.Active;
            if (toplevel.Active)
            {
                CursorState = CursorState.Normal;
            } else {
                CursorState = CursorState.Grabbed;
            }
        }
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
            Title = "Backrooms"
        };

        using var window = new MainWindow(gameWindowSettings, nativeWindowSettings);
        window.Run();
    }
}
