using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;

namespace TAS.Services
{
    public interface IVehicleWeightManagementService
    {

        VehicleWeightsResponseDto GetAllVehicleWeight(SecurityContext securityContext, AuditContext auditContext);

        object SubmitVehicleWeight(VehicleWeightRequestDto VehicleWeightData, SecurityContext securityContext, AuditContext auditContext);

        VehicleWeightResponseDto GetVehicleWeightById(Guid VehicleWeightId, SecurityContext securityContext, AuditContext auditContext);
    }
}
