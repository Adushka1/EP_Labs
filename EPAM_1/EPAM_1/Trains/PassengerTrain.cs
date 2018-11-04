using EPAM1.Wagons.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPAM1.Locomotives.Interfaces;
using EPAM1.Wagons;

namespace EPAM1.Trains
{
    public class PassengerTrain : Train<PassengerWagon>
    {

        public PassengerTrain(ILocomotive locomotive)
        {
            Wagons = new List<PassengerWagon>();
            Locomotive = locomotive;
        }

        public void AddPassenger(int wagonIndex)
        {
            Wagons[wagonIndex].AddPassenger();
        }

        public int PassengersCount()
        {
            return Count(x => x.ElementCount());
        }

        public int BaggageCount()
        {
            return Count(x => x.BaggageCount());
        }

        public override string ToString()
        {
            var k = "";
            foreach (var wagon in Wagons)
            {
                k += wagon;
                k += "||||\n";
            }
            k += Locomotive;
            return k;
        }
    }
}
