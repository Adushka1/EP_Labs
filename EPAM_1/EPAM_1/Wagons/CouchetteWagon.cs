using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM1.Wagons
{
    public class CouchetteWagon : PassengerWagon
    {
        public CouchetteWagon(int elementsAmount, int wagonCapacity = 54) : base(elementsAmount, wagonCapacity = 54)
        {
            WagonCapacity = wagonCapacity;
            ComfortLevel = 55;
        }
    }
}
