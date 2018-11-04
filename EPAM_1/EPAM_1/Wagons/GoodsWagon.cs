using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPAM1.Wagons.Interfaces;

namespace EPAM1.Wagons
{
    public abstract class GoodsWagon : IWagon
    {
        public int WagonCapacity { get; protected set; }
        public int ElementsAmount { get; protected set; }
        public string ElementName { get; }
        public abstract void Load();
        public abstract void Unload();

        protected GoodsWagon(string elementName, int elementsAmount, int wagonCapacity)
        {
            ElementName = elementName;
            ElementsAmount = elementsAmount;
            WagonCapacity = wagonCapacity;
        }
    }
}
