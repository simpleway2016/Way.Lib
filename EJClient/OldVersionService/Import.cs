using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace EJClient.OldVersionService
{
    class Import
    {
        public static void import(string configFile,int projectid,TreeNode.DatabaseNode parentNode)
        {
            //string xsl = Regex.Replace(configFile, @"(\.config)$", ".xsl", RegexOptions.IgnoreCase);
            //DataSet dataset = new DataSet();
            //dataset.ReadXmlSchema(xsl);

            //ProjectConfig proConfig = System.IO.File.ReadAllText(Regex.Replace(configFile, @"(\.config)$", ".pro", RegexOptions.IgnoreCase)).ToJsonObject<ProjectConfig>();
            //EJ.Databases database = new EJ.Databases();
            //database.ProjectID = projectid;
            //database.Name = proConfig.DataBaseName;
            //database.NameSpace = proConfig.NameSpace;
            //database.dllPath = proConfig.ClassFileFolder;
            //database.conStr = string.Format("server={0};uid={1};pwd={2};database={3}", proConfig.DataServerName, proConfig.DataUserName, proConfig.DataPassword, proConfig.DataBaseName);
            //database.dbType = (EJ.Databases_dbTypeEnum)(int)Way.EntityDB.DatabaseType.SqlServer;

            //XMLReadAndWrite xrw = new XMLReadAndWrite();
            //xrw.Read(configFile, dataset);

            //using (Web.DatabaseService web = Helper.CreateWebService())
            //{
            //    database.id = web.ImportDatabaseConfig(projectid, database.ToJsonString(), dataset);
            //    database.ChangedProperties.Clear();
            //    parentNode.Children.Add(new TreeNode.DatabaseItemNode(parentNode, database));
            //}
        }
    }
    public class ProjectConfig
    {
        public string NameSpace
        {
            get;
            set;
        }

        public string ClassFileFolder
        {
            get;
            //{
            //    return this.designerFile.GetSystemKeyValue("ClassFileFolder");
            //}
            set;
        }
        /// <summary>
        /// 数据库名称
        /// </summary>
        /// <value></value>
        public string DataBaseName
        {
            get;
            set;
        }

        /// <summary>
        /// 数据库服务器名称
        /// </summary>
        /// <value></value>
        public string DataServerName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string DataUserName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string DataPassword
        {
            get;
            set;
        }
    }
    /// <summary>
    /// </summary>
    public class XMLReadAndWrite
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Total"></param>
        /// <param name="NowIndex"></param>
        /// <param name="finished"></param>
        /// <param name="error"></param>
        /// <param name="dataSet"></param>
        public delegate void ProcessChangedHandler(int Total, int NowIndex, bool finished, Exception error, DataSet dataSet);
        /// <summary>
        /// 
        /// </summary>
        public event ProcessChangedHandler ProcessChanged;
        /// <summary>
        /// 
        /// </summary>
        internal event ProcessChangedHandler ReadFinished;
        /// <summary>
        /// 总记录数
        /// </summary>
        public int RowCount = 0;
        /// <summary>
        /// 当前已处理的记录数
        /// </summary>
        public int NowProcess = 0;
        /// <summary>
        /// </summary>
        public XMLReadAndWrite()
        {

        }

        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="dset"></param>
        public void Save(string path, DataSet dset)
        {
            XmlTextWriter xtw = new XmlTextWriter(path, System.Text.Encoding.GetEncoding("gb2312"));

            this.NowProcess = 0;
            DataTableCollection tables = dset.Tables;
            foreach (DataTable dt in tables)
            {
                RowCount += dt.Rows.Count;
            }
            xtw.WriteStartDocument();
            xtw.WriteStartElement("NewDataSet");
            xtw.WriteAttributeString("RowCount", RowCount.ToString());


            foreach (DataTable dt in tables)
            {
                DataRowCollection rows = dt.Rows;
                DataColumnCollection columns = dt.Columns;
                foreach (DataRow drow in rows)
                {
                    xtw.WriteWhitespace("\r\n  ");
                    xtw.WriteStartElement(dt.TableName);
                    foreach (DataColumn column in columns)
                    {
                        xtw.WriteWhitespace("\r\n    ");
                        xtw.WriteElementString(column.ColumnName, drow[column.ColumnName].ToString());
                    }
                    xtw.WriteWhitespace("\r\n  ");
                    xtw.WriteEndElement();
                    this.NowProcess++;
                }
            }

            xtw.WriteEndElement();
            xtw.Close();
        }

        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="dset"></param>
        public void Read(string path, DataSet dset)
        {
            XmlTextReader xtr = new XmlTextReader(path);
            bool rowdatas = false;
            bool firstElementReaded = false;//<NewDataSet>是否已经读过了
            DataRow drow = null;
            DataTable dtable = null;
            string columnName = "";
            string rootName = "";
            string tablename = "";
            this.NowProcess = 0;
            while (xtr.Read())
            {

                XmlNodeType nodetype = xtr.NodeType;

                if (nodetype == XmlNodeType.Whitespace || nodetype == XmlNodeType.XmlDeclaration)
                    continue;
                if (nodetype == XmlNodeType.Element && !firstElementReaded)
                {
                    firstElementReaded = true;
                    rootName = xtr.Name;
                    this.RowCount = Convert.ToInt32(xtr.GetAttribute(0));
                    continue;
                }
                string name = xtr.Name;
                if (name == rootName && nodetype == XmlNodeType.EndElement)
                {
                    break;
                }
                if (nodetype == XmlNodeType.Element)
                {
                    #region XmlNodeType.Element
                    if (!rowdatas)
                    {
                        dtable = dset.Tables[name];
                        tablename = name;
                        drow = dtable.NewRow();
                        rowdatas = true;
                        continue;
                    }
                    else
                    {
                        columnName = name;
                    }
                    #endregion
                }
                else if (nodetype == XmlNodeType.Text)
                {
                    drow[columnName] = xtr.Value;
                }
                else if (nodetype == XmlNodeType.EndElement && tablename == name)
                {
                    dtable.Rows.Add(drow);
                    this.NowProcess++;
                    if (this.ProcessChanged != null)
                    {
                        if (this.NowProcess > this.RowCount)
                            this.NowProcess = this.RowCount;
                        this.ProcessChanged(this.RowCount, this.NowProcess, false, null, dset);
                    }
                    rowdatas = false;
                }

            }
            xtr.Close();
            if (this.ReadFinished != null)
            {
                this.ReadFinished(this.RowCount, this.RowCount, true, null, dset);
            }
            if (this.ProcessChanged != null)
            {
                this.ProcessChanged(this.RowCount, this.RowCount, true, null, dset);
            }
        }
    }
}
