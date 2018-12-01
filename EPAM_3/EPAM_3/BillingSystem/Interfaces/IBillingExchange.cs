using System;
using System.Collections.Generic;
using System.Text;
using EPAM_3.BillingSystem.EventArgs;
using EPAM_3.Phones;
using EPAM_3.Phones.Interfaces;

namespace EPAM_3.BillingSystem.Interfaces
{
    public interface IBillingExchange
    {
        void AbonentsConnectedEventHandler(object sender, RingEventArgs e);
        void AbonentsDisconnectedEventHandler(object sender, RingEventArgs e);

        bool AbonentIsAvalible(IPhone abonent);
    }
}
