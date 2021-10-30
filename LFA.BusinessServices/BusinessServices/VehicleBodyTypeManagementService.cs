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
    internal sealed class VehicleBodyTypeManagementService : IVehicleBodyTypeManagementService
    {

        public VehicleBodyTypesResponseDto GetVehicleBodyTypes(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            VehicleBodyTypesResponseDto result = null;

            VehicleBodyTypesRetrievalUnitOfWork uow = new VehicleBodyTypesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public VehicleBodyTypesResponseDto GetParentVehicleBodyTypes(
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            VehicleBodyTypesResponseDto result = null;

            VehicleBodyTypesRetrievalUnitOfWork uow = new VehicleBodyTypesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public VehicleBodyTypeRequestDto AddVehicleBodyType(VehicleBodyTypeRequestDto VehicleBodyType, SecurityContext securityContext,
            AuditContext auditContext) {
                VehicleBodyTypeRequestDto result = new VehicleBodyTypeRequestDto();
                VehicleBodyTypeInsertionUnitOfWork uow = new VehicleBodyTypeInsertionUnitOfWork(VehicleBodyType);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.VehicleBodyTypeInsertion = uow.VehicleBodyType.VehicleBodyTypeInsertion;
                return result;
        }


        public VehicleBodyTypeResponseDto GetVehicleBodyTypeById(Guid VehicleBodyTypeId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            VehicleBodyTypeResponseDto result = new VehicleBodyTypeResponseDto();

            VehicleBodyTypeRetrievalUnitOfWork uow = new VehicleBodyTypeRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.vehicleBodyTypeId = VehicleBodyTypeId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public VehicleBodyTypeRequestDto UpdateVehicleBodyType(VehicleBodyTypeRequestDto VehicleBodyType, SecurityContext securityContext,
           AuditContext auditContext)
        {
            VehicleBodyTypeRequestDto result = new VehicleBodyTypeRequestDto();
            VehicleBodyTypeUpdationUnitOfWork uow = new VehicleBodyTypeUpdationUnitOfWork(VehicleBodyType);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.VehicleBodyTypeInsertion = uow.VehicleBodyType.VehicleBodyTypeInsertion;
            return result;
        }

        
       
    }
}
