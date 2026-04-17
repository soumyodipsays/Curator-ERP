// Controllers/CheckoutController.cs
// ─────────────────────────────────────────────────────────────────────────────
// Handles: /Checkout/Address      → Page 2 — address selection
//          /Checkout/Review       → Page 3 — order review
//          /Address/Save          → AJAX: save new tblAddress row
//          /Address/Select        → AJAX: set tblOrder.DeliveryLocationID
//          /Checkout/SetOrderType → AJAX: set tblOrder.OrderTypeID
// ─────────────────────────────────────────────────────────────────────────────
using Curator.Web.Models.Demo;
using Curator.Web.Models.ViewModels;
using System;
using System.Web.Mvc;

namespace Curator.Web.Controllers
{
    public class CheckoutController : Controller
    {
        // ── GET /Checkout/Address ─────────────────────────────────────────
        // Renders: Views/Checkout/Address.cshtml
        // Calls partials: _CheckoutStepper(2), _AddressSelection,
        //                 _DeliveryOptions, _OrderSummaryPanel
        public ActionResult Address()
        {
            var vm = new AddressViewModel
            {
                Nav = DemoData.GetNav("Cart", cartCount: 2),

                AddressSelection = new AddressSelectionViewModel
                {
                    // tblAddress rows for logged-in user
                    // REPLACE with: _addressService.GetByPerson(PersonId)
                    SavedAddresses = DemoData.GetSavedAddresses(),
                    SelectedAddressID = 1,      // default selected (tblAddress.AddressID)

                    // State lookup for new address dropdown
                    // REPLACE with: _stateService.GetAll()
                    States = DemoData.GetStates()
                },

                // tblOrderType rows (IsActive = true only)
                // REPLACE with: _orderTypeService.GetActive()
                OrderTypes = DemoData.GetOrderTypes(),

                // tblOrder totals sidebar (step 2, no coupon input)
                SummaryPanel = DemoData.GetSummaryPanel(
                    currentStep: 2,
                    showCoupon: false,
                    itemImages: new System.Collections.Generic.List<string>
                    {
                        "https://images.unsplash.com/photo-1551488831-00ddcb6c6bd3?w=80&q=80",
                        "https://images.unsplash.com/photo-1548036328-c9fa89d128fa?w=80&q=80"
                    })
            };

            ViewData["NavData"] = vm.Nav;
            return View(vm);
        }

        // ── GET /Checkout/Review ──────────────────────────────────────────
        // Renders: Views/Checkout/Review.cshtml
        // Calls partials: _CheckoutStepper(3), _OrderReviewItems,
        //                 _OrderSummaryPanel
        public ActionResult Review()
        {
            // REPLACE with: var order = _orderService.GetDraft(UserId);
            var order = DemoData.GetOrderHeader();

            var vm = new OrderReviewViewModel
            {
                Nav = DemoData.GetNav("Cart", cartCount: 2),

                // tblOrder fields
                Order = order,

                // tblOrderDetail rows → _OrderReviewItems.cshtml
                // REPLACE with: _orderDetailService.GetByOrder(order.OrderID)
                OrderDetails = DemoData.GetOrderDetails(),

                // tblAddress resolved from tblOrder.DeliveryLocationID
                // REPLACE with: _addressService.GetById(order.DeliveryLocationID)
                DeliveryAddress = DemoData.GetDeliveryAddress(),

                // tblOrderType resolved from tblOrder.OrderTypeID
                // REPLACE with: _orderTypeService.GetById(order.OrderTypeID)
                SelectedOrderType = DemoData.GetSelectedOrderType(),

                // tblOrder totals sidebar (step 3, no coupon input)
                SummaryPanel = DemoData.GetSummaryPanel(
                    currentStep: 3,
                    showCoupon: false),

                EstimatedDelivery = "Jan 22–24, 2025"
            };

            ViewData["NavData"] = vm.Nav;
            return View(vm);
        }

