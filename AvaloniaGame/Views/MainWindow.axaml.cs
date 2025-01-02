
using Avalonia.Controls;
using AvaloniaGame.GameLogic;
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

        this.Closing += MainLogic.OnCloseCleanUp;
        MainLogic.control = this;
        // TODO: нужен захват мыши
    }
}
