using OnlineShopInfrastructe;
using OnlineShopModels;
using System;
using System.Collections.ObjectModel;
using System.Net.Mail;

namespace OplineShopWPFApplication
{
    public class AddClientViewModel : ClientViewModel
    {
        private ObservableCollection<Client> clients;
        public AddClientViewModel(ObservableCollection<Client> clients, IExceptionHandler handler)
            : base(handler)
        {
            SaveClient = new(AddClientExecute, AddClientCanExecute);
            this.clients = clients;
        }

        public AddClientViewModel(ObservableCollection<Client> clients, IDbAdapter? dbAdapter, IExceptionHandler handler) : this(clients, handler)
        {
            this.dbAdapter = dbAdapter;
        }

        private void AddClientExecute(object sender)
        {
            if (!string.IsNullOrWhiteSpace(Phone) && !ruPhoneMask.IsMatch(Phone))
            {
                ExceptionHandler.ShowException($"Введите номер в формате +79998887733", $"Некорректный номер телефона {Phone}");
                return;
            }
            try
            {
                var mail = new MailAddress(Email);
            }
            catch
            {
                ExceptionHandler.ShowException($"Введите номер в формате my@email.ru", $"Некорректный email {Email}");
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
                ExceptionHandler.ShowException(e);
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