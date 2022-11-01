using OnlineShopInfrastructe;
using System.Text.RegularExpressions;

namespace OplineShopWPFApplication
{
    public class ClientViewModel : BaseViewModel
    {
        private string name;
        private string suname;
        private string patronymic;
        private string email;
        protected Regex ruPhoneMask = new(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$");
        protected IDbAdapter? dbAdapter;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                SaveClient?.OnCanExecuteChanged();
            }
        }
        public string Suname
        {
            get => suname;
            set
            {
                suname = value;
                SaveClient?.OnCanExecuteChanged();
            }
        }
        public string Patronymic
        {
            get => patronymic;
            set
            {
                patronymic = value;
                SaveClient?.OnCanExecuteChanged();
            }
        }
        public string Phone { get; set; }
        public string Email
        {
            get => email;
            set
            {
                email = value;
                SaveClient?.OnCanExecuteChanged();
            }
        }
        public DelegateCommand SaveClient { get; set; }
    }
}