﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using EPAM_3.TelephoneExchange.Enums;

namespace EPAM_3.TelephoneExchange.Interfaces
{
    public interface IPort : IComparable<IPort>
    {
        event EventHandler<PortStatus> PortStatusChanged;
        int Number { get; }
        PortStatus Status { get; set; }

        void OnPortStatusChanged(PortStatus status);

    }
}
