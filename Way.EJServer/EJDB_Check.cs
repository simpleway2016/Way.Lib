using System;
using System.Collections.Generic;
using System.Linq;

namespace Way.EJServer
{
    public class EJDB_Check : EJDB
    {
        

        public override IQueryable<EJ.DBTable> DBTable
        {
            get
            {
                if (LoginUser.Role.GetValueOrDefault().HasFlag(EJ.User_RoleEnum.数据库设计师))
                {
                    return base.DBTable;
                }
                else
                {
                    return from m in base.DBTable
                           join p in TablePower on m.id equals p.TableID
                           where p.UserID == LoginUser.id
                           select m;
                }
            }
        }

        public override IQueryable<EJ.Project> Project
        {
            get
            {
                if (LoginUser.Role == EJ.User_RoleEnum.管理员)
                {
                    return base.Project;
                }
                else
                {
                    return from m in base.Project
                           join p in ProjectPower on m.id equals p.ProjectID
                           where p.UserID == LoginUser.id
                           select m;
                }
            }
        }
        public override IQueryable<EJ.Databases> Databases
        {
            get
            {
                if (LoginUser.Role.GetValueOrDefault().HasFlag(EJ.User_RoleEnum.数据库设计师))
                {
                    return base.Databases;
                }
                else
                {
                    return from m in base.Databases
                           join p in DBPower on m.id equals p.DatabaseID
                           where p.UserID == LoginUser.id
                           select m;
                }
            }
        }
        public override IQueryable<EJ.TableInModule> TableInModule
        {
            get
            {
                if (LoginUser.Role.GetValueOrDefault().HasFlag(EJ.User_RoleEnum.数据库设计师))
                {
                    return base.TableInModule;
                }
                else
                {
                    return from m in base.TableInModule
                           join p in TablePower on m.TableID equals p.TableID
                           where p.UserID == LoginUser.id
                           select m;
                }
            }
        }

        public override IQueryable<EJ.InterfaceModule> InterfaceModule
        {
            get
            {
                if (LoginUser.Role.GetValueOrDefault().HasFlag(EJ.User_RoleEnum.数据库设计师))
                {
                    return base.InterfaceModule;
                }
                else
                {
                    return from m in base.InterfaceModule
                           join p in InterfaceModulePower on m.id equals p.ModuleID
                           where p.UserID == LoginUser.id
                           select m;
                }
               
            }
        }

        public override IQueryable<EJ.InterfaceInModule> InterfaceInModule
        {
            get
            {
                if (LoginUser.Role.GetValueOrDefault().HasFlag(EJ.User_RoleEnum.数据库设计师))
                {
                    return base.InterfaceInModule;
                }
                else
                {
                    var mids = this.InterfaceModule.Select(m=>m.id);
                    return from m in base.InterfaceInModule
                           where mids.Contains(m.ModuleID)
                           select m;

                }
            }
        }

        public IQueryable<EJ.Bug> MyBugList
        {
            get
            {
                if (LoginUser.Role.GetValueOrDefault().HasFlag(EJ.User_RoleEnum.数据库设计师))
                {
                    var query = from m in this.Bug
                                select m;
                    return query;
                }
                else
                {
                    var query = from m in this.Bug
                                where m.HandlerID == LoginUser.id || m.SubmitUserID == LoginUser.id
                                select m;
                    return query;
                }
            }
        }
    }

   
}