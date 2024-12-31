using System.Diagnostics;
using System;

using Avalonia.Controls;

using AvaloniaGame.OpenGL;

namespace AvaloniaGame.Views;

public partial class MainView : UserControl
{
    private OpenGLClass? _glControl;
    public MainView()
    {
        // Типа точка входа наша. Тут инициализируем OpenGL и создаем объект, который всей логикой занимается
        InitializeComponent();

        _glControl = this.FindControl<OpenGLClass>("GameView");

        if (_glControl == null)
        {
            Debug.WriteLine("Couldn't initialize OpenGL");
            Environment.Exit(-1);
        }

        // TopLevel.GetTopLevel(_glControl).KeyDown += _glControl.Player.KeyDownHandler;
        //mainGameLogic = new(_glControl);
        // MainLogic.StartWork(_glControl);
    }

}