        // ── POST /Address/Save ────────────────────────────────────────────
        // AJAX — new address form submission
        // Inserts: tblAddress row
        // All tblAddress columns mapped from form fields
        [HttpPost]
        public ActionResult SaveAddress(
            string address1,    // tblAddress.Address1  varchar(100)
            string address2,    // tblAddress.Address2  varchar(100)
            string city,        // tblAddress.City      varchar(50)
            long stateId,     // tblAddress.StateID   bigint
            string pinCode,     // tblAddress.PinCode   varchar(10)
            string location,    // tblAddress.Location  varchar(1000) — "Home"/"Office"
            string personName,
            string phone)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(address1))
                return Json(new { success = false, message = "Address line 1 is required" });
            if (string.IsNullOrWhiteSpace(city))
                return Json(new { success = false, message = "City is required" });
            if (string.IsNullOrWhiteSpace(pinCode) || pinCode.Length < 6)
                return Json(new { success = false, message = "Enter a valid PIN code" });
            if (stateId <= 0)
                return Json(new { success = false, message = "Please select a state" });

            try
            {
                // REPLACE with real insert:
                // var newAddressId = _addressService.Save(new Address
                // {
                //     PersonID   = CurrentUser.PersonID,  // tblAddress.PersonID
                //     Address1   = address1,              // tblAddress.Address1
                //     Address2   = address2,              // tblAddress.Address2
                //     City       = city,                  // tblAddress.City
                //     StateID    = stateId,               // tblAddress.StateID
                //     PinCode    = pinCode,               // tblAddress.PinCode
                //     Location   = location,              // tblAddress.Location
                //     CreatedBy  = CurrentUser.UserName,  // tblAddress.CreatedBy
                //     CreatedOn  = DateTime.UtcNow        // tblAddress.CreatedOn
                // });

                long newAddressId = 999; // demo fake ID

                return Json(new
                {
                    success = true,
                    addressId = newAddressId,
                    message = "Address saved!",
                    // Return display values for new card
                    display = new
                    {
                        label = location,
                        name = personName,
                        address1 = address1,
                        address2 = address2,
                        city = city,
                        pin = pinCode
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ── POST /Address/Select ──────────────────────────────────────────
        // AJAX — clicking a saved address card
        // Updates: tblOrder.DeliveryLocationID = addressId
        [HttpPost]
        public ActionResult SelectAddress(long addressId)
        {
            // REPLACE with: _orderService.SetDeliveryLocation(orderId, addressId);
            // tblOrder.DeliveryLocationID = addressId
            // tblOrder.ModifiedBy = CurrentUser.UserName
            // tblOrder.ModifiedOn = DateTime.UtcNow

            return Json(new
            {
                success = true,
                addressId = addressId,
                message = "Delivery address updated"
            });
        }

        // ── POST /Checkout/SetOrderType ───────────────────────────────────
        // AJAX — clicking a delivery type chip
        // Updates: tblOrder.OrderTypeID
        [HttpPost]
        public ActionResult SetOrderType(int orderTypeId)
        {
            // REPLACE with: _orderService.SetOrderType(orderId, orderTypeId);
            // tblOrder.OrderTypeID = orderTypeId
            // tblOrder.ModifiedBy  = CurrentUser.UserName
            // tblOrder.ModifiedOn  = DateTime.UtcNow

            // Demo: look up delivery fee
            var types = DemoData.GetOrderTypes();
            var selected = types.Find(t => t.OrderTypeID == orderTypeId);

            if (selected == null)
                return Json(new { success = false, message = "Invalid order type" });

            return Json(new
            {
                success = true,
                orderTypeId = orderTypeId,
                orderType = selected.OrderType,    // tblOrderType.OrderType
                orderCode = selected.OrderTypeCode, // tblOrderType.OrderTypeCode
                deliveryFee = selected.DeliveryFee,
                feeDisplay = selected.DeliveryFee == 0
                                 ? "FREE"
                                 : $"+${selected.DeliveryFee:N0}",
                message = $"{selected.OrderType} delivery selected"
            });
        }
    }
}