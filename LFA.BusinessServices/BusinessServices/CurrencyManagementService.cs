using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;

namespace TAS.Services.BusinessServices
{
    internal sealed class CurrencyManagementService : ICurrencyManagementService
    {
        public CurrenciesResponseDto GetAllCurrencies(
            SecurityContext securityContext, 
            AuditContext auditContext) 
        {
            CurrenciesResponseDto result = null;
            CurrencyRetrievalUnitOfWork uow = new CurrencyRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        
        }

        public CurrencyConversionsResponseDto GetCurrencyConversions(
           SecurityContext securityContext,
           AuditContext auditContext)
        {
            CurrencyConversionsResponseDto result = null;

            CurrencyConversionsRetrievalUnitOfWork uow = new CurrencyConversionsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public CurrencyConversionRequestDto AddCurrencyConversion(CurrencyConversionRequestDto CurrencyConversion, SecurityContext securityContext,
            AuditContext auditContext)
        {
            CurrencyConversionRequestDto result = new CurrencyConversionRequestDto();
            CurrencyConversionInsertionUnitOfWork uow = new CurrencyConversionInsertionUnitOfWork(CurrencyConversion);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.CurrencyConversionInsertion = uow.CurrencyConversion.CurrencyConversionInsertion;
            return result;
        }


        public CurrencyConversionResponseDto GetCurrencyConversionById(Guid CurrencyConversionId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            CurrencyConversionResponseDto result = new CurrencyConversionResponseDto();

            CurrencyConversionRetrievalUnitOfWork uow = new CurrencyConversionRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.CurrencyConversionId = CurrencyConversionId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public CurrencyConversionRequestDto UpdateCurrencyConversion(CurrencyConversionRequestDto CurrencyConversion, SecurityContext securityContext,
           AuditContext auditContext)
        {
            CurrencyConversionRequestDto result = new CurrencyConversionRequestDto();
            CurrencyConversionUpdationUnitOfWork uow = new CurrencyConversionUpdationUnitOfWork(CurrencyConversion);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.CurrencyConversionInsertion = uow.CurrencyConversion.CurrencyConversionInsertion;
            return result;
        }

        public CurrencyConversionPeriodsResponseDto GetCurrencyConversionPeriods(
         SecurityContext securityContext,
         AuditContext auditContext)
        {
            CurrencyConversionPeriodsResponseDto result = null;

            CurrencyConversionPeriodsRetrievalUnitOfWork uow = new CurrencyConversionPeriodsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public CurrencyConversionPeriodRequestDto AddCurrencyConversionPeriod(CurrencyConversionPeriodRequestDto CurrencyConversionPeriod, SecurityContext securityContext,
            AuditContext auditContext)
        {
            CurrencyConversionPeriodRequestDto result = new CurrencyConversionPeriodRequestDto();
            CurrencyConversionPeriodInsertionUnitOfWork uow = new CurrencyConversionPeriodInsertionUnitOfWork(CurrencyConversionPeriod);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.CurrencyConversionPeriodInsertion = uow.CurrencyConversionPeriod.CurrencyConversionPeriodInsertion;
            return result;
        }


        public CurrencyConversionPeriodResponseDto GetCurrencyConversionPeriodById(Guid CurrencyConversionPeriodId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            CurrencyConversionPeriodResponseDto result = new CurrencyConversionPeriodResponseDto();

            CurrencyConversionPeriodRetrievalUnitOfWork uow = new CurrencyConversionPeriodRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.CurrencyConversionPeriodId = CurrencyConversionPeriodId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public CurrencyConversionPeriodRequestDto UpdateCurrencyConversionPeriod(CurrencyConversionPeriodRequestDto CurrencyConversionPeriod, SecurityContext securityContext,
           AuditContext auditContext)
        {
            CurrencyConversionPeriodRequestDto result = new CurrencyConversionPeriodRequestDto();
            CurrencyConversionPeriodUpdationUnitOfWork uow = new CurrencyConversionPeriodUpdationUnitOfWork(CurrencyConversionPeriod);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.CurrencyConversionPeriodInsertion = uow.CurrencyConversionPeriod.CurrencyConversionPeriodInsertion;
            return result;
        }

        public CurrencyEmailsResponseDto GetCurrencyEmails(
           SecurityContext securityContext,
           AuditContext auditContext)
        {
            CurrencyEmailsResponseDto result = null;

            CurrencyEmailsRetrievalUnitOfWork uow = new CurrencyEmailsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public CurrencyEmailRequestDto AddCurrencyEmail(CurrencyEmailRequestDto CurrencyEmail, SecurityContext securityContext,
            AuditContext auditContext)
        {
            CurrencyEmailRequestDto result = new CurrencyEmailRequestDto();
            CurrencyEmailInsertionUnitOfWork uow = new CurrencyEmailInsertionUnitOfWork(CurrencyEmail);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.CurrencyEmailInsertion = uow.CurrencyEmail.CurrencyEmailInsertion;
            return result;
        }


        public CurrencyEmailResponseDto GetCurrencyEmailById(Guid CurrencyEmailId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            CurrencyEmailResponseDto result = new CurrencyEmailResponseDto();

            CurrencyEmailRetrievalUnitOfWork uow = new CurrencyEmailRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.CurrencyEmailId = CurrencyEmailId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public CurrencyEmailRequestDto UpdateCurrencyEmail(CurrencyEmailRequestDto CurrencyEmail, SecurityContext securityContext,
           AuditContext auditContext)
        {
            CurrencyEmailRequestDto result = new CurrencyEmailRequestDto();
            CurrencyEmailUpdationUnitOfWork uow = new CurrencyEmailUpdationUnitOfWork(CurrencyEmail);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.CurrencyEmailInsertion = uow.CurrencyEmail.CurrencyEmailInsertion;
            return result;
        }

        public string ValidateCurrencyPeriodOverlaps(CurrencyConversionPeriodRequestDto CurrencyConversionPeriodRequest, SecurityContext securityContext, AuditContext auditContext)
        {
            String Response = String.Empty;
            CurrencyPeriodOverlapsValidateUnitOfWork uow = new CurrencyPeriodOverlapsValidateUnitOfWork(CurrencyConversionPeriodRequest);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            Response = uow.Result;
            return Response;
        }


        public bool? CurrencyPeriodCheck(SecurityContext securityContext, AuditContext auditContext)
        {
            bool? Response = false;
            CurrencyPeriodCheckUnitOfWork uow = new CurrencyPeriodCheckUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            Response = uow.Result;
            return Response;
        }

        public bool? GetCurrencyRateAvailabilityByCurrencyId(Guid currencyId, SecurityContext securityContext, AuditContext auditContext)
        {
            bool? Response = false;
            CurrencyRateAvailabilityCheckUnitOfWork uow = new CurrencyRateAvailabilityCheckUnitOfWork(currencyId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            Response = uow.Result;
            return Response;
        }

        public object ConvertFromBaseCurrency(decimal value, Guid BaseCurrencyId, Guid BaseCurrencyPeriodId, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = false;
            ConvertFromBaseCurrencyUnitOfWork uow = new ConvertFromBaseCurrencyUnitOfWork(value, BaseCurrencyId, BaseCurrencyPeriodId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            Response = uow.Result;
            return Response;
        }

        public decimal GetConversionRate(Guid BaseCurrencyId, Guid BaseCurrencyPeriodId, SecurityContext securityContext, AuditContext auditContext)
        {
            decimal Response = (decimal)1;
            GetConversionRateUnitOfWork uow = new GetConversionRateUnitOfWork(BaseCurrencyId, BaseCurrencyPeriodId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            Response = uow.Result;
            return Response;
        }
    }
}
