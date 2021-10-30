using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;

namespace TAS.Services.BusinessServices
{
    internal sealed class ManufatureManagementService : IManufacturerManagementService
    {
        public ManufacturesResponseDto GetAllManufatures( 
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ManufacturesResponseDto result = null;
            ManufacturesRetrievalUnitOfWork uow = new ManufacturesRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }

        public ManufacturerRequestDto AddManufacturer(ManufacturerRequestDto Manufacturer, 
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ManufacturerRequestDto result = new ManufacturerRequestDto();
            ManufacturerInsertionUnitOfWork uow = new ManufacturerInsertionUnitOfWork(Manufacturer);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.ManufacturerInsertion = uow.Manufacturer.ManufacturerInsertion;
            return result;
        }


        public ManufacturerResponseDto GetManufacturerById(Guid ManufacturerId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ManufacturerResponseDto result = new ManufacturerResponseDto();

            ManufacturerRetrievalUnitOfWork uow = new ManufacturerRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.ManufacturerId = ManufacturerId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public ManufacturerRequestDto UpdateManufacturer(ManufacturerRequestDto Manufacturer, 
            SecurityContext securityContext,
           AuditContext auditContext)
        {
            ManufacturerRequestDto result = new ManufacturerRequestDto();
            ManufacturerUpdationUnitOfWork uow = new ManufacturerUpdationUnitOfWork(Manufacturer);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.ManufacturerInsertion = uow.Manufacturer.ManufacturerInsertion;
            return result;
        }


    }
}
