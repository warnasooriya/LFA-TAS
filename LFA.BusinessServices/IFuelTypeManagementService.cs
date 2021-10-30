using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface IFuelTypeManagementService
    {
        FuelTypesResponseDto GetFuelTypes(
            SecurityContext securityContext, 
            AuditContext auditContext);


        FuelTypesResponseDto GetParentFuelTypes(
            SecurityContext securityContext, 
            AuditContext auditContext);

        FuelTypeRequestDto AddFuelType(FuelTypeRequestDto FuelType,
            SecurityContext securityContext,
            AuditContext auditContext);


        FuelTypeResponseDto GetFuelTypeById(Guid FuelTypeId,
            SecurityContext securityContext,
            AuditContext auditContext);

        FuelTypeRequestDto UpdateFuelType(FuelTypeRequestDto FuelType,
            SecurityContext securityContext,
            AuditContext auditContext);


    }
}
