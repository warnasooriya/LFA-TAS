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
    internal sealed class TASCommodityTypeByProdIdRetrievalUnitOfWork : UnitOfWork
    {


        public ProductResponseDto Result
        {
            get;
            private set;
        }
        public Guid productId
        {
            get;
            set;
        }
        public Guid tpaId
        {
            get;
            set;
        }
        public TASCommodityTypeByProdIdRetrievalUnitOfWork(Guid productId, Guid tpaId)
        {
            this.productId = productId;
            this.tpaId = tpaId;
        }

        public override bool PreExecute()
        {
            try
            {

                TASEntitySessionManager.OpenSession();
                string dbName = TASTPAEntityManager.GetTPADetailById(tpaId).FirstOrDefault().DBName;
                if (dbName != "")
                {

                    //string tasTpaConnString = TASTPAEntityManager.GetTPAViewOnlyConnStringBydbName(dbName);
                    string tasTpaConnString = TASTPAEntityManager.GetTPADetailById(tpaId).FirstOrDefault().DBConnectionStringViewOnly;

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
                ProductResponseDto productEntity = productEntityManager.GetProductById(productId);
                if (productEntity.IsProductExists == null || productEntity.IsProductExists == false)
                {
                    productEntity.IsProductExists = false;
                }
                else
                {
                    productEntity.IsProductExists = true;
                }

                this.Result = productEntity;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }

    }
}
