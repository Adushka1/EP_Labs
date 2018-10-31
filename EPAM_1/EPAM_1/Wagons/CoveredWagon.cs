using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPAM1.Wagons.Interfaces;

namespace EPAM1.Wagons
{
    class CoveredWagon : IGoodsWagon
    {

        public int WagonCapacity { get; }
        public int ElementsAmount { get; }
        public int ElementCount()
        {
            throw new NotImplementedException();
        }

        public CoveredWagon(int elementsAmount, int wagonCapacity)
        {
            ElementsAmount = elementsAmount;
            WagonCapacity = wagonCapacity;
        }

    }
}
