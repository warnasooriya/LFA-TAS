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
    internal sealed class MakeManagementService : IMakeManagementService
    {

        public MakesResponseDto GetAllMakes(
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            MakesResponseDto result = null;

            MakesRetrievalUnitOfWork uow = new MakesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public MakesResponseDto GetParentMakes(
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            MakesResponseDto result = null;

            MakesRetrievalUnitOfWork uow = new MakesRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }

            result = uow.Result;

            return result;
        }

        public MakeRequestDto AddMake(MakeRequestDto Make, SecurityContext securityContext,
            AuditContext auditContext) {
                MakeRequestDto result = new MakeRequestDto();
                MakeInsertionUnitOfWork uow = new MakeInsertionUnitOfWork(Make);
                uow.SecurityContext = securityContext;
                uow.AuditContext = auditContext;

                if (uow.PreExecute())
                {
                    uow.Execute();
                }
            result = uow.Make;
                return result;
        }


        public MakeResponseDto GetMakeById(Guid MakeId,
    SecurityContext securityContext,
    AuditContext auditContext)
        {
            MakeResponseDto result = new MakeResponseDto();

            MakeRetrievalUnitOfWork uow = new MakeRetrievalUnitOfWork();

            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            uow.MakeId = MakeId;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result = uow.Result;
            return result;
        }

        public MakeRequestDto UpdateMake(MakeRequestDto Make, SecurityContext securityContext,
           AuditContext auditContext)
        {
            MakeRequestDto result = new MakeRequestDto();
            MakeUpdationUnitOfWork uow = new MakeUpdationUnitOfWork(Make);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            result.MakeInsertion = uow.Make.MakeInsertion;
            return result;
        }

        public MakesResponseDto GetMakesByCommodityCategoryId(
            Guid CommodityCategoryId,
            SecurityContext securityContext,
            AuditContext auditContext)
        {
            MakesResponseDto _Response = new MakesResponseDto();
            MakesRetrievalByCommodityCategoryIdUnitOfWork  uow = new MakesRetrievalByCommodityCategoryIdUnitOfWork(CommodityCategoryId);
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;

            if (uow.PreExecute())
            {
                uow.Execute();
            }
            _Response = uow.Result;
            return _Response;
        }



    }
}
