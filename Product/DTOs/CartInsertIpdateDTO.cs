using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Product.DTOs
{
    // 1 = Buy
    // 2 = Remove
    // 3 = SaveForLater
    // 4 = BuyAgain
    // 5 = IncreaseQty
    // 6 = DecreaseQty
    public enum CartStatus
    {
        Buy = 1,
        Remove = 2,
        SaveForLater = 3,
        BuyAgain = 4,
        IncreaseItem = 5,
        DecreaseItem = 6
    }
    public class CartInsertIpdateDTO
    {
        public long CartID { get; set; }
        public long UserID { get; set; }
        public long ProductID { get; set; }
        public int Quantity { get; set; }
        public CartStatus Status { get; set; }
        public string UserName { get; set; }
        public string CartItemsJSON { get; set; }
    }
}