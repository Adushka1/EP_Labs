using System;
using EPAM_5.Entities;
using System.Linq;
using System.Collections.Generic;
using EPAM_5.DAL;
using System.Data.Entity;
using EPAM_5.Common;
using EPAM_5.Models;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;

namespace EPAM_5.DAL
{
    public class ProductRepository : GenericRepository<Entities.Product>, IProductRepository
    {
        public ProductRepository(IStoreDataUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IList<Entities.Product> GetProducts()
        {
            return this.GetAll();
        }

        public Entities.Product GetProductById(int id)
        {
            return this.GetById(id);
        }

        public IList<Models.ProductCM> GetProductList(ProductSearchField productSearchField, string productSearchText,
                            Decimal? priceLow, Decimal? priceHigh, DateTime? dateFrom, DateTime? dateTo, int? statusCode,
                            PaginationRequest paging, out int totalCount, out int newPageIndex)
        {

            IQueryable<Models.ProductCM> query = this.UnitOfWork.Context.Products
                       .GroupJoin(this.UnitOfWork.Context.Categories,
                        p => p.CategoryID, c => c.CategoryID,
                        (p, c) => new { p, c })
                       .GroupJoin(this.UnitOfWork.Context.ProductStatusTypes,
                        p1 => p1.p.StatusCode, s => s.StatusCode,
                       (p1, s) => new { p1, s })
                       .SelectMany(p2 => p2.s.DefaultIfEmpty(), (p2, s2) => new { p2 = p2.p1, s2 = s2 })
                       .Select(f => new Models.ProductCM
                       {
                           ProductID = f.p2.p.ProductID,
                           ProductName = f.p2.p.ProductName,
                           CategoryID = f.p2.p.CategoryID,
                           CategoryName = f.p2.p.Category.CategoryName,
                           UnitPrice = f.p2.p.UnitPrice,
                           StatusCode = f.p2.p.StatusCode,
                           StatusDescription = f.s2.Description,
                           AvailableSince = f.p2.p.AvailableSince
                       });


            var predicate = PredicateBuilder.True<Models.ProductCM>();
            
            if (!string.IsNullOrEmpty(productSearchText))
            {
                if (productSearchField == ProductSearchField.CategoryId && Util.IsNumeric(productSearchText))
                {
                    int categoryId = Convert.ToInt32(productSearchText);
                    predicate = predicate.And(p => p.CategoryID == categoryId);
                }
                if (productSearchField == ProductSearchField.CategoryName)
                {
                    predicate = predicate.And(p => p.CategoryName.ToLower().Contains(productSearchText.ToLower()));
                }
                if (productSearchField == ProductSearchField.ProductId && Util.IsNumeric(productSearchText))
                {
                    int productId = Convert.ToInt32(productSearchText);
                    predicate = predicate.And(p => p.ProductID == productId);
                }
                if (productSearchField == ProductSearchField.ProductName)
                {
                    predicate = predicate.And(p => p.ProductName.ToLower().Contains(productSearchText.ToLower()));
                }
            }
            if (priceLow != null)
            {
                predicate = predicate.And(p => p.UnitPrice >= priceLow.Value);
            }
            if (priceHigh != null)
            {
                predicate = predicate.And(p => p.UnitPrice <= priceHigh.Value);
            }
            if (dateFrom != null)
            {
                predicate = predicate.And(p => p.AvailableSince >= dateFrom.Value);
            }
            if (dateTo != null)
            {
                predicate = predicate.And(p => p.AvailableSince <= dateTo.Value);
            }
            if (statusCode != null)
            {
                predicate = predicate.And(p => p.StatusCode == statusCode.Value);
            }                        
            query = query.Where(predicate);

            IList<Models.ProductCM> resultList =
                GenericSorterPager.GetSortedPagedList<Models.ProductCM>(query, paging, out totalCount);
            
            newPageIndex = -1;
            while (paging.PageIndex > 0 && resultList.Count < 1)
            {
                paging.PageIndex -= 1;
                newPageIndex = paging.PageIndex;
                resultList =
                GenericSorterPager.GetSortedPagedList<Models.ProductCM>(query, paging, out totalCount);                
            }            
            return resultList;
        }

        public IList<Models.ProductCM> GetProductListNew(List<int> newProductIds)
        {
            IQueryable<Models.ProductCM> query = this.UnitOfWork.Context.Products
                       .GroupJoin(this.UnitOfWork.Context.Categories,
                        p => p.CategoryID, c => c.CategoryID,
                        (p, c) => new { p, c })
                       .GroupJoin(this.UnitOfWork.Context.ProductStatusTypes,
                        p1 => p1.p.StatusCode, s => s.StatusCode,
                       (p1, s) => new { p1, s })
                       .SelectMany(p2 => p2.s.DefaultIfEmpty(), (p2, s2) => new { p2 = p2.p1, s2 = s2 })
                       .Select(f => new Models.ProductCM
                       {
                           ProductID = f.p2.p.ProductID,
                           ProductName = f.p2.p.ProductName,
                           CategoryID = f.p2.p.CategoryID,
                           CategoryName = f.p2.p.Category.CategoryName,
                           UnitPrice = f.p2.p.UnitPrice,
                           StatusCode = f.p2.p.StatusCode,
                           StatusDescription = f.s2.Description,
                           AvailableSince = f.p2.p.AvailableSince
                       });            
            
            query = query.Where(a => newProductIds.Contains(a.ProductID));
            return query.ToList();            
        }

        public IList<Models.ProductCM> GetFullProducts(int categoryId, PaginationRequest paging, out int totalCount, out int newPageIndex)
        {
            IQueryable<Models.ProductCM> query = this.UnitOfWork.Context.Products
                       .GroupJoin(this.UnitOfWork.Context.Categories,
                        p => p.CategoryID, c => c.CategoryID,
                        (p, c) => new { p, c })
                       .GroupJoin(this.UnitOfWork.Context.ProductStatusTypes,
                        p1 => p1.p.StatusCode, s => s.StatusCode,
                       (p1, s) => new { p1, s })
                       .SelectMany(p2 => p2.s.DefaultIfEmpty(), (p2, s2) => new { p2 = p2.p1, s2 = s2 })
                       .Select(f => new Models.ProductCM
                       {
                           ProductID = f.p2.p.ProductID,
                           ProductName = f.p2.p.ProductName,
                           CategoryID = f.p2.p.CategoryID,
                           CategoryName = f.p2.p.Category.CategoryName,
                           UnitPrice = f.p2.p.UnitPrice,
                           StatusCode = f.p2.p.StatusCode,
                           StatusDescription = f.s2.Description,
                           AvailableSince = f.p2.p.AvailableSince
                       });

            var predicate = PredicateBuilder.True<Models.ProductCM>();
            if (categoryId > 0)
            {
                predicate = predicate.And(p => p.CategoryID == categoryId);
            }
            query = query.Where(predicate);

            IList<Models.ProductCM> resultList = GenericSorterPager.GetSortedPagedList<Models.ProductCM>(query, paging, out totalCount);

            newPageIndex = -1;
            while (paging.PageIndex > 0 && resultList.Count < 1)
            {
                paging.PageIndex -= 1;
                newPageIndex = paging.PageIndex;
                resultList =
                GenericSorterPager.GetSortedPagedList<Models.ProductCM>(query, paging, out totalCount);
            }    
            return resultList;
        }

        public IList<Entities.Product> GetProductByCategoryId(int categoryId, PaginationRequest paging, out int totalCount, out int newPageIndex)
        {
            IQueryable<Entities.Product> query = this.UnitOfWork.Context.Products
                                         .Where(a => a.CategoryID == categoryId)
                                        .Include(a => a.ProductStatusType)
                                        ;
            IList<Entities.Product> resultList = GenericSorterPager.GetSortedPagedList<Entities.Product>(query, paging, out totalCount, ChildLoad.Include);

            newPageIndex = -1;
            while (paging.PageIndex > 0 && resultList.Count < 1)
            {
                paging.PageIndex -= 1;
                newPageIndex = paging.PageIndex;
                resultList =
                GenericSorterPager.GetSortedPagedList<Entities.Product>(query, paging, out totalCount, ChildLoad.Include);
            }    
            return resultList;
        }

        public IList<Models.ProductCM> GetAllProductsByCategoryId(int categoryId)
        {
            IQueryable<Models.ProductCM> query = this.UnitOfWork.Context.Products
                       .GroupJoin(this.UnitOfWork.Context.Categories,
                        p => p.CategoryID, c => c.CategoryID,
                        (p, c) => new { p, c })
                       .GroupJoin(this.UnitOfWork.Context.ProductStatusTypes,
                        p1 => p1.p.StatusCode, s => s.StatusCode,
                       (p1, s) => new { p1, s })
                       .SelectMany(p2 => p2.s.DefaultIfEmpty(), (p2, s2) => new { p2 = p2.p1, s2 = s2 })
                       .Select(f => new Models.ProductCM
                       {
                           ProductID = f.p2.p.ProductID,
                           ProductName = f.p2.p.ProductName,
                           CategoryID = f.p2.p.CategoryID,
                           CategoryName = f.p2.p.Category.CategoryName,
                           UnitPrice = f.p2.p.UnitPrice,
                           StatusCode = f.p2.p.StatusCode,
                           StatusDescription = f.s2.Description,
                           AvailableSince = f.p2.p.AvailableSince
                       });

            var predicate = PredicateBuilder.True<Models.ProductCM>();
            if (categoryId > 0)
            {
                predicate = predicate.And(p => p.CategoryID == categoryId);
            }
            query = query.Where(predicate);
            return query.ToList();
        }

        public int AddProduct(Entities.Product inputEt)
        {
            inputEt.ProductID = 0;
            inputEt.AuditTime = DateTime.Now;
            this.Insert(inputEt);
            this.CommitAllChanges();
            return inputEt.ProductID;
        }

        public void UpdateProduct(Entities.Product inputEt)
        {
            Entities.Product updEt = GetProductById(inputEt.ProductID);

            if (!string.IsNullOrEmpty(inputEt.ProductName)) updEt.ProductName = inputEt.ProductName;
            if (inputEt.CategoryID > 0) updEt.CategoryID = inputEt.CategoryID;
            if (inputEt.UnitPrice != null) updEt.UnitPrice = inputEt.UnitPrice;
            if (inputEt.StatusCode > 0)
            {
                updEt.StatusCode = inputEt.StatusCode;
            }
            else
            {
                updEt.StatusCode = null;
            }
            updEt.AvailableSince = inputEt.AvailableSince;            
            updEt.AuditTime = DateTime.Now;

            this.Update(updEt);
            this.CommitAllChanges();
        }

        public void DeleteProduct(int id)
        {
            this.Delete(id);
            this.CommitAllChanges();
        }
    }
}
