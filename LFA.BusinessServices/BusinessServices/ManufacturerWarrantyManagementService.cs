using TAS.Services.UnitsOfWork;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.BusinessServices
{
    internal sealed class ManufacturerWarrantyManagementService : IManufacturerWarrantyManagementService
    {

        public ManufacturerWarrantiesResponseDto GetManufacturerWarranties(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            ManufacturerWarrantiesResponseDto result = null;

            ManufacturerWarrantiesRetrievalUnitOfWork uow = new ManufacturerWarrantiesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public ManufacturerWarrantyRequestDto AddManufacturerWarranty(ManufacturerWarrantyRequestDto ManufacturerWarranty, 
            SecurityContext securityContext,
            AuditContext auditContext) {
                ManufacturerWarrantyRequestDto result = new ManufacturerWarrantyRequestDto();
                ManufacturerWarrantyInsertionUnitOfWork uow = new ManufacturerWarrantyInsertionUnitOfWork(ManufacturerWarranty);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.ManufacturerWarrantyInsertion = uow.ManufacturerWarranty.ManufacturerWarrantyInsertion;
                return result;
        }

        public ManufacturerWarrantyResponseDto GetManufacturerWarrantyById(Guid ManufacturerWarrantyId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            ManufacturerWarrantyResponseDto result = new ManufacturerWarrantyResponseDto();

            ManufacturerWarrantyRetrievalUnitOfWork uow = new ManufacturerWarrantyRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.ManufacturerWarrantyId = ManufacturerWarrantyId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public ManufacturerWarrantyRequestDto UpdateManufacturerWarranty(ManufacturerWarrantyRequestDto ManufacturerWarranty, 
            SecurityContext securityContext,
           AuditContext auditContext)
        {
            ManufacturerWarrantyRequestDto result = new ManufacturerWarrantyRequestDto();
            ManufacturerWarrantyUpdationUnitOfWork uow = new ManufacturerWarrantyUpdationUnitOfWork(ManufacturerWarranty);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.ManufacturerWarrantyInsertion = uow.ManufacturerWarranty.ManufacturerWarrantyInsertion;
            return result;
        }

        public object SearchManufacturerWarrantySchemes(
            ManufacturerWarrentySearchRequestDto manufacturerWarrentySearchRequestDto, SecurityContext securityContext,
            AuditContext auditContext)
        {
            SearchManufactureWarrentyUnitOfWork uow = new SearchManufactureWarrentyUnitOfWork(manufacturerWarrentySearchRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public ManufacturerWarrantyResponseDto GetManufacturerDetailsByContractId(Guid ContractId, Guid ModelId, Guid MakeId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            ManufacturerWarrantyResponseDto result = new ManufacturerWarrantyResponseDto();

            ManufacturerWarrantyDetailsRetrievalUnitOfWork uow = new ManufacturerWarrantyDetailsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.ContractId = ContractId;
            uow.MakeId = MakeId;
            uow.ModelId = ModelId;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }
        public object GetManufacturerDetailsByCountryId(Guid CountryId, Guid ModelId, Guid MakeId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            object result = new List <ManufacturerWarrantyResponseDto>();

            ManufacturerWarrantyDetailsRetrievalUnitOfWork2 uow = new ManufacturerWarrantyDetailsRetrievalUnitOfWork2();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.CountryId = CountryId;
            uow.MakeId = MakeId;
            uow.ModelId = ModelId;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }
        
    }
}
