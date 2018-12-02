using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPAM_3.BillingSystem;
using EPAM_3.Phones.Interfaces;
using EPAM_3.Terminals.Interfaces;

namespace EPAM_3.Terminals
{
    public class InfoBuilder : IInfoBuilder
    {
        private BillingUnitOfWork _unit;

        public InfoBuilder(BillingUnitOfWork unit)
        {
            _unit = unit;
        }

        private IEnumerable<CallInfo> GetCallHistory(IPhone phoneNumber)
        {
            return _unit.CallInfos.GetEntities(x => x.Receiver.Phone == phoneNumber || x.Caller.Phone == phoneNumber);
        }

        public IEnumerable<CallInfo> GetCallsInformation(IPhone phoneNumber, Func<CallInfo, bool> predicate)
        {
            var calls = GetCallHistory(phoneNumber);

            return calls.Where(predicate);
        }
    }
}
