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
    internal sealed class GetAllExtensionTypeByMakeModelUnitOfWork : UnitOfWork
    {
        public object Result { get; set; }
        private readonly Guid _DealerId;
        private readonly Guid _CylinderCountId;
        private readonly Guid _EngineCapacityId;
        private readonly Guid _MakeId;
        private readonly Guid _ModelId;
        

        public GetAllExtensionTypeByMakeModelUnitOfWork(Guid DealerId,Guid CylinderCountId, Guid EngineCapacityId, Guid MakeId, Guid ModelId)
        {
            _DealerId = DealerId;
            _CylinderCountId = CylinderCountId;
            _EngineCapacityId = EngineCapacityId;
            _MakeId = MakeId;
            _ModelId = ModelId;
            
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

                this.Result = ContractEntityManager.GetAllExtensionTypeByMakeModel(
                        _DealerId ,
                        _CylinderCountId ,
                        _EngineCapacityId,
                        _MakeId ,
                        _ModelId  
                    );
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
