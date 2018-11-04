using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPAM1.Locomotives;
using EPAM1.Locomotives.Interfaces;
using EPAM1.Trains;
using EPAM1.Wagons;

namespace EPAM1
{
    class Program
    {
        static void Main(string[] args)
        {
            ILocomotive locomotive = new ElectricLocomotive();
            PassengerTrain passengerTrain = new PassengerTrain(locomotive);
            PassengerWagon wagon = new CouchetteWagon(elementsAmount: 54);
            wagon.AddPassenger(baggageAmount: 14);
            PassengerWagon wagon2 = new CoupeWagon(elementsAmount: 15);
            wagon2.AddPassenger();
            PassengerWagon wagon3 = new CouchetteWagon(elementsAmount: 1);

            passengerTrain.AddWagon(wagon);
            passengerTrain.AddWagon(wagon2);
            passengerTrain.AddWagon(wagon3);

            passengerTrain.Sort(x => x.ComfortLevel);
            //  Console.WriteLine(passengerTrain);

            Console.WriteLine("Количество пассажиров: " + passengerTrain.PassengersCount());
            Console.WriteLine("Количество багажа у пассажиров: " + passengerTrain.BaggageCount());
            var rangeList = passengerTrain.RangeOutput(2, 53);
            foreach (var element in rangeList)
            {
                Console.WriteLine(element);
            }

        }
    }
}
