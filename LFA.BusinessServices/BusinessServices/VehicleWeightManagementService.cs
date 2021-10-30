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
    internal sealed class VehicleWeightManagementService : IVehicleWeightManagementService
    {
        public VehicleWeightsResponseDto GetAllVehicleWeight(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            VehicleWeightsResponseDto result = null;
            VehicleWeightsRetrievalUnitOfWork uow = new VehicleWeightsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public object SubmitVehicleWeight(VehicleWeightRequestDto VehicleWeightData, SecurityContext securityContext, AuditContext auditContext)
        {
            object Response = string.Empty;
            SubmitVehicleWeightUnitOfWork uow = new SubmitVehicleWeightUnitOfWork(VehicleWeightData);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
                Response = uow.Result;
            }
            return Response;
        }

        public VehicleWeightResponseDto GetVehicleWeightById(Guid VehicleWeightId,SecurityContext securityContext,
            AuditContext auditContext)
        {
            VehicleWeightResponseDto result = new VehicleWeightResponseDto();

            VehicleWeightRetrievalUnitOfWork uow = new VehicleWeightRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.vehicleWeightId = VehicleWeightId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }
    }
}
