// Models/Demo/DemoData.cs
// ─────────────────────────────────────────────────────────────────────────────
// PURPOSE: Provides hardcoded demo objects that mirror what the real
//          database / service layer would return.
//
// REPLACE: Each method body with a real service / repository call.
//          e.g.  GetCartItems(userId)  →  _cartService.GetByUser(userId)
//
// All field names match tblOrder, tblOrderDetail, tblAddress, tblOrderType.
// ─────────────────────────────────────────────────────────────────────────────

using System;
using System.Collections.Generic;
using Curator.Web.Models.ViewModels;

namespace Curator.Web.Models.Demo
{
    public static class DemoData
    {
        // ── NAV ──────────────────────────────────────────────────────────
        public static NavViewModel GetNav(string activePage = "Home", int cartCount = 2)
            => new NavViewModel
            {
                ActivePage = activePage,
                CartCount = cartCount,
                IsLoggedIn = true,
                UserName = "Arjun Mehta"
            };

        // ── CART ITEMS (tblOrderDetail rows) ─────────────────────────────
        public static List<CartItemViewModel> GetCartItems()
            => new List<CartItemViewModel>
            {
                new CartItemViewModel
                {
                    CartID        = 101,          // tblOrderDetail.CartID
                    ProductID     = 1,            // tblOrderDetail.ProductID
                    Quantity      = 1,            // tblOrderDetail.Quantity
                    Discount      = 0m,           // tblOrderDetail.Discount
                    TotalPrice    = 890m,         // tblOrderDetail.TotalPrice
                    ProductName   = "Sculpted Wool Overcoat",
                    BrandName     = "Signature Series",
                    ImageURL      = "https://images.unsplash.com/photo-1551488831-00ddcb6c6bd3?w=200&q=80",
                    Size          = "M",
                    Color         = "Charcoal",
                    OriginalPrice = null,
                    DiscountPct   = null
                },
                new CartItemViewModel
                {
                    CartID        = 102,
                    ProductID     = 4,
                    Quantity      = 1,
                    Discount      = 160m,         // tblOrderDetail.Discount (saving amount)
                    TotalPrice    = 240m,         // tblOrderDetail.TotalPrice (after discount)
                    ProductName   = "Noir Leather Tote",
                    BrandName     = "Archive Collection",
                    ImageURL      = "https://images.unsplash.com/photo-1548036328-c9fa89d128fa?w=200&q=80",
                    Size          = "One Size",
                    Color         = "Black",
                    OriginalPrice = 400m,
                    DiscountPct   = 40
                }
            };

        // ── RECOMMENDED PRODUCTS (tblFeaturedProducts join) ───────────────
        public static List<RecommendedProductViewModel> GetRecommended()
        {
            return new List<RecommendedProductViewModel>
    {
        new RecommendedProductViewModel
        {
            ProductID = 7,
            ProductName = "Heavyweight Tee",
            ImageURL = "https://images.unsplash.com/photo-1576566588028-4147f3842f27?w=300&q=80",
            Price = 85m
        },

        new RecommendedProductViewModel
        {
            ProductID = 3,
            ProductName = "Monolith Footwear",
            ImageURL = "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=300&q=80",
            Price = 450m
        },

        new RecommendedProductViewModel
        {
            ProductID = 9,
            ProductName = "Logo Cap",
            ImageURL = "https://images.unsplash.com/photo-1588850561407-ed78c282e89b?w=300&q=80",
            Price = 65m
        },

        new RecommendedProductViewModel
        {
            ProductID = 11,
            ProductName = "Studio Tee",
            ImageURL = "https://images.unsplash.com/photo-1581655353564-df123a1eb820?w=300&q=80",
            Price = 95m
        }
    };
        }

