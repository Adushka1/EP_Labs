using EPAM1.Cargos;
using EPAM1.Wagons.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EPAM1.Wagons
{
    public sealed class CoupeWagon : PassengerWagon
    {


        public override int WagonCapacity { get; protected set; } = 34;

        public CoupeWagon(int elementsAmount) : base(elementsAmount)
        {
            ComfortLevel = 80;
        }

        public override string ToString()
        {
            return $"-------\n|Coupe|\n{ElementsAmount}\n-------\n";
        }
    }
}
