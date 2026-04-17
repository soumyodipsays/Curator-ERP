// Models/ViewModels/CartViewModel.cs
// Maps to: tblOrderDetail (CartID, ProductID, Quantity, Discount, TotalPrice)
//          tblOrder       (Amount, DiscountAmount, CouponAmount, PaymentAmount)
//          tblFeaturedProducts (recommended upsell products)

using System.Collections.Generic;

namespace Curator.Web.Models.ViewModels
{
    // ── Root model for Views/Cart/Index.cshtml ──────────────────────────
    public class CartViewModel
    {
        public NavViewModel Nav { get; set; } = new NavViewModel();
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
        public List<RecommendedProductViewModel> RecommendedProducts { get; set; } = new List<RecommendedProductViewModel>();
        public OrderSummaryPanelViewModel SummaryPanel { get; set; } = new OrderSummaryPanelViewModel();
    }

    // ── One row in tblOrderDetail ────────────────────────────────────────
    public class CartItemViewModel
    {
        // tblOrderDetail columns
        public long CartID { get; set; }   // udt_ID:bigint
        public long ProductID { get; set; }   // udt_ID:bigint
        public int Quantity { get; set; }   // udt_ID2:int
        public decimal Discount { get; set; }   // udt_Money:money  (per-item discount amount)
        public decimal TotalPrice { get; set; }   // udt_Money:money  (Qty × UnitPrice − Discount)

        // Resolved via joins (not stored in tblOrderDetail)
        public string ProductName { get; set; } = "";
        public string BrandName { get; set; } = "";
        public string ImageURL { get; set; } = "";
        public string Size { get; set; } = "";
        public string Color { get; set; } = "";
        public decimal? OriginalPrice { get; set; }  // before discount
        public int? DiscountPct { get; set; }   // e.g. 40 = "−40%"
    }

    // ── Upsell card from tblFeaturedProducts ────────────────────────────
    public class RecommendedProductViewModel
    {
        public long ProductID { get; set; }
        public string ProductName { get; set; } = "";
        public string ImageURL { get; set; } = "";
        public decimal Price { get; set; }
    }

    // ── Sticky sidebar — used on Cart, Address, Review pages ────────────
    // Maps: tblOrder columns
    public class OrderSummaryPanelViewModel
    {
        public int CurrentStep { get; set; } = 1;   // 1=Cart, 2=Address, 3=Review
        public int ItemCount { get; set; }

        // tblOrder.Amount
        public decimal Amount { get; set; }
        // tblOrder.DiscountAmount
        public decimal DiscountAmount { get; set; }
        // tblOrder.CouponAmount
        public decimal CouponAmount { get; set; }
        // tblOrder.PaymentAmount
        public decimal PaymentAmount { get; set; }
        // Delivery (resolved from tblOrderType)
        public decimal DeliveryFee { get; set; } = 0;

        // Coupon
        public bool ShowCouponInput { get; set; } = false;
        public string AppliedCouponCode { get; set; } = "";

        // Step-2 sidebar: mini cart images
        public List<string> ItemImageUrls { get; set; } = new List<string>();
    }
}