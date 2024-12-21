using System.Reflection.Metadata;
using MazeGame.GameLogic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MazeGame.Utils
{
    public class SpotlightShader
    {
        public Shader Inner;
        public float shaker = 0;
        private Random random = new();

        public SpotlightShader(string vertexShaderPath, string fragmentShaderPath)
        {
            Inner = new Shader(vertexShaderPath, fragmentShaderPath);
        }

        public void Use() => Inner.Use();

        public void SetFloat(string name, float value)
            => Inner.SetFloat(name, value);

        public void SetMatrix4(string name, Matrix4 matrix)
            => Inner.SetMatrix4(name, matrix);

        public void SetVector3(string name, Vector3 vector)
            => Inner.SetVector3(name, vector);

        public void SetInt(string name, int data)
            => Inner.SetInt(name, data);

        public void SetSpotlight(string name, Spotlight light)
        {
            /* main settings */
            float flicker = 0;
            if (shaker < 0.5f)
                flicker = 0.5f * MathF.Pow(MathF.Sin(12 * shaker), 2);

            light.position[0] += MathF.Cos(shaker) / 8f;
            light.position[1] += MathF.Sin(0.5f*shaker) / 8f;
            light.intensity = /*(MathF.Sin(shaker) * MathF.Sin(2 * shaker)) / 2 + */1f - flicker;

            shaker += 0.05f / MainLogic.gameObjects.Count;
            if (shaker > 5 * MathF.PI)
                shaker = 0f;

            SetVector3($"{name}.position", light.position);
            SetVector3($"{name}.direction", light.direction);
            SetFloat($"{name}.cutoffAngle", light.cutoffAngle);
            SetFloat($"{name}.outerCutoff", light.outerCutoff);
            SetFloat($"{name}.intensity", light.intensity);

            /* attenuation */
            SetFloat($"{name}.constant", light.constant);
            SetFloat($"{name}.linear", light.linear);
            SetFloat($"{name}.quadratic", light.quadratic);

            /* color of light */
            SetVector3($"{name}.color", light.color);
        }

        public void ShakingSpotlight(string name, Spotlight light)
        {
            /* main settings */
            SetVector3($"{name}.position", light.position);
            SetVector3($"{name}.direction", light.direction);
            SetFloat($"{name}.cutoffAngle", light.cutoffAngle);
            SetFloat($"{name}.outerCutoff", light.outerCutoff);
            SetFloat($"{name}.intensity", light.intensity);

            /* attenuation */
            SetFloat($"{name}.constant", light.constant);
            SetFloat($"{name}.linear", light.linear);
            SetFloat($"{name}.quadratic", light.quadratic);

            /* color of light */
            SetVector3($"{name}.color", light.color);
        }

        public void SetMaterial(string name, Material material)
        {
            SetVector3($"{name}.ambient", material.ambient);
            SetVector3($"{name}.diffuse", material.diffuse);
            SetVector3($"{name}.specular", material.specular);
            SetFloat($"{name}.shininess", material.shininess);
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
