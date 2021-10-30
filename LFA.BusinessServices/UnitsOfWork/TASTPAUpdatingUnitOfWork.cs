using TAS.DataTransfer.Requests;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAS.Services.Common;
using System.Threading.Tasks;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class TASTPAUpdatingUnitOfWork:UnitOfWork
    {
        public TASTPARequestDto TPA {get;private set;}
        public bool Result { get; private set; }

        public TASTPAUpdatingUnitOfWork(TASTPARequestDto TPA)
        {
            this.TPA = TPA;
        }
        public override bool PreExecute()
        {
            try
            {
                Common.TASJWTHelper JWTHelper = new Common.TASJWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                string dbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();

                //if (dbName == "TAS")
                //{
                //    int timestamp = 0;
                //    if (int.TryParse(str.FirstOrDefault(f => f.Key == "iat").Value.ToString(), out timestamp))
                //    {
                //        System.DateTime GenerateddtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                //        GenerateddtDateTime = GenerateddtDateTime.AddSeconds(timestamp).ToLocalTime();
                //        var diffInSeconds = (DateTime.Now - GenerateddtDateTime).TotalSeconds;
                //        if (diffInSeconds < Convert.ToInt32(ConfigurationData.tasTokenValidTime.ToString()))
                //        {
                //            return true;
                //        }

                //    }
                //    return false;
                //}
                //return false;

                if (dbName == "TAS")
                {
                    //int timestamp = 0;
                    //if (int.TryParse(str.FirstOrDefault(f => f.Key == "iat").Value.ToString(), out timestamp))
                    //{
                    //System.DateTime GenerateddtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    //GenerateddtDateTime = GenerateddtDateTime.AddSeconds(timestamp).ToLocalTime();
                    //var diffInSeconds = (DateTime.Now - GenerateddtDateTime).TotalSeconds;
                    TASEntitySessionManager.OpenSession();
                    if (JWTHelper.checkTokenValidity(Convert.ToInt32(ConfigurationData.tasTokenValidTime.ToString())))
                    {
                        return true;
                    }
                    TASEntitySessionManager.CloseSession();
                    //}
                    //return false;
                }
                return false;

            }
            catch (Exception e)
            {
                return false;
            }
        }
        public override void Execute()
        {
            try
            {
                TASEntitySessionManager.OpenSession();
                TASTPAEntityManager TPAEntityManager = new TASTPAEntityManager();
                Result = TASTPAEntityManager.UpdateTPA(TPA);

            }
            finally
            {
                TASEntitySessionManager.CloseSession();
            }
        }
    }
}
