using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using AvaloniaGame.GameLogic;
using System;
using static Avalonia.OpenGL.GlConsts;

namespace AvaloniaGame.OpenGL
{
    public class OpenGLClass : OpenGlControlBase
    {
        private bool shouldRender = false;
        private String VertexShader = "";
        private String FragmentShader = "";
        private bool LoadShader(String shaderName)
        {
            return true;
        }
        protected override void OnOpenGlInit(GlInterface gl)
        {
            base.OnOpenGlInit(gl);
        }
        
        protected override void OnOpenGlRender(GlInterface gl, int fb)
        {
            if (!shouldRender)
                return;
            shouldRender = false; // Не трогать, важный костыль

            RenderObject(gl, null);

            foreach (IRenderable obj in MainLogic.renderables)
                RenderObject(gl, obj.mesh);

            gl.Clear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        }

        // Данил рендерить тута
        private void RenderObject(GlInterface gl, Mesh mesh)
        {
            gl.ClearColor(0.5f, 0.5f,0.5f, 1.0f);
            
        }
        public void RenderFrame()
        {
            shouldRender = true;
            RequestNextFrameRendering();
        }
    }
}
