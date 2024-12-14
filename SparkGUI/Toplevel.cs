using OpenTK.Windowing.Common;

namespace SparkGUI
{
    class Toplevel
    {
        private Widget _child;
        public Widget Child
        {
            get => _child;
            set {
                _child = value;
                _child.Position = new(0, 0);
                if (renderLoopID != -1) {
                    Core.LoopRemove(renderLoopID);
                }
                renderLoopID = Core.LoopAdd(_ =>
                {
                    _child.Render();
                    return true;
                });
            }
        }
        
        public void HandleClick(MouseButtonEventArgs args)
        {
            _child.HandleClick(args);
        }
        
        public Toplevel(Widget child)
        {
            Child = child;
            Core._gameWindow.MouseUp += HandleClick;
            Core._gameWindow.MouseDown += HandleClick;
        }

        private int renderLoopID = -1;
    }
}
