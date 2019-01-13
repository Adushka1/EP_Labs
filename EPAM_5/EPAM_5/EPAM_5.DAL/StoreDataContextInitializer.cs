using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using EPAM_5.Entities;

namespace EPAM_5.DAL
{
    public class StoreDataContextInitializer : DropCreateDatabaseIfModelChanges<StoreDataContext>
    {
        protected override void Seed(StoreDataContext context)
        {
            ProductStatusType[] statusTypes = new ProductStatusType[]
            {
                new ProductStatusType { StatusCode = 1, Description = "Available", AuditTime = Convert.ToDateTime("2018-08-26")},
                new ProductStatusType { StatusCode = 2, Description = "Out of Stock", AuditTime = Convert.ToDateTime("2018-09-26")},
                new ProductStatusType { StatusCode = 3, Description = "Back Ordered", AuditTime = Convert.ToDateTime("2018-09-26")},
                new ProductStatusType { StatusCode = 4, Description = "Discontinued", AuditTime = Convert.ToDateTime("2018-10-26")},
                new ProductStatusType { StatusCode = 5, Description = "Undefined", AuditTime = Convert.ToDateTime("2018-10-26")}
            };
            context.ProductStatusTypes.AddRange(statusTypes);

            Category[] categories = new Category[]
            {
                new Category { CategoryID = 1, CategoryName = "Bath", AuditTime = DateTime.Now,
                    Products = new Product[] { 
                        new Product { ProductID = 1, ProductName = "Bath Rug", UnitPrice = 24.5m, StatusCode = 1, AvailableSince = Convert.ToDateTime("2018-07-10"), AuditTime = DateTime.Now},                                                
                        new Product { ProductID = 2, ProductName = "Shower Curtain", UnitPrice = 30.99m, StatusCode = 1, AvailableSince = Convert.ToDateTime("2018-07-13"), AuditTime = DateTime.Now},
                        new Product { ProductID = 3, ProductName = "Soap Dispenser", UnitPrice = 12.4m, StatusCode = 2, AvailableSince = Convert.ToDateTime("2018-08-05"), AuditTime = DateTime.Now},
                        new Product { ProductID = 4, ProductName = "Toilet Tissue", UnitPrice = 15, StatusCode = 3, AvailableSince = Convert.ToDateTime("2018-05-16"), AuditTime = DateTime.Now},
                    }.ToList()},
                new Category { CategoryID = 2, CategoryName = "Bedding", AuditTime = DateTime.Now,
                    Products = new Product[] { 
                        new Product { ProductID = 5, ProductName = "Branket", UnitPrice = 60, StatusCode = 1, AvailableSince = Convert.ToDateTime("2019-08-22"), AuditTime = DateTime.Now},
                        new Product { ProductID = 6, ProductName = "Mattress Protector", UnitPrice = 30.4m, StatusCode = 2, AvailableSince = Convert.ToDateTime("2020-08-22"), AuditTime = DateTime.Now },
                        new Product { ProductID = 7, ProductName = "Sheet Set", UnitPrice = 40.69m, StatusCode = 1, AvailableSince = Convert.ToDateTime("2018-07-26"), AuditTime = DateTime.Now},
                        new Product { ProductID = 8, ProductName = "Pillow", UnitPrice = 10.2m, StatusCode = 1, AvailableSince = Convert.ToDateTime("2019-10-04"), AuditTime = DateTime.Now},
                    }.ToList()},
                new Category { CategoryID = 3, CategoryName = "kitchen", AuditTime = DateTime.Now,
                    Products = new Product[] { 
                        new Product { ProductID = 9, ProductName = "Baking Pan", UnitPrice = 10.99m, StatusCode = 1, AvailableSince = Convert.ToDateTime("2018-10-26"), AuditTime = DateTime.Now},
                        new Product { ProductID = 10, ProductName = "Can Opener", UnitPrice = 7.99m, StatusCode = 5, AvailableSince = Convert.ToDateTime("2018-09-18"), AuditTime = DateTime.Now},
                        new Product { ProductID = 11, ProductName = "Coffee Maker", UnitPrice = 49.39m, StatusCode = 4, AvailableSince = null, AuditTime = DateTime.Now},
                        new Product { ProductID = 12, ProductName = "Knife Set", UnitPrice = 70, StatusCode = 1, AvailableSince = Convert.ToDateTime("2017-10-10"), AuditTime = DateTime.Now},
                        new Product { ProductID = 13, ProductName = "Pressure Cooker", UnitPrice = 90.5m, StatusCode = 2, AvailableSince = Convert.ToDateTime("2019-10-26"), AuditTime = DateTime.Now },
                        new Product { ProductID = 14, ProductName = "Water Pitcher", UnitPrice = 29.99m, StatusCode = 3, AvailableSince = null, AuditTime = DateTime.Now},                        
                    }.ToList()},
                new Category { CategoryID = 4, CategoryName = "Undefined", AuditTime = DateTime.Now},
            };
            context.Categories.AddRange(categories);        
            context.SaveChanges();

            context.Database.ExecuteSqlCommand(
              @"CREATE PROCEDURE dbo.GetAllCategorisAndProducts 
                AS 
                BEGIN
                SET NOCOUNT ON 
                SELECT c.CategoryID,  
	                   c.CategoryName, 
		               p.ProductCount
	            FROM dbo.Category c 
	            JOIN (SELECT count(ProductID) AS ProductCount, CategoryID
		              FROM Product			 
			          GROUP BY CategoryID) p
	              ON p.CategoryID = c.CategoryID 
                SELECT p.ProductID, 
                     p.ProductName, 
                     p.CategoryID,
                     c.CategoryName, 
                     p.StatusCode, 
                     s.Description AS StatusDescription, 
                     p.UnitPrice, 
                     p.AvailableSince 
                FROM dbo.Product p
                join dbo.Category c on c.CategoryID = p.CategoryID 
                join dbo.ProductStatusType s on s.StatusCode = p.StatusCode 
                END "
            );
            context.Database.ExecuteSqlCommand(
               @"CREATE PROCEDURE dbo.GetProductCM 
                AS 
                BEGIN 
                SET NOCOUNT ON 
                SELECT p.ProductID, 
                     p.ProductName, 
                     p.CategoryID,
                     c.CategoryName, 
                     p.StatusCode, 
                     s.Description AS StatusDescription, 
                     p.UnitPrice, 
                     p.AvailableSince 
                FROM dbo.Product p
                join dbo.Category c on c.CategoryID = p.CategoryID 
                join dbo.ProductStatusType s on s.StatusCode = p.StatusCode 
                END "
            );
        }
    }
}