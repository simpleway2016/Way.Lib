using AppLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ECWeb
{
    public class EJDB : EJ.DB.EasyJob
    {
        public static EJ.User LoginUser
        {
            get
            {
                return HttpContext.Current.Session["user"] as EJ.User;
            }
        }
        static Port2DBConfirg[] _Port2DBConfirgs;
        static Port2DBConfirg[] Port2DBConfirgs
        {
            get
            {
                if (_Port2DBConfirgs == null)
                {
                    _Port2DBConfirgs = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("/port2db.config")).ToJsonObject<Port2DBConfirg[]>();
                    var currentFilePath = HttpContext.Current.Server.MapPath("/EasyJob_cur.db");
                    var nullFilePath = HttpContext.Current.Server.MapPath("/EasyJob.db");
                    if (System.IO.File.Exists(nullFilePath) && File.Exists(currentFilePath) == false)
                    {
                        File.Copy(nullFilePath, currentFilePath);
                    }
                    foreach (var item in _Port2DBConfirgs)
                    {
                        if (item.ConnectionString.Contains("EasyJob.db"))
                        {
                            item.ConnectionString = item.ConnectionString.Replace("EasyJob.db", "EasyJob_cur.db");
                        }
                    }
                }
                return _Port2DBConfirgs;
            }
        }

        static string GetConnectionString()
        {
            var portConfig = Port2DBConfirgs.FirstOrDefault(m => m.Port == HttpContext.Current.Request.Url.Port);
            if (portConfig == null)
                portConfig = Port2DBConfirgs[0];
            string conStr = portConfig.ConnectionString;
            conStr = string.Format(conStr,
                    HttpContext.Current.Request.MapPath("/"));
            return conStr;
        }

        static bool setted = false;
        public EJDB()
            : base(GetConnectionString() , EntityDB.DatabaseType.Sqlite)
        {
            if (!setted)
            {
                setted = true;
                EntityDB.DBContext.BeforeInsert += Database_BeforeInsert;
                EntityDB.DBContext.AfterInsert += Database_AfterInsert;
                EntityDB.DBContext.BeforeUpdate += Database_BeforeUpdate;
                EntityDB.DBContext.BeforeDelete += Database_BeforeDelete;
            }
        }

        void Database_AfterInsert(object sender, EntityDB.DatabaseModifyEventArg e)
        {
            EJDB db = sender as EJDB;
            if (db == null)
                return;
            if (e.DataItem is EJ.InterfaceModulePower)
            {
                //子module都加入了权限，parent module也应该有权限
                var data = (EJ.InterfaceModulePower)e.DataItem;
                var module = db.InterfaceModule.FirstOrDefault(m => m.id == data.ModuleID);
                if (module != null && module.ParentID != 0)
                {
                    var parentModule = db.InterfaceModule.FirstOrDefault(m => m.id == module.ParentID);
                    if (parentModule != null && db.InterfaceModulePower.Count(m=>m.ModuleID == parentModule.id && m.UserID == data.UserID) == 0)
                    {
                        
                        var newdata = new EJ.InterfaceModulePower()
                      {
                          ModuleID = parentModule.id,
                          UserID = data.UserID,
                      };
                        db.Insert(newdata);
                    }
                }
            }
        }

        void Database_BeforeDelete(object sender, EntityDB.DatabaseModifyEventArg e)
        {
            if (e.DataItem is EJ.DLLImport)
            {
                if (LoginUser.Role.HasFlag(EJ.User_RoleEnum.管理员) == false)
                    throw new Exception("你没有权限");
            }
            else if (e.DataItem is EJ.Databases)
            {
                if (LoginUser.Role.HasFlag(EJ.User_RoleEnum.管理员) == false)
                    throw new Exception("你没有移除数据库的权限");
            }
            else if (e.DataItem is EJ.DBModule || e.DataItem is EJ.Databases || e.DataItem is EJ.Project || e.DataItem is EJ.TableInModule || e.DataItem is EJ.DBTable || e.DataItem is EJ.DBColumn)
            {
                if (LoginUser.Role.HasFlag(EJ.User_RoleEnum.数据库设计师) == false)
                    throw new Exception("你没有修改数据表的权限");
            }

            EJDB db = sender as EJDB;
            if (db == null)
                return;
            if (e.DataItem is EJ.DBModule)
            {
                if (LoginUser.Role.HasFlag(EJ.User_RoleEnum.数据库设计师) == false)
                    throw new Exception("你没有修改数据表的权限");
                beforeDelete_IDBModule(db, (EJ.DBModule)e.DataItem);
            }
            else if (e.DataItem is EJ.InterfaceModule)
            {
                if (LoginUser.Role.HasFlag(EJ.User_RoleEnum.数据库设计师) == false)
                    throw new Exception("你没有修改数据表的权限");
                beforeDelete_InterfaceModule(db, (EJ.InterfaceModule)e.DataItem);
            }
            else if (e.DataItem is EJ.DBTable || e.DataItem is EJ.DBColumn)
            {
                if (LoginUser.Role.HasFlag(EJ.User_RoleEnum.数据库设计师) == false)
                    throw new Exception("你没有修改数据表的权限");
            }
            else if (e.DataItem is EJ.Databases)
            {
                var databaseid = ((EJ.Databases)e.DataItem).id.GetValueOrDefault();
                db.Database.ExecSqlString("delete from __action where databaseid=" + databaseid);
            }
        }

        static void Database_BeforeUpdate(object sender, EntityDB.DatabaseModifyEventArg e)
        {
            if (e.DataItem is EJ.DLLImport)
            {
                if (LoginUser.Role.HasFlag(EJ.User_RoleEnum.管理员) == false)
                    throw new Exception("你没有权限");
            }
            else if (e.DataItem is EJ.Databases || e.DataItem is EJ.Project || e.DataItem is EJ.DBModule || e.DataItem is EJ.TableInModule || e.DataItem is EJ.DBTable || e.DataItem is EJ.DBColumn)
            {
                if (LoginUser.Role.HasFlag(EJ.User_RoleEnum.数据库设计师) == false)
                    throw new Exception("你没有修改数据表的权限");
            }
            else if (e.DataItem is EJ.DBModule)
            {
                if (LoginUser.Role.HasFlag(EJ.User_RoleEnum.数据库设计师) == false)
                    throw new Exception("你没有此项权限");
            }
        }

        static void Database_BeforeInsert(object sender, EntityDB.DatabaseModifyEventArg e)
        {
            if (e.DataItem is EJ.DLLImport)
            {
                if (LoginUser.Role.HasFlag(EJ.User_RoleEnum.管理员) == false)
                    throw new Exception("你没有权限");
            }
            else if (e.DataItem is EJ.InterfaceModulePower)
            {
                if (LoginUser.Role.HasFlag(EJ.User_RoleEnum.管理员) == false)
                    throw new Exception("你没有权限");
                

            }
            else if (e.DataItem is EJ.Databases || e.DataItem is EJ.Project || e.DataItem is EJ.DBModule || e.DataItem is EJ.DBTable || e.DataItem is EJ.DBColumn)
            {
                if (LoginUser.Role.HasFlag(EJ.User_RoleEnum.数据库设计师) == false)
                    throw new Exception("你没有修改数据表的权限");
            }
        }


        static void beforeDelete_IDBModule(EJDB db, EJ.DBModule item)
        {
            //删除子模块
            var items = db.DBModule.Where(m => m.parentID == item.id).ToList();
            foreach (var t in items)
            {
                db.Delete(t);
            }
              
            var items2 = db.TableInModule.Where(m => m.ModuleID == item.id).ToList();
            foreach (var t in items2)
            {
                db.Delete(t);
            }
        }

        static void beforeDelete_InterfaceModule(EJDB db, EJ.InterfaceModule item)
        {
            //删除子模块
            var items = db.InterfaceModule.Where(m => m.ParentID == item.id).ToList();
            foreach (var t in items)
            {
                db.Delete(t);
            }

            var items2 = db.InterfaceInModule.Where(m => m.ModuleID == item.id).ToList();
            foreach (var t in items2)
            {
                db.Delete(t);
            }
        }

        IQueryable<SearchContent> _SearchContents;
        internal IQueryable<SearchContent> SearchContents
        {
            get
            {
                if (_SearchContents == null)
                {

                    List<IQueryable<SearchContent>> result = new List<IQueryable<SearchContent>>();
                    var query = from m in DBTable
                                select new SearchContent
                                {
                                    ID = m.id,
                                    Title = m.Name + " " + m.caption,
                                    Content = m.Name + " " + m.caption,
                                    Type = "DBTableResult",
                                };
                    result.Add(query);


                    query = from m in DBModule
                            where m.IsFolder == false
                            select new SearchContent
                            {
                                ID = m.id,
                                Title = m.Name,
                                Content = m.Name,
                                Type = "DBModuleResult",
                            };
                    result.Add(query);

                    query = from m in InterfaceModule
                            where m.IsFolder == false
                            select new SearchContent
                            {
                                ID = m.id,
                                Title = m.Name,
                                Content = m.Name,
                                Type = "InterfaceModuleResult",
                            };
                    result.Add(query);

                    query = from m in InterfaceInModule
                            select new SearchContent
                            {
                                ID = m.id,
                                Title = m.JsonData,
                                Content = m.JsonData,
                                Type = "InterfaceInModuleResult",
                            };
                    result.Add(query);

                    query = result[0];
                    for (int i = 1; i < result.Count; i++)
                    {
                        query = query.Concat(result[i]);
                    }
                    _SearchContents = query;
                }
                return _SearchContents;
            }
        }
    }
    class SearchContent
    {
        public int? ID
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string Content
        {
            get;
            set;
        }
        public string Type
        {
            get;
            set;
        }
    }
    class Port2DBConfirg
    {
        public int Port { get; set; }
        public string ConnectionString { get; set; }
    }
}