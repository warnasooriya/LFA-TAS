using TAS.Services.Entities.Management;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Entities.Persistence;
using TAS.Services.Entities;

using TAS.Services.Common;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class UserRolesRetrievalUnitOfWork : UnitOfWork
    {

        public UserRolesResponseDto Result
        {
            get;
            private set;
        }
        public override bool PreExecute()
        {
            try
            {
                Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                string dbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
                if (dbName != "")
                {
                    TASEntitySessionManager.OpenSession();
                    string tasTpaConnString = TASTPAEntityManager.GetTPAConnStringBydbName(dbName);
                    TASEntitySessionManager.CloseSession();
                    if (tasTpaConnString != "")
                    {
                        dbConnectionString = tasTpaConnString;
                        EntitySessionManager.OpenSession(dbConnectionString);
                        if (JWTHelper.checkTokenValidity(Convert.ToInt32(ConfigurationData.tasTokenValidTime.ToString())))
                        {
                            Common.AuthorizeUOW auow = new Common.AuthorizeUOW(SecurityContext, str.FirstOrDefault(f => f.Key == "name").Value.ToString(), this.GetType().Name, dbConnectionString);
                            bool a = auow.checkAuthorization();
                            return true;    //change to a after functioning successfully
                        }
                        EntitySessionManager.CloseSession();
                    }
                }
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
            return false;
        }

        public override void Execute()
        {

            try
            {
                //temp section can remove  //**  lines once preexecute jwt and db name working fine
                if (dbConnectionString != null)   //**
                {     //**
                    EntitySessionManager.OpenSession(dbConnectionString);
                }     //**
                else     //**
                {     //**
                    EntitySessionManager.OpenSession();     //**
                }     //**
                UserEntityManager UserEntityManager = new UserEntityManager();
                List<UserRole> userEntities = UserEntityManager.GetUserRoles();
                UserRolesResponseDto result = new UserRolesResponseDto();
                result.UserRoles = new List<UserRoleResponseDto>();
                foreach (var User in userEntities)
                {
                    UserRoleResponseDto surDto = new UserRoleResponseDto();
                    surDto.RoleCode = User.RoleCode;
                    surDto.RoleName = User.RoleName;
                    surDto.RoleId = User.RoleId;
             
                    result.UserRoles.Add(surDto);
                }
                this.Result = result;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }

    }
}
