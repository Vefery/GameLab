// код взят отсюда https://github.com/opentk/LearnOpenTK/blob/6825eba6a99e097236eb17b768f59c157e6dadde/Common/Shader.cs

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MazeGame.Utils
{
    public class Shader
    {
        public readonly int Handle;
    
        private readonly Dictionary<string, int> _uniformLocations;
    
        public Shader(string vertPath, string fragPath)
        {
            var shaderSource = File.ReadAllText(vertPath);
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, shaderSource);
            CompileShader(vertexShader);
    
            shaderSource = File.ReadAllText(fragPath);
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, shaderSource);
            CompileShader(fragmentShader);
    
            Handle = GL.CreateProgram();
    
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);
    
            LinkProgram(Handle);
    
            // удалить отдельные шейдеры, так как они были уже связаны 
            // в одну программу
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);
    
            // создать список uniform переменных в программе
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);
    
            _uniformLocations = new Dictionary<string, int>();
            for (var i = 0; i < numberOfUniforms; i++)
            {
                var key = GL.GetActiveUniform(Handle, i, out _, out _);
                var location = GL.GetUniformLocation(Handle, key);
    
                _uniformLocations.Add(key, location);
            }
        }
    
        private static void CompileShader(int shader)
        {
            GL.CompileShader(shader);
    
            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
            if (code != (int)All.True)
            {
                var infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
            }
        }
    
        private static void LinkProgram(int program)
        {
            GL.LinkProgram(program);
    
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                throw new Exception($"Error occurred whilst linking Program({program})");
            }
        }
    
        public void Use()
        {
            GL.UseProgram(Handle);
        }
    
        public int GetAttribLocation(string attribName)
        {
            var res = GL.GetAttribLocation(Handle, attribName);
            if (res == -1)
            {
                throw new ArgumentException(
                    $"Не удалось найти атрибут \"{attribName}\""
                );
            }
            
            return res;
        }

        public void SetInt(string name, int data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }

        public void SetFloat(string name, float data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }
    
        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(_uniformLocations[name], true, ref data);
        }
    
        public void SetVector3(string name, Vector3 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform3(_uniformLocations[name], data);
        }
        
        public void SetVector4(string name, Vector4 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform4(_uniformLocations[name], data);
        }


    }
}
