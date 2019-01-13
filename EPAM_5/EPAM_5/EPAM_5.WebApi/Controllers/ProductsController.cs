using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using EPAM_5.BLL;
using EPAM_5.Common;
using EPAM_5.DAL;
using EPAM_5.Models;

namespace EPAM_5.Controllers
{
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        [Route("~/api/getproductList")]
        public ProductListResponse GetProductList([ModelBinder(typeof(FieldValueModelBinder))] GetProductsBySearchRequest request)
        {
            var resp = new ProductListResponse();
            resp.Products = new Models.Products();

            Models.ProductSearchField searchField = 0;
            string searchText = null;
            Decimal? priceLow = null;
            Decimal? priceHigh = null;
            DateTime? dateFrom = null;
            DateTime? dateTo = null;
            if (request.ProductSearchFilter != null)
            {
                searchField = request.ProductSearchFilter.ProductSearchField;
                searchText = request.ProductSearchFilter.ProductSearchText;
            }
            if (request.PriceSearchFilter != null)
            {
                if (!String.IsNullOrEmpty(request.PriceSearchFilter.SearchPriceLow)) priceLow = Convert.ToDecimal(request.PriceSearchFilter.SearchPriceLow);
                if (!String.IsNullOrEmpty(request.PriceSearchFilter.SearchPriceHigh)) priceHigh = Convert.ToDecimal(request.PriceSearchFilter.SearchPriceHigh);
            }
            if (request.DateSearchFilter != null)
            {
                if (!String.IsNullOrEmpty(request.DateSearchFilter.SearchDateFrom)) dateFrom = Convert.ToDateTime(request.DateSearchFilter.SearchDateFrom);
                if (!String.IsNullOrEmpty(request.DateSearchFilter.SearchDateTo)) dateTo = Convert.ToDateTime(request.DateSearchFilter.SearchDateTo);
            }
            int totalCount = 0;
            int newPageIndex = -1;

            IProductBS bs = default(IProductBS);
            bs = DIFactory.GetInstance<IProductBS>();
            IEnumerable<ProductCM> rtnList = bs.GetProductList(searchField, searchText,
                       priceLow, priceHigh, dateFrom, dateTo, request.StatusCode, request.PaginationRequest,
                       out totalCount, out newPageIndex);
            resp.Products.AddRange(rtnList);
            resp.TotalCount = totalCount;
            return resp;
        }

        [Route("~/api/getproductlist_p")]
        public ProductListResponse Post_GetProductList([FromBody] GetProductsBySearchRequest request)
        {
            var resp = new ProductListResponse();
            resp.Products = new Models.Products();

            Models.ProductSearchField searchField = 0;
            string searchText = null;
            Decimal? priceLow = null;
            Decimal? priceHigh = null;
            DateTime? dateFrom = null;
            DateTime? dateTo = null;

            if (request.NewProductIds != null && request.NewProductIds.Count > 0)
            {
                IProductBS bs = DIFactory.GetInstance<IProductBS>();
                IList<Models.ProductCM> rtnList = bs.GetProductListNew(request.NewProductIds);
                resp.Products.AddRange(rtnList);
            }
            else
            {
                if (request.ProductSearchFilter != null)
                {
                    searchField = request.ProductSearchFilter.ProductSearchField;
                    searchText = request.ProductSearchFilter.ProductSearchText;
                }
                if (request.PriceSearchFilter != null)
                {
                    if (!String.IsNullOrEmpty(request.PriceSearchFilter.SearchPriceLow)) priceLow = Convert.ToDecimal(request.PriceSearchFilter.SearchPriceLow);
                    if (!String.IsNullOrEmpty(request.PriceSearchFilter.SearchPriceHigh)) priceHigh = Convert.ToDecimal(request.PriceSearchFilter.SearchPriceHigh);
                }
                if (request.DateSearchFilter != null)
                {
                    if (!String.IsNullOrEmpty(request.DateSearchFilter.SearchDateFrom)) dateFrom = Convert.ToDateTime(request.DateSearchFilter.SearchDateFrom);
                    if (!String.IsNullOrEmpty(request.DateSearchFilter.SearchDateTo)) dateTo = Convert.ToDateTime(request.DateSearchFilter.SearchDateTo);
                }

                int totalCount = 0;
                int newPageIndex = -1;

                IProductBS bs = DIFactory.GetInstance<IProductBS>();
                IList<Models.ProductCM> rtnList = bs.GetProductList(searchField, searchText,
                                        priceLow, priceHigh, dateFrom, dateTo, request.StatusCode, request.PaginationRequest,
                                        out totalCount, out newPageIndex);
                resp.Products.AddRange(rtnList);
                resp.TotalCount = totalCount;
                resp.newPageIndex = newPageIndex;
            }
            return resp;
        }

