using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using TAS.Services.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Common;
using TAS.DataTransfer.Requests;

namespace TAS.Services.UnitsOfWork

{
    internal sealed class ConfirmClaimInvoiceEntryUnitOfWork : UnitOfWork
    {
        public readonly ClaimInvoiceEntryRequestDto ClaimInvoice;
        public string Result { get; set; }

        public ConfirmClaimInvoiceEntryUnitOfWork(ClaimInvoiceEntryRequestDto _claimInvoice)
        {
            ClaimInvoice = _claimInvoice;
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
                ClaimInvoiceEntityManager claimInvoiceEntityManager = new ClaimInvoiceEntityManager();

                bool Result = claimInvoiceEntityManager.ConfirmClaimInvoiceUpdate(ClaimInvoice);

                this.ClaimInvoice.ClaimInvoiceEntryInsertion = Result;

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
