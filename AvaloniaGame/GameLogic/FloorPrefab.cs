using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AvaloniaGame.OpenGL;

namespace AvaloniaGame.GameLogic
{
    public class FloorPrefab : GameObject, IRenderable
    {
        public Mesh mesh { get; private set; }
        public FloorPrefab(Vector3 position, Vector3 rotation)
        {
            this.position = position;
            this.eulerRotation = rotation;
            mesh = ObjReader.ReadObjFile("avares://AvaloniaGame/Assets/Floor.obj");
        }
    }
}
