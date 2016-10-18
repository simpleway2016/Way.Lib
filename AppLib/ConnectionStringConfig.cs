//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.UI.Design;
//using System.Xml;

//namespace AppLib
//{
//    class ConStrItem
//    {
//        public string Name;
//        public string Value;
//        public string DllName;
//        public string linqType;
//    }
//    public class ConnectionStringConfig
//    {
//        public string ConfigFilePath;
//        static List<ConStrItem> Items;
//        public ConnectionStringConfig():this(null)
//        {
//        }
//        internal ConnectionStringConfig(IWebApplication iwebapp )
//        {
//            if (Items == null)
//            {

//                if (iwebapp == null)
//                {
//                    try
//                    {
//                        ConfigFilePath = HttpRuntime.AppDomainAppPath + "database.config";
//                    }
//                    catch
//                    {
//                        ConfigFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\database.config";
//                    }
//                }
//                else
//                {
//                    #region 设计时获取
//                    ConfigFilePath = iwebapp.RootProjectItem.PhysicalPath + "database.config";
//                    #endregion
//                }
//                if (System.IO.File.Exists(ConfigFilePath))
//                {
//                    List<ConStrItem> newItems = new List<ConStrItem>();
//                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
//                    doc.Load(ConfigFilePath);
//                    foreach (XmlElement element in doc.DocumentElement.ChildNodes)
//                    {
//                        ConStrItem item =new ConStrItem()
//                            {
//                                Name = element.GetAttribute("name"),
//                                Value = element.GetAttribute("value"),
//                                DllName = element.GetAttribute("dllname"),
//                                linqType = element.GetAttribute("linqType"),
//                            };
//                        if (doc.DocumentElement.GetAttribute("En") == "1")
//                        {
//                            item.Value = AppHelper.DecryptString(item.Value, "u98=#%l1");
//                        }
//                        newItems.Add(item);
//                    }
//                    Items = newItems;
//                }
//                else
//                {
//                    if(iwebapp == null)
//                        throw (new Exception("无法找到文件" + ConfigFilePath));
//                }
//            }

//            if (Count == 0)
//            {
//                AppLib.Controls.Editor.设置连接字符串 frm = new AppLib.Controls.Editor.设置连接字符串(this);
//                frm.ShowDialog();
//            }
//        }

//        internal void Save(DataTable dtable)
//        {
//            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
//            doc.LoadXml("<?xml version=\"1.0\"?><R/>");
//            Items = new List<ConStrItem>();
//            foreach (DataRow drow in dtable.Rows)
//            {
//                XmlElement element = doc.CreateElement("db");
//                element.SetAttribute("name", drow[0].ToString());
//                element.SetAttribute("value", drow[1].ToString());
//                element.SetAttribute("linqType", "");
//                element.SetAttribute("dllname", "");
//                doc.DocumentElement.AppendChild(element);

//                Items.Add(new ConStrItem()
//                {
//                    Name = element.GetAttribute("name"),
//                    Value = element.GetAttribute("value")
//                });
//            }
//            doc.Save(ConfigFilePath);
//        }

//        public int Count
//        {
//            get
//            {
//                if (Items == null)
//                    return 0;
//                return Items.Count;
//            }
//        }
//        internal ConStrItem this[int index]
//        {
//            get
//            {
//                return Items[index];
//            }
//        }
//        public string this[string name]
//        {
//            get
//            {
//                try
//                {
//                    return Items.FirstOrDefault(m => m.Name == name).Value;
//                }
//                catch
//                {
//                    throw(new Exception("database.config文件缺少" + name + "节点定义"));
//                }
//            }
//        }
//        internal string GetLinqType(string name)
//        {
//            return Items.FirstOrDefault(m => m.Name == name).linqType;
//        }
//        internal string GetDllName(string name)
//        {
//            return Items.FirstOrDefault(m => m.Name == name).DllName;
//        }
//    }
//}
