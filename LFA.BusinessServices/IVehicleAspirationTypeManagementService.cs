using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface IVehicleAspirationTypeManagementService
    {
        VehicleAspirationTypesResponseDto GetVehicleAspirationTypes(
            SecurityContext securityContext, 
            AuditContext auditContext);

        VehicleAspirationTypeRequestDto AddVehicleAspirationType(VehicleAspirationTypeRequestDto VehicleAspirationType,
            SecurityContext securityContext,
            AuditContext auditContext);


        VehicleAspirationTypeResponseDto GetVehicleAspirationTypeById(Guid VehicleAspirationTypeId,
            SecurityContext securityContext,
            AuditContext auditContext);

        VehicleAspirationTypeRequestDto UpdateVehicleAspirationType(VehicleAspirationTypeRequestDto VehicleAspirationType,
            SecurityContext securityContext,
            AuditContext auditContext);



        bool IsExsistingAspirationTypesByCode(Guid Id, string aspirationTypeCode, SecurityContext securityContext, AuditContext auditContext);



        bool IsExsistingHorsePowerByHorsePower(Guid Id, string horsePower, SecurityContext securityContext, AuditContext auditContext);

        bool IsExsistingVehicleKiloWattByKiloWatt(Guid Id, string kiloWatt, SecurityContext securityContext, AuditContext auditContext);
    }
}
