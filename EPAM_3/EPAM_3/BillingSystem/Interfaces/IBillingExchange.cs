using System;
using System.Collections.Generic;
using System.Text;
using EPAM_3.BillingSystem.EventArgs;
using EPAM_3.Phones;

namespace EPAM_3.BillingSystem.Interfaces
{
    public interface IBillingExchange
    {
        void AbonentConnectedEventHandler(object sender, RingEventArgs e);
        void AbonentDisconnectedEventHandler(object sender, RingEventArgs e);

        bool AbonentIsAvalible(Phone abonent);
    }
}
