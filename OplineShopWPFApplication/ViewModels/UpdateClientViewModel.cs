﻿using OnlineShopInfrastructe;
using OnlineShopModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Windows;
using System.Windows.Documents;

namespace OplineShopWPFApplication
{
    public class UpdateClientViewModel : ClientViewModel
    {
        private Client client;
        private readonly ObservableCollection<Client> clients;

        public UpdateClientViewModel(Client client, ObservableCollection<Client> clients)
        {
            SaveClient = new(UpdateClientExecute, UpdateClientCanExecute);
            this.client = client;
            this.clients = clients;
            Name = client.Name;
            Suname = client.Suname;
            Patronymic = client.Patronymic;
            Phone = client.Phone;
            Email = client.Email;
        }

        public UpdateClientViewModel(Client client, IDbAdapter? dbAdapter, ObservableCollection<Client> clients) : this(client, clients)
        {
            this.dbAdapter = dbAdapter;
        }

        private void UpdateClientExecute(object sender)
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
            client = new Client()
            {
                Name = Name,
                Suname = Suname,
                Patronymic = Patronymic,
                Phone = Phone,
                Email = Email
            };
            var c = clients.FirstOrDefault(c => c.Email == client.Email);
            client.Products = c?.Products ?? new List<Product>();
            clients.Remove(c);
            clients.Add(client);
            try
            {
                dbAdapter?.UpdateClient(client);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString(), e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Close();
        }
        private bool UpdateClientCanExecute(object sender)
        {
            return
                !string.IsNullOrWhiteSpace(Name) &&
                !string.IsNullOrWhiteSpace(Suname) &&
                !string.IsNullOrWhiteSpace(Patronymic) &&
                !string.IsNullOrWhiteSpace(Email);
        }
    }
}