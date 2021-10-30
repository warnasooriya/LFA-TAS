using TAS.DataTransfer.Responses;
using TAS.Services.Entities;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Common;


namespace TAS.Services.UnitsOfWork
{
    internal sealed class TPABranchesRetrievalUnitOfWork:UnitOfWork
    {
         public TPABranchesResponseDto Result
        {
            get;
            private set;
        }
        public Guid TPAId
        {
            get;
            set;
        }
        internal TPABranchesRetrievalUnitOfWork(Guid tpaId)
        {
            this.TPAId = tpaId;
        }
        internal TPABranchesRetrievalUnitOfWork()
        {
            this.TPAId = Guid.Empty;
        }
        
        public override bool PreExecute() {
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
                //EntitySessionManager.OpenSession();
                if (dbConnectionString != null)   //**
                {     //**
                    EntitySessionManager.OpenSession(dbConnectionString);


                    TPABranchEntityManager TPABranchEntityManager = new TPABranchEntityManager();
                    List<TPABranch> TPABranchEntities = new List<TPABranch>();
                    if (TPAId == Guid.Empty)
                    {
                        TPABranchEntities = TPABranchEntityManager.GetAllTPABranches();
                    }
                    else
                    {
                        TPABranchEntities = TPABranchEntityManager.GetTPABranchesByTPAId(TPAId);

                    }
                    TPABranchesResponseDto result = new TPABranchesResponseDto();
                    result.TPABranches = new List<TPABranchResponseDto>();
                    foreach (TPABranch tpa in TPABranchEntities)
                    {
                        TPABranchResponseDto TPABranchResponseDto = new TPABranchResponseDto();
                        TPABranchResponseDto.Id = tpa.Id;
                        TPABranchResponseDto.Address = tpa.Address;
                        TPABranchResponseDto.BranchCode = tpa.BranchCode;
                        TPABranchResponseDto.BranchName = tpa.BranchName;
                        TPABranchResponseDto.CityId = tpa.CityId;
                        TPABranchResponseDto.ContryId = tpa.ContryId;
                        TPABranchResponseDto.TimeZone = tpa.TimeZone;
                        TPABranchResponseDto.IsHeadOffice = tpa.IsHeadOffice;
                        TPABranchResponseDto.State = tpa.State;
                        TPABranchResponseDto.TpaId = tpa.TpaId;
                        result.TPABranches.Add(TPABranchResponseDto);
                    }
                    this.Result = result;
                }
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
