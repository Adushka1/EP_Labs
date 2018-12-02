using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using EPAM_3.Terminals;
using EPAM_3.Terminals.Interfaces;
using Xunit;

namespace ATS.Tests
{
    public class TerminalTest
    {
        private BillingUnitOfWork _unit;
        private TimeSpan _callReceivingDelay;
        private ITerminal _senderTerminal;
        private ITerminal _receiverTerminal;
        private List<Phone> _phones;
        private ITariff tariff;
        private IClient firstClient;
        private IClient secondClient;
        private IBillingExchange billingExchange;
        private IInfoBuilder _infoBuilder;

        public TerminalTest()
        {
            tariff = new Tariff(1, costPerMinute: 1000, name: "Basic");
            var phoneNumbers = new int[] { 111, 222 };
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
            billingExchange = new BillingExchange(_unit, TimeSpan.FromSeconds(5), (uint)DateTime.Now.Day, 1);
            _infoBuilder = new InfoBuilder(_unit);

            var exchange = new TelephoneExchange(new HashSet<IPort>(ports), new HashSet<IPhone>(_phones),
                exchangeBilling: billingExchange);

            _callReceivingDelay = TimeSpan.FromMilliseconds(10000);
            _senderTerminal = new Terminal(_phones[0], exchange, _callReceivingDelay, _infoBuilder);
            _receiverTerminal = new Terminal(_phones[1], exchange, _callReceivingDelay, _infoBuilder);
        }

        [Fact]
        public void SuccessfulCall()
        {
            Assert.True(_senderTerminal.ConnectToExchange());
            Assert.True(_receiverTerminal.ConnectToExchange());

            Assert.True(_senderTerminal.MakeCall(_receiverTerminal.PhoneNumber) == CallStatus.Connected);

            Assert.True(_receiverTerminal.ReceiveCall() == CallStatus.Connected);

            Task.Delay(1000).Wait();
            Assert.True(_receiverTerminal.CloseCall() == CallStatus.Disconnected);

            Assert.True(_senderTerminal.DisconnectFromExchange());
            Assert.True(_receiverTerminal.DisconnectFromExchange());
        }

        [Fact]
        public void CallNotReceived()
        {
            Assert.True(_senderTerminal.ConnectToExchange());
            Assert.True(_receiverTerminal.ConnectToExchange());

            Assert.True(_senderTerminal.MakeCall(_receiverTerminal.PhoneNumber) == CallStatus.Connected);

            Task.Delay((int)_callReceivingDelay.TotalMilliseconds + 1000).Wait();
            Assert.True(_receiverTerminal.ReceiveCall() == CallStatus.Disconnected);
        }

        [Fact]
        public void NonexistentPhoneCall()
        {
            _senderTerminal.ConnectToExchange();

            var FakePhone = new Phone(000);

            Assert.True(_senderTerminal.MakeCall(FakePhone) == CallStatus.Error);
        }

        [Theory]
        [InlineData(20, true)]
        [InlineData(30, false)]
        public void ChangePorts(int portValue, bool isSuccessful)
        {
            var successStatus = _senderTerminal.ChangePort(new Port(portValue));

            Assert.Equal(successStatus, isSuccessful);
        }

        [Fact]
        public void CallInfoOutputTest()
        {
            _senderTerminal.ConnectToExchange();
            _receiverTerminal.ConnectToExchange();

            _senderTerminal.MakeCall(_receiverTerminal.PhoneNumber);
            _receiverTerminal.ReceiveCall();

            Task.Delay(1000).Wait();

            _receiverTerminal.CloseCall();

            var info = _senderTerminal.GetCallsHistory(x => x.Caller.Phone == _phones[0]).ToList();

            Assert.Equal("First called from 111 to Second", info.FirstOrDefault()?.ToString());
        }
    }
}
