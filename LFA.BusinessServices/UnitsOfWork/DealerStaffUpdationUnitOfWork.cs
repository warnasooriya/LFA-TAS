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
    internal sealed class DealerStaffUpdationUnitOfWork : UnitOfWork
    {
        public DealerStaffRequestDto DealerStaff;
        public bool Enable;
        public DealerStaffResponseDto ExPr;

        public DealerStaffUpdationUnitOfWork(DealerStaffRequestDto DealerStaff, bool Enable)
        {
            this.Enable = Enable;
            this.DealerStaff = DealerStaff;
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
                            DealerEntityManager DealerEntityManager = new DealerEntityManager();
                            var ce = DealerEntityManager.GetDealerStaffById(DealerStaff.Id);
                            if (ce.IsDealerStaffExists == true)
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
        //        DealerEntityManager DealerEntityManager = new DealerEntityManager();
        //        var ce = DealerEntityManager.GetDealerStaffById(DealerStaff.Id);
        //        if (ce.IsDealerStaffExists == true)
        //        {
        //            DealerStaff.EntryDateTime = ce.EntryDateTime;
        //            DealerStaff.EntryUser = ce.EntryUser;
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
                DealerEntityManager DealerEntityManager = new DealerEntityManager();
                //var ce = DealerEntityManager.GetDealerStaffById(DealerStaff.Id);
                //if (ce.IsDealerStaffExists == true)
                //{
                    bool result = DealerEntityManager.UpdateDealerStaff(DealerStaff,Enable);
                    this.DealerStaff.DealerStaffInsertion = result;
                //}
                //else
                //{
                //    this.DealerStaff.DealerStaffInsertion = false;
                //}

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
