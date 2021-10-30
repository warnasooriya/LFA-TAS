using System;
using System.Linq;
using TAS.DataTransfer.Requests;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class SaveCustomerEnterdInvoiceDetailsUnitOfWork : UnitOfWork
    {
        public object Result { get; set; }
        private readonly SaveCustomerPolicyInfoRequestDto _saveCustomerPolicyInfoRequestDto;

        public SaveCustomerEnterdInvoiceDetailsUnitOfWork(SaveCustomerPolicyInfoRequestDto saveCustomerPolicyInfoRequestDto)
        {
            _saveCustomerPolicyInfoRequestDto = saveCustomerPolicyInfoRequestDto;
        }

        public override bool PreExecute()
        {
            try
            {
                TASEntitySessionManager.OpenSession();
                string dbName = TASTPAEntityManager.GetTPADetailById(_saveCustomerPolicyInfoRequestDto.tpaId).FirstOrDefault().DBName;
                if (dbName != "")
                {
                    string tasTpaConnString = TASTPAEntityManager.GetTPADetailById(_saveCustomerPolicyInfoRequestDto.tpaId).FirstOrDefault().DBConnectionStringViewOnly;
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

                this.Result = PolicyEntityManager.SaveCustomerEnteredInvoiceDetails(_saveCustomerPolicyInfoRequestDto);
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }

    }
}
