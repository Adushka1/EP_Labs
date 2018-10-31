using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPAM1.Cargos.Interfaces;

namespace EPAM1.Cargos
{
    public class Passenger : IBasicCargo
    {
        public int BaggageAmount { get; private set; }

        public Passenger(int baggageAmount)
        {
            BaggageAmount = baggageAmount;
        }
        public Passenger()
        {
            BaggageAmount = 1;
        }
    }
}
