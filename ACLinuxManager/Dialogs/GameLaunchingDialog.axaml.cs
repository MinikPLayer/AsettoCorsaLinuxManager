using ACLinuxManager.Settings;
using ACLinuxManager.Utils;
using AcUtils.Game;
using Avalonia;
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

    private string loadingMessage = "Waiting for the game process to exit...";
    public string LoadingMessage
    {
        get => loadingMessage;
        set => this.RaiseAndSetIfChanged(ref loadingMessage, value);
    }
}

public partial class GameLaunchingDialog : ThemedWindow
{
    private bool lockClosing = true;
    private GameLaunchingViewModel ViewModel => (DataContext as GameLaunchingViewModel)!;
    
    public GameLaunchingDialog()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        ProcessUtils._RunIfNotAvalonia(LaunchGame);
    }

    public async void LaunchGame()
    {
        var result = GameStarter.StartGameProcess(GameInfoSettings.GameRootPath, () =>
        {
            ViewModel.IsLoading = false;
            lockClosing = false;
            ViewModel.LoadingMessage = "Game process exited";
        });

        if (!result.Good)
        {
            ViewModel.IsLoading = false;
            lockClosing = false;
            ViewModel.LoadingMessage = "Game process start failed. Error: " + result.Message;
        }
    }

    protected override bool HandleClosing()
    {
        if (lockClosing)
        {
            ViewModel.LoadingMessage = "Shutting down game process...";
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
        if(!lockClosing)
            this.Close();
    }
}