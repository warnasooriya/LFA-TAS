using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    internal sealed class IncurredErningManagementService : IIncurredErningManagementService
    {
        public List<string> GetUNWYears(SecurityContext securityContext, AuditContext auditContext)
        {
            List<string> Response = null;
            UNWYearsUnitOfWork uow = new UNWYearsUnitOfWork();
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.result;
            }
            return Response;
        }

        public IncurredErningProcessResponseDto IncurredErningProcess(string UNWyear, Guid CountryId, Guid Dealer, Guid Reinsurer, Guid Insurer, Guid MakeId, Guid ModelId, Guid CylindercountId, Guid EngineCapacityId, string BordxStartDate, string BordxEndDate, string ErnedDate, string ClaimedDate, SecurityContext securityContext, AuditContext auditContext)
        {
            IncurredErningProcessResponseDto Response = new IncurredErningProcessResponseDto();
            IncurredErningProcessUnitOfWork uow = new IncurredErningProcessUnitOfWork(UNWyear, CountryId, Dealer, Reinsurer, Insurer,MakeId, ModelId,CylindercountId,EngineCapacityId, BordxStartDate, BordxEndDate, ErnedDate, ClaimedDate);
            //uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public IncurredErningExportResponseDto GetIncurredErningExcel(string UNWyear, Guid CountryId, Guid Dealer, Guid Reinsurer, Guid Insurer, Guid MakeId, Guid ModelId, Guid CylindercountId, Guid EngineCapacityId,Guid InsuaranceLimitationId, string BordxStartDate, string BordxEndDate, string ErnedDate, string ClaimedDate, SecurityContext securityContext, AuditContext auditContext)
        {
            IncurredErningToExcelUnitOfWork uow = new IncurredErningToExcelUnitOfWork(UNWyear, CountryId, Dealer, Reinsurer, Insurer, MakeId, ModelId, CylindercountId, EngineCapacityId, InsuaranceLimitationId, BordxStartDate, BordxEndDate, ErnedDate, ClaimedDate);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }
    }
}
