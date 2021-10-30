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

namespace TAS.Services.UnitsOfWork
{
    internal sealed class RSAAnualPremiumUpdationUnitOfWork : UnitOfWork
    {
        public RSAAnualPremiumRequestDto RSAAnualPremium;
        public RSAAnualPremiumResponseDto ExPr;
       
        public RSAAnualPremiumUpdationUnitOfWork(RSAAnualPremiumRequestDto RSAAnualPremium)
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
                            RSAAnualPremiumEntityManager RSAAnualPremiumEntityManager = new RSAAnualPremiumEntityManager();
                            var ce = RSAAnualPremiumEntityManager.GetRSAAnualPremiumById(RSAAnualPremium.Id);
                            if (ce.IsRSAAnualPremiumExists == true)
                            {
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
               
        //        RSAAnualPremiumEntityManager RSAAnualPremiumEntityManager = new RSAAnualPremiumEntityManager();
        //        var ce = RSAAnualPremiumEntityManager.GetRSAAnualPremiumById(RSAAnualPremium.Id);
        //        if (ce.IsRSAAnualPremiumExists == true)
        //        {
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
                RSAAnualPremiumEntityManager RSAAnualPremiumEntityManager = new RSAAnualPremiumEntityManager();
                //var ce = RSAAnualPremiumEntityManager.GetRSAAnualPremiumById(RSAAnualPremium.Id);
                //if (ce.IsRSAAnualPremiumExists == true)
                //{
                    bool result = RSAAnualPremiumEntityManager.UpdateRSAAnualPremium(RSAAnualPremium);
                    this.RSAAnualPremium.RSAAnualPremiumInsertion = result;
                //}
                //else
                //{
                //    this.RSAAnualPremium.RSAAnualPremiumInsertion = false;
                //}

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
