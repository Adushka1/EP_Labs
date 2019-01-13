using System.Collections.Generic;
using EPAM_5.Entities;

namespace EPAM_5.BLL
{
    public interface ILookupBS 
    {
        IList<Models.Category> LookupCategories();
        IList<Models.ProductStatusType> LookupProductStatusTypes();        
    }
}
