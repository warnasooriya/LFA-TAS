using System;
using System.Linq;
using TAS.DataTransfer.Requests;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class SaveCustomerEnterdPolicyByTpaIdUnitOfWork : UnitOfWork
    {
        public object Result { get; set; }
        private readonly SaveCustomerEnterdPolicyRequestDto _saveCustomerEnterdPolicyRequestDto;

        public SaveCustomerEnterdPolicyByTpaIdUnitOfWork(SaveCustomerEnterdPolicyRequestDto saveCustomerEnterdPolicyRequestDto)
        {
            _saveCustomerEnterdPolicyRequestDto = saveCustomerEnterdPolicyRequestDto;
        }

        public override bool PreExecute()
        {
            try
            {
                TASEntitySessionManager.OpenSession();
                string dbName = TASTPAEntityManager.GetTPADetailById(_saveCustomerEnterdPolicyRequestDto.tpaId).FirstOrDefault().DBName;
                if (dbName != "")
                {
                    string tasTpaConnString = TASTPAEntityManager.GetTPADetailById(_saveCustomerEnterdPolicyRequestDto.tpaId).FirstOrDefault().DBConnectionStringViewOnly;
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

                this.Result = PolicyEntityManager.SaveCustomerEnterdPolicyByTpaId(_saveCustomerEnterdPolicyRequestDto);
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
