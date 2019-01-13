using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace EPAM_5.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Product
    {
        [JsonProperty(PropertyName = "ProductID")]        
        public int ProductID { get; set; }

        [JsonProperty(PropertyName = "ProductName")]
        [Required(ErrorMessage = "Product Name is required<br/>")]
        public string ProductName { get; set; }

        [JsonProperty(PropertyName = "CategoryID")]
        [Range(1, 2147483647, ErrorMessage = "Please select a category<br/>")]
        public int CategoryID { get; set; }

        [JsonProperty(PropertyName = "UnitPrice")]
        [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "Price can't have more than 2 decimal places")]
        [Range(0.01, 1000, ErrorMessage = "Price can't be larger than $1000<br/>")]
        public Nullable<decimal> UnitPrice { get; set; }

        [JsonProperty(PropertyName = "StatusCode")]
        public Nullable<int> StatusCode { get; set; }

        [JsonProperty(PropertyName = "AvailableSince")]
        public Nullable<DateTime> AvailableSince { get; set; }
    }
}
