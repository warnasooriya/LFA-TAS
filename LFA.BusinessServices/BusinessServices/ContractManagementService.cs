using System;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Common.Transformer;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    internal sealed class ContractManagementService : IContractManagementService
    {
        #region ContractExtension
        public ContractExtensionsResponseDto GetContractExtensions(
             SecurityContext securityContext,
             AuditContext auditContext)
        {
            ContractExtensionsResponseDto result = null;

            ContractAllExtentionsRetrievalUnitOfWork uow = new ContractAllExtentionsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public ContractExtensionsRequestDto AddContractExtensions(
            ContractExtensionsRequestDto ContractExtension,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ContractExtensionsRequestDto result = new ContractExtensionsRequestDto();
            ContractExtensionsInsertionUnitOfWork uow = new ContractExtensionsInsertionUnitOfWork(ContractExtension);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.ContractExtension;
            return result;
        }

        public ContractExtensionResponseDto GetContractExtensionsById(
            Guid ContractExtensionId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ContractExtensionResponseDto result = new ContractExtensionResponseDto();

            ContractExtensionsRetrievalUnitOfWork uow = new ContractExtensionsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.ContractExtensionId = ContractExtensionId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public ContractExtensionsRequestDto UpdateContractExtensions(
            ContractExtensionsRequestDto ContractExtension,
            SecurityContext securityContext,
           AuditContext auditContext)
        {
            ContractExtensionsRequestDto result = new ContractExtensionsRequestDto();
            ContractExtensionsUpdationUnitOfWork uow = new ContractExtensionsUpdationUnitOfWork(ContractExtension);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.ContractExtensions;
            return result;
        }
        #endregion

        #region Contract
        public ContractsResponseDto GetContracts(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ContractsResponseDto result = null;

            ContractsRetrievalUnitOfWork uow = new ContractsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public ContractsResponseDto GetContractsByCommodityTypeId(
            Guid commodityTypeId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ContractsResponseDto result = null;

            ContractsByComodityTypeRetrievalUnitOfWork uow = new ContractsByComodityTypeRetrievalUnitOfWork(commodityTypeId);

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }



        public ContractsResponseDto GetContractsByDealerAndProduct(
            Guid dealerId , Guid productId,
           SecurityContext securityContext,
           AuditContext auditContext)
        {
            ContractsResponseDto result = null;

            ContractsRetrievalByDealerAndProductUnitOfWork uow = new ContractsRetrievalByDealerAndProductUnitOfWork(dealerId, productId);

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }


        public ContractRequestDto AddContract(
            ContractRequestDto Contract,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ContractRequestDto result = new ContractRequestDto();
            ContractInsertionUnitOfWork uow = new ContractInsertionUnitOfWork(Contract);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Contract;
            return result;
        }

        public ContractResponseDto GetContractById(
            Guid Id,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ContractResponseDto result = new ContractResponseDto();

            ContractRetrievalUnitOfWork uow = new ContractRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.ContractId = Id;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public ContractRequestDto UpdateContract(
            ContractRequestDto Contract,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ContractRequestDto result = new ContractRequestDto();
            ContractUpdationUnitOfWork uow = new ContractUpdationUnitOfWork(Contract);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Contract;
            return result;
        }

        public object GetContrcts(Guid ProductId, Guid DealerId, DateTime Date,
            Guid CylinderCountId, Guid EngineCapacityId, Guid ItemStatusId, Guid MakeId, Guid ModelId, Guid VariantId, decimal GrossWeight,Guid UsageTypeId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            object result = new object();
            ContrctsRetrievalInPolicyRegistrationUnitOfWork uow = new ContrctsRetrievalInPolicyRegistrationUnitOfWork(ProductId, DealerId, Date,
                CylinderCountId, EngineCapacityId, ItemStatusId, MakeId, ModelId, VariantId, GrossWeight, UsageTypeId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }


        public string AddNewContract(
            ContractRequestV2Dto data,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            String _Response = String.Empty;
            NewContractInsertionUnitOfWork uow = new NewContractInsertionUnitOfWork(data);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            _Response = uow.Result;
            return _Response;
        }

        public string UpdateContractV2(ContractUpdateRequestV2Dto data,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            String _Response = String.Empty;
            ContractUpdateV2UnitOfWork uow = new ContractUpdateV2UnitOfWork(data);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            _Response = uow.Result;
            return _Response;
        }

        public bool ContractsUpdateValidityCheck(Guid ContractId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            bool _Response = false;
            ContractsUpdateValidityCheckUnitOfWork uow = new ContractsUpdateValidityCheckUnitOfWork(ContractId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            _Response = uow.Result;
            return _Response;
        }

        public bool UpdateContractStatus(Guid ContractId,
            bool Status,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            bool _Response = false;
            UpdateContractStatusUnitOfWork uow = new UpdateContractStatusUnitOfWork(ContractId, Status);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            _Response = uow.Result;
            return _Response;
        }
        #endregion

        #region Taxes
        public ContractTaxesesResponseDto GetContractTaxess(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ContractTaxesesResponseDto result = null;

            ContractTaxessRetrievalUnitOfWork uow = new ContractTaxessRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public ContractTaxesRequestDto AddContractTaxes(ContractTaxesRequestDto ContractTaxes,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ContractTaxesRequestDto result = new ContractTaxesRequestDto();
            ContractTaxesInsertionUnitOfWork uow = new ContractTaxesInsertionUnitOfWork(ContractTaxes);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.ContractTaxesInsertion = uow.ContractTaxes.ContractTaxesInsertion;
            return result;
        }

        public ContractTaxesResponseDto GetContractTaxesById(Guid ContractTaxesId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ContractTaxesResponseDto result = new ContractTaxesResponseDto();

            ContractTaxesRetrievalUnitOfWork uow = new ContractTaxesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.ContractTaxesId = ContractTaxesId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public ContractTaxesRequestDto UpdateContractTaxes(ContractTaxesRequestDto ContractTaxes,
            SecurityContext securityContext,
           AuditContext auditContext)
        {
            ContractTaxesRequestDto result = new ContractTaxesRequestDto();
            ContractTaxesUpdationUnitOfWork uow = new ContractTaxesUpdationUnitOfWork(ContractTaxes);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.ContractTaxesInsertion = uow.ContractTaxes.ContractTaxesInsertion;
            return result;
        }
        #endregion


        public object GetContractsForSearchGrid(ContractSearchGridRequestDto ContractSearchGridRequestDto,
             SecurityContext securityContext,
             AuditContext auditContext)
        {
            ContractsForSearchGridRetrievalUnotOfWork uow = new ContractsForSearchGridRetrievalUnotOfWork(ContractSearchGridRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public ContractViewData GetFullContractDetailsById(Guid ContractId, Guid variantId, Guid modelId, bool withTax, SecurityContext securityContext, AuditContext auditContext)
        {
            FullContractDetailsByIdRetrevialUnitOfWork uow = new FullContractDetailsByIdRetrevialUnitOfWork(ContractId, variantId, modelId, withTax);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetContractTaxesByExtensionId(Guid contractId, decimal PremiumForTax, SecurityContext securityContext, AuditContext auditContext)
        {
            ContractTaxesRetrievalByContractIdUnitOfWork uow = new ContractTaxesRetrievalByContractIdUnitOfWork(contractId, PremiumForTax);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }


        public object GetAllInsuaranceLimitaionsByCommodityType(Guid commodityTypeId, string productType, SecurityContext securityContext,
            AuditContext auditContext)
        {
            InsuaranceLimitaionsRetirevalUnitOfWork uow = new InsuaranceLimitaionsRetirevalUnitOfWork(commodityTypeId, productType);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object AddNewInsuaranceLimitation(InsuaranceLimitationRequestDto insuaranceLimitation,
            SecurityContext securityContext, AuditContext auditContext)
        {
            AddNewInsuaranceLimitaionUnitOfWork uow = new AddNewInsuaranceLimitaionUnitOfWork(insuaranceLimitation);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public GetContractDetailsByContractIdDto GetFullContractDetailsByIdV2(Guid contractId, SecurityContext securityContext,
            AuditContext auditContext)
        {
            GetContractDetailsByContractIdUnitOfWork uow = new GetContractDetailsByContractIdUnitOfWork(contractId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetAllAttributeSpecificationsByInsuranceLimitataionId(Guid insuranceLimitationId, Guid contractId,
            SecurityContext securityContext, AuditContext auditContext)
        {
            GetAllAttributeSpecificationsByInsuranceUnitOfWork uow = new GetAllAttributeSpecificationsByInsuranceUnitOfWork(insuranceLimitationId, contractId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetAllMakeModelDetailsByExtensionId(Guid extensionId, SecurityContext securityContext,
            AuditContext auditContext)
        {
            GetAllMakeModelDetailsByExtensionIdUnitOfWork uow = new GetAllMakeModelDetailsByExtensionIdUnitOfWork(extensionId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetAllPremiumByExtensionId(Guid extensionId, SecurityContext securityContext, AuditContext auditContext)
        {
            GetAllPremiumsByExtensionIdUnitOfWork uow = new GetAllPremiumsByExtensionIdUnitOfWork(extensionId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetAllExtensionTypeByContractId(Guid contractId, Guid ProductId, Guid DealerId, DateTime Date,
            Guid CylinderCountId, Guid EngineCapacityId, Guid MakeId, Guid ModelId, Guid VariantId, decimal GrossWeight, SecurityContext securityContext,
            AuditContext auditContext)
        {
            GetAllExtensionTypeByContractIdUnitOfWork uow = new GetAllExtensionTypeByContractIdUnitOfWork(contractId, ProductId, DealerId, Date,
                CylinderCountId, EngineCapacityId, MakeId, ModelId, VariantId, GrossWeight);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }
        public object GetAllExtensionTypeByMakeModel(Guid DealerId,Guid CylinderCountId, Guid EngineCapacityId, Guid MakeId, Guid ModelId, SecurityContext securityContext,
            AuditContext auditContext)
        {
            GetAllExtensionTypeByMakeModelUnitOfWork uow = new GetAllExtensionTypeByMakeModelUnitOfWork(DealerId,CylinderCountId, EngineCapacityId, MakeId, ModelId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }


        public object GetAttributeSpecificationByExtensionId(Guid extensionId, Guid contractId, Guid ProductId, Guid DealerId, DateTime Date,
            Guid CylinderCountId, Guid EngineCapacityId, Guid MakeId, Guid ModelId, Guid VariantId, decimal GrossWeight, SecurityContext securityContext,
            AuditContext auditContext)
        {
            GetAttributeSpecificationByExtensionIdUnitOfWork uow = new GetAttributeSpecificationByExtensionIdUnitOfWork(extensionId,contractId, ProductId, DealerId, Date,
                CylinderCountId, EngineCapacityId, MakeId, ModelId, VariantId, GrossWeight);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }


        public object GetCoverTypesByExtensionId(Guid attributeSpecificationId, Guid extensionId, Guid contractId,
            Guid productId, Guid dealerId, DateTime date, Guid cylinderCountId, Guid engineCapacityId, Guid makeId,
            Guid modelId, Guid variantId, decimal grossWeight, Guid itemStatusId, SecurityContext securityContext, AuditContext auditContext)
        {
            GetGetCoverTypesByExtensionIdUnitOfWork uow = new GetGetCoverTypesByExtensionIdUnitOfWork(attributeSpecificationId,extensionId, contractId,
                productId, dealerId, date,
                cylinderCountId, engineCapacityId, makeId, modelId, variantId, grossWeight, itemStatusId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetCoverTypesByAttributeId(Guid attributeSpecId, SecurityContext securityContext,
            AuditContext auditContext)
        {
            GetCoverTypesByAttributeIdUnitOfWork uow = new GetCoverTypesByAttributeIdUnitOfWork(attributeSpecId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetPremium(Guid contractPremiumId, decimal usage, Guid attributeSpecificationId, Guid extensionId,
            Guid contractId, Guid productId, Guid dealerId, DateTime date, Guid cylinderCountId, Guid engineCapacityId,
            Guid makeId, Guid modelId, Guid variantId, decimal grossWeight,Guid ItemStatusId,decimal DealerPrice,DateTime ItemPurchasedDate, SecurityContext securityContext,
            AuditContext auditContext)
        {
            GetPremiumUnitOfWork uow = new GetPremiumUnitOfWork(contractPremiumId, usage, attributeSpecificationId,
                extensionId, contractId, productId, dealerId, date,
                cylinderCountId, engineCapacityId, makeId, modelId, variantId, grossWeight, ItemStatusId, DealerPrice, ItemPurchasedDate);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public ContractInsuaranceLimitationResponseDto GetContractInsuaranceLimitations(Guid ContractId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ContractInsuaranceLimitationResponseDto result = null;

            GetContractInsuaranceLimitationByContarctIdUnitOfWork uow = new GetContractInsuaranceLimitationByContarctIdUnitOfWork(ContractId);

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public InsuaranceLimitationsResponseDto GetInsuaranceLimitations(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            InsuaranceLimitationsResponseDto result = null;

            InsuaranceLimitationsRetrievalUnitOfWork uow = new InsuaranceLimitationsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public InsuaranceLimitationResponseDto GetInsuaranceLimitationByContractInsuaranceLimitationId(Guid ContractInsuaranceLimitationId,
                        SecurityContext securityContext,
                        AuditContext auditContext)
        {
            InsuaranceLimitationResponseDto result = new InsuaranceLimitationResponseDto();

            InsuaranceLimitationDetailsRetrievalByContractInsuaranceLimitationIdUnitOfWork uow = new InsuaranceLimitationDetailsRetrievalByContractInsuaranceLimitationIdUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.ContractInsuaranceLimitationId = ContractInsuaranceLimitationId;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

    }

    //internal class GetGetCoverTypesByExtensionIdUnitOfWork
    //{
    //    public GetGetCoverTypesByExtensionIdUnitOfWork(Guid attributeSpecificationId, Guid extensionId, Guid contractId, Guid productId, Guid dealerId, DateTime date, Guid cylinderCountId, Guid engineCapacityId, Guid makeId, Guid modelId, Guid variantId, decimal grossWeight)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
