using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Avalonia.Platform;
using OpenTK.Mathematics;
using Silk.NET.OpenGL;

public class Mesh
{
    private uint _vao;
    private uint _vbo;

    // Вершины хранятся в следующей последовательности:
    // |verte x|vertex y|vertex z|normal x|normal y|normal z|texture cord u|rexture cord v|...
    private List<float> _vertices;

    public uint texId;


    // Вершины для обработки коллизий
    public List<Vector3> verticesPos { get; private set; }
    /*
        string FilePathObj - Путя до файла модели
        string FilePathTexture - Путя до файла с текстурой пока нету
    */
    public Mesh(GL gl, string FileObj, string TexturePath)
    {
        _vertices = new List<float>();
        verticesPos = new List<Vector3>();

        LoadObj(FileObj);
        InitBuffers(gl);
    }

    private void InitBuffers(GL gl)
    {
        _vao = gl.GenVertexArray();
        _vbo = gl.GenBuffer();

        gl.BindVertexArray(_vao);
        gl.BindBuffer(GLEnum.ArrayBuffer, _vbo);

        gl.BufferData(
                GLEnum.ArrayBuffer,
                (uint)_vertices.Count * sizeof(float),
                new ReadOnlySpan<float>(_vertices.ToArray()),
                GLEnum.StaticDraw
            );
        // Настройка атрибута позиции вершин
        // pos
        gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
        gl.EnableVertexAttribArray(0);

        // normal
        gl.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
        gl.EnableVertexAttribArray(1);

        // tex cord
        gl.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
        gl.EnableVertexAttribArray(2);

        gl.BindVertexArray(0);
    }
    private void LoadObj(string FileObj)
    {

        using StreamReader reader = new StreamReader(AssetLoader.Open(new Uri(FileObj)));
        string line;

        List<Vector3> position = new List<Vector3>();
        List<Vector3>  normal = new List<Vector3>();
        List<Vector2> uv = new List<Vector2>();
        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";

        while ((line = reader.ReadLine()) != null)
        {
            if(line.StartsWith("v "))
            {
                var parts = line.Split(' ');
                Vector3 tmp = new Vector3
                    (
                        float.Parse(parts[1], NumberStyles.Any, ci),
                        float.Parse(parts[2], NumberStyles.Any, ci),
                        float.Parse(parts[3], NumberStyles.Any, ci)
                    );
                position.Add(tmp);
                verticesPos.Add(tmp);
            }
            else if(line.StartsWith("vn "))
            {
                var parts = line.Split(' ');
                normal.Add(
                    new Vector3
                    (
                        float.Parse(parts[1], NumberStyles.Any, ci),
                        float.Parse(parts[2], NumberStyles.Any, ci),
                        float.Parse(parts[3], NumberStyles.Any, ci)
                    )
                );
            }
            else if(line.StartsWith("vt "))
            {
                var parts = line.Split(' ');
                uv.Add(
                    new Vector2
                    (
                        float.Parse(parts[1], NumberStyles.Any, ci),
                        float.Parse(parts[2], NumberStyles.Any, ci)
                    )
                );
            }
            else if(line.StartsWith("f "))
            {
                var parts = line.Split(' ');
                foreach(var part in parts[1..])
                {
                    var indices = part.Split('/');
                    int positionIndex = int.Parse(indices[0]) - 1;
                    int uvIndex = int.Parse(indices[1]) - 1;
                    int normalIndex = int.Parse(indices[2]) - 1;

                    Vector3 pos = position[positionIndex];
                    Vector2 uv_cord = uv[uvIndex];
                    Vector3 norm = normal[normalIndex];

                    _vertices.AddRange(new[] { pos.X, pos.Y, pos.Z, norm.X, norm.Y, norm.Z, uv_cord.X, uv_cord.Y });
                }
            }
        }
    }

    public void draw(GL gl)
    {
        gl.BindVertexArray(_vao);
        gl.BindBuffer(GLEnum.ArrayBuffer,_vbo);
        gl.DrawArrays(GLEnum.Triangles, 0, (uint)_vertices.Count/8);
        gl.BindVertexArray(0);
    }
}
