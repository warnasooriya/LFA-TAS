using TAS.DataTransfer.Requests;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using TAS.Services.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Common;
using TAS.DataTransfer.Responses;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class DealerStaffInsertionUnitOfWork : UnitOfWork
    {
        public List<DealerStaffRequestDto> DealerStaff;
        public List<DealerBranchRequestDto> DealerBranch;
        public DealerStaffAddResponse Result { get; set; }
        public DealerStaffInsertionUnitOfWork(List<DealerStaffRequestDto> DealerStaff, List<DealerBranchRequestDto> DealerBranch)
        {
            this.DealerStaff = DealerStaff;
            this.DealerBranch = DealerBranch;
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
                DealerEntityManager DealerEntityManager = new DealerEntityManager();
                //var ce = DealerEntityManager.GetDealerStaffById(DealerStaff.Id);
                //if (ce == null)
                // {
                Result = DealerEntityManager.AddDealerStaff(DealerStaff, DealerBranch);
                //this.DealerStaff.DealerStaffInsertion = result;
                // }
                // else
                // {
                //     this.DealerStaff.DealerStaffInsertion = false;
                //  }

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
