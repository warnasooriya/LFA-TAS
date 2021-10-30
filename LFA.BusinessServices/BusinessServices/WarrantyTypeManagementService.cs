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
    internal sealed class WarrantyTypeManagementService : IWarrantyTypeManagementService
    {

        public WarrantyTypesResponseDto GetWarrantyTypes(SecurityContext securityContext, AuditContext auditContext)
        {
            WarrantyTypesResponseDto result = null;

            WarrantyTypesRetrievalUnitOfWork uow = new WarrantyTypesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public WarrantyTypeRequestDto AddWarrantyType(WarrantyTypeRequestDto WarrantyType, SecurityContext securityContext,AuditContext auditContext) {
                WarrantyTypeRequestDto result = new WarrantyTypeRequestDto();
                WarrantyTypeInsertionUnitOfWork uow = new WarrantyTypeInsertionUnitOfWork(WarrantyType);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.WarrantyTypeInsertion = uow.WarrantyType.WarrantyTypeInsertion;
                return result;
        }

        public WarrantyTypeResponseDto GetWarrantyTypeById(Guid WarrantyTypeId,SecurityContext securityContext,AuditContext auditContext)
        {
            WarrantyTypeResponseDto result = new WarrantyTypeResponseDto();

            WarrantyTypeRetrievalUnitOfWork uow = new WarrantyTypeRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.WarrantyTypeId = WarrantyTypeId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public WarrantyTypeRequestDto UpdateWarrantyType(WarrantyTypeRequestDto WarrantyType, SecurityContext securityContext,AuditContext auditContext)
        {
            WarrantyTypeRequestDto result = new WarrantyTypeRequestDto();
            WarrantyTypeUpdationUnitOfWork uow = new WarrantyTypeUpdationUnitOfWork(WarrantyType);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.WarrantyTypeInsertion = uow.WarrantyType.WarrantyTypeInsertion;
            return result;
        }
    }
}
