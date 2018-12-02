using System;
using System.Collections.Generic;
using System.Text;
using EPAM_3.Clients;
using EPAM_3.Clients.Interfaces;
using EPAM_3.Phones;
using EPAM_3.Phones.Interfaces;
using EPAM_3.Repositories;
using EPAM_3.Repositories.Interfaces;
using EPAM_3.Tariffs;
using EPAM_3.Tariffs.Interfaces;

namespace EPAM_3.BillingSystem
{
    public class BillingUnitOfWork
    {
        public PhoneRepository Phones { get; set; }
        public ClientRepository Clients { get; set; }
        public TariffRepository Tariffs { get; set; }
        public CallInfoRepository CallInfos { get; set; }

        public BillingUnitOfWork(PhoneRepository phones, ClientRepository clients, TariffRepository tariffs, CallInfoRepository callInfos)
        {
            Phones = phones;
            Clients = clients;
            Tariffs = tariffs;
            CallInfos = callInfos;
        }
    }
}
