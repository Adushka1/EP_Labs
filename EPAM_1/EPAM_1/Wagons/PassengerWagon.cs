using EPAM_1.Cargos;
using EPAM_1.Wagons.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM_1.Wagons
{
    public abstract class PassengerWagon : IWagon
    {
        public abstract int ComfortLevel { get; set; }
        public abstract int WagonCapacity { get; set; }
        public int ElementsAmount { get; set; }
        public ICollection<Passenger> Passengers { get; private set; }

        public int ElementCount()
        {
            return Passengers.Count();
        }

        public int BaggageCount()
        {
            int baggageAmount = 0;
            foreach (var passenger in Passengers)
            {
                baggageAmount += passenger.BaggageAmount;
            }
            return baggageAmount;
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

        public PassengerWagon(int elementsAmount, int wagonCapacity)
        {
            WagonCapacity = wagonCapacity;

            if (elementsAmount > WagonCapacity)
            {
                Console.WriteLine("Wagon is overload");
                ElementsAmount = WagonCapacity;
            }
            else ElementsAmount = elementsAmount;

            Passengers = new List<Passenger>(elementsAmount);

            for (int i = 0; i < elementsAmount; i++)
            {
                Passengers.Add(new Passenger());
            }

        }
    }
}
