using TAS.Services.Entities.Management;
using TAS.DataTransfer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using TAS.Services.Entities.Persistence;
using TAS.Services.Entities;

using TAS.Services.Common;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class ProductsRetrievalUnitOfWork : UnitOfWork
    {
        public bool isParentOnly = false;
        public bool isChildOnly = false;
        public Guid ParentID = Guid.NewGuid();

        private string UniqueDbName = string.Empty;
        public ProductsResponseDto Result
        {
            get;
            private set;
        }
        public override bool PreExecute()
        {
            try
            {
                Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                string dbName = UniqueDbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
                if (dbName != "")
                {
                    TASEntitySessionManager.OpenSession();
                    string tasTpaConnString = TASTPAEntityManager.GetTPAConnStringBydbName(dbName);
                    TASEntitySessionManager.CloseSession();
                    if (tasTpaConnString != "")
                    {
                        dbConnectionString = tasTpaConnString;
                        EntitySessionManager.OpenSession(dbConnectionString);
                        if (JWTHelper.checkTokenValidity(Convert.ToInt32(ConfigurationData.tasTokenValidTime.ToString())))
                        {
                            return true;
                        }
                        EntitySessionManager.CloseSession();
                    }
                }
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
            return false;

        }
        public override void Execute()
        {
            try
            {
                if (dbConnectionString != null)   //**
                {     //**
                    EntitySessionManager.OpenSession(dbConnectionString);
                }     //**
                else     //**
                {     //**
                    EntitySessionManager.OpenSession();     //**
                }     //**
                ProductEntityManager productEntityManager = new ProductEntityManager();
                List<ProductResponseDto> productEntities = EntityCacheData.GetAllProducts(UniqueDbName);
                if (isParentOnly)
                {
                    productEntities = productEntities.Where(a => a.Isbundledproduct == false).ToList();
                }
                if (isChildOnly && ParentID.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    List<ProductResponseDto> tempProducts = productEntities.ToList();
                    //List<Guid> ParentProducts = new List<Guid>();
                    productEntities = new List<ProductResponseDto>();
                    List<BundledProduct> bp = productEntityManager.GetBundleProducts();

                    //ParentProducts.AddRange(bp.Where(b => b.IsCurrentProduct).Select(s => s.ProductId).ToList());
                    //productEntities.AddRange(tempProducts.Where(a => ParentProducts.Contains(a.Id)).ToList());

                    productEntities.AddRange(tempProducts.Where(a => bp.Any(s => s.ProductId == a.Id && s.IsCurrentProduct)).ToList());


                    // ---Sachith slowness issue fixed
                    /*
                    foreach (var item in bp.FindAll(b => b.IsCurrentProduct))
                    {
                        ParentProducts.Add(item.ProductId);(
                    }

                    foreach (var item in tempProducts) ==
                    {
                        if (!ParentProducts.Contains(item.Id))
                        {
                            productEntities.Add(item);
                        }
                    }
                    */
                }
                if (isChildOnly && ParentID.ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    // ---Sachith slowness issue fixed

                    /*
                    List<Product> tempProducts = productEntities;
                    productEntities = new List<Product>();
                    List<BundledProduct> bp = productEntityManager.GetBundleProducts();
                    foreach (var item in bp.FindAll(b => b.IsCurrentProduct && b.ProductId == ParentID))
                    {
                        Product p = tempProducts.Find(c => c.Id == item.ParentProductId);
                        if (productEntities.Count > 0 || productEntities.FindAll(pp => pp.Id == item.Id).Count == 0)
                            productEntities.Add(p);
                    }
                    if(productEntities.Count==0)
                    {
                        productEntities.Add(tempProducts.Find(p => p.Id == ParentID));
                    }

                    */

                    List<ProductResponseDto> tempProducts = productEntities.Distinct().ToList();
                    productEntities = new List<ProductResponseDto>();
                    List<BundledProduct> bp = productEntityManager.GetBundleProducts().Where(b => b.IsCurrentProduct && b.ProductId == ParentID).Distinct().ToList();
                    productEntities.AddRange((tempProducts.Where(a => bp.Any(s => s.ParentProductId == a.Id)).Distinct().ToList()));
                    if (!productEntities.Any())
                    {
                        productEntities.Add(tempProducts.FirstOrDefault(p => p.Id == ParentID));
                    }
                }
                ProductsResponseDto result = new ProductsResponseDto();
                result.Products = new List<ProductResponseDto>();
                result.Products = productEntities;
                //if (productEntities != null)
                //{

                //    //.Select(s => new ProductResponseDto
                //    //{
                //    //    Id = s.Id,
                //    //    CommodityTypeId = s.CommodityTypeId,
                //    //    CommodityType = s.CommodityType,
                //    //    Productname = s.Productname,
                //    //    Productcode = s.Productcode,
                //    //    Productdescription = s.Productdescription,
                //    //    Productshortdescription = s.Productshortdescription,
                //    //    Displayimage = s.Displayimage,
                //    //    Isbundledproduct = s.Isbundledproduct,
                //    //    Isactive = s.Isactive,
                //    //    Ismandatoryproduct = s.Ismandatoryproduct,
                //    //    Entrydatetime = s.Entrydatetime,
                //    //    Entryuser = s.Entryuser,
                //    //    Lastupdatedatetime = s.Lastupdatedatetime,
                //    //    Lastupdateuser = s.Lastupdateuser,
                //    //    ProductTypeId = s.ProductTypeId,
                //    //    isChecked = false,
                //    //    Premium = "0",
                //    //    CurrencyCode = "",
                //    //    ProductTypeCode = s.Pp
                //    //}).ToList();
                //}

                // ---Sachith slowness issue fixed
                //foreach (var Product in productEntities)
                //{
                //    var productTypecode = productEntityManager.getProductTypeCode(Product.ProductTypeId);
                //    ProductResponseDto pr = new ProductResponseDto();

                //    pr.Id = Product.Id;
                //    pr.CommodityTypeId = Product.CommodityTypeId;
                //    pr.CommodityType = new CommonEntityManager().GetCommodityTypeUniqueCodeById(Product.CommodityTypeId);
                //    pr.Productname = Product.Productname;
                //    pr.Productcode = Product.Productcode;
                //    pr.Productdescription = Product.Productdescription;
                //    pr.Productshortdescription = Product.Productshortdescription;
                //    pr.Displayimage = Product.Displayimage;
                //    pr.Isbundledproduct = Product.Isbundledproduct;
                //    pr.Isactive = Product.Isactive;
                //    pr.Ismandatoryproduct = Product.Ismandatoryproduct;
                //    pr.Entrydatetime = Product.Entrydatetime;
                //    pr.Entryuser = Product.Entryuser;
                //    pr.Lastupdatedatetime = Product.Lastupdatedatetime;
                //    pr.Lastupdateuser = Product.Lastupdateuser;
                //    pr.ProductTypeId = Product.ProductTypeId;
                //    pr.isChecked = false;
                //    pr.Premium = "0";
                //    pr.CurrencyCode = "";
                //    pr.ProductTypeCode = productTypecode.ToString();


                //    //need to write other fields
                //    result.Products.Add(pr);
                //}
                this.Result = result;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }

    }
}
