using System;
using System.Diagnostics;
using System.Threading;
using ACLinuxManager.Settings;
using ACLinuxManager.Utils;
using AcUtils.Game;
using AcUtils.Utils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Themes;
using ReactiveUI;

namespace ACLinuxManager.Dialogs;

public class GameLaunchingViewModel : ReactiveObject
{
    private bool _isLoading = true;
    public bool IsLoading
    {
        get => _isLoading;
        set => this.RaiseAndSetIfChanged(ref _isLoading, value);
    }

    private string _loadingMessage = "Please wait...";
    public string LoadingMessage
    {
        get => _loadingMessage;
        set => this.RaiseAndSetIfChanged(ref _loadingMessage, value);
    }
}

public partial class GameLaunchingDialog : ThemedWindow
{
    private bool _lockClosing = true;
    private GameLaunchingViewModel ViewModel => (DataContext as GameLaunchingViewModel)!;

    private CancellationTokenSource? _tokenSource = null;
    private Process? _gameProcess = null;
    
    public GameLaunchingDialog()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        ProcessUtils._RunIfNotAvalonia(LaunchGame);
    }

    void OnGameClose(string msg)
    {
        ViewModel.IsLoading = false;
        _lockClosing = false;
        ViewModel.LoadingMessage = msg;
    }
    
    public async void LaunchGame()
    {
        try
        {
            _tokenSource = new CancellationTokenSource();

            ViewModel.LoadingMessage = "Starting game process...";
            var result = GameStarter.StartGameProcess();
            if (!result.Good)
                OnGameClose("Game process start failed. Error: " + result.Message);

            _gameProcess =
                (await GameStarter.WaitUntilGameLaunches(_tokenSource.Token)).Expect("Game launch cancelled");
            ViewModel.LoadingMessage = "Game process launched, waiting for exit...";

            var closeResult =
                (await GameStarter.WaitUntilGameExits(_gameProcess, _tokenSource.Token)).Expect("Game closed");
            
            OnGameClose("Game process exited");
        }
        catch (ResultException e)
        {
            OnGameClose(e.Message);
        }

    }

    public void KillGameProcess()
    {
        if (_gameProcess == null || _tokenSource == null)
            return;
        
        _tokenSource.Cancel();
        var ret = GameStarter.KillProcess(_gameProcess);
        if(!ret.Good)
            OnGameClose("Cannot kill the game process");
    }

    protected override bool HandleClosing()
    {
        if (_lockClosing)
        {
            ViewModel.LoadingMessage = "Shutting down game process...";
            KillGameProcess();
            _tokenSource?.Cancel();
            return true;
        }

        base.HandleClosing();
        return false;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void CloseButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if(!_lockClosing)
            this.Close();
    }
}