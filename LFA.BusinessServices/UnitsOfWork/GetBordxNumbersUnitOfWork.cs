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
    internal sealed class GetBordxNumbersUnitOfWork : UnitOfWork
    {

        private readonly int year;
        private readonly int month;
        private readonly Guid commodityTypeId;
        //private readonly Guid countryId;
        private readonly Guid reinsurerId;
        private readonly Guid insurerId;
        private readonly Guid productId;
        public object Result { get; set; }
        public GetBordxNumbersUnitOfWork(int _year, int _month,Guid _commodityTypeId,Guid _reinsurerId,Guid _insurerId , Guid _productId)
        {
            year = _year;
            month = _month;
            commodityTypeId = _commodityTypeId;
            reinsurerId = _reinsurerId;
            insurerId = _insurerId;
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
                this.Result = BordxEntityManager.GetBordxNumbers(commodityTypeId,reinsurerId,insurerId, productId, year, month);
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
