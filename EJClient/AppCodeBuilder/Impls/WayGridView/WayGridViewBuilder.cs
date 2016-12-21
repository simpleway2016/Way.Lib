using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJClient.AppCodeBuilder
{
    class WayGridViewBuilder : IAppCodeBuilder
    {
        public string DefaultControlId
        {
            get
            {
                return "grid1";
            }
        }

        public string Name
        {
            get
            {
                return "WayGridView";
            }
        }
      
        public List<SampleCode> Build(SampleColumn[] columns, string controlId)
        {
            columns = columns.Where(m => m.IsChecked).ToArray();
            if (columns.Length == 0)
                return new List<SampleCode>();

            try
            {
                string templateFolderPath = AppDomain.CurrentDomain.BaseDirectory + "codeTemplate";
                if (System.IO.Directory.Exists(templateFolderPath) == false)
                    throw new Exception("缺乏模板codeTemplate文件夹，不能生成代码");

                using (var web = Helper.CreateWebService())
                {
                    string projectName = web.GetProjectNameByColumnId(columns[0].id.Value);

                    if (System.IO.Directory.Exists(templateFolderPath + "\\" + projectName) )
                    {
                        templateFolderPath = templateFolderPath + "\\" + projectName + "\\WayGridView\\";
                    }
                    else
                    {
                        templateFolderPath = System.IO.Directory.GetDirectories(templateFolderPath)[0] + "\\WayGridView\\";
                    }

                    List<SampleCode> result = new List<AppCodeBuilder.SampleCode>();
                    SampleCode codeitem;
                    string[] pathinfo = web.GetNamespacePathByColumnId(columns[0].id.Value);

                    #region 服务器Controller代码
                    if (true)
                    {
                        string bodyTemplate;
                        string itemTemplate;
                        codeitem = new AppCodeBuilder.SampleCode(150);
                        result.Add(codeitem);
                        codeitem.Name = "服务器Controller代码";

                        bodyTemplate = File.ReadAllText($"{templateFolderPath}ServerController.txt", System.Text.Encoding.UTF8);
                        itemTemplate = File.ReadAllText($"{templateFolderPath}ServerController.Item.txt", System.Text.Encoding.UTF8);


                        bodyTemplate = bodyTemplate.Replace("{@ControlId}", controlId);
                        bodyTemplate = bodyTemplate.Replace("{@ControlDatasource}", $"{pathinfo[0]}.{pathinfo[1]}");

                        StringBuilder buffer = new StringBuilder();
                        foreach (var column in columns)
                        {
                            if (column.RelaTableName.IsNullOrEmpty() == false && column.RelaColumnName.IsNullOrEmpty() == false && column.DisplayColumnName.IsNullOrEmpty() == false)
                            {

                                string mystr = itemTemplate;
                                mystr = mystr.Replace("{@ItemName}", column.RelaTableName);
                                mystr = mystr.Replace("{@ItemDatasource}", $"{pathinfo[0]}.{column.RelaTableName}");
                                buffer.AppendLine(mystr);
                            }
                        }
                        bodyTemplate = bodyTemplate.Replace("{@Items}", buffer.ToString());
                        codeitem.Code = bodyTemplate;
                    }
                    #endregion

                    #region html代码
                    if (true)
                    {
                        string htmlTemplate;
                        string itemTemplate;
                        string headerItemTemplate;
                        string footerItemTemplate;
                        codeitem = new AppCodeBuilder.SampleCode(250);
                        result.Add(codeitem);
                        codeitem.Name = "html代码";

                        htmlTemplate = File.ReadAllText($"{templateFolderPath}html.txt", System.Text.Encoding.UTF8);
                        itemTemplate = File.ReadAllText($"{templateFolderPath}html.Item.txt", System.Text.Encoding.UTF8);
                        headerItemTemplate = File.ReadAllText($"{templateFolderPath}html.HeaderItem.txt", System.Text.Encoding.UTF8);
                        footerItemTemplate = File.ReadAllText($"{templateFolderPath}html.FooterItem.txt", System.Text.Encoding.UTF8);

                    
                        htmlTemplate = htmlTemplate.Replace("{@ControlId}", controlId);

                        StringBuilder itemBuffer = new StringBuilder();
                        foreach (var column in columns)
                        {
                            itemBuffer.AppendLine( itemTemplate.Replace("{@Text}" , "{@"+column.Name+"}") );
                        }
                        StringBuilder headerBuffer = new StringBuilder();
                        foreach (var column in columns)
                        {
                            headerBuffer.AppendLine(itemTemplate.Replace("{@Text}", column.caption));
                        }
                        StringBuilder footerBuffer = new StringBuilder();
                        foreach (var column in columns)
                        {
                            footerBuffer.AppendLine(itemTemplate.Replace("{@Text}", column.caption));
                        }
                        htmlTemplate = htmlTemplate.Replace("{@Items}", itemBuffer.ToString());
                        htmlTemplate = htmlTemplate.Replace("{@HeaderItems}", headerBuffer.ToString());
                        htmlTemplate = htmlTemplate.Replace("{@FooterItems}", footerBuffer.ToString());
                        codeitem.Code = htmlTemplate;
                    }
                    #endregion

                    return result;
                }
            }
            catch(Exception ex)
            {
                Helper.ShowError(ex);
                return new List<AppCodeBuilder.SampleCode>();
            }
        }
    }
}
