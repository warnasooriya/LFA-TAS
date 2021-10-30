using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Common;
using TAS.Services.Entities;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class TempBulkSaveUnitOfWork : UnitOfWork
    {
        public bool Result = false;
        public List<TempBulkUpload> tempbulkdata;
        public string CommodityType;
        public string ProductCode;


        public TempBulkSaveUnitOfWork(List<TempBulkUpload> _tempBulkData, string _commodityType, string _productCode) {
            this.tempbulkdata = _tempBulkData;
            this.CommodityType = _commodityType;
            this.ProductCode = _productCode;
        }

        public override bool PreExecute()
        {
            try
            {
                Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                string dbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
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
                TempBulkUploadEntityManager tempBulkUploadEntityManager = new TempBulkUploadEntityManager();
                Result = tempBulkUploadEntityManager.SaveTempBulkData(tempbulkdata,  CommodityType,  ProductCode);
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
