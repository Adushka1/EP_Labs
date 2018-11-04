using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM1.Wagons
{
    public sealed class CouchetteWagon : PassengerWagon
    {
        public override int WagonCapacity { get; protected set; } = 56;

        public CouchetteWagon(int elementsAmount) : base(elementsAmount)
        {
            ComfortLevel = 50;
        }

        public override string ToString()
        {
            return $"-----------\n|Couchette|\n{ElementsAmount}        \n-----------\n";
        }
    }
}