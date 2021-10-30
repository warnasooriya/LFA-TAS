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
    public interface IMakeManagementService
    {     

        MakesResponseDto GetAllMakes(
           SecurityContext securityContext,
           AuditContext auditContext);

        MakeRequestDto AddMake(MakeRequestDto Make,
            SecurityContext securityContext,
            AuditContext auditContext);


        MakeResponseDto GetMakeById(Guid MakeId,
            SecurityContext securityContext,
            AuditContext auditContext);

        MakeRequestDto UpdateMake(MakeRequestDto Make,
            SecurityContext securityContext,
            AuditContext auditContext);

        MakesResponseDto GetMakesByCommodityCategoryId(Guid id, SecurityContext securityContext, AuditContext auditContext);
    }
}
