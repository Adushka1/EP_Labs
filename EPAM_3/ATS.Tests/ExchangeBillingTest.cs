using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPAM_3.BillingSystem;
using EPAM_3.BillingSystem.Enums;
using EPAM_3.BillingSystem.Interfaces;
using EPAM_3.Clients;
using EPAM_3.Clients.Enums;
using EPAM_3.Clients.Interfaces;
using EPAM_3.Phones;
using EPAM_3.Phones.Interfaces;
using EPAM_3.Repositories;
using EPAM_3.Tariffs;
using EPAM_3.Tariffs.Interfaces;
using EPAM_3.TelephoneExchange;
using EPAM_3.TelephoneExchange.Interfaces;
using EPAM_3.Terminals.Interfaces;
using Xunit;

namespace ATS.Tests
{
    public class ExchangeBillingTest
    {
        private BillingUnitOfWork _unit;
        private TimeSpan _callReceivingDelay;
        private List<Phone> _phones;
        private ITariff tariff;
        private IClient firstClient;
        private IClient secondClient;
        private IBillingExchange _billingExchange;
        private ITelephoneExchange _exchange;

        public ExchangeBillingTest()
        {
            tariff = new Tariff(1, costPerMinute: 1000, name: "Basic");
            var phoneNumbers = new int[] { 111, 222, 333 };
            _phones = phoneNumbers.Select(numb => new Phone(numb)).ToList();

            var portNumbers = new List<int>() { 10, 20 };
            var ports = portNumbers.Select(numb => new Port(numb));

            firstClient = new Client(1, tariff, _phones[0], ClientStatus.Avalible, 0, "First");
            secondClient = new Client(2, tariff, _phones[1], ClientStatus.Avalible, 0, "Second");

            var clients = new List<IClient>
            {
                firstClient,
                secondClient
            };

            _unit = new BillingUnitOfWork(
                new PhoneRepository(_phones),
                new ClientRepository(clients),
                new TariffRepository(new List<ITariff> { tariff }),
                new CallInfoRepository(new List<CallInfo>()));

            _billingExchange = new BillingExchange(_unit, TimeSpan.FromSeconds(5), (uint)DateTime.Now.Day, 1);

            _exchange = new TelephoneExchange(new HashSet<IPort>(ports), new HashSet<IPhone>(_phones),
                exchangeBilling: _billingExchange);
        }

        [Fact]
        public void SuccessfulExchangeConnectDisconnect()
        {
            var user = _phones.First();

            Assert.True(_exchange.ConnectToExchange(user));
            Assert.True(_exchange.DisconnectFromExchange(user));
        }

        [Fact]
        public void DoubleConnection()
        {
            var user = _phones.First();

            Assert.True(_exchange.ConnectToExchange(user));
            Assert.True(_exchange.ConnectToExchange(user));

            Assert.True(_exchange.DisconnectFromExchange(user));
        }

        [Fact]
        public void DoubleDisconnect()
        {
            var user = _phones.First();

            Assert.True(_exchange.ConnectToExchange(user));
            Assert.True(_exchange.DisconnectFromExchange(user));

            Assert.False(_exchange.DisconnectFromExchange(user));
        }

        [Fact]
        public void ConnectWithoutFreePorts()
        {
            _exchange.ConnectToExchange(_phones[0]);
            _exchange.ConnectToExchange(_phones[1]);

            Assert.False(_exchange.ConnectToExchange(_phones[2]));
        }

        [Fact]
        public void SuccessfulRing()
        {
            var sender = _phones[0];
            _exchange.ConnectToExchange(sender);

            var receiver = _phones[1];
            _exchange.ConnectToExchange(receiver);

            Assert.True(_exchange.ConnectAbonents(sender, receiver) == CallStatus.Connected);
            Assert.True(_exchange.DisconnectAbonents(sender, receiver) == CallStatus.Disconnected);
        }

        [Fact]
        public void RingFromDisconnectedUserTest()
        {
            var sender = _phones[0];

            var receiver = _phones[1];
            _exchange.ConnectToExchange(receiver);

            Assert.True(_exchange.ConnectAbonents(sender, receiver) == CallStatus.Error);
        }

        [Fact]
        public void RingToDisconnectedUserTest()
        {
            var sender = _phones[0];
            _exchange.ConnectToExchange(sender);

            var reciver = _phones[1];

            Assert.True(_exchange.ConnectAbonents(sender, reciver) == CallStatus.Error);
        }

        [Fact]
        public void EngagedRingTest()
        {
            var sender = _phones[0];
            _exchange.ConnectToExchange(sender);

            var receiver = _phones[1];
            _exchange.ConnectToExchange(receiver);

            _exchange.ConnectAbonents(sender, receiver);

            var engaged = _phones[2];
            _exchange.ConnectToExchange(engaged);

            Assert.True(_exchange.ConnectAbonents(engaged, sender) == CallStatus.Error);
            Assert.True(_exchange.ConnectAbonents(engaged, receiver) == CallStatus.Error);
        }

        [Fact]
        public void DoubleConnectionTest()
        {
            var sender = _phones[0];
            _exchange.ConnectToExchange(sender);

            var receiver = _phones[1];
            _exchange.ConnectToExchange(receiver);

            Assert.True(_exchange.ConnectAbonents(sender, receiver) == CallStatus.Connected);
            Assert.True(_exchange.ConnectAbonents(sender, receiver) == CallStatus.Engaged);

            Assert.True(_exchange.DisconnectAbonents(receiver, sender) == CallStatus.Disconnected);
        }

        [Fact]
        public void DoubleDisconnectionTest()
        {
            var sender = _phones[0];
            _exchange.ConnectToExchange(sender);

            var receiver = _phones[1];
            _exchange.ConnectToExchange(receiver);

            _exchange.ConnectAbonents(sender, receiver);
            _exchange.DisconnectAbonents(receiver, sender);

            Assert.True(_exchange.DisconnectAbonents(sender, receiver) == CallStatus.Error);
        }

        [Fact]
        public void SelfConnectionTest()
        {
            var sender = _phones[0];
            var receiver = _phones[0];

            _exchange.ConnectToExchange(sender);

            Assert.True(_exchange.ConnectAbonents(sender, sender) == CallStatus.Error);
        }

        [Fact]
        public void SelfDisconnection()
        {
            var sender = _phones[0];
            var receiver = _phones[0];

            _exchange.ConnectToExchange(sender);

            Assert.True(_exchange.DisconnectAbonents(sender, sender) == CallStatus.Error);
        }
    }
}
