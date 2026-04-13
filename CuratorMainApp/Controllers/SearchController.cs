
// Controllers/SearchController.cs
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Curator.Models;

namespace Curator.Controllers
{
    public class SearchController : Controller
    {
        // GET /Search/Suggestions?q=wool
        // Returns JSON for the overlay AJAX call
        [HttpGet]
        public JsonResult Suggestions(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return Json(new { suggestions = new string[0], products = new object[0] },
                            JsonRequestBehavior.AllowGet);

            q = q.ToLower().Trim();

            // --- Replace with real DB queries ---
            var allTerms = new[] {
                "Wool overcoat","Wool blend coat","Silk dress","Silk blouse",
                "Leather tote","Leather jacket","Pleated trousers","Platform heels",
                "Essentials tee","Linen shirt","Monochrome set","Logo cap"
            };

            var suggestions = allTerms
                .Where(t => t.ToLower().Contains(q))
                .Take(6)
                .ToList();

            // Quick product hits — swap for real ProductRepository call
            var allProducts = GetSampleProducts();
            var products = allProducts
                .Where(p => p.Name.ToLower().Contains(q))
                .Take(4)
                .Select(p => new {
                    p.Id,
                    p.Name,
                    Price = p.Price.ToString("C0"),
                    ImageUrl = p.ImageUrl
                });

            return Json(new { suggestions, products },
                        JsonRequestBehavior.AllowGet);
        }

        // GET /Search/Results?q=wool
        public ActionResult Results(string q)
        {
            ViewBag.Query = q;
            // Build your search results ViewModel here
            return View();
        }

        // ---- sample data — delete once you wire up a real repo ----
        private IList<ProductCardViewModel> GetSampleProducts()
        {
            return new List<ProductCardViewModel> {
                new ProductCardViewModel { Id=1, Name="Sculpted Wool Overcoat", Price=890,
                    ImageUrl="https://images.unsplash.com/photo-1551488831-00ddcb6c6bd3?w=80&q=70" },
                new ProductCardViewModel { Id=2, Name="Silk Evening Gown",      Price=1125,
                    ImageUrl="https://images.unsplash.com/photo-1539533018447-63fcce2678e3?w=80&q=70" },
                new ProductCardViewModel { Id=3, Name="Noir Leather Tote",      Price=240,
                    ImageUrl="https://images.unsplash.com/photo-1548036328-c9fa89d128fa?w=80&q=70" },
                new ProductCardViewModel { Id=4, Name="Pleated Trousers",       Price=190,
                    ImageUrl="https://images.unsplash.com/photo-1473966968600-fa801b869a1a?w=80&q=70" },
                new ProductCardViewModel { Id=5, Name="Strata Heels",           Price=595,
                    ImageUrl="https://images.unsplash.com/photo-1543163521-1bf539c55dd2?w=80&q=70" }
            };
        }
    }
}