        // ── ORDER SUMMARY PANEL ───────────────────────────────────────────
        // Maps: tblOrder.Amount, DiscountAmount, CouponAmount, PaymentAmount
        public static OrderSummaryPanelViewModel GetSummaryPanel(
            int currentStep, bool showCoupon = false,
            List<string> itemImages = null)
            => new OrderSummaryPanelViewModel
            {
                CurrentStep = currentStep,
                ItemCount = 2,
                Amount = 1290m,     // tblOrder.Amount          (gross MRP)
                DiscountAmount = 160m,      // tblOrder.DiscountAmount
                CouponAmount = 0m,        // tblOrder.CouponAmount
                PaymentAmount = 1130m,     // tblOrder.PaymentAmount   (final)
                DeliveryFee = 0m,
                ShowCouponInput = showCoupon,
                AppliedCouponCode = "",
                ItemImageUrls = itemImages ?? new List<string>
                {
                    "https://images.unsplash.com/photo-1551488831-00ddcb6c6bd3?w=80&q=80",
                    "https://images.unsplash.com/photo-1548036328-c9fa89d128fa?w=80&q=80"
                }
            };

        // ── SAVED ADDRESSES (tblAddress rows) ────────────────────────────
        public static List<SavedAddressViewModel> GetSavedAddresses()
            => new List<SavedAddressViewModel>
            {
                new SavedAddressViewModel
                {
                    AddressID  = 1,                      // tblAddress.AddressID
                    PersonID   = 500,                    // tblAddress.PersonID
                    Address1   = "42B, Park Street, Lake Gardens",   // tblAddress.Address1
                    Address2   = "Near Ruby Hospital",               // tblAddress.Address2
                    City       = "Kolkata",              // tblAddress.City
                    StateID    = 1,                      // tblAddress.StateID
                    PinCode    = "700045",               // tblAddress.PinCode
                    Location   = "Home",                 // tblAddress.Location
                    PersonName = "Arjun Mehta",
                    StateName  = "West Bengal",
                    IsDefault  = true
                },
                new SavedAddressViewModel
                {
                    AddressID  = 2,
                    PersonID   = 500,
                    Address1   = "7th Floor, Merlin Infinite, Salt Lake",
                    Address2   = "Sector V",
                    City       = "Kolkata",
                    StateID    = 1,
                    PinCode    = "700091",
                    Location   = "Office",
                    PersonName = "Arjun Mehta",
                    StateName  = "West Bengal",
                    IsDefault  = false
                }
            };

        // ── STATE DROPDOWN ────────────────────────────────────────────────
        public static List<StateViewModel> GetStates()
        {
            return new List<StateViewModel>
    {
        new StateViewModel { StateID = 1,  StateName = "West Bengal" },
        new StateViewModel { StateID = 2,  StateName = "Maharashtra" },
        new StateViewModel { StateID = 3,  StateName = "Delhi" },
        new StateViewModel { StateID = 4,  StateName = "Karnataka" },
        new StateViewModel { StateID = 5,  StateName = "Tamil Nadu" },
        new StateViewModel { StateID = 6,  StateName = "Telangana" },
        new StateViewModel { StateID = 7,  StateName = "Gujarat" },
        new StateViewModel { StateID = 8,  StateName = "Rajasthan" },
        new StateViewModel { StateID = 9,  StateName = "Punjab" },
        new StateViewModel { StateID = 10, StateName = "Kerala" }
    };
        }

