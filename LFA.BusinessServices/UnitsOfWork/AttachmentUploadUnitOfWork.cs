using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{

    internal sealed class AttachmentUploadUnitOfWork:UnitOfWork
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly HttpPostedFile httpPostedFile;
        private readonly string Page, Section;
        private string DBName;
        private string userId;
        public object Result { get; set; }

        public AttachmentUploadUnitOfWork(HttpPostedFile _httpPostedFile, string _Page, string _Section)
        {
            httpPostedFile = _httpPostedFile;
            Page = _Page;
            Section = _Section;
            Result = "Failed";
        }
        public override bool PreExecute()
        {
            try
            {
                Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                userId = str.FirstOrDefault(f => f.Key == "userid").Value.ToString();
            }
            catch (Exception ex)
            {
                logger.Warn("User Id Not in JWT : " + ex.Message + ", " + ex.InnerException);
            }

            try
            {
                Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                string dbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
                DBName = dbName;


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
                            return true;
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
                AttachmentEntityManager AttachmentEntityManager = new AttachmentEntityManager();
                this.Result = AttachmentEntityManager.UploadAttachment(httpPostedFile,Page,Section,DBName, userId);
            }finally
            {
                EntitySessionManager.CloseSession();
            }
        }


    }
}
