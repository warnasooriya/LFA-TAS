using TAS.Services.Entities.Management;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Entities.Persistence;
using TAS.Services.Entities;


namespace TAS.Services.UnitsOfWork
{
    internal sealed class TASContractExtensionsRetrievalUnitOfWork : UnitOfWork
    {
       

        public ExtensionTypesResponseDto Result
        {
            get;
            private set;
        }
        public Guid tpaId
        {
            get;
            set;
        }
        public Guid dealerId
        {
            get;
            set;
        }
        public Guid modelId
        {
            get;
            set;
        }
        public TASContractExtensionsRetrievalUnitOfWork(Guid modelId,Guid dealerId,Guid tpaId)
        {
            this.tpaId = tpaId;
            this.dealerId = dealerId;
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
                ContractEntityManager ContractEntityManager = new ContractEntityManager();
                List<ExtensionType> ExtensionTypeEntities = ContractEntityManager.GetContractExtensionsByModelId(this.modelId, this.dealerId);


                ExtensionTypesResponseDto result = new ExtensionTypesResponseDto();
                result.ExtensionTypes = new List<ExtensionTypeResponseDto>();
                foreach (var ExtensionType in ExtensionTypeEntities)
                {
                    ExtensionTypeResponseDto a = new ExtensionTypeResponseDto();

                    a.Id = ExtensionType.Id;
                    a.CommodityTypeId = ExtensionType.CommodityTypeId;
                    a.EntryDateTime = ExtensionType.EntryDateTime;
                    a.EntryUser = ExtensionType.EntryUser;
                    a.ExtensionName = ExtensionType.ExtensionName;
                    a.Hours = ExtensionType.Hours;
                    a.Km = ExtensionType.Km;
                    a.Month = ExtensionType.Month;
					
                    //need to write other fields
                    result.ExtensionTypes.Add(a);
                }
                this.Result = result;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }

    }
}
