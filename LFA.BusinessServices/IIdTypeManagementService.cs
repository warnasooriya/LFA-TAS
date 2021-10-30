﻿using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services
{
    public interface IIdTypeManagementService
    {

        IdTypesResponseDto GetAllIdTypes(SecurityContext securityContext, AuditContext auditContext);
    }
}
