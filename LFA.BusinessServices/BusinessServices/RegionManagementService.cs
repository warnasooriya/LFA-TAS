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
    internal sealed class RegionManagementService : IRegionManagementService
    {

        public RegionesResponseDto GetRegions(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            RegionesResponseDto result = null;

            RegionsRetrievalUnitOfWork uow = new RegionsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public RegionRequestDto AddRegion(RegionRequestDto Region, SecurityContext securityContext,
            AuditContext auditContext) {
                RegionRequestDto result = new RegionRequestDto();
                RegionInsertionUnitOfWork uow = new RegionInsertionUnitOfWork(Region);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.RegionInsertion = uow.Region.RegionInsertion;
                return result;
        }


        public RegionResponseDto GetRegionById(Guid RegionId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            RegionResponseDto result = new RegionResponseDto();

            RegionRetrievalUnitOfWork uow = new RegionRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.RegionId = RegionId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public RegionRequestDto UpdateRegion(RegionRequestDto Region, SecurityContext securityContext,
           AuditContext auditContext)
        {
            RegionRequestDto result = new RegionRequestDto();
            RegionUpdationUnitOfWork uow = new RegionUpdationUnitOfWork(Region);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.RegionInsertion = uow.Region.RegionInsertion;
            return result;
        }

       
       
    }
}
