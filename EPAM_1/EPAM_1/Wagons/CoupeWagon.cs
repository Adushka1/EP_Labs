using EPAM1.Cargos;
using EPAM1.Wagons.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM1.Wagons
{
    public sealed class CoupeWagon : PassengerWagon
    {
        public CoupeWagon(int elementsAmount, int wagonCapacity = 36) : base(elementsAmount, wagonCapacity = 36)
        {
            if (wagonCapacity <= 0) throw new ArgumentOutOfRangeException(nameof(wagonCapacity));
            WagonCapacity = wagonCapacity;
            ComfortLevel = 80;
        }
    }
}
