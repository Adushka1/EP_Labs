using System;
using System.Collections.Generic;
using System.Text;
using EPAM_3.Phones;
using EPAM_3.Phones.Interfaces;

namespace EPAM_3.BillingSystem.EventArgs
{
    public class RingEventArgs : System.EventArgs
    {
        public IPhone Sender { get; }
        public IPhone Receiver { get; }

        public RingEventArgs(IPhone sender, IPhone receiver)
        {
            Sender = sender;
            Receiver = receiver;
        }
    }
}
