using System;
using System.Windows.Input;
using AvaloniaGame.GameLogic;
using ReactiveUI;

namespace AvaloniaGame.ViewModels;

public class MainViewModel : ViewModelBase
{
    public string Greeting => "Hello world!";

    private bool _isPopupVisible;
    public bool IsPopupVisible {
        get => _isPopupVisible;
        set => this.RaiseAndSetIfChanged(ref _isPopupVisible, value);
    }
    public ICommand OnEsc { get; private set; }

    public ICommand OnEasy { get; private set; }
    public ICommand OnMedium { get; private set; }
    public ICommand OnHard { get; private set; }
    public ICommand OnExit { get; private set; }
    public MainViewModel()
    {
        OnEsc = ReactiveCommand.Create(
            () => IsPopupVisible = !IsPopupVisible
        );
        OnEasy = ReactiveCommand.Create(
            () => {
                MainLogic.difficulty = 0;
                MainLogic.finishFlag = true;
                IsPopupVisible = false;
            }
        );
        OnMedium = ReactiveCommand.Create(
            () => {
                MainLogic.difficulty = 1;
                MainLogic.finishFlag = true;
                IsPopupVisible = false;
            }
        );
        OnHard = ReactiveCommand.Create(
            () => {
                MainLogic.difficulty = 2;
                MainLogic.finishFlag = true;
                IsPopupVisible = false;
            }
        );
        OnExit = ReactiveCommand.Create(
            MainLogic.mainWindow.Close
        );
        
    }
}
