// Models/ViewModels/AddressViewModel.cs
// Maps to: tblAddress  (AddressID, PersonID, Address1, Address2,
//                        City, StateID, PinCode, Location,
//                        CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)
//          tblOrderType (OrderTypeID, OrderTypeCode, OrderType, IsActive)

using System.Collections.Generic;

namespace Curator.Web.Models.ViewModels
{
    // ── Root model for Views/Checkout/Address.cshtml ─────────────────────
    public class AddressViewModel
    {
        public NavViewModel Nav { get; set; } = new NavViewModel();
        public AddressSelectionViewModel AddressSelection { get; set; } = new AddressSelectionViewModel();
        public List<OrderTypeViewModel> OrderTypes { get; set; } = new List<OrderTypeViewModel>();
        public OrderSummaryPanelViewModel SummaryPanel { get; set; } = new OrderSummaryPanelViewModel();
    }

    // ── Passed to _AddressSelection.cshtml ───────────────────────────────
    public class AddressSelectionViewModel
    {
        public List<SavedAddressViewModel> SavedAddresses { get; set; } = new List<SavedAddressViewModel>();
        public long? SelectedAddressID { get; set; }
        public List<StateViewModel> States { get; set; } = new List<StateViewModel>();
    }

    // ── One tblAddress row ───────────────────────────────────────────────
    public class SavedAddressViewModel
    {
        // tblAddress columns
        public long AddressID { get; set; }   // udt_ID:bigint  PK
        public long PersonID { get; set; }   // udt_ID:bigint  FK → Person
        public string Address1 { get; set; } = "";  // udt_LongName:varchar(100)
        public string Address2 { get; set; } = "";  // udt_LongName:varchar(100)
        public string City { get; set; } = "";  // udt_Code50:varchar(50)
        public long StateID { get; set; }   // udt_ID:bigint  FK → State
        public string PinCode { get; set; } = "";  // udt_Zip:varchar(10)
        public string Location { get; set; } = "";  // udt_LongDescription:varchar(1000) — address type label

        // Resolved via joins
        public string PersonName { get; set; } = "";   // PersonID → full name
        public string StateName { get; set; } = "";   // StateID  → state name
        public bool IsDefault { get; set; } = false;
    }

    // ── State lookup (for dropdown) ──────────────────────────────────────
    public class StateViewModel
    {
        public long StateID { get; set; }
        public string StateName { get; set; } = "";
    }

    // ── tblOrderType row ─────────────────────────────────────────────────
    public class OrderTypeViewModel
    {
        public int OrderTypeID { get; set; }   // udt_ID2:int
        public string OrderTypeCode { get; set; } = "";  // udt_Code2:char(2)  e.g. "SD","EX"
        public string OrderType { get; set; } = "";  // udt_Code50:varchar(50) e.g. "Standard"
        public bool IsActive { get; set; }   // udt_YesNo:bit

        // UI-only (not in tblOrderType)
        public string Icon { get; set; } = "📦";
        public string Description { get; set; } = "";   // "3–5 business days"
        public decimal DeliveryFee { get; set; } = 0;
        public bool IsDefault { get; set; } = false;
    }
}