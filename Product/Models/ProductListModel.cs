using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Product.Models
{
    public class ProductListModel
    {
       
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductCaption { get; set; }

        public long CategoryID { get; set; }
        public long BrandID { get; set; }
        public string Brand { get; set; }

        public long ColorID { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }

        public long SizeID { get; set; }
        public string Size { get; set; }

        public string ImageURL { get; set; }

     
        public decimal Price { get; set; }
        public int PriceID { get; set; } 

        public long? DiscountID { get; set; }
        public string IsInrOrPercentage { get; set; } 
        public string Discount { get; set; }
        public decimal? INR { get; set; }
        public decimal? Percentage { get; set; }

        public long? CouponID { get; set; }

        public bool IsActive { get; set; }
        public DateTime? DropDate { get; set; }
        public DateTime? ActivationDate { get; set; }
        public string Status { get; set; }

     
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

    
        public double Rating { get; set; }
    }

    public class CategoryListModel
    {
        public long CategoryID { get; set; }
        public string Category { get; set; }
    }

    public class FeaturedProductModel
    {
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public string ImageURL { get; set; }
        public double Price { get; set; }
    }

    public class FlashSaleProductModel
    {
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public string ImageURL { get; set; }
        public double ActualPrice { get; set; }
        public double SalePrice { get; set; }
        public DateTime SaleEndTime { get; set; }
    }

    public class BrandModel
    {
        public string BrandName { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class HomePageModel
    {
        public List<FeaturedProductModel> FeaturedProducts { get; set; }
        public List<FlashSaleProductModel> FlashSaleProducts { get; set; }
        public List<BrandModel> Brands { get; set; }
    }
}