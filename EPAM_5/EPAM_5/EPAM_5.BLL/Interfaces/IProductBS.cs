using System.Collections.Generic;
using EPAM_5.Entities;
using EPAM_5.DAL;
using EPAM_5.Common;
using EPAM_5.Models;
using System;
using System.Threading.Tasks;

namespace EPAM_5.BLL
{
    public interface IProductBS
    {
        IList<Entities.Product> GetProducts();
        Entities.Product GetProductById(int id);        
        IList<Models.ProductCM> GetAllProductsByCategoryId(int categoryId);

        IList<Entities.Product> GetProductByCategoryId(int categoryId, PaginationRequest paging, out int totalCount, out int newPageIndex);
        IList<Models.ProductCM> GetFullProducts(int categoryId, PaginationRequest paging, out int totalCount, out int newPageIndex);
        
        IList<Models.ProductCM> GetProductList(ProductSearchField productSearchField, string productSearchText,
                            Decimal? priceLow, Decimal? priceHigh, DateTime? dateFrom, DateTime? dateTo, int? statusCode,
                            PaginationRequest paging, out int totalCount, out int newPageIndex);
        IList<Models.ProductCM> GetProductListNew(List<int> newProductIds);

        int AddProduct(Entities.Product inputEt);
        void UpdateProduct(Entities.Product inputEt);
        void DeleteProduct(int id);
    }

}