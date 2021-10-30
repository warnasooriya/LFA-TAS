using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;

namespace TAS.Services
{
    public interface ICurrencyManagementService
    {

        CurrenciesResponseDto GetAllCurrencies(
            SecurityContext securityContext,
            AuditContext auditContext);
     
        CurrencyConversionsResponseDto GetCurrencyConversions(
            SecurityContext securityContext,
            AuditContext auditContext);
      
        CurrencyConversionRequestDto AddCurrencyConversion(CurrencyConversionRequestDto CurrencyConversion, 
            SecurityContext securityContext,
            AuditContext auditContext);
       
        CurrencyConversionResponseDto GetCurrencyConversionById(Guid CurrencyConversionId,
            SecurityContext securityContext,
            AuditContext auditContext);
      
        CurrencyConversionRequestDto UpdateCurrencyConversion(CurrencyConversionRequestDto CurrencyConversion, 
            SecurityContext securityContext,
            AuditContext auditContext);
      
        CurrencyConversionPeriodsResponseDto GetCurrencyConversionPeriods(
            SecurityContext securityContext,
            AuditContext auditContext);
       
        CurrencyConversionPeriodRequestDto AddCurrencyConversionPeriod(CurrencyConversionPeriodRequestDto CurrencyConversionPeriod, 
            SecurityContext securityContext,
            AuditContext auditContext);
      
        CurrencyConversionPeriodResponseDto GetCurrencyConversionPeriodById(Guid CurrencyConversionPeriodId,
            SecurityContext securityContext,
            AuditContext auditContext);
     
        CurrencyConversionPeriodRequestDto UpdateCurrencyConversionPeriod(CurrencyConversionPeriodRequestDto CurrencyConversionPeriod, 
            SecurityContext securityContext,
           AuditContext auditContext);
       
        CurrencyEmailsResponseDto GetCurrencyEmails(
           SecurityContext securityContext,
           AuditContext auditContext);

        CurrencyEmailRequestDto AddCurrencyEmail(CurrencyEmailRequestDto CurrencyEmail,
            SecurityContext securityContext,
            AuditContext auditContext);

        CurrencyEmailResponseDto GetCurrencyEmailById(Guid CurrencyEmailId,
            SecurityContext securityContext,
            AuditContext auditContext);

        CurrencyEmailRequestDto UpdateCurrencyEmail(CurrencyEmailRequestDto CurrencyEmail,
            SecurityContext securityContext,
            AuditContext auditContext);

        string ValidateCurrencyPeriodOverlaps(CurrencyConversionPeriodRequestDto CurrencyConversionPeriodRequest, SecurityContext securityContext, AuditContext auditContext);

        bool? CurrencyPeriodCheck(SecurityContext securityContext, AuditContext auditContext);

        bool? GetCurrencyRateAvailabilityByCurrencyId(Guid currencyId, SecurityContext securityContext, AuditContext auditContext);

    
        object ConvertFromBaseCurrency(decimal value,Guid BaseCurrencyId,Guid BaseCurrencyPeriodId,SecurityContext securityContext,AuditContext auditContext);
        decimal GetConversionRate(Guid BaseCurrencyId, Guid BaseCurrencyPeriodId, SecurityContext securityContext, AuditContext auditContext);
    }
}
