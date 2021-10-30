using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class ClaimAjusmentsInsertionUnitOfWork : UnitOfWork
    {
        public object Response { get; set; }
        Guid claimId;
        decimal adjustPartAmount;
        decimal adjustLabourAmount;
        decimal adjustSundryAmount;

        public ClaimAjusmentsInsertionUnitOfWork(Guid _claimId, decimal _adjustPartAmount, decimal _adjustLabourAmount, decimal _adjustSundryAmount)
        {

            claimId = _claimId;
            adjustPartAmount = _adjustPartAmount;
            adjustLabourAmount = _adjustLabourAmount;
            adjustSundryAmount = _adjustSundryAmount;
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
                ClaimInvoiceEntityManager ClaimInvoiceEntityManager = new ClaimInvoiceEntityManager();



                this.Response = ClaimInvoiceEntityManager.AddClaimAjusmentInvoiceEntry(claimId,adjustPartAmount,adjustLabourAmount,adjustSundryAmount);
                  
                    
               

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
