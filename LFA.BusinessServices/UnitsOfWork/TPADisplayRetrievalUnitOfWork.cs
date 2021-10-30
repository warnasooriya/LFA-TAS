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
    internal sealed class TPADisplayRetrievalUnitOfWork : UnitOfWork
    {
        public TPAsResponseDto Result
        {
            get;
            private set;
        }
        public Guid tpaId
        {
            get;
            set;
        }


        internal TPADisplayRetrievalUnitOfWork(Guid tpaId)
        {
            this.tpaId = tpaId;
        }

        public TPADisplayRetrievalUnitOfWork()
        {
            this.tpaId = Guid.Empty;
        }

        public override bool PreExecute()
        {
            try
            {

                TASEntitySessionManager.OpenSession();
                string dbName = TASTPAEntityManager.GetTPADetailById(tpaId).FirstOrDefault().DBName;

                if (dbName != "")
                {
                    tpaName = TASTPAEntityManager.GetTPADetailById(tpaId).FirstOrDefault().Name;
                    //string tasTpaConnString = TASTPAEntityManager.GetTPAViewOnlyConnStringBydbName(dbName);
                    string tasTpaConnString = TASTPAEntityManager.GetTPADetailById(tpaId).FirstOrDefault().DBConnectionStringViewOnly;

                    if (tasTpaConnString != "")
                    {
                        dbConnectionString = tasTpaConnString;
                        return true;
                    }
                }
                return false;

            }
            catch (Exception e)
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

                TPAEntityManager TPAEntityManager = new TPAEntityManager();
                List<TPAResponseDto> TPAEntities = new List<TPAResponseDto>();
                if (tpaId == Guid.Empty)
                {
                    TPAEntities = TPAEntityManager.GetAllTPAs();
                }
                else
                {
                    TPAEntities = TPAEntityManager.GetTPADetailById(tpaId);
                }
                TPAsResponseDto result = new TPAsResponseDto();
                result.TPAs = TPAEntities;
                this.Result = result;
            }
            finally
            {
                //TASEntitySessionManager.CloseSession();
                EntitySessionManager.CloseSession();

            }

            //try
            //{
            //    TASEntitySessionManager.OpenSession();
            //    TASTPAEntityManager TPAEntityManager = new TASTPAEntityManager();
            //    List<TASTPA> TPAEntities = new List<TASTPA>();
            //    if (tpaId == Guid.Empty)
            //    {
            //        TPAEntities = TASTPAEntityManager.GetAllTPAs();
            //    }
            //    else
            //    {
            //        TPAEntities = TASTPAEntityManager.GetTPADetailById(tpaId);

            //    }
            //    TASTPAsResponseDto result = new TASTPAsResponseDto();
            //    result.TPAs = new List<TASTPAResponseDto>();
            //    foreach (TASTPA tpa in TPAEntities)
            //    {
            //        TASTPAResponseDto TPAResponseDto = new TASTPAResponseDto();
            //        TPAResponseDto.Id = tpa.Id;
            //        TPAResponseDto.Address = tpa.Address;
            //        TPAResponseDto.Banner = tpa.Banner;
            //        TPAResponseDto.Banner2 = tpa.Banner2;
            //        TPAResponseDto.Banner3 = tpa.Banner3;
            //        TPAResponseDto.Banner4 = tpa.Banner4;
            //        TPAResponseDto.Banner5 = tpa.Banner5;
            //        TPAResponseDto.DiscountDescription = tpa.DiscountDescription;
            //        TPAResponseDto.Logo = tpa.Logo;
            //        TPAResponseDto.Name = tpa.Name;
            //        TPAResponseDto.TelNumber = tpa.TelNumber;

            //        result.TPAs.Add(TPAResponseDto);
            //    }
            //    this.Result = result;
            //}
            //finally
            //{
            //    TASEntitySessionManager.CloseSession();
            //}
        }


        public string tpaName { get; set; }
    }
}
