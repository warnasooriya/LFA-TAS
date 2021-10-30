using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface IDriveTypeManagementService
    {
        DriveTypesResponseDto GetDriveTypes(
            SecurityContext securityContext, 
            AuditContext auditContext);


        DriveTypesResponseDto GetParentDriveTypes(
            SecurityContext securityContext, 
            AuditContext auditContext);

        DriveTypeRequestDto AddDriveType(DriveTypeRequestDto DriveType,
            SecurityContext securityContext,
            AuditContext auditContext);


        DriveTypeResponseDto GetDriveTypeById(Guid DriveTypeId,
            SecurityContext securityContext,
            AuditContext auditContext);

        DriveTypeRequestDto UpdateDriveType(DriveTypeRequestDto DriveType,
            SecurityContext securityContext,
            AuditContext auditContext);


    }
}
