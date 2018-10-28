using EPAM_1.Cargos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM_1.Cargos
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
