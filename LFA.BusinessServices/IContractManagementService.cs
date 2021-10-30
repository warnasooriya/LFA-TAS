using System;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Common.Transformer;
namespace TAS.Services
{
    public interface IContractManagementService
    {
        ContractExtensionsResponseDto GetContractExtensions(
             SecurityContext securityContext,
             AuditContext auditContext);

        ContractExtensionsRequestDto AddContractExtensions(
            ContractExtensionsRequestDto ContractExtension,
            SecurityContext securityContext,
            AuditContext auditContext);

        ContractExtensionResponseDto GetContractExtensionsById(
            Guid ContractExtensionId,
            SecurityContext securityContext,
            AuditContext auditContext);

        ContractExtensionsRequestDto UpdateContractExtensions(
            ContractExtensionsRequestDto ContractExtension,
            SecurityContext securityContext,
           AuditContext auditContext);

        ContractsResponseDto GetContracts(
            SecurityContext securityContext,
            AuditContext auditContext);

        ContractsResponseDto GetContractsByCommodityTypeId(
            Guid commodityTypeId,
    SecurityContext securityContext,
    AuditContext auditContext);

         ContractsResponseDto GetContractsByDealerAndProduct(
                      Guid dealerId , Guid productId,
            SecurityContext securityContext,
            AuditContext auditContext);

        ContractRequestDto AddContract(
            ContractRequestDto Contract,
            SecurityContext securityContext,
            AuditContext auditContext);

        ContractResponseDto GetContractById(
            Guid Id,
            SecurityContext securityContext,
            AuditContext auditContext);

        ContractRequestDto UpdateContract(
            ContractRequestDto Contract,
            SecurityContext securityContext,
           AuditContext auditContext);
        ContractTaxesesResponseDto GetContractTaxess(
            SecurityContext securityContext,
            AuditContext auditContext);
        ContractTaxesRequestDto AddContractTaxes(ContractTaxesRequestDto ContractTaxes, SecurityContext securityContext,
            AuditContext auditContext);
        ContractTaxesResponseDto GetContractTaxesById(Guid ContractTaxesId,
    SecurityContext securityContext,
    AuditContext auditContext);
        ContractTaxesRequestDto UpdateContractTaxes(ContractTaxesRequestDto ContractTaxes, SecurityContext securityContext,
           AuditContext auditContext);


        object GetContrcts(Guid ProductId, Guid DealerId, DateTime Date,
            Guid CylinderCountId, Guid EngineCapacityId, Guid ItemStatusId, Guid MakeId, Guid ModelId, Guid VariantId,decimal GrossWeight,Guid UsageTypeId,
           SecurityContext securityContext,
           AuditContext auditContext);

        object GetContractsForSearchGrid(ContractSearchGridRequestDto ContractSearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext);

        ContractViewData GetFullContractDetailsById(Guid ContractId, Guid variantId, Guid modelId, bool withTax, SecurityContext securityContext, AuditContext auditContext);

        string AddNewContract(ContractRequestV2Dto data, SecurityContext securityContext, AuditContext auditContext);

        string UpdateContractV2(ContractUpdateRequestV2Dto data, SecurityContext securityContext, AuditContext auditContext);

        bool ContractsUpdateValidityCheck(Guid ContractId, SecurityContext securityContext, AuditContext auditContext);

        bool UpdateContractStatus(Guid ContractId, bool Status, SecurityContext securityContext, AuditContext auditContext);

        object GetContractTaxesByExtensionId(Guid contractId, decimal PremiumForTax, SecurityContext securityContext, AuditContext auditContext);
        object GetAllInsuaranceLimitaionsByCommodityType(Guid commodityTypeId,string productType, SecurityContext context, AuditContext auditContext);
        object AddNewInsuaranceLimitation(InsuaranceLimitationRequestDto insuaranceLimitation, SecurityContext context, AuditContext auditContext);

        GetContractDetailsByContractIdDto GetFullContractDetailsByIdV2(Guid contractId, SecurityContext context, AuditContext auditContext);
        object GetAllAttributeSpecificationsByInsuranceLimitataionId(Guid insuranceLimitationId,Guid contractId, SecurityContext context, AuditContext auditContext);
        object GetAllMakeModelDetailsByExtensionId(Guid extensionId, SecurityContext context, AuditContext auditContext);
        object GetAllPremiumByExtensionId(Guid extensionId, SecurityContext context, AuditContext auditContext);
        object GetAllExtensionTypeByContractId(Guid CcontractId, Guid ProductId, Guid DealerId, DateTime Date,
                     Guid CylinderCountId, Guid EngineCapacityId, Guid MakeId, Guid ModelId, Guid VariantId, decimal GrossWeight, SecurityContext context, AuditContext auditContext);
        object GetAllExtensionTypeByMakeModel( Guid DealerId,Guid CylinderCountId, Guid EngineCapacityId, Guid MakeId, Guid ModelId,SecurityContext context, AuditContext auditContext);

        object GetAttributeSpecificationByExtensionId(Guid extensionId, Guid CcontractId, Guid ProductId, Guid DealerId, DateTime Date,
                     Guid CylinderCountId, Guid EngineCapacityId, Guid MakeId, Guid ModelId, Guid VariantId, decimal GrossWeight, SecurityContext context, AuditContext auditContext);
        object GetCoverTypesByAttributeId(Guid attributeSpecId, SecurityContext context, AuditContext auditContext);
        object GetCoverTypesByExtensionId(Guid attributeSpecificationId, Guid extensionId, Guid contractId, Guid productId, Guid dealerId,
            DateTime date, Guid cylinderCountId, Guid engineCapacityId, Guid makeId, Guid modelId, Guid variantId, decimal grossWeight,
            Guid ItemStatusId, SecurityContext context, AuditContext auditContext);
        object GetPremium(Guid contractPremiumId, decimal usage, Guid attributeSpecificationId, Guid extensionId, Guid contractId, Guid productId,
            Guid dealerId, DateTime date, Guid cylinderCountId, Guid engineCapacityId, Guid makeId, Guid modelId, Guid variantId, decimal grossWeight,
            Guid ItemStatusId,decimal DealerPrice,DateTime ItemPurchasedDate, SecurityContext context, AuditContext auditContext);

        ContractInsuaranceLimitationResponseDto GetContractInsuaranceLimitations(Guid ContractId, SecurityContext securityContext, AuditContext auditContext);

        InsuaranceLimitationsResponseDto GetInsuaranceLimitations(SecurityContext securityContext, AuditContext auditContext);

        InsuaranceLimitationResponseDto GetInsuaranceLimitationByContractInsuaranceLimitationId(Guid ContractInsuaranceLimitationId, SecurityContext securityContext, AuditContext auditContext);
    }
}
