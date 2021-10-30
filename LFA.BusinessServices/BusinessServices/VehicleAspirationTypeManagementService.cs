using TAS.Services.UnitsOfWork;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.BusinessServices
{
    internal sealed class VehicleAspirationTypeManagementService : IVehicleAspirationTypeManagementService
    {

        public VehicleAspirationTypesResponseDto GetVehicleAspirationTypes(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            VehicleAspirationTypesResponseDto result = null;

            VehicleAspirationTypesRetrievalUnitOfWork uow = new VehicleAspirationTypesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.ResultVehicleAspirationType;

            return result;
        }

        public VehicleAspirationTypeRequestDto AddVehicleAspirationType(VehicleAspirationTypeRequestDto VehicleAspirationType, SecurityContext securityContext,
            AuditContext auditContext) {
                VehicleAspirationTypeRequestDto result = new VehicleAspirationTypeRequestDto();
                VehicleAspirationTypeInsertionUnitOfWork uow = new VehicleAspirationTypeInsertionUnitOfWork(VehicleAspirationType);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.VehicleAspirationTypeInsertion = uow.VehicleAspirationType.VehicleAspirationTypeInsertion;
                return result;
        }


        public VehicleAspirationTypeResponseDto GetVehicleAspirationTypeById(Guid VehicleAspirationTypeId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            VehicleAspirationTypeResponseDto result = new VehicleAspirationTypeResponseDto();

            VehicleAspirationTypeRetrievalUnitOfWork uow = new VehicleAspirationTypeRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.VehicleAspirationTypeId = VehicleAspirationTypeId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public VehicleAspirationTypeRequestDto UpdateVehicleAspirationType(VehicleAspirationTypeRequestDto VehicleAspirationType, SecurityContext securityContext,
           AuditContext auditContext)
        {
            VehicleAspirationTypeRequestDto result = new VehicleAspirationTypeRequestDto();
            VehicleAspirationTypeUpdationUnitOfWork uow = new VehicleAspirationTypeUpdationUnitOfWork(VehicleAspirationType);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.VehicleAspirationTypeInsertion = uow.VehicleAspirationType.VehicleAspirationTypeInsertion;
            return result;
        }

        public bool IsExsistingAspirationTypesByCode(Guid Id, string aspirationTypeCode,
             SecurityContext securityContext,
             AuditContext auditContext)
        {
            bool Result = false;
            IsExixtingAspirationTypesByCodeUnitOfWorks uow = new IsExixtingAspirationTypesByCodeUnitOfWorks();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.AspirationTypeCode = aspirationTypeCode;
            uow.EntryUser = "";
            uow.Id = Id;
            if (uow.PreExecute()) { uow.Execute(); }
            Result = uow.Result;
            return Result;
        }

        public bool IsExsistingHorsePowerByHorsePower(Guid Id, string horsePower,
             SecurityContext securityContext,
             AuditContext auditContext)
        {
            bool Result = false;
            IsExixtingHorsePowerByHorsePowerUnitOfWorks uow = new IsExixtingHorsePowerByHorsePowerUnitOfWorks();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.HorsePower = horsePower;
            uow.EntryUser = "";
            uow.Id = Id;
            if (uow.PreExecute()) { uow.Execute(); }
            Result = uow.Result;
            return Result;
        }

        public bool IsExsistingVehicleKiloWattByKiloWatt(Guid Id, string kiloWatt,
             SecurityContext securityContext,
             AuditContext auditContext)
        {
            bool Result = false;
            IsExixtingVehicleKiloWattByKiloWattUnitOfWorks uow = new IsExixtingVehicleKiloWattByKiloWattUnitOfWorks();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.KiloWatt = kiloWatt;
            uow.EntryUser = "";
            uow.Id = Id;
            if (uow.PreExecute()) { uow.Execute(); }
            Result = uow.Result;
            return Result;
        }
       
    }
}
