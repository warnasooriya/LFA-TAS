using System;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    internal sealed class DealerLocationManagementService : IDealerLocationManagementService
    {
        public DealerLocationsRespondDto GetAllDealerLocations(SecurityContext securityContext, AuditContext auditContext)
        {
            DealerLocationsRespondDto result = null;
            DealerLocationsRetrievalUnitOfWork uow = new DealerLocationsRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public object GetAllDealerLocationsByUser(Guid userId, SecurityContext securityContext, AuditContext auditContext)
        {
            object result = null;
            DealerLocationsRetrievalByUserUnitOfWork uow = new DealerLocationsRetrievalByUserUnitOfWork(userId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }



        //public DealerLocationsRespondDto GetAllDealerStaffLocationsByDealerId(SecurityContext securityContext, AuditContext auditContext)
        //{
        //    DealerLocationsRespondDto result = null;
        //    DealerStaffLocationsRetrievalUnitOfWork uow = new DealerStaffLocationsRetrievalUnitOfWork();
        //    uow.SecurityContext = securityContext;
        //    uow.AuditContext = auditContext;
        //    if (uow.PreExecute())
        //    {
        //        uow.Execute();
        //    }
        //    result = uow.Result;
        //    return result;
        //}


        public DealerLocationsRespondDto GetDealerLocations(SecurityContext securityContext, AuditContext auditContext)
        {
            DealerLocationsRespondDto result = null;
            DealerLocationsRetrievalUnitOfWork uow = new DealerLocationsRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public DealerLocationsRespondDto GetParentDealerLocations(SecurityContext securityContext, AuditContext auditContext)
        {
            DealerLocationsRespondDto result = null;
            DealerLocationsRetrievalUnitOfWork uow = new DealerLocationsRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public DealerLocationRequestDto AddDealerLocation(DealerLocationRequestDto DealerLocation, SecurityContext securityContext,
            AuditContext auditContext)
        {
            DealerLocationRequestDto result = new DealerLocationRequestDto();
            DealerLocationInsertionUnitOfWork uow = new DealerLocationInsertionUnitOfWork(DealerLocation);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.DealerLocationInsertion = uow.DealerLocation.DealerLocationInsertion;
            return result;
        }

        public DealerInvoicesGenerateResponseDto GenerateDealerInvoices(DealerInvoicesGenerateRequestDto DealerInvoicesGenerateRequest, SecurityContext securityContext, AuditContext auditContext)
        {

            DealerInvoicesGenerateResponseDto result = new DealerInvoicesGenerateResponseDto();
            DealerInvoicesGenerateUnitOfWork uow = new DealerInvoicesGenerateUnitOfWork(DealerInvoicesGenerateRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public DealerLocationRespondDto GetDealerLocationById(Guid DealerLocationId, SecurityContext securityContext, AuditContext auditContext)
        {
            DealerLocationRespondDto result = new DealerLocationRespondDto();
            DealerLocationRetrievalUnitOfWork uow = new DealerLocationRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.DealerLocationId = DealerLocationId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public DealerLocationRequestDto UpdateDealerLocation(DealerLocationRequestDto DealerLocation, SecurityContext securityContext,
           AuditContext auditContext)
        {
            DealerLocationRequestDto result = new DealerLocationRequestDto();
            DealerLocationUpdationUnitOfWork uow = new DealerLocationUpdationUnitOfWork(DealerLocation);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.DealerLocationInsertion = uow.DealerLocation.DealerLocationInsertion;
            return result;
        }


        public object GetAllDiscountSchemes(SecurityContext securityContext, AuditContext auditContext)
        {
            DealerDiscountSchemeRetrievalUnitOfWork uow = new DealerDiscountSchemeRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object SearchDealerDiscountSchemes(
            DealerDiscountSchemesSearchRequestDto dealerDiscountSchemesSearchRequest, SecurityContext securityContext,
            AuditContext auditContext)
        {
            SearchDealerDiscountSchemesUnitOfWork uow = new SearchDealerDiscountSchemesUnitOfWork(dealerDiscountSchemesSearchRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object SaveDealerDiscount(DealerDiscountSaveRequestDto dealerDiscountSaveRequest,
            SecurityContext securityContext, AuditContext auditContext)
        {
            SaveDealerDiscountUnitOfWork uow = new SaveDealerDiscountUnitOfWork(dealerDiscountSaveRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetDealerDiscountById(Guid dealerDiscountId, SecurityContext securityContext,
            AuditContext auditContext)
        {
            GetDealerDiscountByIdUnitOfWork uow = new GetDealerDiscountByIdUnitOfWork(dealerDiscountId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetAllDealerStaffLocationsByDealerId(Guid DealerId, SecurityContext securityContext, AuditContext auditContext)
        {
            DealerLocationRespondDto result = null;
            DealerStaffLocationsRetrievalUnitOfWork uow = new DealerStaffLocationsRetrievalUnitOfWork(DealerId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object AddDealerLabourCharge(DealerLabourChargeSaveRequestDto dealerLabourChargeSaveRequest,
            SecurityContext securityContext, AuditContext auditContext)
        {
            SaveDealerLabourChargeUnitOfWork uow = new SaveDealerLabourChargeUnitOfWork(dealerLabourChargeSaveRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object SearchDealerLabourChargeSchemes(DealerLabourChargeSearchRequestDto dealerLabourChargeSchemesSearchRequest,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            SearchDealerLabourChargeSchemesUnitOfWork uow = new SearchDealerLabourChargeSchemesUnitOfWork(dealerLabourChargeSchemesSearchRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetDealerLabourChargeById(Guid dealerLabourChargeId, SecurityContext securityContext,
           AuditContext auditContext)
        {
            GetDealerLabourChargeByIdUnitOfWork uow = new GetDealerLabourChargeByIdUnitOfWork(dealerLabourChargeId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }
        public MakesResponseDto GetAllMakesByDealerId(Guid dealerId, SecurityContext securityContext,
           AuditContext auditContext)
        {
            GetMakesBytDealerIdUnitOfWork uow = new GetMakesBytDealerIdUnitOfWork(dealerId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object ValidateUserOnDealerInvoiceCodeGeneration(Guid userId, SecurityContext securityContext, AuditContext auditContext)
        {

            ValidateUserOnDealerInvoiceCodeGeneration uow = new ValidateUserOnDealerInvoiceCodeGeneration(userId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetTyreDetailsByArticleNo(string articleNo, SecurityContext securityContext, AuditContext auditContext)
        {

            GetTyreDetailsByArticleNoUnitOfWork uow = new GetTyreDetailsByArticleNoUnitOfWork(articleNo)
            {
                SecurityContext = securityContext,
                AuditContext = auditContext
            };

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GenerateInvoiceCode(GenerateInvoiceCodeRequestDto generateInvoiceCodeRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            GenerateInvoiceCodeUnitOfWork uow = new GenerateInvoiceCodeUnitOfWork(generateInvoiceCodeRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object SaveTyrePolicyDetails(SaveTyrePolicySalesRequestDto generateInvoiceCodeRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            SaveTyrePolicyUnitOfWork uow = new SaveTyrePolicyUnitOfWork(generateInvoiceCodeRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetTyreContractDetails(TyreContractRequestDto tyreContractRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {
            TyreContractLoadingUnitOfWork uow = new TyreContractLoadingUnitOfWork(tyreContractRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }


        public object SearchDealerInvoiceCode(DealerInvoiceCodeSearchRequestDto invoiceCodeSearchRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            SearchDealerInvoiceCodeUnitOfWork uow = new SearchDealerInvoiceCodeUnitOfWork(invoiceCodeSearchRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object LoadInvoceCodeDetailsById(LoadInvoceCodeByIdRequestDto invoiceLoadByIdRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            LoadInvoceCodeDetailsByIdUnitOfWork uow = new LoadInvoceCodeDetailsByIdUnitOfWork(invoiceLoadByIdRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetAllAvailabelTireSizes(SecurityContext securityContext, AuditContext auditContext)
        {
            GetAllAvailableTireSizesUnitOfWork uow = new GetAllAvailableTireSizesUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetConfirmedBordxByYearAndMonth(int year, int month, SecurityContext securityContext,
            AuditContext auditContext)
        {
            GetAllConfirmedBordxByYearAndMonthUnitOfWork uow = new GetAllConfirmedBordxByYearAndMonthUnitOfWork(year, month);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object DownloadInvoiceSummary(Guid dealerId, Guid bordxId, SecurityContext securityContext, AuditContext auditContext)
        {
            GetInvoiceSummaryDataIdUnitOfWork uow = new GetInvoiceSummaryDataIdUnitOfWork(dealerId, bordxId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetAllAvailabelTireSizesByWidth(LoadTyreDetailsByWidthRequestDto LoadTyreDetailsByWidthRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {
            LoadTyreDetailsByWithUnitOfWork uow = new LoadTyreDetailsByWithUnitOfWork(LoadTyreDetailsByWidthRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }
        public object GetAllAvailabelTireSizesByDiameter(String cross,String Width, SecurityContext securityContext, AuditContext auditContext)
        {
            LoadTyreDetailsByCrossUnitOfWork uow = new LoadTyreDetailsByCrossUnitOfWork(cross, Width);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetAllAvailabelTireSizesByloadSpeed(String cross, String Width,String Diameter, SecurityContext securityContext, AuditContext auditContext)
        {
            LoadTyreDetailsByLoadSpeedUnitOfWork uow = new LoadTyreDetailsByLoadSpeedUnitOfWork(cross, Width, Diameter);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetAllAvailabelTireSizesByPattern(String cross, String Width, String Diameter, string LoadSpeed, SecurityContext securityContext, AuditContext auditContext)
        {
            LoadTyreDetailsByPatternUnitOfWork uow = new LoadTyreDetailsByPatternUnitOfWork(cross, Width, Diameter, LoadSpeed);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetArticleNoByTyreSize(String width, String cross, String diameter, String loadSpeed, String pattern, SecurityContext securityContext, AuditContext auditContext)
        {
            GetArticleNoByTyreSizeUnitOfWork uow = new GetArticleNoByTyreSizeUnitOfWork(width, cross, diameter, loadSpeed, pattern);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }
    }
}
