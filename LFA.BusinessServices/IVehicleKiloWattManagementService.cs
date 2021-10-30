using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface IVehicleKiloWattManagementService
    {
        VehicleKiloWattsResponseDto GetVehicleKiloWatts(
            SecurityContext securityContext, 
            AuditContext auditContext);

        VehicleKiloWattRequestDto AddVehicleKiloWatt(VehicleKiloWattRequestDto VehicleKiloWatt,
            SecurityContext securityContext,
            AuditContext auditContext);


        VehicleKiloWattResponseDto GetVehicleKiloWattById(Guid VehicleKiloWattId,
            SecurityContext securityContext,
            AuditContext auditContext);

        VehicleKiloWattRequestDto UpdateVehicleKiloWatt(VehicleKiloWattRequestDto VehicleKiloWatt,
            SecurityContext securityContext,
            AuditContext auditContext);


    }
}
