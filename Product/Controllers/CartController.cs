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

        public JsonResult AddorEditCart(CartInsertIpdateDTO model)
        {
            try
            {
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
                // 1. Set the UserID and UserName securely from your session/authentication context.
                // Hardcoding for now just so you can test it based on your DB screenshot!
                model.UserID = 14;
                model.UserName = "Sayan";

                // 2. Call your data access method that you just wrote
                // (Assuming it's inside a repository class called _cartRepo)
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