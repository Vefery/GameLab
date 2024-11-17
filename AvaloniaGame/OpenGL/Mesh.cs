using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AvaloniaGame.OpenGL.ObjReader;

namespace AvaloniaGame.OpenGL
{
    public class Mesh
    {
        public List<Vertex> vertices = new List<Vertex>();

        public List<Face> faces = new List<Face>();

        public List<TextureCoordinate> textureCoordinates = new List<TextureCoordinate>();
    }
}
