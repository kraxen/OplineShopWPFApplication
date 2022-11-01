using OnlineShopInfrastructe;
using OnlineShopVMLib;

namespace OplineShopWPFApplication;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new OnlineShopViewModel(App.Db);
    }
}
