using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static AvaloniaGame.OpenGL.ObjReader;

namespace AvaloniaGame.OpenGL
{
    public class Mesh
    {
        public List<Vector3> vertices = new();

        public List<Face> faces = new();

        public List<Vector2> textureCoordinates = new();

        public List<Vector3> normals = new();
    }
}
