using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class AttachmentUploadExternalUnitOfWork : UnitOfWork
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly HttpPostedFile httpPostedFile;
        private readonly string Page, Section;
        private readonly Guid tpaId;
        private string userId;
        private string DBName;
        public object Result { get; set; }

        public AttachmentUploadExternalUnitOfWork(Guid _tpaId,HttpPostedFile _httpPostedFile, string _Page, string _Section)
        {
            httpPostedFile = _httpPostedFile;
            Page = _Page;
            Section = _Section;
            tpaId = _tpaId;
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
                logger.Warn("User ID Not in JWT TOken" + ex.Message + ", " + ex.InnerException);

            }

            try
            {


                TASEntitySessionManager.OpenSession();
                string dbName= DBName = TASTPAEntityManager.GetTPADetailById(tpaId).FirstOrDefault().DBName;
                if (dbName != "")
                {
                    string tasTpaConnString = TASTPAEntityManager.GetTPADetailById(tpaId).FirstOrDefault().DBConnectionStringViewOnly;
                    if (tasTpaConnString != "")
                    {
                        dbConnectionString = tasTpaConnString;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                TASEntitySessionManager.CloseSession();
            }
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
                this.Result = AttachmentEntityManager.UploadAttachment(httpPostedFile, Page, Section, DBName,userId);
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
