using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using TAS.Services.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Common;
using TAS.Caching;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class ProductUpdationUnitOfWork : UnitOfWork
    {
        public ProductRequestDto Product;
        public ProductResponseDto ExPr;
        private string UniqueDbName = string.Empty;
        public ProductUpdationUnitOfWork(ProductRequestDto Product)
        {

            this.Product = Product;
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
                            ProductEntityManager productEntityManager = new ProductEntityManager();
                            var ce = productEntityManager.GetProductById(Product.Id);
                            if (ce.IsProductExists == true)
                            {
                                Product.Entrydatetime = ce.Entrydatetime;
                                Product.Entryuser = ce.Entryuser;
                                return true;
                            }
                            else
                            {
                                return false;
                            }
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

        //public override bool PreExecute()
        //{
        //    try
        //    {
        //        EntitySessionManager.OpenSession();
        //        ProductEntityManager productEntityManager = new ProductEntityManager();
        //        var ce = productEntityManager.GetProductById(Product.Id);
        //        if (ce.IsProductExists == true)
        //        {
        //            //ExPr.Id = ce.Id;
        //            //ExPr.CommodityTypeId = ce.CommodityTypeId;
        //            //ExPr.Productname = ce.Productname;
        //            //ExPr.Productcode = ce.Productcode;
        //            //ExPr.Productdescription = ce.Productdescription;
        //            //ExPr.Productshortdescription = ce.Productshortdescription;
        //            //ExPr.Displayimage = ce.Displayimage;
        //            //ExPr.Isbundledproduct = ce.Isbundledproduct;
        //            //ExPr.Isactive = ce.Isactive;
        //            //ExPr.Ismandatoryproduct = ce.Ismandatoryproduct;
        //            //ExPr.Entrydatetime = ce.Entrydatetime;
        //            //ExPr.Entryuser = ce.Entryuser;
        //            //ExPr.Lastupdatedatetime = ce.Lastupdatedatetime;
        //            //ExPr.Lastupdateuser = ce.Lastupdateuser;
        //            Product.Entrydatetime = ce.Entrydatetime;
        //            Product.Entryuser = ce.Entryuser;
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    finally
        //    {
        //        EntitySessionManager.CloseSession();

        //    }

        //}

        public override void Execute()
        {
            try
            {
                //temp section can remove  //**  lines once preexecute jwt and db name working fine
                if (dbConnectionString != null)   //**
                {     //**
                    EntitySessionManager.OpenSession(dbConnectionString);
                }     //**
                else     //**
                {     //**
                    EntitySessionManager.OpenSession();     //**
                }     //**
                ProductEntityManager productEntityManager = new ProductEntityManager();
                //var ce = productEntityManager.GetProductById(Product.Id);
                //if (ce.IsProductExists == true)
               // {
                    bool result = productEntityManager.UpdateProduct(Product);
                    this.Product.ProductInsertion = result;
                //}
                //else
                //{
                //     this.Product.ProductInsertion = false;
                //}
                ICache cache = CacheFactory.GetCache();
                cache.Remove(UniqueDbName + "_ProductType");
                cache.Remove(UniqueDbName + "_Products");
                cache.Remove(UniqueDbName + "_ProductsByCommodityType_"+ Product.CommodityTypeId.ToString().ToLower());
                cache.Remove(UniqueDbName + "_ProductById_" + Product.Id.ToString().ToLower());

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
