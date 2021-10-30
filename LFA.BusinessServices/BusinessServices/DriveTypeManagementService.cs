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
    internal sealed class DriveTypeManagementService : IDriveTypeManagementService
    {

        public DriveTypesResponseDto GetDriveTypes(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            DriveTypesResponseDto result = null;

            DriveTypesRetrievalUnitOfWork uow = new DriveTypesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public DriveTypesResponseDto GetParentDriveTypes(
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            DriveTypesResponseDto result = null;

            DriveTypesRetrievalUnitOfWork uow = new DriveTypesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public DriveTypeRequestDto AddDriveType(DriveTypeRequestDto DriveType, SecurityContext securityContext,
            AuditContext auditContext) {
                DriveTypeRequestDto result = new DriveTypeRequestDto();
                DriveTypeInsertionUnitOfWork uow = new DriveTypeInsertionUnitOfWork(DriveType);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.DriveTypeInsertion = uow.DriveType.DriveTypeInsertion;
                return result;
        }


        public DriveTypeResponseDto GetDriveTypeById(Guid DriveTypeId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            DriveTypeResponseDto result = new DriveTypeResponseDto();

            DriveTypeRetrievalUnitOfWork uow = new DriveTypeRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.driveTypeId = DriveTypeId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public DriveTypeRequestDto UpdateDriveType(DriveTypeRequestDto DriveType, SecurityContext securityContext,
           AuditContext auditContext)
        {
            DriveTypeRequestDto result = new DriveTypeRequestDto();
            DriveTypeUpdationUnitOfWork uow = new DriveTypeUpdationUnitOfWork(DriveType);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.DriveTypeInsertion = uow.DriveType.DriveTypeInsertion;
            return result;
        }

       
       
    }
}
