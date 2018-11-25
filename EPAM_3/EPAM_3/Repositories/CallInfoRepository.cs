using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPAM_3.BillingSystem;
using EPAM_3.Repositories.Abstract;
using EPAM_3.Repositories.Interfaces;
using EPAM_3.TelephoneExchange;

namespace EPAM_3.Repositories
{
    public class CallInfoRepository : AbstractRepository<CallInfo>
    {
        public CallInfoRepository(IEnumerable<CallInfo> entitySet) : base(entitySet)
        {
            
        }
    }
}
