
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
namespace EJ.DB
{

    /// <summary>
	/// 
	/// </summary>
 [System.Data.Linq.Mapping.DatabaseAttribute(Name = "EasyJob")]
    public class EasyJob : EntityDB.DBContext
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="dbType"></param>
        public EasyJob(string connection, EntityDB.DatabaseType dbType): base(connection, dbType)
        {
            if (!setEvented)
            {
                setEvented = true;
                EntityDB.DBContext.BeforeDelete += Database_BeforeDelete;
            }
        }

        static bool setEvented = false;
 

        static void Database_BeforeDelete(object sender, EntityDB.DatabaseModifyEventArg e)
        {
            var db =  sender as EasyJob;
            if (db == null)
                return;


                if (e.DataItem is EJ.Project)
                {
                    var deletingItem = (EJ.Project)e.DataItem;
                    
    var items0 = (from m in db.Databases
                    where m.ProjectID == deletingItem.id
                    select new EJ.Databases
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items0.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items1 = (from m in db.DLLImport
                    where m.ProjectID == deletingItem.id
                    select new EJ.DLLImport
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items1.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items2 = (from m in db.InterfaceModule
                    where m.ProjectID == deletingItem.id
                    select new EJ.InterfaceModule
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items2.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items3 = (from m in db.ProjectPower
                    where m.ProjectID == deletingItem.id
                    select new EJ.ProjectPower
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items3.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

                }

                if (e.DataItem is EJ.Databases)
                {
                    var deletingItem = (EJ.Databases)e.DataItem;
                    
    var items0 = (from m in db.DBPower
                    where m.DatabaseID == deletingItem.id
                    select new EJ.DBPower
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items0.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items1 = (from m in db.DBTable
                    where m.DatabaseID == deletingItem.id
                    select new EJ.DBTable
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items1.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items2 = (from m in db.DBModule
                    where m.DatabaseID == deletingItem.id
                    select new EJ.DBModule
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items2.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

                }

                if (e.DataItem is EJ.User)
                {
                    var deletingItem = (EJ.User)e.DataItem;
                    
    var items0 = (from m in db.InterfaceModulePower
                    where m.UserID == deletingItem.id
                    select new EJ.InterfaceModulePower
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items0.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items1 = (from m in db.DBPower
                    where m.UserID == deletingItem.id
                    select new EJ.DBPower
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items1.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items2 = (from m in db.TablePower
                    where m.UserID == deletingItem.id
                    select new EJ.TablePower
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items2.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items3 = (from m in db.ProjectPower
                    where m.UserID == deletingItem.id
                    select new EJ.ProjectPower
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items3.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

                }

                if (e.DataItem is EJ.Bug)
                {
                    var deletingItem = (EJ.Bug)e.DataItem;
                    
    var items0 = (from m in db.BugHandleHistory
                    where m.BugID == deletingItem.id
                    select new EJ.BugHandleHistory
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items0.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items1 = (from m in db.BugImages
                    where m.BugID == deletingItem.id
                    select new EJ.BugImages
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items1.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

                }

                if (e.DataItem is EJ.DBTable)
                {
                    var deletingItem = (EJ.DBTable)e.DataItem;
                    
    var items0 = (from m in db.IDXIndex
                    where m.TableID == deletingItem.id
                    select new EJ.IDXIndex
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items0.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items1 = (from m in db.DBDeleteConfig
                    where m.TableID == deletingItem.id
                    select new EJ.DBDeleteConfig
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items1.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items2 = (from m in db.DBDeleteConfig
                    where m.RelaTableID == deletingItem.id
                    select new EJ.DBDeleteConfig
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items2.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items3 = (from m in db.DBColumn
                    where m.TableID == deletingItem.id
                    select new EJ.DBColumn
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items3.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items4 = (from m in db.TableInModule
                    where m.TableID == deletingItem.id
                    select new EJ.TableInModule
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items4.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items5 = (from m in db.TablePower
                    where m.TableID == deletingItem.id
                    select new EJ.TablePower
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items5.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

                }

                if (e.DataItem is EJ.DBColumn)
                {
                    var deletingItem = (EJ.DBColumn)e.DataItem;
                    
    var items0 = (from m in db.DBDeleteConfig
                    where m.RelaColumID == deletingItem.id
                    select new EJ.DBDeleteConfig
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items0.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

                }

                if (e.DataItem is EJ.InterfaceModule)
                {
                    var deletingItem = (EJ.InterfaceModule)e.DataItem;
                    
    var items0 = (from m in db.InterfaceInModule
                    where m.ModuleID == deletingItem.id
                    select new EJ.InterfaceInModule
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items0.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items1 = (from m in db.InterfaceModulePower
                    where m.ModuleID == deletingItem.id
                    select new EJ.InterfaceModulePower
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items1.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

                }

        }

/// <summary>
	/// 
	/// </summary>
 /// <param name="modelBuilder"></param>
         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
   modelBuilder.Entity<EJ.Project>().HasKey(m => m.id);
modelBuilder.Entity<EJ.Databases>().HasKey(m => m.id);
modelBuilder.Entity<EJ.User>().HasKey(m => m.id);
modelBuilder.Entity<EJ.DBPower>().HasKey(m => m.id);
modelBuilder.Entity<EJ.Bug>().HasKey(m => m.id);
modelBuilder.Entity<EJ.DBTable>().HasKey(m => m.id);
modelBuilder.Entity<EJ.DBColumn>().HasKey(m => m.id);
modelBuilder.Entity<EJ.TablePower>().HasKey(m => m.id);
modelBuilder.Entity<EJ.ProjectPower>().HasKey(m => m.id);
modelBuilder.Entity<EJ.DBModule>().HasKey(m => m.id);
modelBuilder.Entity<EJ.DBDeleteConfig>().HasKey(m => m.id);
modelBuilder.Entity<EJ.TableInModule>().HasKey(m => m.id);
modelBuilder.Entity<EJ.IDXIndex>().HasKey(m => m.id);
modelBuilder.Entity<EJ.BugHandleHistory>().HasKey(m => m.id);
modelBuilder.Entity<EJ.BugImages>().HasKey(m => m.id);
modelBuilder.Entity<EJ.DLLImport>().HasKey(m => m.id);
modelBuilder.Entity<EJ.InterfaceModule>().HasKey(m => m.id);
modelBuilder.Entity<EJ.InterfaceInModule>().HasKey(m => m.id);
modelBuilder.Entity<EJ.InterfaceModulePower>().HasKey(m => m.id);
}

System.Linq.IQueryable<EJ.Project> _Project;
 /// <summary>
        /// 项目
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.Project> Project
        {
             get
            {
                if (_Project == null)
                {
                    _Project = new EntityDB.WayQueryable<EJ.Project>(this.Set<EJ.Project>());
                }
                return _Project;
            }
        }

System.Linq.IQueryable<EJ.Databases> _Databases;
 /// <summary>
        /// 数据库
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.Databases> Databases
        {
             get
            {
                if (_Databases == null)
                {
                    _Databases = new EntityDB.WayQueryable<EJ.Databases>(this.Set<EJ.Databases>());
                }
                return _Databases;
            }
        }

System.Linq.IQueryable<EJ.User> _User;
 /// <summary>
        /// 系统用户
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.User> User
        {
             get
            {
                if (_User == null)
                {
                    _User = new EntityDB.WayQueryable<EJ.User>(this.Set<EJ.User>());
                }
                return _User;
            }
        }

System.Linq.IQueryable<EJ.DBPower> _DBPower;
 /// <summary>
        /// 数据库权限
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.DBPower> DBPower
        {
             get
            {
                if (_DBPower == null)
                {
                    _DBPower = new EntityDB.WayQueryable<EJ.DBPower>(this.Set<EJ.DBPower>());
                }
                return _DBPower;
            }
        }

System.Linq.IQueryable<EJ.Bug> _Bug;
 /// <summary>
        /// 错误报告
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.Bug> Bug
        {
             get
            {
                if (_Bug == null)
                {
                    _Bug = new EntityDB.WayQueryable<EJ.Bug>(this.Set<EJ.Bug>());
                }
                return _Bug;
            }
        }

System.Linq.IQueryable<EJ.DBTable> _DBTable;
 /// <summary>
        /// 数据表
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.DBTable> DBTable
        {
             get
            {
                if (_DBTable == null)
                {
                    _DBTable = new EntityDB.WayQueryable<EJ.DBTable>(this.Set<EJ.DBTable>());
                }
                return _DBTable;
            }
        }

System.Linq.IQueryable<EJ.DBColumn> _DBColumn;
 /// <summary>
        /// 字段
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.DBColumn> DBColumn
        {
             get
            {
                if (_DBColumn == null)
                {
                    _DBColumn = new EntityDB.WayQueryable<EJ.DBColumn>(this.Set<EJ.DBColumn>());
                }
                return _DBColumn;
            }
        }

System.Linq.IQueryable<EJ.TablePower> _TablePower;
 /// <summary>
        /// 数据表权限
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.TablePower> TablePower
        {
             get
            {
                if (_TablePower == null)
                {
                    _TablePower = new EntityDB.WayQueryable<EJ.TablePower>(this.Set<EJ.TablePower>());
                }
                return _TablePower;
            }
        }

System.Linq.IQueryable<EJ.ProjectPower> _ProjectPower;
 /// <summary>
        /// 项目权限
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.ProjectPower> ProjectPower
        {
             get
            {
                if (_ProjectPower == null)
                {
                    _ProjectPower = new EntityDB.WayQueryable<EJ.ProjectPower>(this.Set<EJ.ProjectPower>());
                }
                return _ProjectPower;
            }
        }

System.Linq.IQueryable<EJ.DBModule> _DBModule;
 /// <summary>
        /// 数据模块
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.DBModule> DBModule
        {
             get
            {
                if (_DBModule == null)
                {
                    _DBModule = new EntityDB.WayQueryable<EJ.DBModule>(this.Set<EJ.DBModule>());
                }
                return _DBModule;
            }
        }

System.Linq.IQueryable<EJ.DBDeleteConfig> _DBDeleteConfig;
 /// <summary>
        /// 级联删除
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.DBDeleteConfig> DBDeleteConfig
        {
             get
            {
                if (_DBDeleteConfig == null)
                {
                    _DBDeleteConfig = new EntityDB.WayQueryable<EJ.DBDeleteConfig>(this.Set<EJ.DBDeleteConfig>());
                }
                return _DBDeleteConfig;
            }
        }

System.Linq.IQueryable<EJ.TableInModule> _TableInModule;
 /// <summary>
        /// TableInModule
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.TableInModule> TableInModule
        {
             get
            {
                if (_TableInModule == null)
                {
                    _TableInModule = new EntityDB.WayQueryable<EJ.TableInModule>(this.Set<EJ.TableInModule>());
                }
                return _TableInModule;
            }
        }

System.Linq.IQueryable<EJ.IDXIndex> _IDXIndex;
 /// <summary>
        /// 唯一值索引
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.IDXIndex> IDXIndex
        {
             get
            {
                if (_IDXIndex == null)
                {
                    _IDXIndex = new EntityDB.WayQueryable<EJ.IDXIndex>(this.Set<EJ.IDXIndex>());
                }
                return _IDXIndex;
            }
        }

System.Linq.IQueryable<EJ.BugHandleHistory> _BugHandleHistory;
 /// <summary>
        /// Bug处理历史记录
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.BugHandleHistory> BugHandleHistory
        {
             get
            {
                if (_BugHandleHistory == null)
                {
                    _BugHandleHistory = new EntityDB.WayQueryable<EJ.BugHandleHistory>(this.Set<EJ.BugHandleHistory>());
                }
                return _BugHandleHistory;
            }
        }

System.Linq.IQueryable<EJ.BugImages> _BugImages;
 /// <summary>
        /// Bug附带截图
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.BugImages> BugImages
        {
             get
            {
                if (_BugImages == null)
                {
                    _BugImages = new EntityDB.WayQueryable<EJ.BugImages>(this.Set<EJ.BugImages>());
                }
                return _BugImages;
            }
        }

System.Linq.IQueryable<EJ.DLLImport> _DLLImport;
 /// <summary>
        /// 引入的dll
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.DLLImport> DLLImport
        {
             get
            {
                if (_DLLImport == null)
                {
                    _DLLImport = new EntityDB.WayQueryable<EJ.DLLImport>(this.Set<EJ.DLLImport>());
                }
                return _DLLImport;
            }
        }

System.Linq.IQueryable<EJ.InterfaceModule> _InterfaceModule;
 /// <summary>
        /// 接口设计的目录结构
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.InterfaceModule> InterfaceModule
        {
             get
            {
                if (_InterfaceModule == null)
                {
                    _InterfaceModule = new EntityDB.WayQueryable<EJ.InterfaceModule>(this.Set<EJ.InterfaceModule>());
                }
                return _InterfaceModule;
            }
        }

System.Linq.IQueryable<EJ.InterfaceInModule> _InterfaceInModule;
 /// <summary>
        /// 
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.InterfaceInModule> InterfaceInModule
        {
             get
            {
                if (_InterfaceInModule == null)
                {
                    _InterfaceInModule = new EntityDB.WayQueryable<EJ.InterfaceInModule>(this.Set<EJ.InterfaceInModule>());
                }
                return _InterfaceInModule;
            }
        }

System.Linq.IQueryable<EJ.InterfaceModulePower> _InterfaceModulePower;
 /// <summary>
        /// InterfaceModule权限设定表
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.InterfaceModulePower> InterfaceModulePower
        {
             get
            {
                if (_InterfaceModulePower == null)
                {
                    _InterfaceModulePower = new EntityDB.WayQueryable<EJ.InterfaceModulePower>(this.Set<EJ.InterfaceModulePower>());
                }
                return _InterfaceModulePower;
            }
        }
}}