using System;
using System.Collections.Generic;
using System.Text;
using EPAM_3.Clients.Enums;
using EPAM_3.Phones.Interfaces;
using EPAM_3.Tariffs.Interfaces;

namespace EPAM_3.Clients.Interfaces
{
    public interface IClient
    {
        int Id { get; }
        ITariff Tariff { get; }
        string Name { get; }
        IPhone Phone { get; }
        ClientStatus Status { get; }
        decimal Balance { get; }

        void ChangeStatus(ClientStatus status);
        void ChangeBalance(decimal newBalance);
    }
}
