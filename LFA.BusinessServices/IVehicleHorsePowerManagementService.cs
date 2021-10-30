using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface IVehicleHorsePowerManagementService
    {
        VehicleHorsePowersResponseDto GetVehicleHorsePowers(
            SecurityContext securityContext, 
            AuditContext auditContext);

        VehicleHorsePowerRequestDto AddVehicleHorsePower(VehicleHorsePowerRequestDto VehicleHorsePower,
            SecurityContext securityContext,
            AuditContext auditContext);


        VehicleHorsePowerResponseDto GetVehicleHorsePowerById(Guid VehicleHorsePowerId,
            SecurityContext securityContext,
            AuditContext auditContext);

        VehicleHorsePowerRequestDto UpdateVehicleHorsePower(VehicleHorsePowerRequestDto VehicleHorsePower,
            SecurityContext securityContext,
            AuditContext auditContext);


    }
}
