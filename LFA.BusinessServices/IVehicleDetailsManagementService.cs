using System;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
namespace TAS.Services
{
    public interface IVehicleDetailsManagementService
    {
        VehicleAllDetailsResponseDto GetVehicleAllDetails(
            SecurityContext securityContext,
            AuditContext auditContext);

        VehicleDetailsRequestDto AddVehicleDetails(VehicleDetailsRequestDto VehicleDetails,
            SecurityContext securityContext,
            AuditContext auditContext);


        VehicleDetailsResponseDto GetVehicleDetailsById(Guid VehicleDetailsId,
            SecurityContext securityContext,
            AuditContext auditContext);

        VehicleDetailsResponseDto GetVehicleDetailsByVin(string VinNo,
            SecurityContext securityContext,
            AuditContext auditContext);

        VehicleDetailsRequestDto UpdateVehicleDetails(VehicleDetailsRequestDto VehicleDetails,
            SecurityContext securityContext,
            AuditContext auditContext);



        object GetAllVehiclesForSearchGrid(VehicleSearchGridRequestDto VehicleSearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext);

        string ValidateDealerCurrency(Guid guid, SecurityContext securityContext, AuditContext auditContext);

        object GetAllVehiclesForSearchGridByDealerId(VehicleSearchGridRequestDto VehicleSearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext);
        bool CheckMoreThanOneVehicleByWinNo(string vinNo, SecurityContext securityContext, AuditContext auditContext);
    }
}
