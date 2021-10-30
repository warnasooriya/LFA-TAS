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
    internal sealed class LoadTpaMenuUnitOfWork : UnitOfWork
    {
        private string dbName
        {
            get;
            set;
        }

        private string UserName
        {
            get;
            set;
        }

        public MenusResponseDto Result
        {
            get;
            set;
        }

        public LoadTpaMenuUnitOfWork()
        {

        }

        public override bool PreExecute()
        {

            try
            {

                Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                string dbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
                UserName = str.FirstOrDefault(f => f.Key == "name").Value.ToString();
                if (dbName != "")
                {
                    TASEntitySessionManager.OpenSession();
                    string tasTpaConnString = TASTPAEntityManager.GetTPAConnStringBydbName(dbName);
                    TASEntitySessionManager.CloseSession();
                    if (tasTpaConnString != "")
                    {
                        dbConnectionString = tasTpaConnString;
                        return true;
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


                SystemMenuEntityManager sem = new SystemMenuEntityManager();
                //List<Menu> menus = sem.GetTPAMenu();
                List<MenuResponseDto> menus = sem.GetTPAMenuByUserName(UserName);
                if (menus.Count > 0)
                {
                    //MenuResponseDto _mr;
                    Result = new MenusResponseDto();
                    //List<MenuResponseDto> mrd = new List<MenuResponseDto>();
                    //foreach (var item in menus)
                    //{
                    //    _mr = new MenuResponseDto();

                    //    _mr.Id = item.Id;
                    //    _mr.MenuName = item.MenuName;
                    //    _mr.LinkURL = item.LinkURL;
                    //    _mr.ParentMenuId = item.ParentMenuId;
                    //    _mr.Icon = item.Icon;
                    //    _mr.OrderVal = item.OrderVal;

                    //    mrd.Add(_mr);
                    //}
                    Result.Menus = menus;
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
