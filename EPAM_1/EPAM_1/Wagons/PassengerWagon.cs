using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPAM1.Cargos;
using EPAM1.Wagons.Interfaces;

namespace EPAM1.Wagons
{
    public abstract class PassengerWagon : IWagon
    {
        public int ComfortLevel { get; set; }
        public abstract int WagonCapacity { get; protected set; }
        public int ElementsAmount { get; protected set; }
        public ICollection<Passenger> Passengers { get; protected set; }

        public int ElementCount()
        {
            return Passengers.Count;
        }

        public int BaggageCount()
        {
            return Passengers.Sum(passenger => passenger.BaggageAmount);
        }

        public void AddPassenger(int baggageAmount = 1)
        {
            ElementsAmount++;
            if (WagonCapacity <= ElementsAmount)
            {
                throw new ArgumentOutOfRangeException();
            }
            Passengers.Add(new Passenger(baggageAmount));
        }

        protected PassengerWagon(int elementsAmount)
        {
            Passengers = new List<Passenger>(elementsAmount);

            for (var i = 0; i < elementsAmount; i++)
            {
                AddPassenger();
            }
        }
    }
}
