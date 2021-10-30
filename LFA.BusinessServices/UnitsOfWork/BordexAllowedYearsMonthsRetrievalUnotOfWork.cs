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
    internal sealed class BordexAllowedYearsMonthsRetrievalUnotOfWork:UnitOfWork
    {
       // private Guid CountryId;
        private Guid InsurerId;
        private Guid ReinsurerId;
        private Guid CommodityTypeId;

        public BordexAllowedYearsMonthsRetrievalUnotOfWork(Guid _InsurerId, Guid _ReinsurerId, Guid _CommodityTypeId)
        {
            InsurerId = _InsurerId;
            ReinsurerId = _ReinsurerId;
            CommodityTypeId = _CommodityTypeId;
        }
        public object Result { get; set; }
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
                this.Result = BordxEntityManager.GetBordexAllowedYearsMonths(InsurerId,ReinsurerId, CommodityTypeId);
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
