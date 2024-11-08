using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using static Avalonia.OpenGL.GlConsts;

namespace AvaloniaGame.OpenGL
{
    public class OpenGLClass : OpenGlControlBase
    {
        protected override void OnOpenGlInit(GlInterface gl)
        {
            base.OnOpenGlInit(gl);
        }
        protected override void OnOpenGlRender(GlInterface gl, int fb)
        {
            gl.ClearColor(0f, 0f, 0f, 1f);
            gl.Clear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        }
    }
}
