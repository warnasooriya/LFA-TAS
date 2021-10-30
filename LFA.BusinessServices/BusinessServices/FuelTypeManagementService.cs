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
    internal sealed class FuelTypeManagementService : IFuelTypeManagementService
    {

        public FuelTypesResponseDto GetFuelTypes(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            FuelTypesResponseDto result = null;

            FuelTypesRetrievalUnitOfWork uow = new FuelTypesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public FuelTypesResponseDto GetParentFuelTypes(
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            FuelTypesResponseDto result = null;

            FuelTypesRetrievalUnitOfWork uow = new FuelTypesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public FuelTypeRequestDto AddFuelType(FuelTypeRequestDto FuelType, SecurityContext securityContext,
            AuditContext auditContext) {
                FuelTypeRequestDto result = new FuelTypeRequestDto();
                FuelTypeInsertionUnitOfWork uow = new FuelTypeInsertionUnitOfWork(FuelType);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.FuelTypeInsertion = uow.FuelType.FuelTypeInsertion;
                return result;
        }


        public FuelTypeResponseDto GetFuelTypeById(Guid FuelTypeId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            FuelTypeResponseDto result = new FuelTypeResponseDto();

            FuelTypeRetrievalUnitOfWork uow = new FuelTypeRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.fuelTypeId = FuelTypeId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public FuelTypeRequestDto UpdateFuelType(FuelTypeRequestDto FuelType, SecurityContext securityContext,
           AuditContext auditContext)
        {
            FuelTypeRequestDto result = new FuelTypeRequestDto();
            FuelTypeUpdationUnitOfWork uow = new FuelTypeUpdationUnitOfWork(FuelType);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.FuelTypeInsertion = uow.FuelType.FuelTypeInsertion;
            return result;
        }


       
    }
}
