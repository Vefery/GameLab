
using Avalonia.Controls;

using AvaloniaGame.OpenGL;

namespace AvaloniaGame.Views;

public partial class MainWindow : Window
{
    public static string assetsPath = "avares://AvaloniaGame/Assets/";

    public MainWindow()
    {
        InitializeComponent();
        var view = this.FindControl<MainView>("MainView")!;
        var glControl = view.FindControl<OpenGLClass>("GameView")!;

        // TODO: игрок пересоздаётся и подписки на клавиши и мышь пропадают
        KeyDown += glControl.Player.KeyDownHandler;
        KeyUp += glControl.Player.KeyUpHandler;
        PointerMoved += glControl.Player.PointerMovedHandler;
        // TODO: нужен захват мыши
    }
}
