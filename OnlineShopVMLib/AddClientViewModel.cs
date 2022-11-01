using OnlineShopInfrastructe;
using System.Collections.ObjectModel;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows;

namespace OnlineShopVMLib
{
    internal class AddClientViewModel : BaseViewModel
    {
        private ObservableCollection<Client> clients;
        private string name;
        private string suname;
        private string patronymic;
        private string email;
        private Regex ruPhoneMask = new(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$");
        private Regex emailMask = new(@"[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}", RegexOptions.IgnoreCase);
        private IDbAdapter? dbAdapter;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                AddClient?.OnCanExecuteChanged();
            }
        }
        public string Suname
        {
            get => suname;
            set
            {
                suname = value;
                AddClient?.OnCanExecuteChanged();
            }
        }
        public string Patronymic
        {
            get => patronymic;
            set
            {
                patronymic = value;
                AddClient?.OnCanExecuteChanged();
            }
        }
        public string Phone { get; set; }
        public string Email 
        { 
            get => email;
            set
            {
                email = value;
                AddClient?.OnCanExecuteChanged();
            }
        }
        public DelegateCommand AddClient { get; set; }
        public AddClientViewModel(ObservableCollection<Client> clients)
        {
            AddClient = new(AddClientExecute, AddClientCanExecute);
            this.clients = clients;
        }

        public AddClientViewModel(ObservableCollection<Client> clients, IDbAdapter dbAdapter) : this(clients)
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
                Products = new()
            };
            clients.Add(client);
            dbAdapter?.AddClient(client);
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