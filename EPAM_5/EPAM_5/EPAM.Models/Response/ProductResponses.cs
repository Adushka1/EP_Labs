using System;
using System.Collections.Generic;

namespace EPAM_5.Models
{
    public class ProductResponse 
    {
        public Product Product { get; set; }
    }
    public class ProductCMResponse 
    {
        public ProductCM ProductCM { get; set; }
    }
     
    public class ProductListResponse 
    {
        public Products Products { get; set; }
        public int TotalCount { get; set; }
        public int newPageIndex { get; set; }
    }

    public class Products : List<ProductCM> { }

    public class AddProductResponse
    {
        public int ProductID { get; set; }
    }
}
