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
    internal sealed class RSAProviderManagementService : IRSAProviderManagementService
    {
        #region RSAProvider
        public RSAProvideresResponseDto GetRSAProviders(
            SecurityContext securityContext, 
            AuditContext auditContext)
        {
            RSAProvideresResponseDto result = null;

            RSAProvidersRetrievalUnitOfWork uow = new RSAProvidersRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public RSAProviderRequestDto AddRSAProvider(RSAProviderRequestDto RSAProvider, 
            SecurityContext securityContext,
            AuditContext auditContext) {
                RSAProviderRequestDto result = new RSAProviderRequestDto();
                RSAProviderInsertionUnitOfWork uow = new RSAProviderInsertionUnitOfWork(RSAProvider);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
                result.RSAProviderInsertion = uow.RSAProvider.RSAProviderInsertion;
                return result;
        }

        public RSAProviderResponseDto GetRSAProviderById(Guid RSAProviderId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            RSAProviderResponseDto result = new RSAProviderResponseDto();

            RSAProviderRetrievalUnitOfWork uow = new RSAProviderRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.RSAProviderId = RSAProviderId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public RSAProviderRequestDto UpdateRSAProvider(RSAProviderRequestDto RSAProvider, 
            SecurityContext securityContext,
           AuditContext auditContext)
        {
            RSAProviderRequestDto result = new RSAProviderRequestDto();
            RSAProviderUpdationUnitOfWork uow = new RSAProviderUpdationUnitOfWork(RSAProvider);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.RSAProviderInsertion = uow.RSAProvider.RSAProviderInsertion;
            return result;
        }
        #endregion

        #region
        public RSAAnualPremiumsResponseDto GetRSAAnualPremiums(
           SecurityContext securityContext,
           AuditContext auditContext)
        {
            RSAAnualPremiumsResponseDto result = null;
            RSAAnualPremiumsRetrievalUnitOfWork uow = new RSAAnualPremiumsRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public RSAAnualPremiumRequestDto AddRSAAnualPremium(RSAAnualPremiumRequestDto RSAProvider,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            RSAAnualPremiumRequestDto result = new RSAAnualPremiumRequestDto();
            RSAAnualPremiumInsertionUnitOfWork uow = new RSAAnualPremiumInsertionUnitOfWork(RSAProvider);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.RSAAnualPremiumInsertion = uow.RSAAnualPremium.RSAAnualPremiumInsertion;
            return result;
        }

        public RSAAnualPremiumResponseDto GetRSAAnualPremiumById(Guid RSAProviderId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            RSAAnualPremiumResponseDto result = new RSAAnualPremiumResponseDto();
            RSAAnualPremiumRetrievalUnitOfWork uow = new RSAAnualPremiumRetrievalUnitOfWork();
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.RSAAnualPremiumId = RSAProviderId;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public RSAAnualPremiumRequestDto UpdateRSAAnualPremium(RSAAnualPremiumRequestDto RSAAnualPremium,
            SecurityContext securityContext,
           AuditContext auditContext)
        {
            RSAAnualPremiumRequestDto result = new RSAAnualPremiumRequestDto();
            RSAAnualPremiumUpdationUnitOfWork uow = new RSAAnualPremiumUpdationUnitOfWork(RSAAnualPremium);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.RSAAnualPremiumInsertion = uow.RSAAnualPremium.RSAAnualPremiumInsertion;
            return result;
        }
        #endregion
    }
}
