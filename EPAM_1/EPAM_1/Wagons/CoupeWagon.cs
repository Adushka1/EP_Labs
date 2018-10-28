using EPAM_1.Cargos;
using EPAM_1.Wagons.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM_1.Wagons
{
    public class CoupeWagon : PassengerWagon
    {
        public override int ComfortLevel { get; set; }
        public override int WagonCapacity { get; set; }

        public CoupeWagon(int elementsAmount, int wagonCapacity = 36) : base(elementsAmount, wagonCapacity = 36)
        {
            WagonCapacity = wagonCapacity;
            ComfortLevel = 80;
        }
    }
}
