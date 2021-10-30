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
    internal sealed class TaxManagementService : ITaxManagementService
    {
        public TaxesResponseDto GetAllTaxes(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            TaxesResponseDto result = null;
            TaxesRetrievalUnitOfWork uow = new TaxesRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }

        public TaxRequestDto AddTax(TaxRequestDto Tax,
         SecurityContext securityContext,
         AuditContext auditContext)
        {
            TaxRequestDto result = new TaxRequestDto();
            TaxInsertionUnitOfWork uow = new TaxInsertionUnitOfWork(Tax);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.TaxInsertion = uow.Tax.TaxInsertion;
            return result;
        }

        public TaxResponseDto GetTaxById(Guid TaxId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            TaxResponseDto result = new TaxResponseDto();

            TaxRetrievalUnitOfWork uow = new TaxRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.TaxId = TaxId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public TaxRequestDto UpdateTax(TaxRequestDto Tax,
            SecurityContext securityContext,
           AuditContext auditContext)
        {
            TaxRequestDto result = new TaxRequestDto();
            TaxUpdationUnitOfWork uow = new TaxUpdationUnitOfWork(Tax);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.TaxInsertion = uow.Tax.TaxInsertion;
            return result;
        }

        public CountryTaxessResponseDto GetAllCountryTaxes(
          SecurityContext securityContext,
          AuditContext auditContext)
        {
            CountryTaxessResponseDto result = null;
            CountryTaxesTaxessRetrievalUnitOfWork uow = new CountryTaxesTaxessRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }

        public CountryTaxessResponseDto GetCountryTaxesByCountryId(
            Guid countryId,
          SecurityContext securityContext,
          AuditContext auditContext)
        {
            CountryTaxessResponseDto result = null;
            CountryTaxRetrievalByCountryUnitOfWork uow = new CountryTaxRetrievalByCountryUnitOfWork(countryId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }


        public CountryTaxesRequestDto AddCountryTaxes(CountryTaxesRequestDto CountryTaxes,
         SecurityContext securityContext,
         AuditContext auditContext)
        {
            CountryTaxesRequestDto result = new CountryTaxesRequestDto();
            CountryTaxesInsertionUnitOfWork uow = new CountryTaxesInsertionUnitOfWork(CountryTaxes);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.CountryTaxesInsertion = uow.CountryTaxes.CountryTaxesInsertion;
            return result;
        }

        public CountryTaxesResponseDto GetCountryTaxesById(Guid CountryTaxesId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            CountryTaxesResponseDto result = new CountryTaxesResponseDto();

            CountryTaxesRetrievalUnitOfWork uow = new CountryTaxesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.CountryTaxesId = CountryTaxesId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public CountryTaxesRequestDto UpdateCountryTaxes(CountryTaxesRequestDto CountryTaxes, bool delete,
            SecurityContext securityContext,
           AuditContext auditContext)
        {
            CountryTaxesRequestDto result = new CountryTaxesRequestDto();
            CountryTaxesUpdationUnitOfWork uow = new CountryTaxesUpdationUnitOfWork(CountryTaxes, delete);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.CountryTaxesInsertion = uow.CountryTaxes.CountryTaxesInsertion;
            return result;
        }

        public bool IsExistingTaxName(Guid Id, string taxName,
           SecurityContext securityContext,
           AuditContext auditContext)
        {
            bool Result = false;
            IsExixtingTaxCodeNameUnitOfWorks uow = new IsExixtingTaxCodeNameUnitOfWorks();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.TaxName = taxName;
            uow.TaxCode = "";
            uow.Id = Id;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            Result = uow.Result;
            return Result;
        }

        public bool IsExistingTaxCode(Guid Id, string taxCode,
           SecurityContext securityContext,
           AuditContext auditContext)
        {
            bool Result = false;
            IsExixtingTaxCodeNameUnitOfWorks uow = new IsExixtingTaxCodeNameUnitOfWorks();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.TaxCode = taxCode;
            uow.TaxName = "";
            uow.Id = Id;
            if (uow.PreExecute()) { uow.Execute(); }
            Result = uow.Result;
            return Result;
        }

        public ContractTaxesesResponseDto GetAllContactTaxes(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ContractTaxesesResponseDto result = null;
            ContactTaxesRetrievalUnitOfWork uow = new ContactTaxesRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }
    }
}
