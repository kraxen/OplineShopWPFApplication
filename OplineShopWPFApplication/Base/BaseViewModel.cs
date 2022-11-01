using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OplineShopWPFApplication;

public class BaseViewModel : INotifyPropertyChanged
{
    public event EventHandler? OnClosed;
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    protected void Close()
    {
        OnClosed?.Invoke(null, null);
    }
}