using OnlineShopInfrastructe;
using OnlineShopModels;
using System;
using System.Linq;
using System.Windows;

namespace OplineShopWPFApplication
{
    public class AddProductViewModel : BaseViewModel
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
            try
            {
                dbAdapter?.AddProduct(product);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            client.Products.Append(product);
            Close();
        }
        private bool AddProductCanExcecute(object sender)
        {
            return !string.IsNullOrWhiteSpace(Name);
        }
    }
}