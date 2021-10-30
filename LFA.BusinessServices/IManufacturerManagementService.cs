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
    public interface IManufacturerManagementService
    {
       // DataTransfer.Responses.ManufacturesResponseDto GetAllManufatures(DataTransfer.Common.SecurityContext securityContext, DataTransfer.Common.AuditContext auditContext);


        ManufacturesResponseDto GetAllManufatures(
           SecurityContext securityContext,
           AuditContext auditContext);

        ManufacturerRequestDto AddManufacturer(ManufacturerRequestDto Manufacturer,
            SecurityContext securityContext,
            AuditContext auditContext);


        ManufacturerResponseDto GetManufacturerById(Guid ManufacturerId,
            SecurityContext securityContext,
            AuditContext auditContext);

        ManufacturerRequestDto UpdateManufacturer(ManufacturerRequestDto Manufacturer,
            SecurityContext securityContext,
            AuditContext auditContext);
    }

}

