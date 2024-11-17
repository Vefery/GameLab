using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using System;
using static Avalonia.OpenGL.GlConsts;

namespace AvaloniaGame.OpenGL
{
    public class OpenGLClass : OpenGlControlBase
    {
        private bool shouldRender = false;
        protected override void OnOpenGlInit(GlInterface gl)
        {
            base.OnOpenGlInit(gl);
        }
        
        protected override void OnOpenGlRender(GlInterface gl, int fb)
        {
            if (!shouldRender)
                return;
            shouldRender = false; // Не трогать, важный костыль

            gl.Clear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        }

        // Данил рендерить тута
        private void RenderObject(GlInterface gl, Mesh mesh)
        {

        }
        public void RenderFrame()
        {
            shouldRender = true;
            RequestNextFrameRendering();
        }
    }
}
