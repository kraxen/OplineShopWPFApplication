using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace OplineShopWPFApplication;
public static class BaseWindowFactory<W,T>
    where W : Window, new()
    where T: BaseViewModel
{
    public static W Get()
    {
        W w = new();
        var dc = App.AppHost.Services.GetService<T>();

        void OnClosed(object? sender, EventArgs e)
        {
            dc.OnClosed -= OnClosed;
            w.Close();
        }

        dc!.OnClosed += OnClosed;
        w.DataContext = dc;

        return w;
    }
}
