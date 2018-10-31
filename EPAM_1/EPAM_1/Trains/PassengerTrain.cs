﻿using EPAM1.Wagons.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPAM1.Locomotives.Interfaces;
using EPAM1.Trains.Interfaces;
using EPAM1.Wagons;

namespace EPAM1.Trains
{
    public class PassengerTrain : ITrain<PassengerWagon>
    {
        public ILocomotive Locomotive { get; }
        public ICollection<PassengerWagon> Wagons { get; set; }

        public PassengerTrain(ILocomotive locomotive)
        {
            Wagons = new List<PassengerWagon>();
            Locomotive = locomotive;
        }

        public int Count(Func<PassengerWagon, int> comparer)
        {
            return Wagons.Sum(comparer);
        }

        public void Sort(Func<PassengerWagon, int> comparerFunc)
        {
            Wagons = Wagons.OrderBy(comparerFunc).ToList();

        }

        public void RangeOutput(int firstElement, int secondElement)
        {
            var rangeElements = Wagons.Where(x => x.ElementsAmount > firstElement && x.ElementsAmount < secondElement);
            foreach (var element in rangeElements)
            {
                Console.WriteLine(element.ElementsAmount);
            }
        }

        public void AddWagon(PassengerWagon wagon)
        {
            Wagons.Add(wagon);
        }

        public void RemoveWagon(PassengerWagon wagon)
        {
            Wagons.Remove(wagon);
        }
    }
}
