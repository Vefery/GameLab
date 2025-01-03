using System;
using System.Diagnostics;

using Avalonia.Controls;
using Avalonia.Interactivity;

using AvaloniaGame.GameLogic;
using AvaloniaGame.OpenGL;

namespace AvaloniaGame.Views;

public partial class MainWindow : Window
{
    private OpenGLClass? _glControl;
    public static string assetsPath = "avares://AvaloniaGame/Assets/";
    public MainWindow()
    {
        InitializeComponent();
        
        MainLogic.mainWindow = this;
        _glControl = this.FindControl<OpenGLClass>("GameView");

        this.Closing += MainLogic.OnCloseCleanUp;
        MainLogic.mainWindow = this;
        // TODO: нужен захват мыши
    }
}
