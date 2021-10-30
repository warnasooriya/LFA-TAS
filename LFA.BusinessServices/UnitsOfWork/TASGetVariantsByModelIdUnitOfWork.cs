using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class TASGetVariantsByModelIdUnitOfWork :UnitOfWork
    {
        public VariantsResponseDto Result
        {
            get;
            private set;
        }
        public Guid tpaId, modelId;
      
        public TASGetVariantsByModelIdUnitOfWork(Guid modelId, Guid tpaId)
        {
            this.tpaId = tpaId;
            this.modelId = modelId;

        }

        public override bool PreExecute()
        {
            try
            {

                TASEntitySessionManager.OpenSession();
                string dbName = TASTPAEntityManager.GetTPADetailById(tpaId).FirstOrDefault().DBName;
                if (dbName != "")
                {

                    //string tasTpaConnString = TASTPAEntityManager.GetTPAViewOnlyConnStringBydbName(dbName);
                    string tasTpaConnString = TASTPAEntityManager.GetTPADetailById(tpaId).FirstOrDefault().DBConnectionStringViewOnly;

                    if (tasTpaConnString != "")
                    {
                        dbConnectionString = tasTpaConnString;
                        return true;
                    }
                }
                return false;

            }
            catch (Exception e)
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
                
                
                this.Result = VariantEntityManager.GetAllVarientsByModelIds(new List<Guid>() { modelId });
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
