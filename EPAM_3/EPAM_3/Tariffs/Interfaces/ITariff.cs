using System;
using System.Collections.Generic;
using System.Text;
using EPAM_3.Phones;
using EPAM_3.Phones.Interfaces;

namespace EPAM_3.Tariffs.Interfaces
{
    public interface ITariff
    {
        int Id { get; }

        decimal GetCallCost(IPhone sender, IPhone receiver, TimeSpan duration);
    }
}
