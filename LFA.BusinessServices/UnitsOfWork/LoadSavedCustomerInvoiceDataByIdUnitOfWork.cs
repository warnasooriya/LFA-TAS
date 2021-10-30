using System;
using System.Linq;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class LoadSavedCustomerInvoiceDataByIdUnitOfWork : UnitOfWork
    {
        public object Result { get; set; }
        private readonly Guid _tpaId;
        private readonly Guid _tempInvocieId;

        public LoadSavedCustomerInvoiceDataByIdUnitOfWork(Guid tpaId, Guid tempInvocieId)
        {
            _tpaId = tpaId;
            _tempInvocieId = tempInvocieId;
        }

        public override bool PreExecute()
        {
            try
            {
                TASEntitySessionManager.OpenSession();
                string dbName = TASTPAEntityManager.GetTPADetailById(_tpaId).FirstOrDefault().DBName;
                if (dbName != "")
                {
                    string tasTpaConnString = TASTPAEntityManager.GetTPADetailById(_tpaId).FirstOrDefault().DBConnectionStringViewOnly;
                    if (tasTpaConnString != "")
                    {
                        dbConnectionString = tasTpaConnString;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
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

                this.Result = PolicyEntityManager.LoadSavedCustomerInvoiceDataById(_tempInvocieId);
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
