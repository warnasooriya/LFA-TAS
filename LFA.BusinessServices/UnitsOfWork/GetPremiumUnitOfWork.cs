using System;
using System.Linq;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class GetPremiumUnitOfWork : UnitOfWork
    {
        public object Result { get; set; }
        private readonly Guid _ContractPremiumId;
        private readonly decimal _Usage;
        private readonly Guid _AttributeSpecificationId;
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
        private readonly Guid _ItemStatusId;
        private readonly decimal _DealerPrice;
        private readonly DateTime _ItemPurchasedDate;
        private readonly decimal _GrossWeight;


        public GetPremiumUnitOfWork(Guid ContractPremiumId, decimal Usage, Guid AttributeSpecificationId, Guid ExtensionId, Guid ContractId, 
            Guid ProductId, Guid DealerId, DateTime Date,
            Guid CylinderCountId, Guid EngineCapacityId, Guid MakeId, Guid ModelId, Guid VariantId, decimal GrossWeight,
            Guid ItemStatusId, decimal DealerPrice, DateTime ItemPurchasedDate)
        {
            _ContractPremiumId = ContractPremiumId;
            _Usage = Usage;
            _AttributeSpecificationId = AttributeSpecificationId;
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
            _ItemStatusId = ItemStatusId;
            _DealerPrice = DealerPrice;
            _ItemPurchasedDate = ItemPurchasedDate;
        }

        public override bool PreExecute()
        {
            try
            {
                var JWTHelper = new JWTHelper(SecurityContext);
                var str = JWTHelper.DecodeAuthenticationToken();
                var dbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
                if (dbName != "")
                {
                    TASEntitySessionManager.OpenSession();
                    var tasTpaConnString = TASTPAEntityManager.GetTPAConnStringBydbName(dbName);
                    TASEntitySessionManager.CloseSession();
                    if (tasTpaConnString != "")
                    {
                        dbConnectionString = tasTpaConnString;
                        EntitySessionManager.OpenSession(dbConnectionString);
                        if (JWTHelper.checkTokenValidity(Convert.ToInt32(ConfigurationData.tasTokenValidTime)))
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
                if (dbConnectionString != null) //**
                {
                    //**
                    EntitySessionManager.OpenSession(dbConnectionString);
                } //**
                else //**
                {
                    //**
                    EntitySessionManager.OpenSession(); //**
                } //**
                var ContractEntityManager = new ContractEntityManager();
                Result = ContractEntityManager.GetPremium(
                    _ContractPremiumId,
                    _Usage,
                    _AttributeSpecificationId,
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
                    _GrossWeight,
                    _ItemStatusId,
                    _DealerPrice,
                    _ItemPurchasedDate
                    );
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}