using OnlineShopDB;
using OnlineShopInfrastructe;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace OplineShopWPFApplication;
public partial class App
{
    public static IDbAdapter Db { get; internal set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        Db = new OnlineShopDbContext();
        base.OnStartup(e);
    }
    protected async override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        await Task.Run(() =>
        {
            Db.SaveChanges();
        });
    }
}
