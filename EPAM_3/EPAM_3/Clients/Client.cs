using System;
using System.Collections.Generic;
using System.Text;
using EPAM_3.Clients.Enums;
using EPAM_3.Clients.Interfaces;
using EPAM_3.Phones.Interfaces;
using EPAM_3.Tariffs.Interfaces;

namespace EPAM_3.Clients
{
    public class Client : IClient
    {
        public int Id { get; }
        public string Name { get; }
        public ITariff Tariff { get; }
        public IPhone Phone { get; }
        public ClientStatus Status { get; private set; }
        public decimal Balance { get; private set; }

        public Client(int id, ITariff tariff, IPhone phone, ClientStatus status, decimal balance, string name)
        {
            Id = id;
            Tariff = tariff;
            Phone = phone;
            Status = status;
            Balance = balance;
            Name = name;
        }

        public void ChangeStatus(ClientStatus status)
        {
            Status = status;
        }

        public void ChangeBalance(decimal newBalance)
        {
            Balance = newBalance;
        }
    }
}
