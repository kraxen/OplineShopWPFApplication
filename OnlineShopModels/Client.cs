using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopVMLib;
public class Client
{
    public Client()
    {
        Products = new();
    }
    public string Name { get; set; }
    public string Suname { get; set; }
    public string Patronymic { get; set; }
    public string? Phone { get; set; }
    public string Email { get; set; }
    public ObservableCollection<Product> Products { get; set; }
}