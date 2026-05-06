using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Product.Models
{
    public class CartModel
    {
        public long CartID { get; set; }
        public long UserID { get; set; }
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public DateTime? AddDate { get; set; }
        public bool IsBuy { get; set; }
        public DateTime? BuyDate { get; set; }
        public bool IsRemove { get; set; }
        public DateTime? RemoveDate { get; set; }
        public bool IsSaveForLater { get; set; }
        public DateTime? SaveForLaterDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ImageURL { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string BrandName { get; set; }
    }
}