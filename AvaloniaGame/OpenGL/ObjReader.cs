using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Platform;

namespace AvaloniaGame.OpenGL
{
    public class ObjReader
    {
        public struct Face
        {
            public int[] VertexIndices;
        }
        public static Mesh ReadObjFile(string filePath)
        {
            CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            Mesh newMesh = new Mesh();
            using (var reader = new StreamReader(AssetLoader.Open(new Uri(filePath))))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(' ');

                    switch (parts[0])
                    {
                        case "v":
                            newMesh.vertices.Add(new Vector3(
                                float.Parse(parts[1], NumberStyles.Any, ci),
                                float.Parse(parts[2], NumberStyles.Any, ci),
                                float.Parse(parts[3], NumberStyles.Any, ci)
                            ));
                            break;
                        case "vn":
                            newMesh.normals.Add(new Vector3(
                                float.Parse(parts[1], NumberStyles.Any, ci),
                                float.Parse(parts[2], NumberStyles.Any, ci),
                                float.Parse(parts[3], NumberStyles.Any, ci)
                            ));
                            break;
                        case "vt":
                            newMesh.textureCoordinates.Add(new Vector2(
                                float.Parse(parts[1], NumberStyles.Any, ci),
                                float.Parse(parts[2], NumberStyles.Any, ci)
                            ));
                            break;
                        case "f":
                            newMesh.faces.Add(new Face
                            {
                                VertexIndices = parts.Skip(1).Select(x => int.Parse(x.Split('/')[0])).ToArray()
                            });
                            break;
                    }
                }
            }
            return newMesh;
        }
    }
}
