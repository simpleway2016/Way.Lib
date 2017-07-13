using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Way.Lib.ScriptRemoting;

namespace Way.EJServer
{
    public class EJDB : EJ.DB.easyjob
    {
        public static EJ.User LoginUser
        {
            get
            {
                var session = RemotingContext.Current.Controller.Session;
                return session["user"] as EJ.User;
            }
        }
      
        static string _ConnectionString = null;
        static string GetConnectionString()
        {
            if (_ConnectionString != null)
                return _ConnectionString;

            _ConnectionString = $"Data Source=\"{Way.Lib.ScriptRemoting.RemotingController.WebRoot}EasyJob.db\"";
            return _ConnectionString;
        }

        static bool setted = false;
        public EJDB()
            : base(GetConnectionString() , Way.EntityDB.DatabaseType.Sqlite)
        {
            if (!setted)
            {
                setted = true;
                Way.EntityDB.DBContext.BeforeInsert += Database_BeforeInsert;
                Way.EntityDB.DBContext.AfterInsert += Database_AfterInsert;
                Way.EntityDB.DBContext.BeforeUpdate += Database_BeforeUpdate;
                Way.EntityDB.DBContext.BeforeDelete += Database_BeforeDelete;
            }
        }

        void Database_AfterInsert(object sender, Way.EntityDB.DatabaseModifyEventArg e)
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

        void Database_BeforeDelete(object sender, Way.EntityDB.DatabaseModifyEventArg e)
        {
            if (e.DataItem is EJ.DLLImport)
            {
                if (LoginUser.Role.GetValueOrDefault().HasFlag(EJ.User_RoleEnum.管理员) == false)
                    throw new Exception("你没有权限");
            }
            else if (e.DataItem is EJ.Databases)
            {
                if (LoginUser.Role.GetValueOrDefault().HasFlag(EJ.User_RoleEnum.管理员) == false)
                    throw new Exception("你没有移除数据库的权限");
            }
            else if (e.DataItem is EJ.DBModule || e.DataItem is EJ.Databases || e.DataItem is EJ.Project || e.DataItem is EJ.TableInModule || e.DataItem is EJ.DBTable || e.DataItem is EJ.DBColumn)
            {
                if (LoginUser.Role.GetValueOrDefault().HasFlag(EJ.User_RoleEnum.数据库设计师) == false)
                    throw new Exception("你没有修改数据表的权限");
            }

            EJDB db = sender as EJDB;
            if (db == null)
                return;
            if (e.DataItem is EJ.DBModule)
            {
                if (LoginUser.Role.GetValueOrDefault().HasFlag(EJ.User_RoleEnum.数据库设计师) == false)
                    throw new Exception("你没有修改数据表的权限");
                beforeDelete_IDBModule(db, (EJ.DBModule)e.DataItem);
            }
            else if (e.DataItem is EJ.InterfaceModule)
            {
                if (LoginUser.Role.GetValueOrDefault().HasFlag(EJ.User_RoleEnum.数据库设计师) == false)
                    throw new Exception("你没有修改数据表的权限");
                beforeDelete_InterfaceModule(db, (EJ.InterfaceModule)e.DataItem);
            }
            else if (e.DataItem is EJ.DBTable || e.DataItem is EJ.DBColumn)
            {
                if (LoginUser.Role.GetValueOrDefault().HasFlag(EJ.User_RoleEnum.数据库设计师) == false)
                    throw new Exception("你没有修改数据表的权限");
            }
            else if (e.DataItem is EJ.Databases)
            {
                var databaseid = ((EJ.Databases)e.DataItem).id.GetValueOrDefault();
                db.Database.ExecSqlString("delete from __action where databaseid=" + databaseid);
            }
        }

        static void Database_BeforeUpdate(object sender, Way.EntityDB.DatabaseModifyEventArg e)
        {
            if (e.DataItem is EJ.DLLImport)
            {
                if (LoginUser.Role.GetValueOrDefault().HasFlag(EJ.User_RoleEnum.管理员) == false)
                    throw new Exception("你没有权限");
            }
            else if (e.DataItem is EJ.Databases || e.DataItem is EJ.Project || e.DataItem is EJ.DBModule || e.DataItem is EJ.TableInModule || e.DataItem is EJ.DBTable || e.DataItem is EJ.DBColumn)
            {
                if (LoginUser.Role.GetValueOrDefault().HasFlag(EJ.User_RoleEnum.数据库设计师) == false)
                    throw new Exception("你没有修改数据表的权限");
            }
            else if (e.DataItem is EJ.DBModule)
            {
                if (LoginUser.Role.GetValueOrDefault().HasFlag(EJ.User_RoleEnum.数据库设计师) == false)
                    throw new Exception("你没有此项权限");
            }
        }

        static void Database_BeforeInsert(object sender, Way.EntityDB.DatabaseModifyEventArg e)
        {
            if (e.DataItem is EJ.DLLImport)
            {
                if (LoginUser.Role.GetValueOrDefault().HasFlag(EJ.User_RoleEnum.管理员) == false)
                    throw new Exception("你没有权限");
            }
            else if (e.DataItem is EJ.InterfaceModulePower)
            {
                if (LoginUser.Role.GetValueOrDefault().HasFlag(EJ.User_RoleEnum.管理员) == false)
                    throw new Exception("你没有权限");
                

            }
            else if (e.DataItem is EJ.Databases || e.DataItem is EJ.Project || e.DataItem is EJ.DBModule || e.DataItem is EJ.DBTable || e.DataItem is EJ.DBColumn)
            {
                if (LoginUser.Role.GetValueOrDefault().HasFlag(EJ.User_RoleEnum.数据库设计师) == false)
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
                    var query = from m in Set<EJ.DBTable>()
                                select new SearchContent
                                {
                                    ID = m.id,
                                    Title = m.Name + " " + m.caption,
                                    Content = m.Name + " " + m.caption,
                                    Type = "DBTableResult",
                                };
                    result.Add(query);


                    query = from m in Set<EJ.DBModule>()
                            where m.IsFolder == false
                            select new SearchContent
                            {
                                ID = m.id,
                                Title = m.Name,
                                Content = m.Name,
                                Type = "DBModuleResult",
                            };
                    result.Add(query);

                    query = from m in Set<EJ.InterfaceModule>()
                            where m.IsFolder == false
                            select new SearchContent
                            {
                                ID = m.id,
                                Title = m.Name,
                                Content = m.Name,
                                Type = "InterfaceModuleResult",
                            };
                    result.Add(query);

                    query = from m in Set<EJ.InterfaceInModule>()
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
                    _SearchContents = query.Where(m=>m.Content != null);
                }
                return _SearchContents;
            }
        }
    }
    public class SearchContent
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

}