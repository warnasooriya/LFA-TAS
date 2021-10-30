using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using System;
using TAS.Services.Entities;
namespace TAS.Services
{
    public interface ICustomerManagementService
    {
        #region Customer
        CustomersResponseDto GetCustomers(
            SecurityContext securityContext,
            AuditContext auditContext);

        CustomerResponseDto GetCustomerDataById(Guid Id,
         SecurityContext securityContext,
         AuditContext auditContext);


        CustomerRequestDto AddCustomer(CustomerRequestDto customer,
            SecurityContext securityContext,
            AuditContext auditContext);


        CustomerResponseDto GetCustomerById(Guid Id,
            SecurityContext securityContext,
            AuditContext auditContext);

        CustomerResponseDto GetCustomerByUserName(string username,
         SecurityContext securityContext,
         AuditContext auditContext);

        CustomerRequestDto UpdateCustomer(CustomerRequestDto Dealer, SecurityContext securityContext,
         AuditContext auditContext);
        #endregion

        #region Occupation
        OccupationsResponseDto GetOccupations(
          SecurityContext securityContext,
          AuditContext auditContext);

        OccupationRequestDto AddOccupation(OccupationRequestDto Occupation, SecurityContext securityContext,
            AuditContext auditContext);
        #endregion

        #region Title
        TitlesResponseDto GetTitles(
          SecurityContext securityContext,
          AuditContext auditContext);
        #endregion

        #region MarritalStatus
        MarritalStatusesResponseDto GetMarritalStatuses(
          SecurityContext securityContext,
          AuditContext auditContext);
        #endregion

        object GetAllCustomersForSearchGrid(CustomerSearchGridRequestDto CustomerSearchGridRequestDto, SecurityContext securityContext, AuditContext auditContext);

        //CustomersResponseDto GetCustomerIdByUserName(SecurityContext securityContext, AuditContext auditContext, string customerUserName);

        CustomersResponseDto GetCustomerIdByUserName(SecurityContext securityContext, AuditContext auditContext, string customerUserName, string tpaName);

        string GetCustomerNameById(Guid customerID,Guid tpaID, SecurityContext securityContext, AuditContext auditContext);

        bool ChangePassword(ChangePasswordCustomerRequestDto requestData, SecurityContext securityContext, AuditContext auditContext);



        bool RequestNewPasswordCustomer(ForgotPasswordCustomerRequestDto forgotPasswordRequestDto, SecurityContext securityContext, AuditContext auditContext);

        bool ChangeForgotPasswordCustomer(ChangePasswordForgotCustomerRequestDto requestData, SecurityContext securityContext, AuditContext auditContext);
        CustomerAddPolicyResponseDto GetCustomerByIdforIloe(Guid guid, SecurityContext context1, AuditContext context2);
    }
}
