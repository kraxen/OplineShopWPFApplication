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
                services.AddScoped<IDbAdapter, OnlineShopDbContext>();
                services.AddSingleton<OnlineShopViewModel>();

                services.AddTransient((serviceProvider) => 
                    new AddClientViewModel(serviceProvider.GetService<OnlineShopViewModel>()!.Clients,
                                            serviceProvider.GetService<IDbAdapter>()));

                services.AddTransient((serviceProvider) => new AddProductViewModel(serviceProvider.GetService<OnlineShopViewModel>()!.SelectedClient!,
                                                    serviceProvider.GetService<IDbAdapter>()!));
                services.AddTransient((serviceProvider) => BaseWindowFactory<AddProductWindow, AddProductViewModel>.Get());
                services.AddTransient((serviceProvider) =>
                {
                    var vm = serviceProvider.GetService<OnlineShopViewModel>()!;
                    return new UpdateClientViewModel(vm.SelectedClient!, serviceProvider.GetService<IDbAdapter>(), vm.Clients);
                });
                services.AddTransient((serviceProvider) => BaseWindowFactory<ClientWindow, UpdateClientViewModel>.Get());
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
