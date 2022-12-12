using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace ACLinuxManager.Dialogs;

public class MessageDialogViewModel : ReactiveObject
{
    private string message = "";
    public string Message
    {
        get => message;
        set => this.RaiseAndSetIfChanged(ref message, value);
    }
}

public partial class MessageValueDialog : ValueDialog<bool>
{
    public MessageValueDialog()
    {
        this.DataContext = new MessageDialogViewModel();
    }

    public MessageValueDialog(string message, string title)
    {
        this.DataContext = new MessageDialogViewModel();
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        
        this.Title = title;
        (this.DataContext as MessageDialogViewModel).Message = message;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public static async Task<bool> Show(string message, string title = "", Window? win = null)
    {
        win ??= MainWindow.Singleton;

        var box = new MessageValueDialog(message, title);
        return await box.ShowValueDialog(win);
    }

    private void Okbutton_OnClick(object? sender, RoutedEventArgs e)
    {
        DialogValue = true;
        this.Close();
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        DialogValue = false;
        this.Close();
    }
}