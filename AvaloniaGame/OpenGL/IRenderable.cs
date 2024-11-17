using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaGame.OpenGL
{
    public interface IRenderable
    {
        Mesh mesh { get; }
    }
}
