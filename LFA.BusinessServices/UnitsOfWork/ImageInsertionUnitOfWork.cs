﻿using TAS.DataTransfer.Requests;
using TAS.Services.Entities;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Common;


namespace TAS.Services.UnitsOfWork
{
    internal sealed class ImageInsertionUnitOfWork : UnitOfWork
    {
        public Guid Result
        {
            get;
            private set;
        }
        public ImageRequestDto ImageRequestDto
        {
            get;
            set;
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
                    //int timestamp = 0;
                    //if (int.TryParse(str.FirstOrDefault(f => f.Key == "iat").Value.ToString(), out timestamp))
                    //{

                    //System.DateTime GenerateddtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    //GenerateddtDateTime = GenerateddtDateTime.AddSeconds(timestamp).ToLocalTime();
                    //var diffInSeconds = (DateTime.Now - GenerateddtDateTime).TotalSeconds;

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
                }

                ImageEntityManager ImageEntityManager = new ImageEntityManager();
                Guid imageId = new Guid();

                imageId = ImageEntityManager.SaveImage(ImageRequestDto);
                this.Result = imageId;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
