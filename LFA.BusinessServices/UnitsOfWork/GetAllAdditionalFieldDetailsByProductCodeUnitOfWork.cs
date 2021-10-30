using System;
using System.Linq;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class GetAllAdditionalFieldDetailsByProductCodeUnitOfWork : UnitOfWork
    {
        public object Result { get; set; }
        private readonly Guid _tpaId;
        private readonly string _productCode;

        public GetAllAdditionalFieldDetailsByProductCodeUnitOfWork(Guid tpaId , string productCode)
        {
            _tpaId = tpaId;
            _productCode = productCode;
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

                this.Result = PolicyEntityManager.GetAllAdditionalFieldDetailsByProductCode(_productCode);
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
