using System;
using System.Collections.Generic;
using System.Text;
using EPAM_3.Phones;
using EPAM_3.Phones.Interfaces;
using EPAM_3.Tariffs.Interfaces;

namespace EPAM_3.Tariffs
{
    public class Tariff : ITariff
    {
        public int Id { get; }
        public decimal CostPerMinute { get; set; }
        public string Name { get; set; }

        public Tariff(int id, decimal costPerMinute, string name)
        {
            Id = id;
            CostPerMinute = costPerMinute;
            Name = name;
        }

        public decimal GetCallCost(IPhone sender, IPhone receiver, TimeSpan duration)
        {
            return CostPerMinute * (decimal)duration.TotalMinutes;
        }
    }
}
