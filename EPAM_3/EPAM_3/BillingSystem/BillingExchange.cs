using System;
using System.Collections.Generic;
using System.Text;
using EPAM_3.BillingSystem.EventArgs;
using EPAM_3.BillingSystem.Interfaces;
using EPAM_3.Clients.Enums;
using EPAM_3.Phones;
using EPAM_3.Phones.Interfaces;

namespace EPAM_3.BillingSystem
{
    public class BillingExchange : IBillingExchange
    {
        private BillingUnitOfWork _unit;
        private IBalance _balance;

        public TimeSpan CallDelay { get; private set; }
        private IDictionary<(IPhone, IPhone), CallInfo> _currentCalls;

        public BillingExchange(BillingUnitOfWork data, TimeSpan callReceivingDelay,
            uint balanceCheckDayNumber = 1, double balanceCountIntervalSeconds = 30)
        {
            _unit = data;
            CallDelay = callReceivingDelay;

            _balance = new Balance(data.Clients, data.CallInfos, TimeSpan.FromSeconds(balanceCountIntervalSeconds));

            _currentCalls = new Dictionary<(IPhone, IPhone), CallInfo>();
        }

        public bool AbonentIsAvalible(IPhone abonent)
        {
            var client = _unit.Clients.GetEntityOrDefault(x => x.Phone == abonent);
            return client != null && client.Status == ClientStatus.Avalible;
        }

        public void AbonentsConnectedEventHandler(object sender, RingEventArgs e)
        {
            var senderClient = _unit.Clients.GetEntityOrDefault(x => x.Phone == e.Sender);
            var receiverClient = _unit.Clients.GetEntityOrDefault(x => x.Phone == e.Receiver);

            var call = new CallInfo(senderClient, receiverClient, DateTimeOffset.Now);
            _currentCalls.Add((e.Sender, e.Receiver), call);
        }

        public void AbonentsDisconnectedEventHandler(object sender, RingEventArgs e)
        {
            if (!_currentCalls.ContainsKey((e.Sender, e.Receiver))) return;

            var call = _currentCalls[(e.Sender, e.Receiver)];
            var callTariff = call.Caller.Tariff;

            var duration = DateTimeOffset.Now - call.Date;
            decimal cost = 0;

            if (duration > CallDelay)
            {
                cost = callTariff.GetCallCost(e.Sender, e.Receiver, duration);
            }

            call.Duration = duration;
            call.Cost = cost;
            _unit.CallInfos.AddEntity(call);

            _currentCalls.Remove((e.Sender, e.Receiver));
        }
    }
}
