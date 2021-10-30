﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class ConfirmedBordxForGridRetrievalUnitOfWork:UnitOfWork
    {
        public object Result;
        private readonly ConfirmedBordxForGridRequestDto _ConfirmedBordxForGridRequestDto;
        public ConfirmedBordxForGridRetrievalUnitOfWork(ConfirmedBordxForGridRequestDto ConfirmedBordxForGridRequestDto)
        {
            _ConfirmedBordxForGridRequestDto = ConfirmedBordxForGridRequestDto;
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
                            if (_ConfirmedBordxForGridRequestDto == null
                                || _ConfirmedBordxForGridRequestDto.PaginationOptionsbordxSearchGrid == null)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
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
                BordxEntityManager BordxEntityManager = new BordxEntityManager();
                Result = BordxEntityManager.GetConfirmedConfirmedBordxForGrid(
                    _ConfirmedBordxForGridRequestDto.BordxSearchGridSearchCriterias,
                    _ConfirmedBordxForGridRequestDto.PaginationOptionsbordxSearchGrid,
                    _ConfirmedBordxForGridRequestDto.action
                    );
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}