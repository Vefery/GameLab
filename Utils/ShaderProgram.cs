using OpenTK.Graphics.OpenGL4;

namespace MazeGame.Utils
{
    public class ShaderProgram
    {
        private readonly int _programId;

        public ShaderProgram(string vertexShaderPath, string fragmentShaderPath)
        {
            string vertexShaderSource = File.ReadAllText(vertexShaderPath);
            string fragmentShaderSource = File.ReadAllText(fragmentShaderPath);

            int vertexShader = CompileShader(ShaderType.VertexShader, vertexShaderSource);
            int fragmentShader = CompileShader(ShaderType.FragmentShader, fragmentShaderSource);

            _programId = GL.CreateProgram();
            GL.AttachShader(_programId, vertexShader);
            GL.AttachShader(_programId, fragmentShader);
            GL.LinkProgram(_programId);

            GL.GetProgram(_programId, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(_programId);
                throw new Exception($"Program linking error: {infoLog}");
            }

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        private int CompileShader(ShaderType type, string source)
        {
            int shader = GL.CreateShader(type);
            GL.ShaderSource(shader, source);
            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"{type} compilation error: {infoLog}");
            }

            return shader;
        }

        public void Use() => GL.UseProgram(_programId);

        public void SetUniform(string name, float value)
        {
            int location = GL.GetUniformLocation(_programId, name);
            if (location == -1) throw new Exception($"Uniform '{name}' not found.");
            GL.Uniform1(location, value);
        }

        public void SetUniform(string name, OpenTK.Mathematics.Matrix4 matrix)
        {
            int location = GL.GetUniformLocation(_programId, name);
            if (location == -1) throw new Exception($"Uniform '{name}' not found.");
            GL.UniformMatrix4(location, true, ref matrix);
        }

        public void SetUniform(string name, Spotlight light)
        {
            /* main settings */
            SetUniform($"{name}.position", light.position);
            SetUniform($"{name}.direction", light.direction);
            SetUniform($"{name}.cutoffAngle", light.cutoffAngle);
            SetUniform($"{name}.outerCutoff", light.outerCutoff);
            SetUniform($"{name}.intensity", light.intensity);

            /* attenuation */
            SetUniform($"{name}.constant", light.constant);
            SetUniform($"{name}.linear", light.linear);
            SetUniform($"{name}.quadratic", light.quadratic);

            /* color of light */
            SetUniform($"{name}.color", light.color);

        }

        public void SetUniform(string name, Material material)
        {
            SetUniform($"{name}.ambient", material.ambient);
            SetUniform($"{name}.diffuse", material.diffuse);
            SetUniform($"{name}.specular", material.specular);
            SetUniform($"{name}.shininess", material.shininess);

        }

        public void SetUniform(string name, OpenTK.Mathematics.Vector3 vector)
        {
            int location = GL.GetUniformLocation(_programId, name);
            if (location == -1) throw new Exception($"Uniform '{name}' not found.");
            GL.Uniform3(location, vector);
           
        }
    }

    public struct Spotlight
    {
        public OpenTK.Mathematics.Vector3 position;
        public OpenTK.Mathematics.Vector3 direction;
        public float cutoffAngle;
        public float outerCutoff;
        public float intensity;
        public float constant;
        public float linear;
        public float quadratic;
        public OpenTK.Mathematics.Vector3 color;
        
    }

    public struct Material
    {
        public OpenTK.Mathematics.Vector3 ambient;
        public OpenTK.Mathematics.Vector3 diffuse;
        public OpenTK.Mathematics.Vector3 specular;
        public float shininess;
    }
}
