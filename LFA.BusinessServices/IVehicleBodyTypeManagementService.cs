using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface IVehicleBodyTypeManagementService
    {
        VehicleBodyTypesResponseDto GetVehicleBodyTypes(
            SecurityContext securityContext, 
            AuditContext auditContext);


        VehicleBodyTypesResponseDto GetParentVehicleBodyTypes(
            SecurityContext securityContext, 
            AuditContext auditContext);

        VehicleBodyTypeRequestDto AddVehicleBodyType(VehicleBodyTypeRequestDto VehicleBodyType,
            SecurityContext securityContext,
            AuditContext auditContext);


        VehicleBodyTypeResponseDto GetVehicleBodyTypeById(Guid VehicleBodyTypeId,
            SecurityContext securityContext,
            AuditContext auditContext);

        VehicleBodyTypeRequestDto UpdateVehicleBodyType(VehicleBodyTypeRequestDto VehicleBodyType,
            SecurityContext securityContext,
            AuditContext auditContext);


    }
}
