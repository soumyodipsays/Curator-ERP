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
    }
}