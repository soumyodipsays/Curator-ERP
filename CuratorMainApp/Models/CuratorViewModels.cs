// Models/curatorViewModels.cs
using System;
using System.Collections.Generic;

namespace Curator.Models
{
    // =============================================
    // Home page composite ViewModel
    // =============================================
    public class HomeViewModel
    {
        public HeroViewModel Hero { get; set; }
        public IList<string> Brands { get; set; }
        public CategoryPillsViewModel CategoryPills { get; set; }
        public IList<ProductCardViewModel> FeaturedProducts { get; set; }
        public FlashSaleViewModel FlashSale { get; set; }
        public IList<RatedProductViewModel> TopRatedProducts { get; set; }
    }

    // =============================================
    // Hero section
    // =============================================
    public class HeroViewModel
    {
        public string ImageUrl { get; set; }
        public string ImageAlt { get; set; } = "Hero background";
        public string EyebrowLabel { get; set; }
        public string Heading { get; set; }   // supports <br>
        public string Subtitle { get; set; }
        public string PrimaryCtaText { get; set; }
        public string PrimaryCtaUrl { get; set; }
        public string SecondaryCtaText { get; set; }
        public string SecondaryCtaUrl { get; set; }
    }

    // =============================================
    // Category pills
    // =============================================
    public class CategoryPillsViewModel
    {
        public IList<CategoryItem> Categories { get; set; }
    }

    public class CategoryItem
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Icon { get; set; }  // Bootstrap icon class e.g. "bi-grid-3x3-gap"
        public bool IsActive { get; set; }
    }

    // =============================================
    // Generic product card (featured grid)
    // =============================================
    public class ProductCardViewModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }   // e.g. "Signature Series"
        public decimal Price { get; set; }
    }

    // =============================================
    // Flash sale
    // =============================================
    public class FlashSaleViewModel
    {
        public DateTime SaleEndTime { get; set; }
        public IList<SaleProductViewModel> Products { get; set; }
    }

    public class SaleProductViewModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int DiscountPercent { get; set; }
    }

    // =============================================
    // Top rated / community favorites
    // =============================================
    public class RatedProductViewModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public double Rating { get; set; }   // e.g. 4.9
        public int ReviewCount { get; set; }

        // Returns filled star HTML (★) — keep it simple
        public string StarsHtml
        {
            get
            {
                int full = (int)Math.Floor(Rating);
                bool half = (Rating - full) >= 0.5;
                int empty = 5 - full - (half ? 1 : 0);

                return new string('★', full)
                     + (half ? "½" : "")
                     + new string('☆', empty);
            }
        }
    }
}