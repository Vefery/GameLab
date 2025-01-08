using System;
using System.Windows.Input;
using AvaloniaGame.GameLogic;
using ReactiveUI;

namespace AvaloniaGame.ViewModels;

public class MainViewModel : ViewModelBase
{
    private bool _isPopupVisible = false;
    private bool _isWaiting = false;
    private bool _isMenuVisible = true;
    private bool _isMultiplayerMenuVisible = false;
    private bool _isDifficultyMenuVisible = false;
    private bool _isFinishScreenVisible = false;
    private bool _isConnectMenuVisible = false;
    private bool _isMultiplayer;
    private string _finishText;
    private bool _isHost;

    public string FinishText
    {
        get => _finishText;
        set => this.RaiseAndSetIfChanged(ref _finishText, value);
    }
    public bool IsPopupVisible {
        get => _isPopupVisible;
        set
        {
            this.RaiseAndSetIfChanged(ref _isPopupVisible, value);
            MainLogic.mainWindow.pauseTimer = value;
        }
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
    public bool IsDifficultyMenuVisible
    {
        get => _isDifficultyMenuVisible;
        set => this.RaiseAndSetIfChanged(ref _isDifficultyMenuVisible, value);
    }
    public bool IsFinishScreenVisible
    {
        get => _isFinishScreenVisible;
        set
        {
            this.RaiseAndSetIfChanged(ref _isFinishScreenVisible, value);
            MainLogic.mainWindow.pauseTimer = value;
        }
    }
    public bool IsHost
    {
        get => _isHost;
        set => this.RaiseAndSetIfChanged(ref _isHost, value);
    }
    public bool IsConnectMenuVisible
    {
        get => _isConnectMenuVisible;
        set => this.RaiseAndSetIfChanged(ref _isConnectMenuVisible, value);
    }
    public bool IsPause
    {
        get => IsWaiting || IsMenuVisible || IsMultiplayerMenuVisible || IsPopupVisible || IsDifficultyMenuVisible || IsFinishScreenVisible;
    }
    public ICommand OnEsc { get; private set; }

    public ICommand OnEasy { get; private set; }
    public ICommand OnMedium { get; private set; }
    public ICommand OnHard { get; private set; }
    public ICommand OnExit { get; private set; }
    public ICommand OnSingleplayer { get; private set; }
    public ICommand OnMultiplayer {  get; private set; }
    public ICommand OnClient { get; private set; }
    public ICommand OnConnect { get; private set; }
    public ICommand OnHost {  get; private set; }
    public ICommand OnRestart { get; private set; }
    public MainViewModel()
    {
        OnEsc = ReactiveCommand.Create(
            () => IsPopupVisible = !IsPopupVisible
        );
        OnEasy = ReactiveCommand.Create(
            () => {
                MainLogic.difficulty = 0;
                if (_isMultiplayer)
                {
                    IsWaiting = true;
                    MainLogic.StartMultiplayerAsHost();
                    MainLogic.networkManager.SendMessage("Difficulty: 0");
                }
                else
                    MainLogic.StartSingleplayer();

                IsDifficultyMenuVisible = false;
            }
        );
        OnMedium = ReactiveCommand.Create(
            () => {
                MainLogic.difficulty = 1;
                if (_isMultiplayer)
                {
                    IsWaiting = true;
                    MainLogic.StartMultiplayerAsHost();
                    MainLogic.networkManager.SendMessage("Difficulty: 1");
                }
                else
                    MainLogic.StartSingleplayer();

                IsDifficultyMenuVisible = false;
            }
        );
        OnHard = ReactiveCommand.Create(
            () => {
                MainLogic.difficulty = 2;
                if (_isMultiplayer)
                {
                    IsWaiting = true;
                    MainLogic.StartMultiplayerAsHost();
                    MainLogic.networkManager.SendMessage("Difficulty: 2");
                }
                else
                    MainLogic.StartSingleplayer();

                IsDifficultyMenuVisible = false;
            }
        );
        OnExit = ReactiveCommand.Create(
            MainLogic.mainWindow.Close
        );
        OnSingleplayer = ReactiveCommand.Create(
            () =>
            {
                IsMenuVisible = false;
                IsDifficultyMenuVisible = true;
                _isMultiplayer = false;
            }
        );
        OnMultiplayer = ReactiveCommand.Create(
            () =>
            {
                IsMenuVisible = false;
                IsMultiplayerMenuVisible = true;
                _isMultiplayer = true;
            }
        );
        OnHost = ReactiveCommand.Create(
            () =>
            {
                IsHost = true;
                IsMultiplayerMenuVisible = false;
                IsDifficultyMenuVisible = true;
            }
        );
        OnClient = ReactiveCommand.Create(
            () =>
            {
                IsHost = false;
                IsMultiplayerMenuVisible = false;
                IsConnectMenuVisible = true;
            }
        );
        OnConnect = ReactiveCommand.Create(
            (string ip) =>
            {
                IsConnectMenuVisible = false;
                IsWaiting = true;
                MainLogic.StartMultiplayerAsClient(ip);
            }
        );
        OnRestart = ReactiveCommand.Create(
            () =>
            {
                IsFinishScreenVisible = false;
                MainLogic.networkManager.SendMessage("Restart: ");
            }
        );
    }
}
