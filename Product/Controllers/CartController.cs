using Product.DAL;
using Product.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Product.Controllers
{
    public class CartController : Controller
    {
        private readonly CartDAL _cartDAL = new CartDAL();
        public JsonResult GetCartDetails(long UserID)
        {
            var result = _cartDAL.GetCartDetails(UserID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrEditCart(CartInsertIpdateDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid request"
                    });
                }
                _cartDAL.AddOrEditCart(model);
                return Json(new
                {
                    success = true,
                    message = "Cart successfully updated",
                });
            }
            catch (SqlException sqlEx)
            {
                return Json(new
                {
                    success = false,
                    message = sqlEx.Message
                }); 
            }
            catch (Exception ex) 
            { 
                return Json(new
                {
                    success = false,
                    message = ex.Message
                }); 
            }
        }

        [HttpPost]
        public JsonResult SyncCartBulk(CartInsertIpdateDTO model)
        {
            try
            {
                _cartDAL.UpdateCartToProceed(model);

                return Json(new { success = true, message = "Cart synced successfully" });
            }
            catch (Exception ex)
            {
                // Log the error
                return Json(new { success = false, message = "Failed to sync cart" });
            }
        }
    }
}