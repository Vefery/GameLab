// код взят отсюда https://github.com/opentk/LearnOpenTK/blob/6825eba6a99e097236eb17b768f59c157e6dadde/Common/Shader.cs

using System;
using System.Collections.Generic;
using System.IO;

using Avalonia.Platform;
using OpenTK.Mathematics;
using Silk.NET.OpenGL;

using static Utils.InteropExt;

namespace AvaloniaGame.Utils
{
    public class Shader
    {
        public readonly uint Handle;

        private readonly Dictionary<string, int> _uniformLocations;

        public Shader(GL gl, string vertPath, string fragPath)
        {
            var shaderSourceStream = AssetLoader.Open(new Uri(vertPath));
            var shaderSource = new StreamReader(shaderSourceStream).ReadToEnd();
            var vertexShader = gl.CreateShader(ShaderType.VertexShader);
            gl.ShaderSource(vertexShader, shaderSource);
            CompileShader(gl, vertexShader);

            shaderSourceStream = AssetLoader.Open(new Uri(fragPath));
            shaderSource = new StreamReader(shaderSourceStream).ReadToEnd();
            var fragmentShader = gl.CreateShader(ShaderType.FragmentShader);
            gl.ShaderSource(fragmentShader, shaderSource);
            CompileShader(gl, fragmentShader);

            Handle = gl.CreateProgram();

            gl.AttachShader(Handle, vertexShader);
            gl.AttachShader(Handle, fragmentShader);

            LinkProgram(gl, Handle);

            // удалить отдельные шейдеры, так как они были уже связаны
            // в одну программу
            gl.DetachShader(Handle, vertexShader);
            gl.DetachShader(Handle, fragmentShader);
            gl.DeleteShader(fragmentShader);
            gl.DeleteShader(vertexShader);

            // создать список uniform переменных в программе
            gl.GetProgram(Handle, GLEnum.ActiveUniforms, out var numberOfUniforms);

            _uniformLocations = new Dictionary<string, int>();
            for (uint i = 0; i < numberOfUniforms; i++)
            {
                var key = gl.GetActiveUniform(Handle, i, out _, out _);
                var location = gl.GetUniformLocation(Handle, key);

                _uniformLocations.Add(key, location);
            }
        }

        private static void CompileShader(GL gl, uint shader)
        {
            gl.CompileShader(shader);

            gl.GetShader(shader, GLEnum.CompileStatus, out var code);
            if (code != (int)GLEnum.True)
            {
                var infoLog = gl.GetShaderInfoLog(shader);
                throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
            }
        }

        private static void LinkProgram(GL gl, uint program)
        {
            gl.LinkProgram(program);

            gl.GetProgram(program, GLEnum.LinkStatus, out var code);
            if (code != (int)GLEnum.True)
            {
                throw new Exception($"Error occurred whilst linking Program({program})");
            }
        }

        public void Use(GL gl)
        {
            gl.UseProgram(Handle);
        }

        public int GetAttribLocation(GL gl, string attribName)
        {
            var res = gl.GetAttribLocation(Handle, attribName);
            if (res == -1)
            {
                throw new ArgumentException(
                    $"Не удалось найти атрибут \"{attribName}\""
                );
            }

            return res;
        }

        public void SetInt(GL gl, string name, int data)
        {
            gl.UseProgram(Handle);
            gl.Uniform1(_uniformLocations[name], data);
        }

        public void SetFloat(GL gl, string name, float data)
        {
            gl.UseProgram(Handle);
            gl.Uniform1(_uniformLocations[name], data);
        }

        unsafe public void SetMatrix4(GL gl, string name, Matrix4 data)
        {
            gl.UseProgram(Handle);
            fixed (float *data_p = data.Flatten())
            {
                gl.UniformMatrix4(_uniformLocations[name], 1, true, data_p);
            }
        }

        unsafe public void SetVector3(GL gl, string name, Vector3 data)
        {
            gl.UseProgram(Handle);
            gl.Uniform3(_uniformLocations[name], data.X, data.Y, data.Z);
        }

        public void SetVector4(GL gl, string name, Vector4 data)
        {
            gl.UseProgram(Handle);
            gl.Uniform4(_uniformLocations[name], data.X, data.Y, data.Z, data.W);
        }

    }
}
