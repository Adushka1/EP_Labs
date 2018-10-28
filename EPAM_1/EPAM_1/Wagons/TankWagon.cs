using EPAM_1.Wagons.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM_1.Wagons
{
    class TankWagon : IGoodsWagon
    {
        public int WagonCapacity { get; private set; }

        public int ElementsAmount { get; private set; }

        public int ElementCount()
        {
            return ElementsAmount;
        }
    }
}
