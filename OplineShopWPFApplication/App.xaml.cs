using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineShopDB;
using OnlineShopInfrastructe;
using OplineShopWPFApplication.Windows;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace OplineShopWPFApplication;
public partial class App
{
    public static IHost AppHost { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IDbAdapter, OnlineShopDbContext>();
                services.AddSingleton<OnlineShopViewModel>();

                services.AddTransient((serviceProvider) => 
                    new AddClientViewModel(serviceProvider.GetService<OnlineShopViewModel>()!.Clients,
                                            serviceProvider.GetService<IDbAdapter>()));
                services.AddTransient((serviceProvider) =>
                {
                    var w = new AddClientWindow();
                    var dc = serviceProvider.GetService<AddClientViewModel>();

                    void OnClosed(object? sender, EventArgs e)
                    {
                        dc.OnClosed -= OnClosed;
                        w.Close();
                    }

                    dc!.OnClosed += OnClosed;
                    w.DataContext = dc;

                    return w;
                });

                services.AddTransient((serviceProvider) =>
                {
                    return new AddProductViewModel(serviceProvider.GetService<OnlineShopViewModel>()!.SelectedClient!,
                                                    serviceProvider.GetService<IDbAdapter>()!);
                });
                services.AddTransient((serviceProvider) =>
                {
                    var w = new AddProductWindow();
                    var dc = serviceProvider.GetService<AddProductViewModel>();

                    void OnClosed(object? sender, EventArgs e)
                    {
                        dc.OnClosed -= OnClosed;
                        w.Close();
                    }

                    dc!.OnClosed += OnClosed;
                    w.DataContext = dc;

                    return w;
                });
            })
            .Build();
        base.OnStartup(e);
    }
    protected async override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        await Task.Run(() =>
        {
            AppHost.Services.GetService<IDbAdapter>()?.SaveChanges();
        });
    }
}
