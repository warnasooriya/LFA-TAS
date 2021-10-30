using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface ITransmissionTypeManagementService
    {
        TransmissionTypesResponseDto GetTransmissionTypes(
            SecurityContext securityContext, 
            AuditContext auditContext);

        TransmissionTypeRequestDto AddTransmissionType(TransmissionTypeRequestDto TransmissionType,
            SecurityContext securityContext,
            AuditContext auditContext);


        TransmissionTypeResponseDto GetTransmissionTypeById(Guid TransmissionTypId,
            SecurityContext securityContext,
            AuditContext auditContext);

        TransmissionTypeRequestDto UpdateTransmissionType(TransmissionTypeRequestDto TransmissionType,
            SecurityContext securityContext,
            AuditContext auditContext);
      
        TransmissionTechnologiesResponseDto GetTransmissionTechnologies(
         SecurityContext securityContext,
         AuditContext auditContext);

    }
}
