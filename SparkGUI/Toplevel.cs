using OpenTK.Windowing.Common;

namespace SparkGUI
{
    class Toplevel
    {
        private Rect ContentBounds = new(0, 0, 0, 0);
        private bool _active = true;
        public bool Active
        {
            get => _active;
            set {
                _active = value;
                if (value)
                {
                    if (renderLoopID == -1)
                    {
                        renderLoopID = Core.LoopAdd(RenderChild);
                    }
                } else {
                    if (renderLoopID != -1)
                    {
                        Core.LoopRemove(renderLoopID);
                        renderLoopID = -1;
                    }
                }
            }
        }

        private Widget _child;
        public Widget Child
        {
            get => _child;
            set {
                _child = value;
                var x = (ContentBounds.Width - _child.Width) / 2f;
                var y = (ContentBounds.Height - _child.Height) / 2f;
                _child.Position = new(x, y);
                if (renderLoopID != -1) {
                    Core.LoopRemove(renderLoopID);
                }
                renderLoopID = Core.LoopAdd(RenderChild);
            }
        }
        private bool RenderChild(DateTime lastTick) {
            _child.Render();
            return true;
        } 
        
        public void HandleClick(MouseButtonEventArgs args)
        {
            if (Active)
            {
                _child.HandleClick(args);
            }
        }
        
        public void HandleMotion(MouseMoveEventArgs args)
        {
            if (Active)
            {
                _child.HandleMotion(args);
            }
        }
        
        public Toplevel(Widget child)
        {
            ContentBounds.Y2 = Core._gameWindow.Bounds.Size.Y;
            ContentBounds.X2 = Core._gameWindow.Bounds.Size.X;
            Child = child;
            Core._gameWindow.Resize += ResizeCallback;
            Core._gameWindow.MouseUp += HandleClick;
            Core._gameWindow.MouseDown += HandleClick;
            Core._gameWindow.MouseMove += HandleMotion;
        }
        
        private void ResizeCallback(ResizeEventArgs args)
        {
            // в предположении что Toplevel находится в (0, 0)
            ContentBounds.Y2 = args.Height;
            ContentBounds.X2 = args.Width;
            
            var x = (ContentBounds.Width - _child.Width) / 2f;
            var y = (ContentBounds.Height - _child.Height) / 2f;
            _child.Position = new(x, y);
        }

        private int renderLoopID = -1;
    }
}
