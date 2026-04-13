using Product.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;


namespace Product.Controllers
{

    [Route("api/products")]
    public class ProductsController : Controller
    {
        private readonly ProductListDAL product_list_dal = new ProductListDAL();

        // GET: Products
        //public JsonResult GetProducts()
        //{
        //    var products = new List<Models.ProductListModel>{ new Models.ProductListModel
        //    {
        //        ProductID = 1,
        //        ProductName = "Sculpted Wool Overcoat",
        //        Price = 890,
        //        ImageURL = "https://images.unsplash.com/photo-1551488831-00ddcb6c6bd3?w=600&q=80",
        //        Rating = 4.9,
        //    }
        //};

        //    return Json(products, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetProductList(long CustomerID)
        {
            var result = product_list_dal.GetProductList(CustomerID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCategoryList(long CustomerID)
        {
            var result = product_list_dal.GetCategoryList(CustomerID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}