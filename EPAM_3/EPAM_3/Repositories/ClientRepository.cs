using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPAM_3.Clients;
using EPAM_3.Clients.Interfaces;
using EPAM_3.Repositories.Abstract;
using EPAM_3.Repositories.Interfaces;

namespace EPAM_3.Repositories
{
    public class ClientRepository : AbstractRepository<IClient>
    {
        public ClientRepository(IEnumerable<IClient> entitySet) : base(entitySet)
        {
        }
    }
}
