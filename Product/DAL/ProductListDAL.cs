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

        public List<ProductListModel> GetProductList(long CustomerID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@CustomerID", CustomerID);
            return _db.ExecuteMultipleRow<ProductListModel>("spProduct_GetList",param);
        }

        public List<CategoryListModel> GetCategoryList(long CustomerID)
        {
            DynamicParameters param = new DynamicParameters();
            var proc = "spProductCategory_DDL";
            param.Add("@CustomerID", CustomerID);
            return _db.ExecuteMultipleRow<CategoryListModel>(proc, param);
        }
    }
}