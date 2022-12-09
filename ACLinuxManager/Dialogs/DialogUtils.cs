using System.Threading.Tasks;
using Avalonia.Controls;

namespace ACLinuxManager.Dialogs;

public static class DialogUtils
{
    public static async Task ShowDialogHideOwner(this Window window, Window owner)
    {
        owner.Hide();
        window.Show();
        bool closed = false;
        window.Closing += (sender, args) => closed = true;
        while (!closed)
            await Task.Delay(10);
        owner.Show();
    }
}