        [Route("~/api/getproducts")]
        public IList<Models.ProductCM> GetAllProducts()
        {
            IProductBS bs = DIFactory.GetInstance<IProductBS>();
            IList<Models.ProductCM> rtn = default(IList<Models.ProductCM>);
            rtn = bs.GetAllProductsByCategoryId(0);
            return rtn;
        }

        [Route("~/api/getallproducts")]
        public ProductListResponse GetAllProducts([ModelBinder(typeof(FieldValueModelBinder))] GetProductsBySearchRequest request)
        {
            var resp = new ProductListResponse();
            resp.Products = new Models.Products();
            int totalCount = 0;
            int newPageIndex = -1;

            IProductBS bs = DIFactory.GetInstance<IProductBS>();
            IList<Models.ProductCM> rtnList = bs.GetFullProducts(0, request.PaginationRequest, out totalCount, out newPageIndex);
            resp.Products.AddRange(rtnList);
            resp.TotalCount = totalCount;
            return resp;
        }

        [Route("~/api/getallproductsbycategoryid/{categoryId:int}")]
        public IList<Models.ProductCM> GetAllProductsByCategoryId(int categoryId)
        {
            IProductBS bs = DIFactory.GetInstance<IProductBS>();
            return bs.GetAllProductsByCategoryId(categoryId);
        }

        [Route("{id:int}", Name = "GetProductById")]
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProductById(int id)
        {
            IProductBS bs = DIFactory.GetInstance<IProductBS>();
            var eProduct = bs.GetProductById(id);
            if (eProduct == null)
            {
                return NotFound();
            }
            else
            {
                IBaseConverter<Entities.Product, Models.Product> convtResult = new AutoMapConverter<Entities.Product, Models.Product>();
                Models.Product mProduct = convtResult.ConvertObject(eProduct);
                return Ok(mProduct);
            }
        }

        [Route("~/api/addproduct")]
        public AddProductResponse Post_AddProduct([FromBody] Models.Product mProduct)
        {
            IProductBS bs = DIFactory.GetInstance<IProductBS>();
            IBaseConverter<Models.Product, Entities.Product> convtResult = new AutoMapConverter<Models.Product, Entities.Product>();
            Entities.Product eProduct = convtResult.ConvertObject(mProduct);
            bs.AddProduct(eProduct);

            var addProductResponse = new AddProductResponse()
            {
                ProductID = eProduct.ProductID
            };
            return addProductResponse;

        }

        [Route("~/api/updateproduct")]
        public void Post_UpdateProduct([FromBody] Models.Product mProduct)
        {
            IProductBS bs = DIFactory.GetInstance<IProductBS>();
            IBaseConverter<Models.Product, Entities.Product> convtResult = new AutoMapConverter<Models.Product, Entities.Product>();
            Entities.Product eProduct = convtResult.ConvertObject(mProduct);
            bs.UpdateProduct(eProduct);
        }

        [Route("~/api/deleteproduct")]
        public void DeleteProduct(int id)
        {
            IProductBS bs = DIFactory.GetInstance<IProductBS>();
            bs.DeleteProduct(id);
        }

        [Route("~/api/deleteproducts")]
        public void Post_DeleteProduct(List<int> ids)
        {
            IProductBS bs = DIFactory.GetInstance<IProductBS>();
            if (ids.Count > 0)
            {
                ids.ForEach(delegate (int id)
                {
                    bs.DeleteProduct(id);
                });
            }
        }
    }
}
