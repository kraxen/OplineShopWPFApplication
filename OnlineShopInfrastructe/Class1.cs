using OnlineShopVMLib;

namespace OnlineShopInfrastructe
{
    public interface IDbAdapter
    {
        IEnumerable<Client> Clients { get; set; }
        IEnumerable<Product> Products { get; set; }
        void SaveChanges();
        IEnumerable<Client> Where(Func<Client, bool> func);
        IEnumerable<Product> Where(Func<Product, bool> func);
        void AddClient(Client client);
        void RemoveClient(Client client);
        void UpdateClient(Client client);
        void AddProduct(Product product);
        void RemoveProduct(Product product);
        void UpdateProduct(Product product);
    }
}