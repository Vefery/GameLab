using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Globalization;

public class Mesh
{

    private int _vao, _vbo;

    // Вершины хранятся в следующей последовательности:
    // |verte x|vertex y|vertex z|normal x|normal y|normal z|texture cord u|rexture cord v|...
    private List<float> _vertices;
    /*
        string FilePathObj - Путя до файла модели
        string FilePathTexture - Путя до файла с текстурой пока нету  
     */
    public Mesh(string FileObj)
    {
        _vertices = new List<float>();
        
        LoadObj(FileObj);
        InitBuffers();
    }

    private void InitBuffers()
    {
        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();

        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Count * sizeof(float), _vertices.ToArray(), BufferUsageHint.StaticDraw);

        // Настройка атрибута позиции вершин
        // pos
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        // normal
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        // tex cord
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
        GL.EnableVertexAttribArray(2);

        GL.BindVertexArray(0);
    }

    private void LoadObj(string FileObj)
    {
        //_vertices =
        //[
        //    0.0f,  0.5f, 0.0f, 0, 0, 0, 0, 0, // Вершина сверху
        //    -0.5f, -0.5f, 0.0f,  0, 0, 0, 0, 0, // Левая вершина
        //     0.5f, -0.5f, 0.0f,  0, 0, 0, 0, 0  // Правая вершина
        //];

        using StreamReader reader = new StreamReader(FileObj);
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
                position.Add(
                    new Vector3
                    (
                        float.Parse(parts[1], NumberStyles.Any,ci),
                        float.Parse(parts[2], NumberStyles.Any, ci),
                        float.Parse(parts[3], NumberStyles.Any, ci)
                    )
                );
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
        Console.WriteLine(position.Count);

    }

    public void draw()
    {
        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer,_vbo);
        GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Count/8);
        GL.BindVertexArray(0);
    }
}
