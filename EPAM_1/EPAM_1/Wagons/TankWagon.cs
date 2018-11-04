using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM1.Wagons
{
    public class TankWagon : GoodsWagon
    {


        protected TankWagon(string elementName, int elementsAmount, int wagonCapacity) : base(elementName, elementsAmount, wagonCapacity)
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
            return $"-----\n|Tank|\n{ElementsAmount}\n-----\n";
        }
    }
}
