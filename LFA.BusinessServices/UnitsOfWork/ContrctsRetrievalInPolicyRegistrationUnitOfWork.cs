using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class ContrctsRetrievalInPolicyRegistrationUnitOfWork : UnitOfWork
    {
        private Guid ProductId;
        private Guid DealerId;
        private DateTime Date;
        private Guid CylinderCountId;
        private Guid EngineCapacityId;
        private Guid ItemStatusId;
        private Guid MakeId;
        private Guid ModelId;
        private Guid VariantId;
        private decimal GrossWeight;
        private Guid UsageTypeId;

        public object Result;

        public ContrctsRetrievalInPolicyRegistrationUnitOfWork(Guid ProductId, Guid DealerId, DateTime Date,
            Guid CylinderCountId, Guid EngineCapacityId, Guid ItemStatusId, Guid MakeId, Guid ModelId, Guid VariantId,decimal GrossWeight,Guid UsageTypeId)
        {
            this.ProductId = ProductId;
            this.DealerId = DealerId;
            this.Date = Date;
            this.CylinderCountId = CylinderCountId;
            this.EngineCapacityId = EngineCapacityId;
            this.MakeId = MakeId;
            this.ModelId = ModelId;
            this.VariantId = VariantId;
            this.GrossWeight = GrossWeight;
            this.UsageTypeId = UsageTypeId;
            this.ItemStatusId=ItemStatusId;
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
            ContractEntityManager ContractEntityManager = new ContractEntityManager();
            Result = ContractEntityManager.ContrctsRetrievalInPolicyRegistration(
                this.ProductId,
                this.DealerId,
                this.Date,
                this.CylinderCountId,
                this.EngineCapacityId,
                this.ItemStatusId,
                this.MakeId,
                this.ModelId,
                this.VariantId,
                this.UsageTypeId,
                this.GrossWeight
                );
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
