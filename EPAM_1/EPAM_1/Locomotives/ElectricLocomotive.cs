using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPAM1.Locomotives.Interfaces;

namespace EPAM1.Locomotives
{
    class ElectricLocomotive : ILocomotive
    {
        public void Move()
        {
        }

        public override string ToString()
        {
            return $"---------\n|Electric|\nlocomotive\n----\\/---\n";
        }
    }
}
