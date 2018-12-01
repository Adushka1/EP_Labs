using System;
using System.Collections.Generic;
using System.Text;
using EPAM_3.BillingSystem;
using EPAM_3.BillingSystem.Enums;
using EPAM_3.Phones.Interfaces;
using EPAM_3.TelephoneExchange.Interfaces;

namespace EPAM_3.Terminals.Interfaces
{
    public interface ITerminal
    {
        IPhone PhoneNumber { get; }
        IPort CurrentPort { get; }

        CallStatus MakeCall(IPhone phone);
        CallStatus ReceiveCall();
        CallStatus CloseCall();

        IEnumerable<CallInfo> GetCallsHistory();
        bool ConnectToExchange();
        bool DisconnectFromExchange();

        bool ChangePort(IPort newPort);

    }
}
