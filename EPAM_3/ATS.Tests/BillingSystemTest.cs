using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using EPAM_3;
using EPAM_3.BillingSystem;
using EPAM_3.BillingSystem.EventArgs;
using EPAM_3.BillingSystem.Interfaces;
using EPAM_3.Clients;
using EPAM_3.Clients.Enums;
using EPAM_3.Clients.Interfaces;
using EPAM_3.Phones;
using EPAM_3.Phones.Interfaces;
using EPAM_3.Repositories;
using EPAM_3.Repositories.Interfaces;
using EPAM_3.Tariffs;
using EPAM_3.Tariffs.Interfaces;

namespace ATS.Tests
{
    public class BillingSystemTest
    {
        private BillingUnitOfWork _unit;
        private IBillingExchange billingExchange;
        private IClient firstClient;
        private IClient secondClient;
        private List<IPhone> phones;
        private ITariff tariff;

        public BillingSystemTest()
        {
            phones = new List<IPhone> { new Phone(1), new Phone(2) };
            tariff = new Tariff(1, costPerMinute: 1000, name: "Basic");

            firstClient = new Client(1, tariff, phones[0], ClientStatus.Avalible, 0, "First");
            secondClient = new Client(2, tariff, phones[1], ClientStatus.Avalible, 0, "Second");

            var clients = new List<IClient>
            {
                firstClient,
                secondClient
            };

            _unit = new BillingUnitOfWork(
                new PhoneRepository(phones),
                new ClientRepository(clients),
                new TariffRepository(new List<ITariff> { tariff }),
                new CallInfoRepository(new List<CallInfo>()));
            billingExchange = new BillingExchange(_unit, TimeSpan.FromSeconds(5), (uint)DateTime.Now.Day, 1);
        }

        [Fact]
        public void BasicTest()
        {
            var callEventArgs = new RingEventArgs(firstClient.Phone, secondClient.Phone);
            billingExchange.AbonentsConnectedEventHandler(null, callEventArgs);

            Task.Delay(20000).Wait();

            billingExchange.AbonentsDisconnectedEventHandler(null, callEventArgs);
            Task.Delay(21000).Wait();

            Assert.True(_unit.CallInfos.Count > 0);
            Assert.True(_unit.CallInfos.GetEntities(b => b.Checked).Any());
            Task.Delay(5000).Wait();

            Assert.True(firstClient.Balance < 0);
            Task.Delay(3000).Wait();

            Assert.True(secondClient.Balance == 0);
        }
    }
}
