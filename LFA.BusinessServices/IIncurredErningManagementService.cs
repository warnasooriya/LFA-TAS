using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;

namespace TAS.Services
{
    public interface IIncurredErningManagementService
    {
        List<string> GetUNWYears(SecurityContext securityContext, AuditContext auditContext);

        IncurredErningProcessResponseDto IncurredErningProcess(string UNWyear, Guid CountryId, Guid Dealer, Guid Reinsurer, Guid Insurer, Guid MakeId, Guid ModelId, Guid CylindercountId, Guid EngineCapacityId, string BordxStartDate, string BordxEndDate, string ErnedDate, string ClaimedDate, SecurityContext securityContext, AuditContext auditContext);

        IncurredErningExportResponseDto GetIncurredErningExcel(string UNWyear, Guid CountryId, Guid Dealer, Guid Reinsurer, Guid Insurer, Guid MakeId, Guid ModelId, Guid CylindercountId, Guid EngineCapacityId,Guid InsuaranceLimitationId, string BordxStartDate, string BordxEndDate, string ErnedDate, string ClaimedDate, SecurityContext securityContext, AuditContext auditContext);

        //bool ClaimBordxProcessUpdate(Guid ClaimbordxprocessId, Guid UserId, bool IsConfirm, SecurityContext securityContext, AuditContext auditContext);

        //object ClaimBordxProcessedDetailsByYear(int Year, Guid Insurerid, Guid Reinsurerid, SecurityContext securityContext, AuditContext auditContext);

        //bool ClaimBordxCanConfirmedByYear(int year, Guid insurerid, Guid reinsurerid, SecurityContext securityContext, AuditContext auditContext);


    }
}
