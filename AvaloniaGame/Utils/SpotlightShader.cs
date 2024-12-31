using System;

using AvaloniaGame.GameLogic;
using OpenTK.Mathematics;
using Silk.NET.OpenGL;

namespace AvaloniaGame.Utils
{
    public class SpotlightShader
    {
        public Shader Inner;
        public float shaker = 0;
        private Random random = new();

        public SpotlightShader(GL gl, string vertexShaderPath, string fragmentShaderPath)
        {
            Inner = new Shader(gl, vertexShaderPath, fragmentShaderPath);
        }

        public void Use(GL gl) => Inner.Use(gl);

        public void SetFloat(GL gl, string name, float value)
            => Inner.SetFloat(gl, name, value);

        public void SetMatrix4(GL gl, string name, Matrix4 matrix)
            => Inner.SetMatrix4(gl, name, matrix);

        public void SetVector3(GL gl, string name, Vector3 vector)
            => Inner.SetVector3(gl, name, vector);

        public void SetInt(GL gl, string name, int data)
            => Inner.SetInt(gl, name, data);

        public void SetSpotlight(GL gl, string name, Spotlight light)
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

            SetVector3(gl, $"{name}.position", light.position);
            SetVector3(gl, $"{name}.direction", light.direction);
            SetFloat(gl, $"{name}.cutoffAngle", light.cutoffAngle);
            SetFloat(gl, $"{name}.outerCutoff", light.outerCutoff);
            SetFloat(gl, $"{name}.intensity", light.intensity);

            /* attenuation */
            SetFloat(gl, $"{name}.constant", light.constant);
            SetFloat(gl, $"{name}.linear", light.linear);
            SetFloat(gl, $"{name}.quadratic", light.quadratic);

            /* color of light */
            SetVector3(gl, $"{name}.color", light.color);
        }

        public void ShakingSpotlight(GL gl, string name, Spotlight light)
        {
            /* main settings */
            SetVector3(gl, $"{name}.position", light.position);
            SetVector3(gl, $"{name}.direction", light.direction);
            SetFloat(gl, $"{name}.cutoffAngle", light.cutoffAngle);
            SetFloat(gl, $"{name}.outerCutoff", light.outerCutoff);
            SetFloat(gl, $"{name}.intensity", light.intensity);

            /* attenuation */
            SetFloat(gl, $"{name}.constant", light.constant);
            SetFloat(gl, $"{name}.linear", light.linear);
            SetFloat(gl, $"{name}.quadratic", light.quadratic);

            /* color of light */
            SetVector3(gl, $"{name}.color", light.color);
        }

        public void SetMaterial(GL gl, string name, Material material)
        {
            SetVector3(gl, $"{name}.ambient", material.ambient);
            SetVector3(gl, $"{name}.diffuse", material.diffuse);
            SetVector3(gl, $"{name}.specular", material.specular);
            SetFloat(gl, $"{name}.shininess", material.shininess);
        }
    }

    public struct Spotlight
    {
        public Vector3 position;
        public Vector3 direction;
        public float cutoffAngle;
        public float outerCutoff;
        public float intensity;
        public float constant;
        public float linear;
        public float quadratic;
        public Vector3 color;
    }

    public struct Material
    {
        public Vector3 ambient;
        public Vector3 diffuse;
        public Vector3 specular;
        public float shininess;
    }
}
