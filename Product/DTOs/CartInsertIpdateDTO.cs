using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Product.DTOs
{
    public class CartInsertIpdateDTO
    {
        public long CartID { get; set; }
        public long UserID { get; set; }
        public long ProductID { get; set; }
        public int Quantity { get; set; }
        public int Status { get; set; }
        // 1 = Buy
        // 2 = Remove
        // 3 = SaveForLater
        // 4 = BuyAgain
        // 5 = IncreaseQty
        // 6 = DecreaseQty
        public string UserName { get; set; }
    }
}