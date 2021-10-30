using System;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;

namespace TAS.Services
{
    public interface IBrokerManagementService
    {
        BrokerSResponseDto GetAllBrokers(SecurityContext securityContext, AuditContext auditContext);

        BrokerSResponseDto GetBrokerDetailsByBrokerId(SecurityContext securityContext, AuditContext auditContext, Guid BrokerId);

        bool SaveBroker(DataTransfer.Requests.BrokerRequestDto BrokerData, SecurityContext securityContext, AuditContext auditContext);

        bool UpdateBroker(DataTransfer.Requests.BrokerRequestDto BrokerData, SecurityContext securityContext, AuditContext auditContext);
        
        BrokerSResponseDto GetAllBrokersByCountry(SecurityContext securityContext, AuditContext auditContext,Guid CountryId);

        bool IsExsistsBrokerCode(Guid Id, string BrokerName, string BrokerCode, SecurityContext securityContext, AuditContext auditContext);
    }
}
