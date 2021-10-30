using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using TAS.Services.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Common;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class PriviledgeLevelUpdationUnitOfWork : UnitOfWork
    {
        public PriviledgeLevelRequestDto PriviledgeLevel;
        public PriviledgeLevelResponseDto ExPr;
       
        public PriviledgeLevelUpdationUnitOfWork(PriviledgeLevelRequestDto PriviledgeLevel)
        {
            
            this.PriviledgeLevel = PriviledgeLevel;
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
                            UserEntityManager UserEntityManager = new UserEntityManager();
                            var ce = UserEntityManager.GetPriviledgeLevelById(PriviledgeLevel.Id);
                            if (ce.IsPriviledgeLevelExists == true)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
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

        //public override bool PreExecute()
        //{
        //    try
        //    {
        //        EntitySessionManager.OpenSession();
        //        UserEntityManager UserEntityManager = new UserEntityManager();
        //        var ce = UserEntityManager.GetPriviledgeLevelById(PriviledgeLevel.Id);
        //        if (ce.IsPriviledgeLevelExists == true)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    finally
        //    {
        //        EntitySessionManager.CloseSession();
                
        //    }
            
        //}
        
        public override void Execute()
        {
            try
            {
                EntitySessionManager.OpenSession();
                UserEntityManager UserEntityManager = new UserEntityManager();
                //var ce = UserEntityManager.GetPriviledgeLevelById(PriviledgeLevel.Id);
                //if (ce.IsPriviledgeLevelExists == true)
                //{
                    bool result = UserEntityManager.UpdatePriviledgeLevel(PriviledgeLevel);
                    this.PriviledgeLevel.PriviledgeLevelInsertion = result;
                //}
                //else
                //{
                //    this.PriviledgeLevel.PriviledgeLevelInsertion = false;
                //}

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
