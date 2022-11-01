namespace OnlineShopVMLib;
public class Client
{
    public Client()
    {
    }
    public string Name { get; set; }
    public string Suname { get; set; }
    public string Patronymic { get; set; }
    public string? Phone { get; set; }
    public string Email { get; set; }
    public IEnumerable<Product> Products { get; set; }
}