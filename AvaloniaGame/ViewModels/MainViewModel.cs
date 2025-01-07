using System;
using System.Windows.Input;
using AvaloniaGame.GameLogic;
using ReactiveUI;

namespace AvaloniaGame.ViewModels;

public class MainViewModel : ViewModelBase
{
    public string Greeting => "Hello world!";

    private bool _isPopupVisible = false;
    private bool _isWaiting = false;
    private bool _isMenuVisible = true;
    private bool _isMultiplayerMenuVisible = false;

    public bool IsPopupVisible {
        get => _isPopupVisible;
        set => this.RaiseAndSetIfChanged(ref _isPopupVisible, value);
    }
    public bool IsWaiting
    {
        get => _isWaiting;
        set => this.RaiseAndSetIfChanged(ref _isWaiting, value);
    }
    public bool IsMenuVisible
    {
        get => _isMenuVisible;
        set => this.RaiseAndSetIfChanged(ref _isMenuVisible, value);
    }
    public bool IsMultiplayerMenuVisible
    {
        get => _isMultiplayerMenuVisible;
        set => this.RaiseAndSetIfChanged(ref _isMultiplayerMenuVisible, value);
    }
    public bool IsPause
    {
        get => IsWaiting || IsMenuVisible || IsMultiplayerMenuVisible || IsPopupVisible;
    }
    public ICommand OnEsc { get; private set; }

    public ICommand OnEasy { get; private set; }
    public ICommand OnMedium { get; private set; }
    public ICommand OnHard { get; private set; }
    public ICommand OnExit { get; private set; }
    public ICommand OnSingleplayer { get; private set; }
    public ICommand OnMultiplayer {  get; private set; }
    public ICommand OnClient { get; private set; }
    public ICommand OnHost {  get; private set; }
    public MainViewModel()
    {
        OnEsc = ReactiveCommand.Create(
            () => IsPopupVisible = !IsPopupVisible
        );
        OnEasy = ReactiveCommand.Create(
            () => {
                if (MainLogic.isMultiplayer)
                    MainLogic.networkManager.SendMessage("Difficulty: 0");
                MainLogic.difficulty = 0;
                MainLogic.finishFlag = true;
                IsPopupVisible = false;
            }
        );
        OnMedium = ReactiveCommand.Create(
            () => {
                if(MainLogic.isMultiplayer)
                    MainLogic.networkManager.SendMessage("Difficulty: 1");
                MainLogic.difficulty = 1;
                MainLogic.finishFlag = true;
                IsPopupVisible = false;
            }
        );
        OnHard = ReactiveCommand.Create(
            () => {
                if (MainLogic.isMultiplayer)
                    MainLogic.networkManager.SendMessage("Difficulty: 2");
                MainLogic.difficulty = 2;
                MainLogic.finishFlag = true;
                IsPopupVisible = false;
            }
        );
        OnExit = ReactiveCommand.Create(
            MainLogic.mainWindow.Close
        );
        OnSingleplayer = ReactiveCommand.Create(
            () =>
            {
                IsMenuVisible = false;
                MainLogic.StartSingleplayer();
            }
        );
        OnMultiplayer = ReactiveCommand.Create(
            () =>
            {
                IsMenuVisible = false;
                IsMultiplayerMenuVisible = true;
            }
        );
        OnHost = ReactiveCommand.Create(
            () =>
            {
                IsMultiplayerMenuVisible = false;
                IsWaiting = true;
                MainLogic.StartMultiplayer(true);
            }
        );
        OnClient = ReactiveCommand.Create(
            () =>
            {
                IsMultiplayerMenuVisible = false;
                IsWaiting = true;
                MainLogic.StartMultiplayer(false);
            }
        );
    }
}
