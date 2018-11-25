using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPAM_3.BillingSystem.Interfaces;
using EPAM_3.Clients;
using EPAM_3.Repositories.Interfaces;
using System.Timers;
using EPAM_3.Clients.Enums;
using EPAM_3.Clients.Interfaces;

namespace EPAM_3.BillingSystem
{
    public class Balance : IBalance
    {
        private Timer _balanceControlTimer;
        private Timer _balanceCountTimer;

        private readonly TimeSpan _balanceCountTimerSpan;
        private IRepository<IClient> _clients;
        private IRepository<CallInfo> _callInfos;

        public Balance(IRepository<IClient> clients, IRepository<CallInfo> callInfos, TimeSpan balanceCountTimerSpan)
        {
            _clients = clients;
            _callInfos = callInfos;
            _balanceCountTimerSpan = balanceCountTimerSpan;

            _balanceCountTimer = new Timer(_balanceCountTimerSpan.TotalMilliseconds);
            _balanceCountTimer.Elapsed += CountClientBalance;
            _balanceCountTimer.Start();
        }

        public void SetControlDay(uint dayNumber)
        {
            if (!(dayNumber > 1 && dayNumber < 32))
            {
                throw new ArgumentOutOfRangeException("Day number must be from 1 to 31");
            }

            var interval = TimeSpan.FromDays(1).TotalMilliseconds;
            _balanceControlTimer = new Timer(interval);
            _balanceControlTimer.Elapsed += BalanceControl;

            void BalanceControl(object sender, ElapsedEventArgs e)
            {
                if (DateTimeOffset.Now.Day != dayNumber)
                {
                    return;
                }

                foreach (var client in _clients.GetAllEntities())
                {
                    if (client.Balance < 0)
                    {
                        client.ChangeStatus(ClientStatus.Locked);
                    }
                }
            }
        }

        protected void CountClientBalance(object sender, ElapsedEventArgs e)
        {
            var newCall = _callInfos.GetEntities(b => !b.Checked);
            var clientGroup = newCall.GroupBy(b => b.Caller);

            foreach (var clientCalls in clientGroup)
            {
                var client = clientCalls.Key;
                foreach (var call in clientCalls)
                {
                    client.ChangeBalance(client.Balance - call.Cost);
                    call.Checked = true;
                }
            }
        }
    }
}
