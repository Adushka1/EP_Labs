using System;
using System.Collections.Generic;
using System.Text;
using EPAM_3.BillingSystem;
using EPAM_3.Phones.Interfaces;

namespace EPAM_3.Terminals.Interfaces
{
    public interface IInfoBuilder
    {
        IEnumerable<CallInfo> GetCallHistory(IPhone phoneNumber);
    }
}
