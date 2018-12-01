using System;
using System.Collections.Generic;
using System.Text;
using EPAM_3.BillingSystem.Enums;
using EPAM_3.BillingSystem.EventArgs;
using EPAM_3.Phones;
using EPAM_3.Phones.Interfaces;

namespace EPAM_3.TelephoneExchange.Interfaces
{
    public interface ITelephoneExchange
    {
        event EventHandler<RingEventArgs> AbonentsConnected;
        CallStatus ConnectAbonents(IPhone sender, IPhone receiver);

        event EventHandler<RingEventArgs> AbonentsDisconnected;
        CallStatus DisconnectAbonents(IPhone sender, IPhone receiver);

        bool MapToPort(IPhone phoneNumber, IPort port);

        bool ConnectToExchange(IPhone phone);
        bool DisconnectFromExchange(IPhone phone);
    }
}
