using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM_1.Wagons.Interfaces
{
    public interface IWagon
    {
        int WagonCapacity { get; }
        int ElementsAmount { get; }

        int ElementCount();

    }
}
