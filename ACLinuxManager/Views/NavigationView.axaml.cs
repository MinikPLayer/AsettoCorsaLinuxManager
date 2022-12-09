using System.Collections.Generic;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ACLinuxManager.Views;

public partial class NavigationView : UserControl
{
    public static NavigationView? Singleton;

    private static readonly Stack<UserControl?> ObjectsStack = new Stack<UserControl?>();

    public NavigationView()
    {
        InitializeComponent();
        Singleton = this;
        Navigate<DriveView>();
    }

    static void ChangeScreen(UserControl obj)
    {
        if (Singleton != null)
        {
            Singleton.ContentFrame.Content = obj;
            Singleton.PageName.Text = obj.Name;
        }
    }
    
    public static void Navigate<T>(bool forceChange = false) where T : UserControl, new()
    {
        if (Singleton?.ContentFrame.Content != null)
        {
            if (!forceChange && Singleton?.ContentFrame.Content.GetType() == typeof(T))
                return;
            
            ObjectsStack.Push(Singleton?.ContentFrame.Content as UserControl);
        }
        else
            Debug.WriteLine("Content is null");
        
        var obj = new T();
        ChangeScreen(obj);
    }

    public static bool GoBack()
    {
        if (!ObjectsStack.TryPop(out var obj))
            return false;

        if (obj == null)
            return false;
        
        ChangeScreen(obj);
        return true;
    }

    private void DriveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Navigate<SettingsView>();
    }

    private void NavBackButton_OnClick(object? sender, RoutedEventArgs e)
    {
        GoBack();
    }
}