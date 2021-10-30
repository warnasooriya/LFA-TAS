using TAS.Services.Entities.Management;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Entities.Persistence;
using TAS.Services.Entities;


namespace TAS.Services.UnitsOfWork
{
    internal sealed class TPAProductsRetrievalUnitOfWork : UnitOfWork
    {
        public Guid TPAId;

        public TPAProductsResponseDto Result
        {
            get;
            private set;
        }


        internal TPAProductsRetrievalUnitOfWork(Guid tpaId)
        {
            this.TPAId = tpaId;
        }

        public override bool PreExecute()
        {
            try
            {

                TASEntitySessionManager.OpenSession();
                string dbName = TASTPAEntityManager.GetTPADetailById(TPAId).FirstOrDefault().DBName;
                if (dbName != "")
                {

                    string tasTpaConnString = TASTPAEntityManager.GetTPAViewOnlyConnStringBydbName(dbName);

                    if (tasTpaConnString != "")
                    {
                        dbConnectionString = tasTpaConnString;
                        return true;
                    }
                }
                return false;

            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                TASEntitySessionManager.CloseSession();
            }
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
                List<TPAProductResponseDto> tpaProductEntities = productEntityManager.GetProductsByTPAId(TPAId);

                TPAProductsResponseDto result = new TPAProductsResponseDto();
                result.TPAProducts = new List<TPAProductResponseDto>();



                foreach (var Product in tpaProductEntities)
                {
                    TPAProductResponseDto pr = new TPAProductResponseDto();

                    pr.Id = Product.Id;
                    pr.CommodityTypeId = Product.CommodityTypeId;
                    pr.CommodityType = Product.CommodityType;
                    pr.CommodityTypeDisplayDescription = Product.CommodityTypeDisplayDescription;
                    pr.Productname = Product.Productname;
                    pr.Productcode = Product.Productcode;
                    pr.Productdescription = Product.Productdescription;
                    pr.Productshortdescription = Product.Productshortdescription;
                    pr.Displayimage = Product.Displayimage;
                    pr.DisplayImageSrc = Product.DisplayImageSrc;
                    pr.Isbundledproduct = Product.Isbundledproduct;
                    pr.Isactive = Product.Isactive;
                    pr.Ismandatoryproduct = Product.Ismandatoryproduct;
                    pr.Entrydatetime = Product.Entrydatetime;
                    pr.Entryuser = Product.Entryuser;
                    pr.Lastupdatedatetime = Product.Lastupdatedatetime;
                    pr.Lastupdateuser = Product.Lastupdateuser;

                    //need to write other fields
                    result.TPAProducts.Add(pr);
                }
                this.Result = result;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }

    }
}
