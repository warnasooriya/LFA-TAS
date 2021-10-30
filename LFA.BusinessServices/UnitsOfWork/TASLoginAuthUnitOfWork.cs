using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class TASLoginAuthUnitOfWork:UnitOfWork
    {
        LoginRequestDto LoginRequest;
        public LoginResponseDto Result
        {
            get;
            private set;
        }
        public TASLoginAuthUnitOfWork(LoginRequestDto LoginRequest)
        {
            this.LoginRequest = LoginRequest;
            this.Result = new LoginResponseDto();
        }
       
        public override bool PreExecute()
        {
            if (LoginRequest.UserName.Length > 0 && LoginRequest.Password.Length > 0) {
                this.Result.IsValid = true;
                return true;
            }
            else
            {
                Result.IsValid = false;
                this.Result.JsonWebToken = null;
                return false;
            }

        }
        public override void Execute()
        {
            try
            {
                TASEntitySessionManager.OpenSession();
                TASSystemUserEntityManager TASsystemUserEntityManager = new TASSystemUserEntityManager();
                LoginResponseDto result = TASsystemUserEntityManager.LoginAuth(LoginRequest);
                this.Result = result;
            }
            finally
            {
                TASEntitySessionManager.CloseSession();
            }
        }

    }
}
