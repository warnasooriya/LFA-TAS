using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class GetNextBordxNumberUnitOfWork : UnitOfWork
    {
        private readonly int year;
        private readonly int month;
        //private readonly Guid CountryId;
        private readonly Guid CommodityTypeId;
        private readonly Guid reinsurerId;
        private readonly Guid insurerId;
        private readonly Guid productId;
        public string Result { get; set; }
        public GetNextBordxNumberUnitOfWork(int _year, int _month, Guid _reinsurerId,Guid _insurerId,Guid _productId, Guid _CommodityTypeId )
        {
            year = _year;
            month = _month;
            //CountryId = _CountryId;
            reinsurerId = _reinsurerId;
            insurerId = _insurerId;
            CommodityTypeId = _CommodityTypeId;
            productId = _productId;
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
                BordxEntityManager BordxEntityManager = new BordxEntityManager();
                this.Result = BordxEntityManager.GetNextBordxNumber(year, month,reinsurerId,insurerId,productId,CommodityTypeId);
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }


    }
}
