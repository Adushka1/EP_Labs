using System;
using System.Collections.Generic;
using System.Linq;
using EPAM1.Locomotives.Interfaces;
using EPAM1.Wagons;
using EPAM1.Wagons.Interfaces;

namespace EPAM1.Trains
{
    public class GoodsTrain : Train<GoodsWagon>
    {
        public ILocomotive Locomotive { get; }
        public ICollection<GoodsWagon> Wagons { get; }

        public GoodsTrain(ILocomotive locomotive)
        {
            Locomotive = locomotive;
            Wagons = new List<GoodsWagon>();
        }

        public int WeightCount()
        {
            return Count(x => x.ElementsAmount);
        }
    }
}