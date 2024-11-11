using Avalonia.Controls;
using AvaloniaGame.OpenGL;
using System.Diagnostics;
using System;
using AvaloniaGame.GameLogic;

namespace AvaloniaGame.Views;

public partial class MainView : UserControl
{
    private OpenGLClass? _glControl;
    private MainLogicLoop mainGameLogic;
    public MainView()
    {
        // Типа точка входа наша. Тут инициализируем OpenGL и создаем объект, который всей логикой занимается
        InitializeComponent();

        _glControl = this.FindControl<OpenGLClass>("OpenGLControl");

        if (_glControl == null)
        {
            Debug.WriteLine("Couldn't initialize OpenGL");
            Environment.Exit(-1);
        }
        mainGameLogic = new(_glControl);
    }
}
