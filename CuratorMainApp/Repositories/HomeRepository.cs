using Curator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Curator.Repositories
{
    public class HomeRepository
    {
        public HomeViewModel GetHomePageData()
        {
            return new HomeViewModel
            {
                Hero = GetHero(),
                Brands = GetBrands(),
                CategoryPills = GetCategoryPills(),
                FeaturedProducts = GetFeaturedProducts(),
                FlashSale = GetFlashSale(),
                TopRatedProducts = GetTopRatedProducts()
            };
        }

        private HeroViewModel GetHero()
        {
            return new HeroViewModel
            {
                ImageUrl = "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?w=1800&q=85",
                EyebrowLabel = "New Season — Spring 2025",
                Heading = "Style that<br>speaks for itself.",
                Subtitle = "Meticulously curated pieces for the modern wardrobe.",
                PrimaryCtaText = "Shop New Arrivals",
                PrimaryCtaUrl = "/products",
                SecondaryCtaText = "View Collections",
                SecondaryCtaUrl = "/collections"
            };
        }

        private List<string> GetBrands()
        {
            return new List<string>
            {
                "VOGUE","HYPEBEAST","GQ","BAZAAR",
                "COMPLEX","DAZED","SSENSE","MATCHESFASHION"
            };
        }

        private CategoryPillsViewModel GetCategoryPills()
        {
            return new CategoryPillsViewModel
            {
                Categories = new List<CategoryItem>
                    {
                        new CategoryItem { Name="All",        Slug="all",        Icon="bi-grid-3x3-gap", IsActive=true },
                        new CategoryItem { Name="Tops",       Slug="tops",       Icon="bi-bag-heart" },
                        new CategoryItem { Name="Outerwear",  Slug="outerwear",  Icon="bi-scissors" },
                        new CategoryItem { Name="Accessories",Slug="accessories",Icon="bi-gem" },
                        new CategoryItem { Name="Footwear",   Slug="footwear" },
                        new CategoryItem { Name="Bottoms",    Slug="bottoms" },
                        new CategoryItem { Name="Dresses",    Slug="dresses" },
                        new CategoryItem { Name="New In",     Slug="new-in" }
                    }
            };
        }

        private List<ProductCardViewModel> GetFeaturedProducts()
        {
            return new List<ProductCardViewModel>
            {
                    new ProductCardViewModel { Id=1, Name="Sculpted Wool Overcoat",  Label="Signature Series", Price=890,
                        ImageUrl="https://images.unsplash.com/photo-1551488831-00ddcb6c6bd3?w=900&q=80" },
                    new ProductCardViewModel { Id=2, Name="Essential Heavy Tee",      Label="Essentials",       Price=120,
                        ImageUrl="https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=600&q=80" },
                    new ProductCardViewModel { Id=3, Name="Monolith Footwear",        Label="Footwear",         Price=450,
                        ImageUrl="https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=600&q=80" }
            };
        }

        private FlashSaleViewModel GetFlashSale()
        {
            return new FlashSaleViewModel
            {
                SaleEndTime = DateTime.UtcNow.AddHours(8).AddMinutes(42).AddSeconds(15),
                Products = new List<SaleProductViewModel>
                    {
                        new SaleProductViewModel { Id=10, Name="Noir Leather Tote",  OriginalPrice=400,  SalePrice=240,  DiscountPercent=40,
                            ImageUrl="https://images.unsplash.com/photo-1548036328-c9fa89d128fa?w=500&q=80" },
                        new SaleProductViewModel { Id=11, Name="Silk Evening Gown",  OriginalPrice=1500, SalePrice=1125, DiscountPercent=25,
                            ImageUrl="https://images.unsplash.com/photo-1539533018447-63fcce2678e3?w=500&q=80" },
                        new SaleProductViewModel { Id=12, Name="Pleated Trousers",   OriginalPrice=380,  SalePrice=190,  DiscountPercent=50,
                            ImageUrl="https://images.unsplash.com/photo-1473966968600-fa801b869a1a?w=500&q=80" },
                        new SaleProductViewModel { Id=13, Name="Strata Heels",       OriginalPrice=850,  SalePrice=595,  DiscountPercent=30,
                            ImageUrl="https://images.unsplash.com/photo-1543163521-1bf539c55dd2?w=500&q=80" }
                    }
            };
        }

        private List<RatedProductViewModel> GetTopRatedProducts()
        {
            return new List<RatedProductViewModel>
            {
                new RatedProductViewModel { Id=20, Name="Heavyweight Tee",  Price=85,  Rating=4.9, ReviewCount=248,
                    ImageUrl="https://images.unsplash.com/photo-1576566588028-4147f3842f27?w=500&q=80" },
                new RatedProductViewModel { Id=21, Name="Utility Shorts",   Price=145, Rating=4.8, ReviewCount=192,
                    ImageUrl="https://encrypted-tbn1.gstatic.com/shopping?q=tbn:ANd9GcT7a-vgAPGdFmCl56gag_7XwK1hTDYVgh4pjt0qS27DoixuyaelxhHgkLo6JAtdjop42U8ifyAI_LbdyC4SiVnsCFmLB77qtfTMExggQt9qrY3Ch3c-U3JF" },
                new RatedProductViewModel { Id=22, Name="Studio Tee",       Price=95,  Rating=5.0, ReviewCount=310,
                    ImageUrl="https://images.unsplash.com/photo-1581655353564-df123a1eb820?w=500&q=80" },
                new RatedProductViewModel { Id=23, Name="Logo Cap",         Price=65,  Rating=4.7, ReviewCount=147,
                    ImageUrl="https://images.unsplash.com/photo-1588850561407-ed78c282e89b?w=500&q=80" }
            };
        }
    }
}