        // ── ORDER TYPES (tblOrderType rows) ──────────────────────────────
        public static List<OrderTypeViewModel> GetOrderTypes()
            => new List<OrderTypeViewModel>
            {
                new OrderTypeViewModel
                {
                    OrderTypeID   = 1,          // tblOrderType.OrderTypeID
                    OrderTypeCode = "SD",        // tblOrderType.OrderTypeCode
                    OrderType     = "Standard",  // tblOrderType.OrderType
                    IsActive      = true,        // tblOrderType.IsActive
                    Icon          = "🚀",
                    Description   = "3–5 business days",
                    DeliveryFee   = 0m,
                    IsDefault     = true
                },
                new OrderTypeViewModel
                {
                    OrderTypeID   = 2,
                    OrderTypeCode = "EX",
                    OrderType     = "Express",
                    IsActive      = true,
                    Icon          = "⚡",
                    Description   = "1–2 business days",
                    DeliveryFee   = 15m,
                    IsDefault     = false
                },
                new OrderTypeViewModel
                {
                    OrderTypeID   = 3,
                    OrderTypeCode = "SM",
                    OrderType     = "Same Day",
                    IsActive      = true,
                    Icon          = "📦",
                    Description   = "Before 10 PM today",
                    DeliveryFee   = 25m,
                    IsDefault     = false
                }
            };

        // ── ORDER HEADER (tblOrder) ───────────────────────────────────────
        public static OrderHeaderViewModel GetOrderHeader()
            => new OrderHeaderViewModel
            {
                OrderID = 9001,       // tblOrder.OrderID
                UserID = 500,         // tblOrder.UserID
                OrderTypeID = 1,           // tblOrder.OrderTypeID
                Amount = 1290m,       // tblOrder.Amount
                DiscountAmount = 160m,        // tblOrder.DiscountAmount
                CouponAmount = 0m,          // tblOrder.CouponAmount
                PaymentAmount = 1130m,       // tblOrder.PaymentAmount
                DeliveryLocationID = 1,           // tblOrder.DeliveryLocationID → tblAddress.AddressID
                CreatedBy = "System",    // tblOrder.CreatedBy
                CreatedOn = DateTime.Now,
                ModifiedBy = "System",
                ModifiedOn = DateTime.Now
            };

        // ── ORDER DETAILS (tblOrderDetail rows) ──────────────────────────
        public static List<OrderDetailViewModel> GetOrderDetails()
            => new List<OrderDetailViewModel>
            {
                new OrderDetailViewModel
                {
                    OrderDetailID = 2001,        // tblOrderDetail.OrderDetailID
                    OrderID       = 9001,        // tblOrderDetail.OrderID
                    CartID        = 101,         // tblOrderDetail.CartID
                    ProductID     = 1,           // tblOrderDetail.ProductID
                    Quantity      = 1,           // tblOrderDetail.Quantity
                    Discount      = 0m,          // tblOrderDetail.Discount
                    TotalPrice    = 890m,        // tblOrderDetail.TotalPrice
                    CreatedBy     = "System",
                    CreatedOn     = DateTime.Now,
                    ProductName   = "Sculpted Wool Overcoat",
                    BrandName     = "Signature Series",
                    ImageURL      = "https://images.unsplash.com/photo-1551488831-00ddcb6c6bd3?w=150&q=80",
                    Size          = "M",
                    OriginalPrice = null
                },
                new OrderDetailViewModel
                {
                    OrderDetailID = 2002,
                    OrderID       = 9001,
                    CartID        = 102,
                    ProductID     = 4,
                    Quantity      = 1,
                    Discount      = 160m,        // tblOrderDetail.Discount
                    TotalPrice    = 240m,        // tblOrderDetail.TotalPrice
                    CreatedBy     = "System",
                    CreatedOn     = DateTime.Now,
                    ProductName   = "Noir Leather Tote",
                    BrandName     = "Archive Collection",
                    ImageURL      = "https://images.unsplash.com/photo-1548036328-c9fa89d128fa?w=150&q=80",
                    Size          = "One Size",
                    OriginalPrice = 400m
                }
            };

        // ── DELIVERY ADDRESS (tblAddress via DeliveryLocationID) ──────────
        public static SavedAddressViewModel GetDeliveryAddress()
            => GetSavedAddresses()[0]; // AddressID = 1 (default)

        // ── SELECTED ORDER TYPE ───────────────────────────────────────────
        public static OrderTypeViewModel GetSelectedOrderType()
            => GetOrderTypes()[0]; // Standard (default)
    }
}