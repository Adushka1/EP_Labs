using EPAM_1.Locomotives;
using EPAM_1.Locomotives.Interfaces;
using EPAM_1.Trains;
using EPAM_1.Trains.Interfaces;
using EPAM_1.Wagons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM_1
{
    class Program
    {
        static void Main(string[] args)
        {
            ILocomotive locomotive = new DiselLocomotive();
            PassengerTrain passengerTrain = new PassengerTrain(locomotive);
            PassengerWagon wagon = new CouchetteWagon(4);
            wagon.AddPassenger(15);
            PassengerWagon wagon2 = new CoupeWagon(15);
            PassengerWagon wagon3 = new CouchetteWagon(14);

            passengerTrain.AddWagon(wagon);
            passengerTrain.AddWagon(wagon2);
            passengerTrain.AddWagon(wagon3);
            Console.WriteLine(passengerTrain.GetPassengersAmount());
            Console.WriteLine(passengerTrain.GetBagageAmount());
            Console.WriteLine("Выводит количество пассажиров которые попадают в диапазон:");
            passengerTrain.RangeOutput(13, 20); 
            

        }
    }
}
