using Avalonia.Controls;

namespace JAIS.Extensions;

internal static class UserControlExtensions
{
    public static void ShowDialog(this UserControl userControl, UserControl dialog)
    {
        MainView.ShowDialog(dialog);
    }
}