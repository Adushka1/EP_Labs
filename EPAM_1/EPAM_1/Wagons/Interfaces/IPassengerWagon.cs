using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM1.Wagons.Interfaces
{
    public interface IPassengerWagon : IWagon
    {
        int BaggageCount();
        void AddPassenger(int baggageAmount);
    }
}
