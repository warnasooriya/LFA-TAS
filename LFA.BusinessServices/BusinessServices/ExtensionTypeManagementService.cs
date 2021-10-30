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
    internal sealed class ExtensionTypeManagementService : IExtensionTypeManagementService
    {

        public ExtensionTypesResponseDto GetExtensionTypes(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            ExtensionTypesResponseDto result = null;

            ExtensionTypesRetrievalUnitOfWork uow = new ExtensionTypesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public ExtensionTypeRequestDto AddExtensionType(
            ExtensionTypeRequestDto ExtensionType, 
            SecurityContext securityContext,
            AuditContext auditContext) {
                ExtensionTypeRequestDto result = new ExtensionTypeRequestDto();
                ExtensionTypeInsertionUnitOfWork uow = new ExtensionTypeInsertionUnitOfWork(ExtensionType);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result = uow.ExtensionType;
                return result;
        }


        public ExtensionTypeResponseDto GetExtensionTypeById(
            Guid ExtensionTypeId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ExtensionTypeResponseDto result = new ExtensionTypeResponseDto();
            ExtensionTypeRetrievalUnitOfWork uow = new ExtensionTypeRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.ExtensionTypeId = ExtensionTypeId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public ExtensionTypeRequestDto UpdateExtensionType(
            ExtensionTypeRequestDto ExtensionType, 
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ExtensionTypeRequestDto result = new ExtensionTypeRequestDto();
            ExtensionTypeUpdationUnitOfWork uow = new ExtensionTypeUpdationUnitOfWork(ExtensionType);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.ExtensionType;
            return result;
        }

       
       
    }
}
