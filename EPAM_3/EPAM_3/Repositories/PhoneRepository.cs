using System;
using System.Collections.Generic;
using System.Text;
using EPAM_3.Phones;
using EPAM_3.Phones.Interfaces;
using EPAM_3.Repositories.Abstract;

namespace EPAM_3.Repositories
{
    public class PhoneRepository : AbstractRepository<IPhone>
    {
        public PhoneRepository(IEnumerable<IPhone> callInfoSet) : base(callInfoSet)
        {
        }
    }
}
