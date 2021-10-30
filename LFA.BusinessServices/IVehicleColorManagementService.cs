using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface IVehicleColorManagementService
    {
        VehicleColorsResponseDto GetVehicleColors(
            SecurityContext securityContext, 
            AuditContext auditContext);


        VehicleColorsResponseDto GetParentVehicleColors(
            SecurityContext securityContext, 
            AuditContext auditContext);

        VehicleColorRequestDto AddVehicleColor(VehicleColorRequestDto VehicleColor,
            SecurityContext securityContext,
            AuditContext auditContext);


        VehicleColorResponseDto GetVehicleColorById(Guid VehicleColorId,
            SecurityContext securityContext,
            AuditContext auditContext);

        VehicleColorRequestDto UpdateVehicleColor(VehicleColorRequestDto VehicleColor,
            SecurityContext securityContext,
            AuditContext auditContext);


    }
}
