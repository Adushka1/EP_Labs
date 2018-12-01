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

        public IEnumerable<CallInfo> GetCallHistory(IPhone phoneNumber)
        {
            var t = _unit.CallInfos.GetAllEntities().ToList();
            var receivedCallHistory = _unit.CallInfos.GetEntities(x => x.Receiver.Phone.Id == phoneNumber.Id);
            var senderCallHistory = _unit.CallInfos.GetEntities(x => x.Caller.Phone.Id == phoneNumber.Id);
            return senderCallHistory.Union(receivedCallHistory);
        }
    }
}
