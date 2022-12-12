using ACLinuxManager.Utils;
using Avalonia.Themes;

namespace ACLinuxManager;

public partial class MainWindow : ThemedWindow
{
    public static MainWindow Singleton = null!;
    
    public MainWindow()
    {
        Singleton = this;
        InitializeComponent();
    }
}