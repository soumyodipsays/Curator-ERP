// Controllers/CartController.cs
// ─────────────────────────────────────────────────────────────────────────────
// Handles: /Cart          → Page 1 of checkout (cart view)
//          /Cart/Add      → AJAX: add product to cart
//          /Cart/UpdateQty→ AJAX: change quantity  (tblOrderDetail.Quantity)
//          /Cart/Remove   → AJAX: remove item      (tblOrderDetail.CartID)
//          /Cart/ApplyCoupon → AJAX: apply coupon  (tblOrder.CouponAmount)
// ─────────────────────────────────────────────────────────────────────────────

using Curator.Web.Models.Demo;
using Curator.Web.Models.ViewModels;
using System;
using System.Web.Mvc;

namespace Curator.Web.Controllers
{
    public class CartController : Controller
    {
        // ── GET /Cart ─────────────────────────────────────────────────────
        // Renders: Views/Cart/Index.cshtml
        // Calls partials: _CheckoutStepper(1), _CartItems, _OrderSummaryPanel
        public ActionResult Index()
        {
            // REPLACE with: var items = _cartService.GetByUser(UserId);
            var items = DemoData.GetCartItems();

            var vm = new CartViewModel
            {
                Nav = DemoData.GetNav("Cart", items.Count),

                // tblOrderDetail rows → _CartItems.cshtml
                Items = items,

                // tblFeaturedProducts → upsell row
                RecommendedProducts = DemoData.GetRecommended(),

                // tblOrder totals → _OrderSummaryPanel.cshtml (step 1, show coupon)
                SummaryPanel = DemoData.GetSummaryPanel(
                    currentStep: 1,
                    showCoupon: true)
            };

            // Pass NavData to _Layout so _Navbar + _BottomNav get cart count
            ViewData["NavData"] = vm.Nav;

            return View(vm);
        }

        // ── POST /Cart/Add ────────────────────────────────────────────────
        // AJAX — called from product cards ("Add to Bag" button)
        // Body: { productId, qty, size, color }
        // Inserts row into tblOrderDetail
        [HttpPost]
        public ActionResult Add(long productId, int qty = 1,
                                 string size = "", string color = "")
        {
            try
            {
                // REPLACE with: _cartService.Add(UserId, productId, qty, size, color);
                var newCartCount = 3; // demo: pretend count went up

                return Json(new
                {
                    success = true,
                    cartCount = newCartCount,
                    message = "Added to bag!"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ── POST /Cart/UpdateQty ──────────────────────────────────────────
        // AJAX — qty stepper on Cart page
        // Updates: tblOrderDetail.Quantity, TotalPrice
        [HttpPost]
        public ActionResult UpdateQty(long cartId, int qty)
        {
            if (qty < 1 || qty > 10)
                return Json(new { success = false, message = "Invalid quantity" });

            // REPLACE with: _cartService.UpdateQty(cartId, qty);
            // Also recalculates tblOrder.Amount / PaymentAmount

            // Demo: return updated line total
            decimal unitPrice = 890m;          // fetch from DB in real impl
            decimal lineTotal = unitPrice * qty;

            return Json(new
            {
                success = true,
                lineTotal = lineTotal.ToString("C"),
                // Updated order totals (tblOrder fields)
                subtotal = "$1,130",
                total = "$1,130"
            });
        }

        // ── POST /Cart/Remove ─────────────────────────────────────────────
        // AJAX — remove item button on Cart page
        // Deletes: tblOrderDetail row by CartID
        [HttpPost]
        public ActionResult Remove(long cartId)
        {
            try
            {
                // REPLACE with: _cartService.Remove(cartId);
                return Json(new
                {
                    success = true,
                    cartCount = 1,              // updated cart count
                    message = "Item removed"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ── POST /Cart/ApplyCoupon ────────────────────────────────────────
        // AJAX — coupon code input in summary panel
        // Updates: tblOrder.CouponAmount, PaymentAmount
        [HttpPost]
        public ActionResult ApplyCoupon(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return Json(new { success = false, message = "Enter a coupon code" });

            // REPLACE with: var result = _couponService.Apply(orderId, code);
            // Demo: hardcoded valid code
            if (code.ToUpper() == "CURATOR10")
            {
                decimal couponSaving = 113m;     // 10% of 1130
                decimal newTotal = 1017m;

                return Json(new
                {
                    success = true,
                    message = "✓ Coupon applied — 10% off!",
                    couponAmount = $"−${couponSaving:N0}",  // tblOrder.CouponAmount display
                    newTotal = $"${newTotal:N0}",        // tblOrder.PaymentAmount display
                    saving = $"${couponSaving:N0}"
                });
            }

            return Json(new { success = false, message = "Invalid or expired coupon" });
        }
    }
}