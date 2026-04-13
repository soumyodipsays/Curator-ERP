using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Product.Models
{
    public class Product
    {
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductCaption { get; set; }

        public long CategoryID { get; set; }
        public long SubCategoryID { get; set; }
        public long BrandID { get; set; }

        public long? SizeID { get; set; }
        public long? ColorID { get; set; }

        public string ImageURL { get; set; }
        public decimal Price { get; set; }

        public long? DiscountID { get; set; }
        public long? CouponID { get; set; }
        public long? CustomerID { get; set; }

        public bool IsActive { get; set; }
        public DateTime? DropDate { get; set; }
        public DateTime? ActivationDate { get; set; }
        public string Status { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public Double Rating { get; set; }
        public int Reviews { get; set; }
        public bool IsNew { get; set; }
    }
}