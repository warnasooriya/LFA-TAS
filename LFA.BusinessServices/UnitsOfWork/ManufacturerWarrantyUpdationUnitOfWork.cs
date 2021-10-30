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
    internal sealed class ManufacturerWarrantyUpdationUnitOfWork : UnitOfWork
    {
        public ManufacturerWarrantyRequestDto ManufacturerWarranty;
        public ManufacturerWarrantyResponseDto ExPr;
       
        public ManufacturerWarrantyUpdationUnitOfWork(ManufacturerWarrantyRequestDto ManufacturerWarranty)
        {
            
            this.ManufacturerWarranty = ManufacturerWarranty;
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
                            ManufacturerWarrantyEntityManager ManufacturerWarrantyEntityManager = new ManufacturerWarrantyEntityManager();
                            var ce = ManufacturerWarrantyEntityManager.GetManufacturerWarrantyById(ManufacturerWarranty.Id);
                            if (ce.IsManufacturerWarrantyExists == true)
                            {
                                ManufacturerWarranty.EntryDateTime = ce.EntryDateTime;
                                ManufacturerWarranty.EntryUser = ce.EntryUser;
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
        //        ManufacturerWarrantyEntityManager ManufacturerWarrantyEntityManager = new ManufacturerWarrantyEntityManager();
        //        var ce = ManufacturerWarrantyEntityManager.GetManufacturerWarrantyById(ManufacturerWarranty.Id);
        //        if (ce.IsManufacturerWarrantyExists == true)
        //        {
        //            ManufacturerWarranty.EntryDateTime = ce.EntryDateTime;
        //            ManufacturerWarranty.EntryUser = ce.EntryUser;
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
                //temp section can remove  //**  lines once preexecute jwt and db name working fine
                if (dbConnectionString != null)   //**
                {     //**
                    EntitySessionManager.OpenSession(dbConnectionString);
                }     //**
                else     //**
                {     //**
                    EntitySessionManager.OpenSession();     //**
                }     //**
                ManufacturerWarrantyEntityManager ManufacturerWarrantyEntityManager = new ManufacturerWarrantyEntityManager();
                //var ce = ManufacturerWarrantyEntityManager.GetManufacturerWarrantyById(ManufacturerWarranty.Id);
                //if (ce.IsManufacturerWarrantyExists == true)
                //{
                    bool result = ManufacturerWarrantyEntityManager.UpdateManufacturerWarranty(ManufacturerWarranty);
                    this.ManufacturerWarranty.ManufacturerWarrantyInsertion = result;
                //}
                //else
                //{
                //    this.ManufacturerWarranty.ManufacturerWarrantyInsertion = false;
                //}

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
