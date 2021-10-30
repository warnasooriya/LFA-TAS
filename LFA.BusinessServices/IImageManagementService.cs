using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Responses;

namespace TAS.Services
{
    public interface IImageManagementService
    {

        Guid SaveImage(DataTransfer.Requests.ImageRequestDto imageRequestDto, DataTransfer.Common.SecurityContext securityContext, DataTransfer.Common.AuditContext auditContext);

        DataTransfer.Responses.ImageResponseDto GetImageById(Guid ImageId, DataTransfer.Common.SecurityContext securityContext, DataTransfer.Common.AuditContext auditContext);


        ImageResponseDto GetTPAImageById(Guid TPAId, Guid guid, DataTransfer.Common.SecurityContext securityContext, DataTransfer.Common.AuditContext auditContext);
    }
}
