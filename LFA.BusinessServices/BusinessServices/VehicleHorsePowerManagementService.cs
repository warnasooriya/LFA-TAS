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
    internal sealed class VehicleHorsePowerManagementService : IVehicleHorsePowerManagementService
    {

        public VehicleHorsePowersResponseDto GetVehicleHorsePowers(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            VehicleHorsePowersResponseDto result = null;

            VehicleHorsePowersRetrievalUnitOfWork uow = new VehicleHorsePowersRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.ResultVehicleHorsePower;

            return result;
        }

        public VehicleHorsePowerRequestDto AddVehicleHorsePower(VehicleHorsePowerRequestDto VehicleHorsePower, SecurityContext securityContext,
            AuditContext auditContext) {
                VehicleHorsePowerRequestDto result = new VehicleHorsePowerRequestDto();
                VehicleHorsePowerInsertionUnitOfWork uow = new VehicleHorsePowerInsertionUnitOfWork(VehicleHorsePower);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.VehicleHorsePowerInsertion = uow.VehicleHorsePower.VehicleHorsePowerInsertion;
                return result;
        }


        public VehicleHorsePowerResponseDto GetVehicleHorsePowerById(Guid VehicleHorsePowerId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            VehicleHorsePowerResponseDto result = new VehicleHorsePowerResponseDto();

            VehicleHorsePowerRetrievalUnitOfWork uow = new VehicleHorsePowerRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.VehicleHorsePowerId = VehicleHorsePowerId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public VehicleHorsePowerRequestDto UpdateVehicleHorsePower(VehicleHorsePowerRequestDto VehicleHorsePower, SecurityContext securityContext,
           AuditContext auditContext)
        {
            VehicleHorsePowerRequestDto result = new VehicleHorsePowerRequestDto();
            VehicleHorsePowerUpdationUnitOfWork uow = new VehicleHorsePowerUpdationUnitOfWork(VehicleHorsePower);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.VehicleHorsePowerInsertion = uow.VehicleHorsePower.VehicleHorsePowerInsertion;
            return result;
        }

       
       
    }
}
