using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPAM1.Locomotives.Interfaces;
using EPAM1.Wagons;
using EPAM1.Wagons.Interfaces;

namespace EPAM1.Trains
{
    public abstract class Train<T> where T : IWagon
    {
        public ILocomotive Locomotive { get; protected set; }
        public IList<T> Wagons { get; protected set; }

        public int Count(Func<T, int> comparer)
        {
            return Wagons.Sum(comparer);
        }

        public void Sort(Func<T, int> comparerFunc)
        {
            Wagons = Wagons.OrderBy(comparerFunc).ToList();
        }

        public ICollection<T> RangeOutput(int leftBorder, int rightBorder)
        {
            return Wagons.Where(x => x.ElementsAmount > leftBorder && x.ElementsAmount < rightBorder).ToList();
        }

        public void AddWagon(T wagon)
        {
            Wagons.Add(wagon);
        }

        public void RemoveWagon(T wagon)
        {
            Wagons.Remove(wagon);
        }
    }
}
