using OnlineShopInfrastructe;
using OnlineShopModels;
using OplineShopWPFApplication.Windows;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace OplineShopWPFApplication;

public class OnlineShopViewModel : BaseViewModel
{
    private Client selectedClient;
    private Product selectedProduct;
    private IDbAdapter dbAdapter;
    private string searchText;


    public OnlineShopViewModel(IDbAdapter dbAdapter, IExceptionHandler handler)
        :base(handler)
    {
        this.dbAdapter = dbAdapter;
        try
        {
            Clients = new ObservableCollection<Client>(dbAdapter.Clients);
        }
        catch (Exception e)
        {
            ExceptionHandler.ShowException(e);
            return;
        }
        SelectedClient = Clients?.FirstOrDefault();
        if (SelectedClient is null) Products = new();
        else Products = new(SelectedClient.Products);
        SelectedProduct = SelectedClient?.Products?.FirstOrDefault();
        OpenAddClientWindow = new(OpenAddClientWindowExecute, (s) => true);
        OpenAddProductWindow = new(OpenAddProductWindowExecute, (s) => SelectedClient is not null);
        RemoveClient = new(RemoveClientExecute, (s) => SelectedClient is not null);
        RemoveProduct = new(RemoveProductExecute, (s) => SelectedProduct is not null);
        Search = new(SearchExecute, (s) => !string.IsNullOrWhiteSpace(SearchText));
        ClearSearch = new(ClearSearchExecute, (s) => true);
        UpdateClient = new(UpdateClientExecute, (s) => SelectedClient is not null);
    }

    public ObservableCollection<Client> Clients { get; set; }
    public ObservableCollection<Product> Products { get; set; }
    public Client? SelectedClient
    {
        get => selectedClient;
        set
        {
            selectedClient = value!;
            if (SelectedClient is null) Products = new();
            else Products = new(SelectedClient.Products);
            SelectedProduct = Products.FirstOrDefault();
            RemoveClient?.OnCanExecuteChanged();
            OpenAddProductWindow?.OnCanExecuteChanged();
            RemoveProduct?.OnCanExecuteChanged();
        }
    }
    public Product? SelectedProduct
    {
        get => selectedProduct;
        set
        {
            selectedProduct = value!;
            OpenAddProductWindow?.OnCanExecuteChanged();
            RemoveProduct?.OnCanExecuteChanged();
        }
    }
    public string SearchText
    { 
        get => searchText;
        set 
        { 
            searchText = value;
            Search?.OnCanExecuteChanged();
        }
    }
    public DelegateCommand OpenAddClientWindow { get; set; }
    public DelegateCommand OpenAddProductWindow { get; set; }
    public DelegateCommand RemoveClient { get; set; }
    public DelegateCommand RemoveProduct { get; set; }
    public DelegateCommand Search { get; set; }
    public DelegateCommand ClearSearch { get; set; }
    public DelegateCommand UpdateClient { get; set; }

    private void OpenAddClientWindowExecute(object sender)
    {
        BaseWindowFactory<ClientWindow, AddClientViewModel>.Get()?.ShowDialog();
    }
    private void OpenAddProductWindowExecute(object sender)
    {
        BaseWindowFactory<AddProductWindow, AddProductViewModel>.Get()?.ShowDialog();
        Products = new(SelectedClient.Products);
    }

    private void RemoveClientExecute(object sender)
    {
        dbAdapter.RemoveClient(SelectedClient!);
        Clients.Remove(SelectedClient!);
        SelectedClient = Clients.FirstOrDefault();
    }
    private void RemoveProductExecute(object sender)
    {
        dbAdapter?.RemoveProduct(SelectedProduct);
        var products = SelectedClient!.Products.ToList();
        products.Remove(SelectedProduct!);
        SelectedClient.Products = products;
        Products = new(SelectedClient.Products);
        SelectedProduct = Products.FirstOrDefault();
    }
    private void ClearSearchExecute(object sender)
    {
        Clients = new(dbAdapter?.Clients!);
        SearchText = "";
    }
    private async void SearchExecute(object sender)
    {
        
        var searchClients = await Task.Run(() =>
        {
            // Из БД (Для задания)
            return dbAdapter?.Where(new Func<Client, bool>(
                c => 
                c.Email.Contains(SearchText) ||
                c.Name.Contains(SearchText) ||
                c.Suname.Contains(SearchText) ||
                c.Patronymic.Contains(SearchText) ||
                c.Phone?.Contains(SearchText) is true
                ));

            // Из кэша (Лучше)
            //return Clients.Where(new Func<Client, bool>(
            //    c =>
            //    c.Email.Contains(SearchText) ||
            //    c.Name.Contains(SearchText) ||
            //    c.Suname.Contains(SearchText) ||
            //    c.Patronymic.Contains(SearchText) ||
            //    c.Phone?.Contains(SearchText) is true
            //    ));
        });

        Clients = new(searchClients!);
    }
    private void UpdateClientExecute(object sender)
    {
        BaseWindowFactory<ClientWindow, UpdateClientViewModel>.Get()?.ShowDialog();
        SelectedClient = Clients.FirstOrDefault();
    }
}
