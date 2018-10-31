using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPAM1.Locomotives.Interfaces;
using EPAM1.Wagons.Interfaces;

namespace EPAM1.Trains.Interfaces
{
    public interface ITrain<T> where T : IWagon
    {
        ILocomotive Locomotive { get; }
        ICollection<T> Wagons { get; }

        void AddWagon(T wagon);
        void RemoveWagon(T wagon);

    }
}
