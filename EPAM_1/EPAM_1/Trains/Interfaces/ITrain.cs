using EPAM_1.Locomotives.Interfaces;
using EPAM_1.Wagons.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM_1.Trains.Interfaces
{
    public interface ITrain< T> where T : IWagon
    {
        ILocomotive Locomotive { get; }
        ICollection<T> Wagons { get; }

        void AddWagon(T wagon);
        void RemoveWagon(T wagon);

    }
}
