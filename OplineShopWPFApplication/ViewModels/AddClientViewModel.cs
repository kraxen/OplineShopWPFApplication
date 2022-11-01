using OnlineShopInfrastructe;
using OnlineShopModels;
using System;
using System.Collections.ObjectModel;
using System.Net.Mail;
using System.Windows;

namespace OplineShopWPFApplication
{
    public class AddClientViewModel : ClientViewModel
    {
        private ObservableCollection<Client> clients;
        public AddClientViewModel(ObservableCollection<Client> clients)
        {
            SaveClient = new(AddClientExecute, AddClientCanExecute);
            this.clients = clients;
        }

        public AddClientViewModel(ObservableCollection<Client> clients, IDbAdapter? dbAdapter) : this(clients)
        {
            this.dbAdapter = dbAdapter;
        }

        private void AddClientExecute(object sender)
        {
            if (!string.IsNullOrWhiteSpace(Phone) && !ruPhoneMask.IsMatch(Phone))
            {
                MessageBox.Show($"Введите номер в формате +79998887733", $"Некорректный номер телефона {Phone}", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var mail = new MailAddress(Email);
            }
            catch
            {
                MessageBox.Show($"Введите номер в формате my@email.ru", $"Некорректный email {Email}", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var client = new Client()
            {
                Name = Name,
                Suname = Suname,
                Patronymic = Patronymic,
                Phone = Phone,
                Email = Email,
                Products = new ObservableCollection<Product>()
            };
            clients.Add(client);
            try
            {
                dbAdapter?.AddClient(client);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            Close();
        }
        private bool AddClientCanExecute(object sender)
        {
            return
                !string.IsNullOrWhiteSpace(Name) &&
                !string.IsNullOrWhiteSpace(Suname) &&
                !string.IsNullOrWhiteSpace(Patronymic) &&
                !string.IsNullOrWhiteSpace(Email);
        }
    }
}