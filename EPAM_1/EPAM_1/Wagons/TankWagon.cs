using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPAM1.Wagons.Interfaces;

namespace EPAM1.Wagons
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
