using System;
using System.Collections.Generic;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    internal sealed class BordxManagementService : IBordxManagementService
    {
        public BordxsResponseDto GetBordxs(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            BordxsResponseDto result = null;

            BordxsRetrievalUnitOfWork uow = new BordxsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public BordxRequestDto AddBordx(BordxRequestDto Bordx,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            BordxRequestDto result = new BordxRequestDto();
            BordxInsertionUnitOfWork uow = new BordxInsertionUnitOfWork(Bordx);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = Bordx;
            result.BordxInsertion = uow.Bordx.BordxInsertion;
            return result;
        }

        public BordxResponseDto GetBordxById(Guid BordxId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            BordxResponseDto result = new BordxResponseDto();

            BordxRetrievalUnitOfWork uow = new BordxRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.BordxId = BordxId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public BordxRequestDto UpdateBordx(BordxRequestDto Bordx,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            BordxRequestDto result = new BordxRequestDto();
            BordxUpdationUnitOfWork uow = new BordxUpdationUnitOfWork(Bordx);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.BordxInsertion = uow.Bordx.BordxInsertion;
            return result;
        }

        public BordxsDetailsResponseDto GetBordxDetails(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            BordxsDetailsResponseDto result = null;

            BordxsDetailsRetrievalUnitOfWork uow = new BordxsDetailsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public BordxDetailsRequestDto AddBordxDetails(BordxDetailsRequestDto Bordx,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            BordxDetailsRequestDto result = new BordxDetailsRequestDto();
            BordxDetailsInsertionUnitOfWork uow = new BordxDetailsInsertionUnitOfWork(Bordx);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.BordxDetailsInsertion = uow.Bordx.BordxDetailsInsertion;
            return result;
        }

        public BordxDetailsResponseDto GetBordxDetailsById(Guid BordxId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            BordxDetailsResponseDto result = new BordxDetailsResponseDto();

            BordxDetailsRetrievalUnitOfWork uow = new BordxDetailsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.BordxId = BordxId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public BordxDetailsRequestDto UpdateBordxDetails(BordxDetailsRequestDto Bordx,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            BordxDetailsRequestDto result = new BordxDetailsRequestDto();
            BordxDetailsUpdationUnitOfWork uow = new BordxDetailsUpdationUnitOfWork(Bordx);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.BordxDetailsInsertion = uow.Bordx.BordxDetailsInsertion;
            return result;
        }

        public object GetBordxReportColumns(BordxColumnRequestDto bordxColumnRequestDto,
           SecurityContext securityContext,
           AuditContext auditContext)
        {
            object result = null;

            BordxReportColumnsesRetrievalUnitOfWork uow = new BordxReportColumnsesRetrievalUnitOfWork(bordxColumnRequestDto);

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public BordxReportColumnsMapsResponseDto GetBordxReportColumnsMaps(Guid UserId,
          SecurityContext securityContext,
          AuditContext auditContext)
        {
            BordxReportColumnsMapsResponseDto result = null;
            BordxReportColumnsMapsRetrievalUnitOfWork uow = new BordxReportColumnsMapsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.UserId = UserId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public BordxReportColumnsMapRequestDto AddBordxReportColumnsMap(BordxReportColumnsMapRequestDto Bordx,
          SecurityContext securityContext,
          AuditContext auditContext)
        {
            BordxReportColumnsMapRequestDto result = new BordxReportColumnsMapRequestDto();
            BordxReportColumnsMapInsertionUnitOfWork uow = new BordxReportColumnsMapInsertionUnitOfWork(Bordx);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = Bordx;
            result.BordxReportColumnsMapInsertion = uow.Bordx.BordxReportColumnsMapInsertion;
            return result;
        }

        public BordxReportColumnsMapResponseDto GetBordxReportColumnsMapById(Guid BordxId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            BordxReportColumnsMapResponseDto result = new BordxReportColumnsMapResponseDto();

            BordxReportColumnsMapRetrievalUnitOfWork uow = new BordxReportColumnsMapRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.BordxId = BordxId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public BordxReportColumnsMapRequestDto UpdateBordxReportColumnsMap(BordxReportColumnsMapRequestDto Bordx,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            BordxReportColumnsMapRequestDto result = new BordxReportColumnsMapRequestDto();
            BordxReportColumnsMapUpdationUnitOfWork uow = new BordxReportColumnsMapUpdationUnitOfWork(Bordx);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.BordxReportColumnsMapInsertion = uow.Bordx.BordxReportColumnsMapInsertion;
            return result;
        }

        public object GetConfirmedBordxForGrid(ConfirmedBordxForGridRequestDto ConfirmedBordxForGridRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            ConfirmedBordxForGridResponseDto ConfirmedBordxForGridResponseDto = new ConfirmedBordxForGridResponseDto();
            ConfirmedBordxForGridRetrievalUnitOfWork uow = new ConfirmedBordxForGridRetrievalUnitOfWork(ConfirmedBordxForGridRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object ConfirmedBordxYears(SecurityContext securityContext, AuditContext auditContext)
        {
            ConfirmedBordxYearsUnitOfWork uow = new ConfirmedBordxYearsUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Results;

        }

        public object getAllBordxAllowedYearsMonths(Guid InsurerId,Guid ReinsurerId, Guid CommodityTypeId, SecurityContext securityContext, AuditContext auditContext)
        {
            BordexAllowedYearsMonthsRetrievalUnotOfWork uow = new BordexAllowedYearsMonthsRetrievalUnotOfWork(InsurerId, ReinsurerId, CommodityTypeId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object getBordxNumbersYearsAndMonth(string bordxYear, string bordxMonth, SecurityContext securityContext, AuditContext auditContext)
        {
            BordexNumbersYearsMonthsRetrievalUnotOfWork uow = new BordexNumbersYearsMonthsRetrievalUnotOfWork(bordxYear, bordxMonth);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetAllBordxDetailsByYearMonth(BordxDetailsByYearMonthRequestDto bordxRequestDetails, SecurityContext securityContext, AuditContext auditContext)
        {
            BordxDetailsRetrivalByYearMonthUnitOfWork uow = new BordxDetailsRetrivalByYearMonthUnitOfWork(bordxRequestDetails);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public string ProcessBordx(BordxProcessRequestDto bordxProcessRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            ProcessBordxUnitOfWork uow = new ProcessBordxUnitOfWork(bordxProcessRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public string ConfirmBordx(BordxProcessRequestDto bordxProcessRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            ConfirmBordxUnitOfWork uow = new ConfirmBordxUnitOfWork(bordxProcessRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public string CreateBordx(BordxCreateRequestDto bordxCreateRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            CreateBordxUnitOfWork uow = new CreateBordxUnitOfWork(bordxCreateRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetLast10Bordx( BordxListRequestDto bordxListRequestDto,SecurityContext securityContext, AuditContext auditContext)
        {
            GetLast10BordxUnitOfWork uow = new GetLast10BordxUnitOfWork(bordxListRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;

        }

        public string GetNextBordxNumber(int year, int month, Guid reinsurerId,Guid insurerId, Guid productId, Guid CommodityTypeId, SecurityContext securityContext, AuditContext auditContext)
        {
            GetNextBordxNumberUnitOfWork uow = new GetNextBordxNumberUnitOfWork(year, month, reinsurerId, insurerId,productId, CommodityTypeId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public string DeleteBordx(Guid BordxId, SecurityContext securityContext, AuditContext auditContext)
        {
            DeleteBordxUnitOfWork uow = new DeleteBordxUnitOfWork(BordxId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetNextBordxNumbers(Guid commodityTypeId, Guid reinsurerId, Guid insurerId, Guid productId, int year, int month, SecurityContext securityContext, AuditContext auditContext)
        {
            GetBordxNumbersUnitOfWork uow = new GetBordxNumbersUnitOfWork(year, month, commodityTypeId, reinsurerId, insurerId, productId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public string TransferPolicyToBordx(BordxTransferRequestDto BordxTransferRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {

            TransferPolicyToBordxUnitOfWork uow = new TransferPolicyToBordxUnitOfWork(BordxTransferRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;

        }

        public string BordxReopen(BordxReopenRequestDto BordxReopenRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {
            BordxReopenUnitOfWork uow = new BordxReopenUnitOfWork(BordxReopenRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public ValidateContractWithTaxResponseDto ValidateContractWithTax(Guid countryTaxId, Guid ContractId, Guid PolicId, SecurityContext securityContext, AuditContext auditContext)
        {
            ContractWithTaxValidityUnitOfWork uow = new ContractWithTaxValidityUnitOfWork(countryTaxId, ContractId, PolicId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetAllBordxReportTemplateForSearchGrid(
            BordxReportTemplateSearchGridRequestDto bordxReportTemplateSearchGridRequestDto,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            BordxReportTemplateRetrievalForSearchGridUnitOfWork uow = new BordxReportTemplateRetrievalForSearchGridUnitOfWork(bordxReportTemplateSearchGridRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }

            return uow.result;
        }

        public BordxReportTemplateResponseDto GetBordxReportTemplateById(Guid bordxReportTemplateId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            BordxReportTemplateRetrieveByIdUnitOfWork uow = new BordxReportTemplateRetrieveByIdUnitOfWork(bordxReportTemplateId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.result;
        }

        public bool BordxReportTemplateNameIsExists(BordxReportTemplateRequestDto bordxReportTemplate,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            BordxReportTemplateNameIsExistsUnitOfWork uow = new BordxReportTemplateNameIsExistsUnitOfWork(bordxReportTemplate);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.result;
        }

        public bool SaveBordxReportTemplate(BordxReportTemplateRequestDto bordxReportTemplate,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            BordxReportTemplateInsertionUnitOfWork uow = new BordxReportTemplateInsertionUnitOfWork(bordxReportTemplate);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.result;
        }

        public object GetBordxReportTemplateColumns(BordxColumnRequestDto bordxColumnRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {
            object result = null;

            BordxReportTemplateColumnsRetrievalUnitOfWork uow = new BordxReportTemplateColumnsRetrievalUnitOfWork(bordxColumnRequestDto);

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public List<BordxReportTemplateResponseDto> GetBordxReportTemplates(BordxTemplateRequestDto bordxTemplateRequestDto , SecurityContext securityContext, AuditContext auditContext)
        {
            BordxReportTemplateRetrievalUnitOfWork uow = new BordxReportTemplateRetrievalUnitOfWork(bordxTemplateRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.result;
        }
    }
}
