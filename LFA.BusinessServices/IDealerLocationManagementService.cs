using System;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;

namespace TAS.Services
{
    public interface IDealerLocationManagementService
    {

        DealerLocationsRespondDto GetAllDealerLocations(
            SecurityContext securityContext,
            AuditContext auditContext);

        object GetAllDealerLocationsByUser(Guid userId,
    SecurityContext securityContext,
    AuditContext auditContext);


        DealerLocationRequestDto AddDealerLocation(DealerLocationRequestDto ProductType,
            SecurityContext securityContext,
            AuditContext auditContext);


        DealerLocationRespondDto GetDealerLocationById(Guid DealerLocationId,
            SecurityContext securityContext,
            AuditContext auditContext);

        DealerLocationRequestDto UpdateDealerLocation(DealerLocationRequestDto ProductType,
            SecurityContext securityContext,
            AuditContext auditContext);




        DealerInvoicesGenerateResponseDto GenerateDealerInvoices(DealerInvoicesGenerateRequestDto DealerInvoicesGenerateRequest, SecurityContext securityContext, AuditContext auditContext);

        object GetAllDiscountSchemes(SecurityContext securityContext, AuditContext auditContext);

        object SearchDealerDiscountSchemes(DealerDiscountSchemesSearchRequestDto dealerDiscountSchemesSearchRequest, SecurityContext securityContext, AuditContext auditContext);

        object SaveDealerDiscount(DealerDiscountSaveRequestDto dealerDiscountSaveRequest, SecurityContext securityContext, AuditContext auditContext);

        object GetDealerDiscountById(Guid dealerDiscountId, SecurityContext securityContext, AuditContext auditContext);

        //DealerLocationsRespondDto GetAllDealerStaffLocationsByDealerId(SecurityContext securityContext, AuditContext auditContext);

        object GetAllDealerStaffLocationsByDealerId(Guid DealerId, SecurityContext securityContext, AuditContext auditContext);

        object AddDealerLabourCharge(DealerLabourChargeSaveRequestDto dealerLabourChargeSaveRequest, SecurityContext securityContext, AuditContext auditContext);

        object SearchDealerLabourChargeSchemes(DealerLabourChargeSearchRequestDto dealerLabourChargeSchemesSearchRequest, SecurityContext securityContext, AuditContext auditContext);

        object GetDealerLabourChargeById(Guid dealerLabourChargeId, SecurityContext securityContext, AuditContext auditContext);

        MakesResponseDto GetAllMakesByDealerId(Guid dealerId, SecurityContext securityContext, AuditContext auditContext);
        object ValidateUserOnDealerInvoiceCodeGeneration(Guid userId, SecurityContext securityContext, AuditContext auditContext);
        object GetTyreDetailsByArticleNo(string articleNo, SecurityContext securityContext, AuditContext auditContext);
        object GenerateInvoiceCode(GenerateInvoiceCodeRequestDto generateInvoiceCodeRequest, SecurityContext securityContext, AuditContext auditContext);
        object SaveTyrePolicyDetails(SaveTyrePolicySalesRequestDto generateInvoiceCodeRequest, SecurityContext securityContext, AuditContext auditContext);
        object GetTyreContractDetails(TyreContractRequestDto tyreContractRequestDto, SecurityContext securityContext, AuditContext auditContext);
        object SearchDealerInvoiceCode(DealerInvoiceCodeSearchRequestDto invoiceCodeSearchRequest, SecurityContext securityContext, AuditContext auditContext);
        object LoadInvoceCodeDetailsById(LoadInvoceCodeByIdRequestDto invoiceLoadByIdRequest, SecurityContext securityContext, AuditContext auditContext);
        object GetAllAvailabelTireSizes(SecurityContext securityContext, AuditContext auditContext);
        object GetConfirmedBordxByYearAndMonth(int year, int month, SecurityContext securityContext,
            AuditContext auditContext);
        object DownloadInvoiceSummary(Guid dealerId, Guid bordxId, SecurityContext securityContext, AuditContext auditContext);
        //object GetAllAvailabelTireSizesByWidth(string widthFront, SecurityContext securityContext, AuditContext auditContext);
        object GetAllAvailabelTireSizesByWidth(LoadTyreDetailsByWidthRequestDto LoadTyreDetailsByWidthRequestDto, SecurityContext securityContext, AuditContext auditContext);
        object GetAllAvailabelTireSizesByDiameter(string cross,string Width, SecurityContext securityContext, AuditContext auditContext);
        object GetAllAvailabelTireSizesByloadSpeed(string cross, string width, string diameter, SecurityContext securityContext, AuditContext auditContext);
        object GetAllAvailabelTireSizesByPattern(string cross, string width, string diameter, string loadSpeed, SecurityContext securityContext, AuditContext auditContext);
        object GetArticleNoByTyreSize(string width, string cross, string diameter, string loadSpeed, string pattern, SecurityContext securityContext, AuditContext auditContext);
        //object GetArticleNoByTyreSize(string width, string cross, string diameter, string loadSpeed, string pattern, SecurityContext context1, AuditContext context2);
    }
}
