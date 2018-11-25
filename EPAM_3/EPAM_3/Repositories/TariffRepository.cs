using System;
using System.Collections.Generic;
using System.Text;
using EPAM_3.Repositories.Abstract;
using EPAM_3.Tariffs.Interfaces;

namespace EPAM_3.Repositories
{
    public class TariffRepository : AbstractRepository<ITariff>
    {
        public TariffRepository(IEnumerable<ITariff> entitySet) : base(entitySet)
        {
        }
    }
}
