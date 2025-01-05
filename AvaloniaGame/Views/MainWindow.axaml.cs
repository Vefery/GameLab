using System;
using System.Diagnostics;

using Avalonia.Controls;
using Avalonia.Interactivity;

using AvaloniaGame.GameLogic;
using AvaloniaGame.OpenGL;

namespace AvaloniaGame.Views;

using Avalonia.Controls;
using System;
using System.Timers;
using Avalonia.Threading;

public partial class MainWindow : Window
{
    private OpenGLClass? _glControl;
    public static string assetsPath = "avares://AvaloniaGame/Assets/";

    private Timer _timer;
    public TimeSpan _timeElapsed;

    public MainWindow()
    {
        InitializeComponent();
        
        MainLogic.mainWindow = this;
        _glControl = this.FindControl<OpenGLClass>("GameView");

        this.Closing += MainLogic.OnCloseCleanUp;
        MainLogic.mainWindow = this;

        _timeElapsed = TimeSpan.Zero;
        TimerTextBlock.Text = _timeElapsed.ToString(@"mm\:ss\.ff");

        // TODO: нужен захват мыши
    }

    public void StartTimer()
    {
        _timer = new Timer(10);
        _timer.Elapsed += OnTimerElapsed;
        _timer.Start();
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        _timeElapsed = _timeElapsed.Add(TimeSpan.FromMilliseconds(10));
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            TimerTextBlock.Text = _timeElapsed.ToString(@"mm\:ss\.ff");
        });
    }

    // Метод для обновления таймера при переходе на новый уровень
    public void UpdateTimer()
    {
        // Здесь вы можете сбросить таймер или изменить его поведение
        _timeElapsed = TimeSpan.Zero; // Сброс таймера
        TimerTextBlock.Text = _timeElapsed.ToString(@"mm\:ss\.ff");
    }

}
