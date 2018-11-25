using System;
using System.Collections.Generic;
using System.Text;
using EPAM_3.TelephoneExchange.Enums;
using EPAM_3.TelephoneExchange.Interfaces;

namespace EPAM_3.TelephoneExchange
{
    public class Port : IPort
    {
        public event EventHandler<PortStatus> PortStatusChanged;
        public int Number { get; }
        public PortStatus Status { get; }

        public void OnPortStatusChanged(PortStatus status)
        {
            PortStatusChanged?.Invoke(this, status);
        }
    }
}
