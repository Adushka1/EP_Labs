using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM1.Wagons
{
    public class CouchetteWagon : PassengerWagon
    {
        public override int ComfortLevel { get; set; }
        public override int WagonCapacity { get; set; }

        public CouchetteWagon(int elementsAmount, int wagonCapacity = 54) : base(elementsAmount, wagonCapacity = 54)
        {
            WagonCapacity = wagonCapacity;
            ComfortLevel = 55;
        }
    }
}
