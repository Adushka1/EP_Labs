using System.Collections.Generic;
using System;
using EPAM_5.Entities;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EPAM_5.Api.DAL;
using EPAM_5.Api.Common;
using EPAM_5.Api.Models;
using EPAM_5.Threading.Tasks;

namespace EPAM_5.BLL
{
    public class ProductBS : IProductBS
    {
        private IProductRepository _productRepository;
        
        public ProductBS(IProductRepository productRepository)
        {
            if (productRepository != null)
                this._productRepository = productRepository;            
        }
        
        public IList<Entities.Product> GetProducts()
        {
            return this._productRepository.GetProducts();
        }

        public Entities.Product GetProductById(int id)
        {
            return this._productRepository.GetProductById(id);
        }

        public IList<Entities.Product> GetProductByCategoryId(int categoryId, PaginationRequest paging, out int totalCount, out int newPageIndex)
        {
            return this._productRepository.GetProductByCategoryId(categoryId, paging, out totalCount, out newPageIndex);
        }

        public IList<Models.ProductCM> GetFullProducts(int categoryId, PaginationRequest paging, out int totalCount, out int newPageIndex)
        {
            return this._productRepository.GetFullProducts(categoryId, paging, out totalCount, out newPageIndex);
        }

        public IList<Models.ProductCM> GetProductList(ProductSearchField productSearchField, string productSearchText,
                            Decimal? priceLow, Decimal? priceHigh, DateTime? dateFrom, DateTime? dateTo, int? statusCode,
                            PaginationRequest paging, out int totalCount, out int newPageIndex)
        {
            return this._productRepository.GetProductList(productSearchField, productSearchText,
                   priceLow, priceHigh, dateFrom, dateTo, statusCode, paging, out totalCount, out newPageIndex);
        }

        public IList<Models.ProductCM> GetProductListNew(List<int> newProductIds)
        {
            return this._productRepository.GetProductListNew(newProductIds);
        }

        public IList<Models.ProductCM> GetAllProductsByCategoryId(int categoryId)
        {
            return this._productRepository.GetAllProductsByCategoryId(categoryId);
        }
        
        public int AddProduct(Entities.Product inputEt)
        {
            return this._productRepository.AddProduct(inputEt);
        }

        public void UpdateProduct(Entities.Product inputEt)
        {
            this._productRepository.UpdateProduct(inputEt);
        }

        public void DeleteProduct(int id)
        {
            this._productRepository.DeleteProduct(id);
        }
    }
}
