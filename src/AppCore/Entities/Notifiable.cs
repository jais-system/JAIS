using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AppCore.Entities;

public class Notifiable : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected void Set<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(storage, value))
        {
            return;
        }

        storage = value;

        if (!string.IsNullOrEmpty(propertyName))
        {
            OnPropertyChanged(propertyName);
        }
    }
    
    private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
