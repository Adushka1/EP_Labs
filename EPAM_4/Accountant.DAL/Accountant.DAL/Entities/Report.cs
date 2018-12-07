using System.Collections.Generic;

namespace Accountant.DAL.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Client Customer { get; set; }
        public int ProductId { get; set; }
        public Product PurchasedProduct { get; set; }
        public decimal Cost { get; set; }
    }
}