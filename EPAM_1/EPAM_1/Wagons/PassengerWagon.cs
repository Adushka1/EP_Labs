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
        public abstract int ComfortLevel { get; set; }
        public abstract int WagonCapacity { get; set; }
        public int ElementsAmount { get; set; }
        public ICollection<Passenger> Passengers { get; }

        public int ElementCount()
        {
            return Passengers.Count();
        }

        public int BaggageCount()
        {
            return Passengers.Sum(passenger => passenger.BaggageAmount);
        }

        public void AddPassenger(int baggageAmount)
        {
            if (WagonCapacity <= ElementsAmount)
            {
                Console.WriteLine("Wagon is overload after addS");
            }
            else
            {
                ElementsAmount++;
                Passengers.Add(new Passenger(baggageAmount));
            }
        }

        protected PassengerWagon(int elementsAmount, int wagonCapacity)
        {
            WagonCapacity = wagonCapacity;

            Passengers = new List<Passenger>(elementsAmount);

            for (var i = 0; i < elementsAmount; i++)
            {
                Passengers.Add(new Passenger());
            }

        }
    }
}
