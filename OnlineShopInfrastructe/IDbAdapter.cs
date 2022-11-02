using OnlineShopModels;

namespace OnlineShopInfrastructe
{
    public interface IDbAdapter
    {
        IEnumerable<Client> GetClients();
        IEnumerable<Product> GetProducts();
        void SaveChanges();
        void AddClient(Client client);
        void RemoveClient(string email);
        void UpdateClient(Client newClient);
        void AddProduct(Product product);
        void RemoveProduct(int id);
        void UpdateProduct(Product product);
    }
}