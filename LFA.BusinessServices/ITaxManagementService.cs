using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface ITaxManagementService
    {
        TaxesResponseDto GetAllTaxes(
            SecurityContext securityContext,
            AuditContext auditContext);

        TaxRequestDto AddTax(TaxRequestDto Tax,
            SecurityContext securityContext,
            AuditContext auditContext);


        TaxResponseDto GetTaxById(Guid TaxId,
            SecurityContext securityContext,
            AuditContext auditContext);

        TaxRequestDto UpdateTax(TaxRequestDto Tax,
            SecurityContext securityContext,
            AuditContext auditContext);

        CountryTaxessResponseDto GetAllCountryTaxes(
          SecurityContext securityContext,
          AuditContext auditContext);

        CountryTaxessResponseDto GetCountryTaxesByCountryId(
            Guid countryId,
            SecurityContext securityContext,
            AuditContext auditContext);

        CountryTaxesRequestDto AddCountryTaxes(CountryTaxesRequestDto CountryTaxes,
         SecurityContext securityContext,
         AuditContext auditContext);

        CountryTaxesResponseDto GetCountryTaxesById(Guid CountryTaxesId,
            SecurityContext securityContext,
            AuditContext auditContext);

        CountryTaxesRequestDto UpdateCountryTaxes(CountryTaxesRequestDto CountryTaxes, bool delete,
            SecurityContext securityContext,
           AuditContext auditContext);

        bool IsExistingTaxName(Guid Id, string taxName,
           SecurityContext securityContext,
           AuditContext auditContext);

        bool IsExistingTaxCode(Guid Id, string taxName,
           SecurityContext securityContext,
           AuditContext auditContext);

        ContractTaxesesResponseDto GetAllContactTaxes(SecurityContext securityContext, AuditContext auditContext);
    }
}
