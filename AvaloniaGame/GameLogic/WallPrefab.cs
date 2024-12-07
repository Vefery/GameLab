﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Platform;
using Avalonia;
using AvaloniaGame.OpenGL;

namespace AvaloniaGame.GameLogic
{
    public class WallPrefab : GameObject, IRenderable
    {
        public Mesh mesh {  get; private set; }
        public WallPrefab()
        {
            mesh = ObjReader.ReadObjFile("avares://AvaloniaGame/Assets/Wall.model");
        }
    }
}
