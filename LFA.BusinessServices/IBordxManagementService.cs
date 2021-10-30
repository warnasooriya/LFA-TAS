using System;
using System.Collections.Generic;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
namespace TAS.Services
{
    public interface IBordxManagementService
    {
        BordxsResponseDto GetBordxs(
            SecurityContext securityContext,
            AuditContext auditContext);

        BordxRequestDto AddBordx(BordxRequestDto Bordx,
            SecurityContext securityContext,
            AuditContext auditContext);

        BordxResponseDto GetBordxById(Guid BordxId,
            SecurityContext securityContext,
            AuditContext auditContext);

        BordxRequestDto UpdateBordx(BordxRequestDto Bordx,
            SecurityContext securityContext,
            AuditContext auditContext);

        BordxsDetailsResponseDto GetBordxDetails(
            SecurityContext securityContext,
            AuditContext auditContext);

        BordxDetailsRequestDto AddBordxDetails(BordxDetailsRequestDto Bordx,
            SecurityContext securityContext,
            AuditContext auditContext);

        BordxDetailsResponseDto GetBordxDetailsById(Guid BordxId,
            SecurityContext securityContext,
            AuditContext auditContext);

        BordxDetailsRequestDto UpdateBordxDetails(BordxDetailsRequestDto Bordx,
           SecurityContext securityContext,
           AuditContext auditContext);

        object GetBordxReportColumns(BordxColumnRequestDto bordxColumnRequestDto,
          SecurityContext securityContext,
          AuditContext auditContext);

        BordxReportColumnsMapsResponseDto GetBordxReportColumnsMaps(Guid UserId,
          SecurityContext securityContext,
          AuditContext auditContext);

        BordxReportColumnsMapRequestDto AddBordxReportColumnsMap(BordxReportColumnsMapRequestDto Bordx,
          SecurityContext securityContext,
          AuditContext auditContext);

        BordxReportColumnsMapResponseDto GetBordxReportColumnsMapById(Guid BordxId,
            SecurityContext securityContext,
            AuditContext auditContext);

        BordxReportColumnsMapRequestDto UpdateBordxReportColumnsMap(BordxReportColumnsMapRequestDto Bordx,
            SecurityContext securityContext,
            AuditContext auditContext);

        object GetConfirmedBordxForGrid(ConfirmedBordxForGridRequestDto ConfirmedBordxForGridRequest, SecurityContext securityContext, AuditContext auditContext);

        object ConfirmedBordxYears(SecurityContext securityContext, AuditContext auditContext);

        // object getAllBordxAllowedYearsMonths(SecurityContext securityContext, AuditContext auditContext);

        object GetAllBordxDetailsByYearMonth(BordxDetailsByYearMonthRequestDto bordxRequestDetails, SecurityContext securityContext, AuditContext auditContext);

        string ProcessBordx(BordxProcessRequestDto bordxProcessRequest, SecurityContext securityContext, AuditContext auditContext);

        string ConfirmBordx(BordxProcessRequestDto bordxProcessRequest, SecurityContext securityContext, AuditContext auditContext);

        string CreateBordx(BordxCreateRequestDto bordxCreateRequest, SecurityContext securityContext, AuditContext auditContext);

        object GetLast10Bordx(BordxListRequestDto bordxListRequestDto,SecurityContext securityContext, AuditContext auditContext);

        //  string GetNextBordxNumber(int year, int month, SecurityContext securityContext, AuditContext auditContext);

        string DeleteBordx(Guid BordxId, SecurityContext securityContext, AuditContext auditContext);

        // object GetNextBordxNumbers(int year, int month, SecurityContext securityContext, AuditContext auditContext);

        string TransferPolicyToBordx(BordxTransferRequestDto BordxTransferRequestDto, SecurityContext securityContext, AuditContext auditContext);

        object getAllBordxAllowedYearsMonths(Guid InsurerId, Guid ReinsurerId, Guid CommodityTypeId, SecurityContext securityContext, AuditContext auditContext);

        string GetNextBordxNumber(int year, int month, Guid reinsurerId,Guid insurerId, Guid productId, Guid CommodityTypeId, SecurityContext securityContext, AuditContext auditContext);

        object GetNextBordxNumbers(Guid commodityTypeId,Guid reinsurerId, Guid insurerId,Guid productId, int year, int month, SecurityContext securityContext, AuditContext auditContext);

        string BordxReopen(BordxReopenRequestDto BordxReopenRequestDto, SecurityContext securityContext, AuditContext auditContext);

        ValidateContractWithTaxResponseDto ValidateContractWithTax(Guid countryTaxId, Guid ContractId, Guid PolicyId, SecurityContext securityContext, AuditContext auditContext);

        object GetBordxReportTemplateColumns(BordxColumnRequestDto bordxColumnRequestDto,SecurityContext securityContext, AuditContext auditContext);

        object GetAllBordxReportTemplateForSearchGrid(BordxReportTemplateSearchGridRequestDto bordxReportTemplateSearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext);

        BordxReportTemplateResponseDto GetBordxReportTemplateById(Guid bordxReportTemplateId, SecurityContext securityContext, AuditContext auditContext);
        bool BordxReportTemplateNameIsExists(BordxReportTemplateRequestDto bordxReportTemplate, SecurityContext securityContext, AuditContext auditContext);
        bool SaveBordxReportTemplate(BordxReportTemplateRequestDto bordxReportTemplate, SecurityContext securityContext, AuditContext auditContext);

        List<BordxReportTemplateResponseDto> GetBordxReportTemplates(BordxTemplateRequestDto bordxTemplateRequestDto, SecurityContext securityContext, AuditContext auditContext);
        object getBordxNumbersYearsAndMonth(string bordxYear, string bordxMonth, SecurityContext securityContext, AuditContext auditContext);
    }
}
