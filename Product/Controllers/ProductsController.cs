using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Product.Controllers
{
    [Route("api/products")]
    public class ProductsController : Controller
    {
        // GET: Products
        public JsonResult GetProducts()
        {
            var products = new List<Models.Product>{ new Models.Product
            {
                ProductID = 1,
                ProductName = "Sculpted Wool Overcoat",
                Price = 890,
                ImageURL = "https://images.unsplash.com/photo-1551488831-00ddcb6c6bd3?w=600&q=80",
                Rating = 4.9,
                Reviews = 248,
                IsNew = true
            }
        };

            return Json(products, JsonRequestBehavior.AllowGet);
        }
    }
}