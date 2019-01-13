using System.Collections.Generic;
using System.Linq;

namespace EPAM_5.Common
{
    public abstract class BaseConverter<TSourceObj, TDestinationObj> : IBaseConverter<TSourceObj, TDestinationObj>
        where TSourceObj : class
        where TDestinationObj : class
    {
        public abstract TDestinationObj ConvertObject(TSourceObj srcObj);

        public virtual List<TDestinationObj> ConvertObjectCollection(IEnumerable<TSourceObj> srcObjList)
        {
            if (srcObjList == null) return null;
            var destList = srcObjList.Select(item => this.ConvertObject(item));
            return destList.ToList();
        }
    }
}
