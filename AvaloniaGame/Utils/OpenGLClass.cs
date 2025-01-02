using System;

using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.Platform;
using OpenTK.Mathematics;
using Silk.NET.OpenGL;
using StbImageSharp;

using AvaloniaGame.GameLogic;
using AvaloniaGame.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AvaloniaGame.OpenGL
{
    public class OpenGLClass : OpenGlControlBase
    {
        private bool shouldRender = false;
        private string VertexShader = "";
        private string FragmentShader = "";
        private DateTime lastTick = DateTime.Now;
        private SpotlightShader _shaderProgram;
        AudioObject AudioAmbient = new();
        private Matrix4 projectionMatrix, viewMatrix, modelMatrix, norm_matrix;
 
        /* Параметры освещения */
        private Spotlight _spotLightParam; // Параметры настройки освещения. Менаются в зависимости от положения камеры.
        private Vector3 _globalAmbient;
        private Material Material;
        public Player Player = MainLogic.InitializePlayer();


        private bool LoadShader(string shaderName)
        {
            return true;
        }

        private static void CheckError(GlInterface aGL)
        {
            var gl = GL.GetApi(aGL.GetProcAddress);

            GLEnum err;
            while ((err = gl.GetError()) != GLEnum.NoError)
            {
                Console.WriteLine(err);
            }
        }

        protected override void OnOpenGlInit(GlInterface aGL)
        {
            base.OnOpenGlInit(aGL);
            CheckError(aGL);
            var gl = GL.GetApi(aGL.GetProcAddress);
            
            MainLogic.gl = gl;
            InitializeGraphics(gl);
            InitializeAudio();
            MainLogic.InitializeScene();
            MainLogic.OnFinished += () => Player = MainLogic.ReloadLevel();
            CheckError(aGL);
        }

        private void InitializeGraphics(GL gl)
        {
            // Устанавливаем параметры OpenGL
            gl.Enable(EnableCap.DepthTest);

            // Загрузка шейдера
            _shaderProgram = new SpotlightShader(gl, "avares://AvaloniaGame/Assets/" + "Shaders/Spotlight.vert", "avares://AvaloniaGame/Assets/" + "Shaders/Spotlight.frag");

            StbImage.stbi_set_flip_vertically_on_load(1); // приводит текстуры к формату opengl
            InitializeTexturePool(gl);
        }

        private void InitializeAudio()
        {
            AudioAmbient.LoadAudio("Ambient");
            AudioAmbient.PlayAudio(looped: true);
        }

        private void InitializeTexturePool(GL gl)
        {
            uint textureId;
            ImageResult image;
            // 1 - Пол
            gl.GenTextures(1, out textureId);
            gl.BindTexture(GLEnum.Texture2D, textureId);
            image = ImageResult.FromStream(AssetLoader.Open(new Uri("avares://AvaloniaGame/Assets/" + "Textures/Floor.png")), ColorComponents.RedGreenBlue);
            gl.TexImage2D(GLEnum.Texture2D, 0, (int)InternalFormat.Rgb, (uint)image.Width, (uint)image.Height, 0, GLEnum.Rgb, GLEnum.UnsignedByte, new ReadOnlySpan<byte>(image.Data));
            gl.GenerateMipmap(GLEnum.Texture2D);
            SetupTextureParams(gl);

            // 2 - Стены
            gl.GenTextures(1, out textureId);
            gl.BindTexture(GLEnum.Texture2D, textureId);
            image = ImageResult.FromStream(AssetLoader.Open(new Uri("avares://AvaloniaGame/Assets/" + "Textures/Wall.png")), ColorComponents.RedGreenBlue);
            gl.TexImage2D(GLEnum.Texture2D, 0, (int)InternalFormat.Rgb, (uint)image.Width, (uint)image.Height, 0, GLEnum.Rgb, GLEnum.UnsignedByte, new ReadOnlySpan<byte>(image.Data));
            gl.GenerateMipmap(GLEnum.Texture2D);
            SetupTextureParams(gl);

            // 3 - Потолок
            gl.GenTextures(1, out textureId);
            gl.BindTexture(GLEnum.Texture2D, textureId);
            image = ImageResult.FromStream(AssetLoader.Open(new Uri("avares://AvaloniaGame/Assets/" + "Textures/Ceiling.png")), ColorComponents.RedGreenBlue);
            gl.TexImage2D(GLEnum.Texture2D, 0, (int)InternalFormat.Rgb, (uint)image.Width, (uint)image.Height, 0, GLEnum.Rgb, GLEnum.UnsignedByte, new ReadOnlySpan<byte>(image.Data));
            gl.GenerateMipmap(GLEnum.Texture2D);
            SetupTextureParams(gl);

            // 4 - Дверь
            gl.GenTextures(1, out textureId);
            gl.BindTexture(GLEnum.Texture2D, textureId);
            image = ImageResult.FromStream(AssetLoader.Open(new Uri("avares://AvaloniaGame/Assets/" + "Textures/Exit.png")), ColorComponents.RedGreenBlue);
            gl.TexImage2D(GLEnum.Texture2D, 0, (int)InternalFormat.Rgb, (uint)image.Width, (uint)image.Height, 0, GLEnum.Rgb, GLEnum.UnsignedByte, new ReadOnlySpan<byte>(image.Data));
            gl.GenerateMipmap(GLEnum.Texture2D);
            SetupTextureParams(gl);
        }

        private void SetupTextureParams(GL gl)
        {
            // Set texture parameters
            gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.GenerateMipmapSgis, (int)GLEnum.False);
            gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        }

        protected override void OnOpenGlRender(GlInterface aGL, int fb)
        {
            var gl = GL.GetApi(aGL.GetProcAddress);
            
            MainLogic.CallUpdate( (float) (DateTime.Now - lastTick).TotalSeconds );
            lastTick = DateTime.Now;

            gl.Viewport(0, 0, (uint)Bounds.Width, (uint)Bounds.Height);
            projectionMatrix = Player.camera.getProjectionMatrix( (float)(Bounds.Width / Bounds.Height) );

            gl.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
            gl.Clear( (uint) (GLEnum.ColorBufferBit | GLEnum.DepthBufferBit) );

            _shaderProgram.Use(gl);

            // Устанавливаем матрицы камеры
            viewMatrix = Player.camera.GetMatrix();

            // Не трогать
            _shaderProgram.SetMatrix4(gl, "view", viewMatrix);
            _shaderProgram.SetMatrix4(gl, "projection", projectionMatrix);

            foreach (var obj in MainLogic.renderables)
            {
                DrawObject(gl, obj.mesh, obj.position, obj.radianRotation, Vector3.One);
            }
            RequestNextFrameRendering();
        }

        private void DrawObject(GL gl, Mesh mesh, Vector3 position, Vector3 eulerRotation, Vector3 scale)
        {
            modelMatrix = Matrix4.CreateRotationX(eulerRotation.X);
            modelMatrix *= Matrix4.CreateRotationY(eulerRotation.Y);
            modelMatrix *= Matrix4.CreateRotationZ(eulerRotation.Z);
            modelMatrix *= Matrix4.CreateTranslation(position);
            modelMatrix *= Matrix4.CreateScale(scale);

            norm_matrix = (modelMatrix*viewMatrix).Inverted();
            norm_matrix.Transpose();

            /* Настройки параметров освещения */
            _globalAmbient = new Vector3(0.3f, 0.3f, 0.3f);
            _spotLightParam.position = Vector3.Zero;
            _spotLightParam.direction = -Vector3.UnitZ;


            _spotLightParam.cutoffAngle = MathF.Cos( (float)Math.PI / 180.0f * 15.5f);
            _spotLightParam.outerCutoff = MathF.Cos( (float)Math.PI / 180.0f * 45.0f);
            _spotLightParam.constant = 1.0f;
            _spotLightParam.linear = 0.09f;
            _spotLightParam.quadratic = 0.032f;
            _spotLightParam.color = new Vector3(1.0f, 1.0f, 0.7f);

            /* Вкинем материал */
            SetupMaterial(mesh.texId);


            _shaderProgram.SetMaterial(gl, "material", Material);
            _shaderProgram.SetSpotlight(gl, "spotLight", _spotLightParam);
            _shaderProgram.SetVector3(gl, "globalAmbient", _globalAmbient);
            _shaderProgram.SetVector3(gl, "viewPos",_spotLightParam.position);
            _shaderProgram.SetMatrix4(gl, "model", modelMatrix);
            _shaderProgram.SetMatrix4(gl, "norm_matrix", norm_matrix);

            gl.ActiveTexture(TextureUnit.Texture0); // Activate texture unit 0
            gl.BindTexture(GLEnum.Texture2D, mesh.texId);
            _shaderProgram.SetInt(gl, "u_texture", 0);

            mesh.draw(gl);
        }

        private void SetupMaterial(uint id)
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

        public void RenderFrame()
        {
            shouldRender = true;
            RequestNextFrameRendering();
        }
    }
}
