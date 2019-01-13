using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPAM_5.Common
{
    public class AutoMapConverter<TSourceObj, TDestinationObj> : BaseConverter<TSourceObj, TDestinationObj>, IBaseConverter<TSourceObj, TDestinationObj>
        where TSourceObj : class
        where TDestinationObj : class
    {
        public AutoMapConverter()
        {
            AutoMapper.Mapper.CreateMap<TSourceObj, TDestinationObj>();
        }

        public override TDestinationObj ConvertObject(TSourceObj srcObj)
        {
            return AutoMapper.Mapper.Map<TSourceObj, TDestinationObj>(srcObj);
        }        
    }   
}