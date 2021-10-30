using TAS.DataTransfer.Requests;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using TAS.Services.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Entities;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class TempUserInsertionUnitOfWork : UnitOfWork
    {
        public UserRequestDto User;
        public string tpaID;

        private string dbName
        {
            get;
            set;
        }


        public TempUserInsertionUnitOfWork(UserRequestDto User)
        {

            this.User = User;
        }
        public override bool PreExecute()
        {
            try
            {
                TASEntitySessionManager.OpenSession();
                List<TASTPA> tasTpa = new List<TASTPA>();
                tasTpa = TASTPAEntityManager.GetTPADetailById(Guid.Parse(tpaID));
                TASEntitySessionManager.CloseSession();
                if (tasTpa.FirstOrDefault() != null)
                {
                    if (tasTpa.First().DBConnectionString != null && tasTpa.First().DBName != null && tasTpa.First().DBConnectionString != "" && tasTpa.First().DBName != "")
                    {
                        dbConnectionString = tasTpa.First().DBConnectionString;
                        dbName = tasTpa.First().DBName;
                        return true;
                    }
                    else
                    {
                        return false;
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
                var ce = UserEntityManager.GetUserByUserName(User.Id);
                if (ce == null || ce.IsUserExists == false)
                {
                    bool result = UserEntityManager.AddTempUser(User);
                    this.User.Id = UserEntityManager.tmpCustomerId.ToString();
                    this.User.UserInsertion = result;
                }
                else
                {
                    this.User.UserInsertion = false;
                }
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
