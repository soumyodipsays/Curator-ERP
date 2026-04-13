using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DataAccessLayer.Dapper;
using Product.Models;


namespace Product.DAL
{
    public class ProductListDAL
    {
        private readonly DapperConn _db = new DapperConn();

        public List<ProductListModel> GetProductList(int CustomerID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@CustomerID", CustomerID);
            return _db.ExecuteMultipleRow<ProductListModel>("spProduct_GetList",param);
        }

    }
}