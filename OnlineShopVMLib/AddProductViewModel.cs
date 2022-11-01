using OnlineShopInfrastructe;
using System.Collections.ObjectModel;

namespace OnlineShopVMLib
{
    internal class AddProductViewModel : BaseViewModel
    {
        private Client client;
        private string name;
        private IDbAdapter? dbAdapter;

        public int Kod { get; set; }
        public string Name 
        { 
            get => name;
            set
            {
                name = value;
                AddProduct?.OnCanExecuteChanged();
            }
        }
        public DelegateCommand AddProduct { get; set; }

        public AddProductViewModel(Client client)
        {
            this.client = client;
            AddProduct = new(AddProductExcecute, AddProductCanExcecute);
        }

        public AddProductViewModel(Client client, IDbAdapter dbAdapter) : this(client)
        {
            this.dbAdapter = dbAdapter;
        }

        private void AddProductExcecute(object sender)
        {
            var product = new Product()
            {
                Email = client.Email,
                Name = Name,
                Kod = Kod,
                Client = client
            };
            client.Products.Append(product);
            dbAdapter?.AddProduct(product);
            Close();
        }
        private bool AddProductCanExcecute(object sender)
        {
            return !string.IsNullOrWhiteSpace(Name);
        }
    }
}