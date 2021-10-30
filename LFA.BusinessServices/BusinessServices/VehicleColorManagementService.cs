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
    internal sealed class VehicleColorManagementService : IVehicleColorManagementService
    {

        public VehicleColorsResponseDto GetVehicleColors(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            VehicleColorsResponseDto result = null;

            VehicleColorsRetrievalUnitOfWork uow = new VehicleColorsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public VehicleColorsResponseDto GetParentVehicleColors(
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            VehicleColorsResponseDto result = null;

            VehicleColorsRetrievalUnitOfWork uow = new VehicleColorsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public VehicleColorRequestDto AddVehicleColor(VehicleColorRequestDto VehicleColor, SecurityContext securityContext,
            AuditContext auditContext) {
                VehicleColorRequestDto result = new VehicleColorRequestDto();
                VehicleColorInsertionUnitOfWork uow = new VehicleColorInsertionUnitOfWork(VehicleColor);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.VehicleColorInsertion = uow.VehicleColor.VehicleColorInsertion;
                return result;
        }


        public VehicleColorResponseDto GetVehicleColorById(Guid VehicleColorId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            VehicleColorResponseDto result = new VehicleColorResponseDto();

            VehicleColorRetrievalUnitOfWork uow = new VehicleColorRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.vehicleColorId = VehicleColorId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public VehicleColorRequestDto UpdateVehicleColor(VehicleColorRequestDto VehicleColor, SecurityContext securityContext,
           AuditContext auditContext)
        {
            VehicleColorRequestDto result = new VehicleColorRequestDto();
            VehicleColorUpdationUnitOfWork uow = new VehicleColorUpdationUnitOfWork(VehicleColor);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.VehicleColorInsertion = uow.VehicleColor.VehicleColorInsertion;
            return result;
        }

        
       
    }
}
