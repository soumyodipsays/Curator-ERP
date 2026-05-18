using Product.DAL;
using Product.DTOs;
using Product.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;


namespace Product.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductListDAL product_list_dal = new ProductListDAL();

        public JsonResult GetProductList(long CustomerID)
        {
            var result = product_list_dal.GetProductList(CustomerID);
            foreach (var item in result)
            {
                item.VariantList = string.IsNullOrEmpty(item.Variants)
                    ? new List<ProductVariantModel>()
                    : Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProductVariantModel>>(item.Variants);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductListWithPagination(ProductListDTO dto)
        {
            var result = product_list_dal.GetProductListWithOffset(dto);
            foreach (var item in result)
            {
                item.VariantList = string.IsNullOrEmpty(item.Variants)
                    ? new List<ProductVariantModel>()
                    : Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProductVariantModel>>(item.Variants);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCategoryList(long CustomerID)
        {
            var result = product_list_dal.GetCategoryList(CustomerID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSideFilterDetails(long CustomerID)
        {
            var result = product_list_dal.GetSideFilterDetails(CustomerID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ValidateCoupon(CouponValidateDTO couponBody)
        {
            var result = product_list_dal.ValidateCoupon(couponBody);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}