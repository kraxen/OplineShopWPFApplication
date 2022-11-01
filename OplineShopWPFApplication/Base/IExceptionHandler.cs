using System;
using System.Windows;

namespace OplineShopWPFApplication;

public interface IExceptionHandler
{
    void ShowException(Exception e);
    void ShowException(string message, string caption);
}
public class MessageBoxExceptionHandler : IExceptionHandler
{
    public void ShowException(Exception e)
    {
        MessageBox.Show(e.ToString(), e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public void ShowException(string message, string caption)
    {
        MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
    }
}