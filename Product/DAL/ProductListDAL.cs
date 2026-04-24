using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DataAccessLayer.Dapper;
using Product.Models;
using Product.DTOs;


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

        public HomePageModel GetHomePageDetails(long CustomerID)
        {
            DynamicParameters param = new DynamicParameters();
            var proc = "spHomePage_Get";
            param.Add("@CustomerID", CustomerID);
            return _db.ExecuteMultipleResultSet(proc, multi =>
            new HomePageModel
            {
                FeaturedProducts = multi.Read<FeaturedProductModel>().ToList(),
                FlashSaleProducts = multi.Read<FlashSaleProductModel>().ToList(),
                Brands = multi.Read<BrandModel>().ToList()
            }, param);
       
        }

        public SideFilterModel GetSideFilterDetails(long CustomerID)
        {
            var proc = "sp_GetFilterSidebarDetails";
            
            DynamicParameters param = new DynamicParameters();
            param.Add("@CustomerID", CustomerID);

            return _db.ExecuteMultipleResultSet(proc, multi =>
            new SideFilterModel
            {
                BrandList = multi.Read<SideFilterBrand>().ToList(),
                SizeList = multi.Read<SideFilterSize>().ToList()
            },param);

        }

        
    }
}