// Models/ViewModels/OrderReviewViewModel.cs
// Maps to: tblOrder       (OrderID, UserID, OrderTypeID, Amount, DiscountAmount,
//                           CouponAmount, PaymentAmount, DeliveryLocationID,
//                           CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)
//          tblOrderDetail  (OrderDetailID, OrderID, CartID, ProductID,
//                           Quantity, Discount, TotalPrice,
//                           CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)
//          tblOrderType    (OrderTypeID, OrderTypeCode, OrderType, IsActive)
//          tblAddress      (resolved via DeliveryLocationID)

using System;
using System.Collections.Generic;

namespace Curator.Web.Models.ViewModels
{
    // ── Root model for Views/Checkout/Review.cshtml ──────────────────────
    public class OrderReviewViewModel
    {
        public NavViewModel Nav { get; set; } = new NavViewModel();
        public OrderHeaderViewModel Order { get; set; } = new OrderHeaderViewModel();
        public List<OrderDetailViewModel> OrderDetails { get; set; } = new List<OrderDetailViewModel>();
        public SavedAddressViewModel DeliveryAddress { get; set; } = new SavedAddressViewModel();
        public OrderTypeViewModel SelectedOrderType { get; set; } = new OrderTypeViewModel();
        public OrderSummaryPanelViewModel SummaryPanel { get; set; } = new OrderSummaryPanelViewModel();
        public string EstimatedDelivery { get; set; } = "";
    }

    // ── tblOrder header ──────────────────────────────────────────────────
    public class OrderHeaderViewModel
    {
        // tblOrder columns
        public long OrderID { get; set; }   // udt_ID:bigint
        public long UserID { get; set; }   // udt_ID:bigint
        public int OrderTypeID { get; set; }   // udt_ID2:int
        public decimal Amount { get; set; }   // udt_Money:money  (gross MRP)
        public decimal DiscountAmount { get; set; }   // udt_Money:money
        public decimal CouponAmount { get; set; }   // udt_Money:money
        public decimal PaymentAmount { get; set; }   // udt_Money:money  (final payable)
        public long DeliveryLocationID { get; set; }   // udt_ID:bigint → tblAddress.AddressID

        // Audit — tblOrder
        public string CreatedBy { get; set; } = "";   // udt_User:varchar(40)
        public DateTime CreatedOn { get; set; }         // udt_Datetime:datetime
        public string ModifiedBy { get; set; } = "";   // udt_User:varchar(40)
        public DateTime? ModifiedOn { get; set; }        // udt_Datetime:datetime

        // Computed helpers
        public decimal TotalSaving => Amount - PaymentAmount;
    }

    // ── One tblOrderDetail row ───────────────────────────────────────────
    public class OrderDetailViewModel
    {
        // tblOrderDetail columns
        public long OrderDetailID { get; set; }   // udt_ID:bigint
        public long OrderID { get; set; }   // udt_ID:bigint
        public long? CartID { get; set; }   // udt_ID:bigint  (nullable)
        public long ProductID { get; set; }   // udt_ID:bigint
        public int Quantity { get; set; }   // udt_ID2:int
        public decimal Discount { get; set; }   // udt_Money:money (per-line discount amount)
        public decimal TotalPrice { get; set; }   // udt_Money:money

        // Audit — tblOrderDetail
        public string CreatedBy { get; set; } = "";
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; } = "";
        public DateTime? ModifiedOn { get; set; }

        // Resolved via joins
        public string ProductName { get; set; } = "";
        public string BrandName { get; set; } = "";
        public string ImageURL { get; set; } = "";
        public string Size { get; set; } = "";
        public decimal? OriginalPrice { get; set; }
    }
}