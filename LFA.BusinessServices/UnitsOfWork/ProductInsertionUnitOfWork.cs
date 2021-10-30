using TAS.DataTransfer.Requests;
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
    internal sealed class ProductInsertionUnitOfWork : UnitOfWork
    {
        public ProductRequestDto Product;
        private string UniqueDbName = string.Empty;

        public ProductInsertionUnitOfWork(ProductRequestDto Product)
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
                var ce = productEntityManager.GetProductById(Product.Id);
                if (ce == null || ce.IsProductExists == false)
                {
                    bool result = productEntityManager.AddProduct(Product);
                    this.Product.ProductInsertion = result;
                    if (Product.ProductInsertion)
                    {
                        /* remove cache */
                        ICache cache = CacheFactory.GetCache();
                        cache.Remove(UniqueDbName + "_ProductType");
                        cache.Remove(UniqueDbName + "_Products");
                        cache.Remove(UniqueDbName + "_ProductsByCommodityType_" + Product.CommodityTypeId.ToString().ToLower());
                        cache.Remove(UniqueDbName + "_ProductById_" + Product.Id.ToString().ToLower());
                    }
                }
                else
                {
                    this.Product.ProductInsertion = false;
                }

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
