using System;
using System.Collections.Generic;
using System.Text;
using EPAM_3.Clients;
using EPAM_3.Clients.Interfaces;

namespace EPAM_3.BillingSystem
{
    public class CallInfo
    {
        public IClient Caller { get; }
        public IClient Receiver { get; }
        public decimal Cost { get; set; }
        public DateTimeOffset Date { get; }
        public bool Checked { get; set; }
        public TimeSpan Duration { get; set; }

        public CallInfo(IClient caller, IClient receiver, DateTimeOffset date)
        {
            Caller = caller;
            Receiver = receiver;
            Date = date;
        }

        public CallInfo(IClient caller, IClient receiver, decimal cost, DateTimeOffset date, TimeSpan duration)
        {
            Caller = caller;
            Receiver = receiver;
            Cost = cost;
            Date = date;
            Duration = duration;
        }

        public override string ToString()
        {
            return $"{Caller.Name} called from {Caller.Phone.Id} to {Receiver.Name}";
        }
    }
}
