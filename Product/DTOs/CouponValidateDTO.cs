using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Product.DTOs
{
    public class CouponValidateDTO
    {
        public long CustomerID { get; set; }
        public string CouponCode { get; set; }
        public float CartTotal { get; set; }
    }

    public class CouponValidateResponseDTO
    {
        public bool IsValid { get; set; }
        public string ReturnMessage { get; set; }
        public long CouponID { get; set; }
        // P = Percentage, I = INR
        public char? DiscountType { get; set; }
        public float FlatDiscountAmount { get; set; }
        public float PercentageValue { get; set; }
        public float CalculatedDiscountAmount { get; set; }
        public bool ApplyToAllProducts { get; set; }
    }
}
