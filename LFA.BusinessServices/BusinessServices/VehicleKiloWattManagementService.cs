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
    internal sealed class VehicleKiloWattManagementService : IVehicleKiloWattManagementService
    {

        public VehicleKiloWattsResponseDto GetVehicleKiloWatts(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            VehicleKiloWattsResponseDto result = null;

            VehicleKiloWattsRetrievalUnitOfWork uow = new VehicleKiloWattsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.ResultVehicleKiloWatt;

            return result;
        }

        public VehicleKiloWattRequestDto AddVehicleKiloWatt(VehicleKiloWattRequestDto VehicleKiloWatt, SecurityContext securityContext,
            AuditContext auditContext) {
                VehicleKiloWattRequestDto result = new VehicleKiloWattRequestDto();
                VehicleKiloWattInsertionUnitOfWork uow = new VehicleKiloWattInsertionUnitOfWork(VehicleKiloWatt);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.VehicleKiloWattInsertion = uow.VehicleKiloWatt.VehicleKiloWattInsertion;
                return result;
        }


        public VehicleKiloWattResponseDto GetVehicleKiloWattById(Guid VehicleKiloWattId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            VehicleKiloWattResponseDto result = new VehicleKiloWattResponseDto();

            VehicleKiloWattRetrievalUnitOfWork uow = new VehicleKiloWattRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.VehicleKiloWattId = VehicleKiloWattId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public VehicleKiloWattRequestDto UpdateVehicleKiloWatt(VehicleKiloWattRequestDto VehicleKiloWatt, SecurityContext securityContext,
           AuditContext auditContext)
        {
            VehicleKiloWattRequestDto result = new VehicleKiloWattRequestDto();
            VehicleKiloWattUpdationUnitOfWork uow = new VehicleKiloWattUpdationUnitOfWork(VehicleKiloWatt);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.VehicleKiloWattInsertion = uow.VehicleKiloWatt.VehicleKiloWattInsertion;
            return result;
        }

       
       
    }
}
