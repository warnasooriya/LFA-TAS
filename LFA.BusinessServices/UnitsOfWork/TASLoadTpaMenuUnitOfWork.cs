using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using TAS.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.UnitsOfWork
{
    class TASLoadTpaMenuUnitOfWork : UnitOfWork
    {
        private string dbName
        {
            get;
            set;
        }

        public MenusResponseDto Result
        {
            get;
            set;
        }

        public TASLoadTpaMenuUnitOfWork()
        {

        }

        public override bool PreExecute()
        {

            try
            {

                Common.TASJWTHelper JWTHelper = new Common.TASJWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                string dbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
                if (dbName == "TAS")
                {
                    //TASEntitySessionManager.OpenSession();
                    //string tasTpaConnString = TASTPAEntityManager.GetTPAConnStringBydbName(dbName);
                    //TASEntitySessionManager.CloseSession();
                    //if (tasTpaConnString != "")
                    //{
                    //    dbConnectionString = tasTpaConnString;
                    //    return true;
                    //}
                    return true;
                }
                return false;

            }
            catch (Exception e)
            {
                return false;
            }
        }
        public override void Execute()
        {
            try
            {
                //temp section can remove  //**  lines once preexecute jwt and db name working fine
                //if (dbConnectionString != null)   //**
                //{     //**
                //    EntitySessionManager.OpenSession(dbConnectionString);
                //}     //**
                //else     //**
                //{     //**
                //    EntitySessionManager.OpenSession();     //**
                //}     //**

                TASEntitySessionManager.OpenSession();

                SystemMenuEntityManager sem = new SystemMenuEntityManager();
                List<TASMenu> menus = sem.GetTPATASMenu();
                if (menus.Count > 0)
                {
                    MenuResponseDto _mr;
                    Result = new MenusResponseDto();
                    List<MenuResponseDto> mrd = new List<MenuResponseDto>();
                    foreach (var item in menus)
                    {
                        _mr = new MenuResponseDto();

                        _mr.Id = item.Id;
                        _mr.MenuName = item.MenuName;
                        _mr.LinkURL = item.LinkURL;
                        _mr.ParentMenuId = item.ParentMenuId;
                        _mr.Icon = item.Icon;

                        mrd.Add(_mr);
                    }
                    Result.Menus = mrd;
                }
                else
                {
                    Result = null;
                }
            }
            catch (Exception e) { }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }

    }
    }

