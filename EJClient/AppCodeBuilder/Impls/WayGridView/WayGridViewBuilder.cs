using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EJClient.AppCodeBuilder
{
    class WayGridViewBuilder : IAppCodeBuilder
    {
        public WayGridViewBuilder()
        {
            this.ControlId = "grid1";
            this.PageSize = 10;
        }

        public string ControlId
        {
            get;
            set;
        }
       
        public int PageSize
        {
            get;
            set;
        }
        [System.ComponentModel.Description("显示搜索区")]
        public bool ShowSearchArea
        {
            get;
            set;
        }
        [System.ComponentModel.Browsable(false)]
        public string Name
        {
            get
            {
                return "WayGridView";
            }
        }
      
        public List<SampleCode> Build(SampleColumn[] columns)
        {
            columns = columns.Where(m => m.IsChecked).ToArray();
            if (columns.Length == 0)
                return new List<SampleCode>();

            try
            {
                string templateFolderPath = AppDomain.CurrentDomain.BaseDirectory + "codeTemplate";
                if (System.IO.Directory.Exists(templateFolderPath) == false)
                    throw new Exception("缺乏模板codeTemplate文件夹，不能生成代码");

                var encode = System.Text.Encoding.GetEncoding("gb2312");

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

                        bodyTemplate = File.ReadAllText($"{templateFolderPath}ServerController.txt", encode);
                        itemTemplate = File.ReadAllText($"{templateFolderPath}ServerController.Item.txt", encode);


                        string datasource = $"{pathinfo[0]}.{pathinfo[1]}";

                        StringBuilder buffer = new StringBuilder();
                        buffer.AppendLine(@"    if (datasourceName == """+this.ControlId+@"_datasource"")
    {
        return """+ datasource + @""";
    }");
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

                    #region 搜索区
                    if(this.ShowSearchArea)
                    {
                        string htmlTemplate;
                       
                        codeitem = new AppCodeBuilder.SampleCode(150);
                        result.Add(codeitem);
                        codeitem.Name = "html实现搜索代码";

                        htmlTemplate = File.ReadAllText($"{templateFolderPath}html.Search.txt", encode);
                        string textBoxTemplate = File.ReadAllText($"{templateFolderPath}html.Search.Item.textbox.txt", encode);
                        string dropdownlistTemplate = File.ReadAllText($"{templateFolderPath}html.Search.Item.dropdownlist.txt", encode);


                        htmlTemplate = htmlTemplate.Replace("{@ControlId}", this.ControlId)
                            .Replace("{@SearchElementId}", this.ControlId + "_search");

                        StringBuilder itemBuffer = new StringBuilder();
                        foreach (var column in columns)
                        {
                            if (column.EnumDefine.IsNullOrEmpty() == false && column.dbType.Contains("int"))
                            {
                                MatchCollection matches = Regex.Matches(column.EnumDefine, @"(?<n>(\w)+)( )?\=");
                                StringBuilder options = new StringBuilder();
                                options.AppendLine($"<option value=\"\"></option>");
                                foreach (Match m in matches )
                                {
                                    options.AppendLine($"<option value=\"{m.Groups["n"].Value}\">{m.Groups["n"].Value}</option>");
                                }
                                itemBuffer.AppendLine(dropdownlistTemplate.Replace("{@Caption}", column.caption).Replace("{@Name}", column.Name).Replace("{@Options}", options.ToString()));
                            }
                            else
                            {
                                itemBuffer.AppendLine(textBoxTemplate.Replace("{@Caption}", column.caption).Replace("{@Name}", column.Name));
                            }
                        }
                       
                        htmlTemplate = htmlTemplate.Replace("{@Items}", itemBuffer.ToString());
                        codeitem.Code = htmlTemplate;
                    }
                    #endregion

                    #region html代码
                    if (true)
                    {
                        string htmlTemplate;
                        string itemTemplate;
                        string headerItemTemplate;
                        string footerItemTemplate;
                        codeitem = new AppCodeBuilder.SampleCode(350);
                        result.Add(codeitem);
                        codeitem.Name = "html代码";

                        htmlTemplate = File.ReadAllText($"{templateFolderPath}html.txt", encode);
                        itemTemplate = File.ReadAllText($"{templateFolderPath}html.Item.txt", encode);
                        headerItemTemplate = File.ReadAllText($"{templateFolderPath}html.HeaderItem.txt", encode);
                        footerItemTemplate = File.ReadAllText($"{templateFolderPath}html.FooterItem.txt", encode);

                      
                    
                        htmlTemplate = htmlTemplate.Replace("{@ControlId}", this.ControlId)
                            .Replace("{@PageSize}" , this.PageSize.ToString())
                            .Replace("{@ControlDatasource}", this.ControlId + "_datasource");
                        

                        if (this.ShowSearchArea)
                        {
                            htmlTemplate = htmlTemplate.Replace("{@SetSearchExpression}", this.ControlId + ".searchModel = WayDataBindHelper.dataBind(\""+this.ControlId+"_search\", {});");
                        }

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
