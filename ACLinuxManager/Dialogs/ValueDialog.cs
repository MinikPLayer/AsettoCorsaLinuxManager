using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Themes;

namespace ACLinuxManager.Dialogs;

public class ValueDialog<T> : ThemedWindow
{
    public T DialogValue;

    public async Task<T> ShowValueDialogHideOwner(Window owner)
    {
        await this.ShowDialogHideOwner(owner);
        return DialogValue;
    }
    
    public async Task<T> ShowValueDialog(Window owner)
    {
        await base.ShowDialog(owner);
        return DialogValue;
    }
}