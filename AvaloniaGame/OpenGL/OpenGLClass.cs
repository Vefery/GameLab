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
        // Данил рендерить тута
        protected override void OnOpenGlRender(GlInterface gl, int fb)
        {
            if (!shouldRender)
                return;
            shouldRender = false; // Не трогать, важный костыль

            gl.ClearColor(0f, 0f, 0f, 1f);
            gl.Clear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        }

        public void RenderFrame()
        {
            shouldRender = true;
            this.RequestNextFrameRendering();
        }
    }
}
