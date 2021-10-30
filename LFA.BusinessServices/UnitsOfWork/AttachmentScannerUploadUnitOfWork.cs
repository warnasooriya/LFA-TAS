using iTextSharp.text;
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
    internal sealed class AttachmentScannerUploadUnitOfWork:UnitOfWork
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly byte[] pdfdocument;
        private readonly string Page, Section, Filename, AttachmentType;
        private string DBName;
        private string userId;
        public object Result { get; set; }


        public AttachmentScannerUploadUnitOfWork(byte[] document, string _Page, string _Section, string _Filename, string _AttachmentType)
        {
            pdfdocument = document;
            Page = _Page;
            Filename = _Filename;
            AttachmentType = _AttachmentType;
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

                if (pdfdocument.Length > 0)
                {
                    AttachmentEntityManager AttachmentEntityManager = new AttachmentEntityManager();
                    this.Result = AttachmentEntityManager.UploadScannedAttachment(pdfdocument, Page, Section,Filename,AttachmentType, DBName, userId);
                }

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }


    }
}
