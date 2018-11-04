using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPAM1.Wagons.Interfaces;

namespace EPAM1.Wagons
{
    public class CoveredWagon : GoodsWagon
    {
        public CoveredWagon(string elementName, int elementsAmount, int wagonCapacity) : base(elementName, elementsAmount, wagonCapacity)
        {
        }

        public override void Load()
        {
        }

        public override void Unload()
        {
        }

        public override string ToString()
        {
            return $"-------\n|Covered|\n{ElementsAmount}\n-------\n";
        }
    }
}
