using ACLinuxManager.Dialogs;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace ACLinuxManager.Views;

public partial class DriveView : UserControl
{
    public DriveView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void TestStartButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var dialog = new GameLaunchingDialog();
        _ = dialog.ShowDialogHideOwner(MainWindow.Singleton);
    }
}