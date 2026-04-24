using Dapper;
using DataAccessLayer.Dapper;
using Product.DTOs;
using Product.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Product.DAL
{
    public class CartDAL
    {
        private readonly DapperConn _db = new DapperConn();
        public void AddOrEditCart(CartInsertIpdateDTO model)
        {
            var proc = "spCart_InsertUpdate";

            var param = new DynamicParameters();
            param.Add("@CartID", model.CartID);
            param.Add("@UserID", model.UserID);
            param.Add("@ProductID", model.ProductID);
            param.Add("@Quantity", model.Quantity);
            param.Add("@Status", model.Status);
            param.Add("@Username", model.UserName);

            _db.ExecuteWithoutReturn(proc, param);
        }

        public List<CartModel> GetCartDetails(long UserID)
        {
            var sp = "spCartItems_GetList";

            var param = new DynamicParameters();
            param.Add("@UserID", UserID);

            return _db.ExecuteMultipleRow<CartModel>(sp, param);
        }
    }
}