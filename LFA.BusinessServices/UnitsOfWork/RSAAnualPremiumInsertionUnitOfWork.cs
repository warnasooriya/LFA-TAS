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

namespace TAS.Services.UnitsOfWork
{
    internal sealed class RSAAnualPremiumInsertionUnitOfWork : UnitOfWork
    {
        public RSAAnualPremiumRequestDto RSAAnualPremium;
       
        public RSAAnualPremiumInsertionUnitOfWork(RSAAnualPremiumRequestDto RSAAnualPremium)
        {
            
            this.RSAAnualPremium = RSAAnualPremium;
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
                RSAAnualPremiumEntityManager RSAAnualPremiumEntityManager = new RSAAnualPremiumEntityManager();
                var ce = RSAAnualPremiumEntityManager.GetRSAAnualPremiumById(RSAAnualPremium.Id);
                if (ce == null)
                {
                    bool result = RSAAnualPremiumEntityManager.AddRSAAnualPremium(RSAAnualPremium);
                    this.RSAAnualPremium.RSAAnualPremiumInsertion = result;
                }
                else
                {
                    this.RSAAnualPremium.RSAAnualPremiumInsertion = false;
                }

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
