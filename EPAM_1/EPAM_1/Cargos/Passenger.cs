using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPAM1.Cargos;

namespace EPAM1.Cargos
{
    public class Passenger
    {
        public int BaggageAmount { get; }

        public Passenger(int baggageAmount)
        {
            if (baggageAmount < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            BaggageAmount = baggageAmount;
        }

        public Passenger()
        {
            BaggageAmount = 1;
        }
    }
}
