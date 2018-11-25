using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM_3.TelephoneExchange.Interfaces
{
    public interface ITelephoneExchange
    {
         ISet<IPort> Ports { get;}
    }
}
