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
    internal sealed class GetAttributeSpecificationByExtensionIdUnitOfWork : UnitOfWork
    {
        public object Result { get; set; }
        private readonly Guid _ExtensionId;
        private readonly Guid _ContractId;
        private readonly Guid _ProductId;
        private readonly Guid _DealerId;
        private readonly DateTime _Date;
        private readonly Guid _CylinderCountId;
        private readonly Guid _EngineCapacityId;
        private readonly Guid _MakeId;
        private readonly Guid _ModelId;
        private readonly Guid _VariantId;
        private readonly decimal _GrossWeight;

        public GetAttributeSpecificationByExtensionIdUnitOfWork(Guid ExtensionId, Guid ContractId, Guid ProductId, Guid DealerId, DateTime Date,
            Guid CylinderCountId, Guid EngineCapacityId, Guid MakeId, Guid ModelId, Guid VariantId, decimal GrossWeight)
        {
            _ExtensionId = ExtensionId;
            _ContractId = ContractId;
            _ProductId = ProductId;
            _DealerId = DealerId;
            _Date = Date;
            _CylinderCountId = CylinderCountId;
            _EngineCapacityId = EngineCapacityId;
            _MakeId = MakeId;
            _ModelId = ModelId;
            _VariantId = VariantId;
            _GrossWeight = GrossWeight;
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

                this.Result = ContractEntityManager.GetAttributeSpecificationByExtensionId(
                    _ExtensionId,
                     _ContractId,
                        _ProductId,
                        _DealerId,
                        _Date,
                        _CylinderCountId,
                        _EngineCapacityId,
                        _MakeId,
                        _ModelId,
                        _VariantId,
                        _GrossWeight
                    );
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
