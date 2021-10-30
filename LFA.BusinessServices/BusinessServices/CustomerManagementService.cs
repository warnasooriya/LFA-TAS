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
    internal sealed class CustomerManagementService : ICustomerManagementService
    {
        #region Customer
        public CustomersResponseDto GetCustomers(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            CustomersResponseDto result = null;

            CustomersRetrievalUnitOfWork uow = new CustomersRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public CustomerResponseDto GetCustomerDataById(Guid Id,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            CustomerResponseDto result = null;

            CustomersRetrievalByCustomerIDUnitOfWork uow = new CustomersRetrievalByCustomerIDUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.Id = Id;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public CustomerAddPolicyResponseDto GetCustomerByIdforIloe(Guid Id,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            CustomerAddPolicyResponseDto result = null;

            CustomersInILOERetrievalByCustomerIDUnitOfWork uow = new CustomersInILOERetrievalByCustomerIDUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.Id = Id;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public CustomerRequestDto AddCustomer(CustomerRequestDto Customer, SecurityContext securityContext,
            AuditContext auditContext)
        {
            CustomerRequestDto result = new CustomerRequestDto();
            CustomerInsertionUnitOfWork uow = new CustomerInsertionUnitOfWork(Customer);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Customer;
            return result;
        }


        public CustomerResponseDto GetCustomerById(Guid Id,
      SecurityContext securityContext,
      AuditContext auditContext)
        {
            CustomerResponseDto result = new CustomerResponseDto();

            CustomerRetrievalUnitOfWork uow = new CustomerRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.Id = Id;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public CustomerResponseDto GetCustomerByUserName(string username,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            CustomerResponseDto result = new CustomerResponseDto();

            CustomerRetrievalUnitOfWork uow = new CustomerRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.UserName = username;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public CustomerRequestDto UpdateCustomer(CustomerRequestDto Dealer, SecurityContext securityContext,
          AuditContext auditContext)
        {
            CustomerRequestDto result = new CustomerRequestDto();
            CustomerUpdationUnitOfWork uow = new CustomerUpdationUnitOfWork(Dealer);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Customer;
            return result;
        }

        public object GetAllCustomersForSearchGrid(
            CustomerSearchGridRequestDto CustomerSearchGridRequestDto,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            CustomersRetrievalForSearchGridUnitOfWork uow = new CustomersRetrievalForSearchGridUnitOfWork(CustomerSearchGridRequestDto);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }

            return uow.Result;
        }
        #endregion

        #region Occupation
        public OccupationsResponseDto GetOccupations(
          SecurityContext securityContext,
          AuditContext auditContext)
        {
            OccupationsResponseDto result = null;

            OccupationsRetrievalUnitOfWork uow = new OccupationsRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public OccupationRequestDto AddOccupation(OccupationRequestDto Occupation, SecurityContext securityContext,
            AuditContext auditContext)
        {
            OccupationRequestDto result = new OccupationRequestDto();
            OccupationInsertionUnitOfWork uow = new OccupationInsertionUnitOfWork(Occupation);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Occupation;
            return result;
        }
        #endregion

        #region Title
        public TitlesResponseDto GetTitles(
          SecurityContext securityContext,
          AuditContext auditContext)
        {
            TitlesResponseDto result = null;

            TitlesRetrievalUnitOfWork uow = new TitlesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }
        #endregion

        #region MarritalStatus
        public MarritalStatusesResponseDto GetMarritalStatuses(
          SecurityContext securityContext,
          AuditContext auditContext)
        {
            MarritalStatusesResponseDto result = null;

            MarritalStatusesRetrievalUnitOfWork uow = new MarritalStatusesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }
        #endregion


        #region Login Customer

        public CustomersResponseDto GetCustomerIdByUserName(
        SecurityContext securityContext,
        AuditContext auditContext, string customerUserName, string tpaName)
        {
            CustomersResponseDto result = null;
            CustomerDisplayRetrievalByUserNameUnitOfWork uow = new CustomerDisplayRetrievalByUserNameUnitOfWork(customerUserName, tpaName);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;

        }

        public string GetCustomerNameById(Guid customerID, Guid tpaID, SecurityContext securityContext, AuditContext auditContext)
        {
            string result = string.Empty;
            CustomerLoginAuthUnitOfWork uow = new CustomerLoginAuthUnitOfWork(customerID, tpaID);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                result = uow.tpaName;
            }

            return result;
        }

        #endregion


        public bool RequestNewPasswordCustomer(ForgotPasswordCustomerRequestDto forgotPasswordRequestDto, SecurityContext securityContext, AuditContext auditContext)
        {

            try
            {
                ForgotPasswordCustomerUnitOfWork uow = new ForgotPasswordCustomerUnitOfWork(forgotPasswordRequestDto);
                if (uow.PreExecute())
                {
                    uow.Execute();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool ChangeForgotPasswordCustomer(ChangePasswordForgotCustomerRequestDto requestData, SecurityContext securityContext, AuditContext auditContext)
        {
            try
            {
                ChangeForgotPasswordCustomerUnotOfWork uow = new ChangeForgotPasswordCustomerUnotOfWork(requestData);
                if (uow.PreExecute())
                {
                    uow.Execute();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {

                return false;
            }
        }
        public bool ChangePassword(ChangePasswordCustomerRequestDto requestData, SecurityContext securityContext, AuditContext auditContext)
        {

            ChangePasswordCustomerUnitOfWork uow = new ChangePasswordCustomerUnitOfWork(requestData);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        

    }
}
