using Microsoft.Extensions.DependencyInjection;

namespace OplineShopWPFApplication;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = App.AppHost.Services.GetService<OnlineShopViewModel>();
    }
}
