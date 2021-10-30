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
    internal sealed class ClaimBordexCanConfirmedByYearUnitOfWork : UnitOfWork
    {
        public int _year;
        public Guid _insurerid;
        public Guid _reinsurerid;
        public bool result;

        public ClaimBordexCanConfirmedByYearUnitOfWork(int year, Guid insurerid, Guid reinsurerid)
        {
            this._year = year;
            this._insurerid = insurerid;
            this._reinsurerid = reinsurerid;
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
                ClaimBordxProcessEntityManager ClaimBordxProcessEntityManager = new ClaimBordxProcessEntityManager();
                this.result = ClaimBordxProcessEntityManager.BordexCanConfirmedByYear(this._year , this._insurerid , this._reinsurerid);
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}