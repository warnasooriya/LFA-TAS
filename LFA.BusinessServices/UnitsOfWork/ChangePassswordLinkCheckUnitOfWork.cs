using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class ChangePassswordLinkCheckUnitOfWork : UnitOfWork
    {
        Guid TpaId, RequestId = Guid.Empty;
        public String Result = String.Empty;
        private SystemUserEntityManager systemUserEntityManager;
        public ChangePassswordLinkCheckUnitOfWork(ChangePassswordLinkRequestDto changePassswordLinkRequestDto)
        {
            TpaId = changePassswordLinkRequestDto.tpaId;
            RequestId = changePassswordLinkRequestDto.requestId;
            systemUserEntityManager = new Entities.Management.SystemUserEntityManager();

        }
        public override bool PreExecute()
        {
            bool response = false;
            try
            {
                if (TpaId != Guid.Empty && RequestId != Guid.Empty)
                {
                    TASEntitySessionManager.OpenSession();
                    string dbName = TASTPAEntityManager.GetTPADetailById(TpaId).FirstOrDefault().DBName;
                    if (!String.IsNullOrEmpty(dbName))
                    {
                        string tasTpaConnString = TASTPAEntityManager.GetTPADetailById(TpaId).FirstOrDefault().DBConnectionStringViewOnly;

                        if (!String.IsNullOrEmpty(tasTpaConnString))
                        {
                            dbConnectionString = tasTpaConnString;
                            EntitySessionManager.OpenSession(dbConnectionString);
                            Result = systemUserEntityManager.ValidateChangePasswordRequestId(RequestId);
                            if (!String.IsNullOrEmpty(Result))
                            {
                                response = true;
                            }
                        }
                    }
                }
            }
            finally
            {
                TASEntitySessionManager.CloseSession();
            }
            return response;
        }

        public override void Execute()
        {
            //nothing to implement now
        }
    }
}
