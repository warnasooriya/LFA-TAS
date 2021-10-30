using System;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    internal sealed class VehicleDetailsManagementService : IVehicleDetailsManagementService
    {

        public VehicleAllDetailsResponseDto GetVehicleAllDetails(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            VehicleAllDetailsResponseDto result = null;

            VehicleDetailssRetrievalUnitOfWork uow = new VehicleDetailssRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public VehicleAllDetailsResponseDto GetParentVehicleDetailss(
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            VehicleAllDetailsResponseDto result = null;

            VehicleDetailssRetrievalUnitOfWork uow = new VehicleDetailssRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public VehicleDetailsRequestDto AddVehicleDetails(VehicleDetailsRequestDto VehicleDetails, SecurityContext securityContext,
            AuditContext auditContext)
        {
            VehicleDetailsRequestDto result = new VehicleDetailsRequestDto();
            VehicleDetailsInsertionUnitOfWork uow = new VehicleDetailsInsertionUnitOfWork(VehicleDetails);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.VehicleDetails;
            return result;
        }


        public VehicleDetailsResponseDto GetVehicleDetailsById(Guid VehicleDetailsId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            VehicleDetailsResponseDto result = new VehicleDetailsResponseDto();

            VehicleDetailsRetrievalUnitOfWork uow = new VehicleDetailsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.VehicleDetailsId = VehicleDetailsId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public VehicleDetailsResponseDto GetVehicleDetailsByVin(string VinNo,
   SecurityContext securityContext,
   AuditContext auditContext)
        {
            VehicleDetailsResponseDto result = new VehicleDetailsResponseDto();

            VehicleDetailsRetrievalByVinUnitOfWork uow = new VehicleDetailsRetrievalByVinUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.VinNo = VinNo;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }


        public VehicleDetailsRequestDto UpdateVehicleDetails(VehicleDetailsRequestDto VehicleDetails, SecurityContext securityContext,
           AuditContext auditContext)
        {
            VehicleDetailsRequestDto result = new VehicleDetailsRequestDto();
            VehicleDetailsUpdationUnitOfWork uow = new VehicleDetailsUpdationUnitOfWork(VehicleDetails);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.VehicleDetails;
            return result;
        }

        public object GetAllVehiclesForSearchGrid(VehicleSearchGridRequestDto VehicleSearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {
            VehiclesRetrievalForSearchGridUnitOfWork uow = new VehiclesRetrievalForSearchGridUnitOfWork(VehicleSearchGridRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object GetAllVehiclesForSearchGridByDealerId(VehicleSearchGridRequestDto VehicleSearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {
            VehiclesRetrievalForSearchGridByDealerIdUnitOfWork uow = new VehiclesRetrievalForSearchGridByDealerIdUnitOfWork(VehicleSearchGridRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }




        public string ValidateDealerCurrency(Guid dealerId, SecurityContext securityContext, AuditContext auditContext)
        {
            DealerCurrencyValidationUnitOfWork uow = new DealerCurrencyValidationUnitOfWork(dealerId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public bool CheckMoreThanOneVehicleByWinNo(string VinNo,SecurityContext securityContext,AuditContext auditContext)
        {
            bool result = false;

            VehicleDetailsValidateByVinUnitOfWork uow = new VehicleDetailsValidateByVinUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.VinNo = VinNo;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }
    }
}
