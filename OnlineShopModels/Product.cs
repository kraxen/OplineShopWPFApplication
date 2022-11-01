namespace OnlineShopVMLib;

public class Product
{
    public int Id { get; set; }
    public int Kod { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Client Client { get; set; }
}