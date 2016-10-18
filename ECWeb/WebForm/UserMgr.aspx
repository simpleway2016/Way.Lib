<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserMgr.aspx.cs" Inherits="ECWeb.WebForm.UserMgr" %>

<%@ Register assembly="AppLib" namespace="AppLib.Controls" tagprefix="way" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>用户</title>
    <script type="text/javascript" src="/inc/js/WayJsDialog.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <script lang="ja">
        var dialog = new WayJsDialog();
        function setProjectPower(userid)
        {
            dialog.show("setpower/projectPower.aspx?userid=" + userid, "设置项目权限", 300, 300);
         }
        function setDBPower(userid) {
            dialog.show("setpower/databasePower.aspx?userid=" + userid, "设置数据库权限", 300, 300);
        }
        function setTablePower(userid) {
            dialog.show("setpower/tablePower.aspx?userid=" + userid, "设置数据表权限", 500, 500);
        }
        function setIMPower(userid) {
            dialog.show("setpower/SetIMPower.aspx?userid=" + userid, "设置接口说明权限", 500, 500);
        }
    </script>
        <div style="panel_projectList"></div>
        <way:EntityGridViewSearch ID="EntityGridViewSearch1" runat="server">
            <div class="Search">
                <div class="SearchItemT">
                    Name
                </div>
                <div class="SearchItemC">
                    <asp:TextBox ID="txt_search_Name" runat="server" _DataField="Name" _Search="1" CssClass="search_control"></asp:TextBox>
                </div>
                &nbsp;&nbsp;<input class="page_button" onclick="_EntityGridView_Search('EntityGridView1')" type="button" value="查询" />
            </div>
        </way:EntityGridViewSearch>

        <way:EntityToolbar ID="RYToolbar1" runat="server">
             <div style="padding-bottom:10px;">
                 <input type="button" value="添加" onclick="<%=EntityEditor1.InsertJS%>" class="page_button" />&nbsp;
                 <input type="button" value="删除" onclick="<%=EntityGridView1.DeleteJS%>" class="page_button_red" />
            </div>
        </way:EntityToolbar>

        <way:EntityGridView ID="EntityGridView1" runat="server" AutoGenerateColumns="False" DatabaseConfig="ECWeb.EJDB" GridSearch="EntityGridViewSearch1" IDFieldName="id" ShowHeaderWhenEmpty="True" SqlOrderby="" SqlTermString="" TableName="User">
            <Columns>
                <way:EntityCheckboxColumn DataField="id">
                    <ItemStyle CssClass="border-bottom-chk" />
                </way:EntityCheckboxColumn>
                <way:EntityGridViewColumn DataField="Role" HeaderText="角色" Statistical="False">
                    <ItemStyle CssClass="border-bottom" />
                </way:EntityGridViewColumn>
                <way:EntityGridViewColumn DataField="Name" HeaderText="Name" Statistical="False">
                    <ItemStyle CssClass="border-bottom" />
                </way:EntityGridViewColumn>
                <asp:TemplateField>
                     <ItemStyle CssClass="border-bottom" />
                    <ItemTemplate>
                        <a href="javascript:" onclick="<%#EntityEditor1.GetModifyDataJS(Eval("id")) %>">编辑</a>&nbsp;
                        <a href="javascript:" onclick="setProjectPower(<%#Eval("id") %>)">Project权限</a>&nbsp;
                        <a href="javascript:" onclick="setDBPower(<%#Eval("id") %>)">Database权限</a>&nbsp;
                         <a href="javascript:" onclick="setTablePower(<%#Eval("id") %>)">DBTable权限</a>&nbsp;
                         <a href="javascript:" onclick="setIMPower(<%#Eval("id") %>)">Interface权限</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </way:EntityGridView>
        <way:EntityEditor ID="EntityEditor1" runat="server" DatabaseConfig="ECWeb.EJDB" IDFieldName="id" InsertSuccessedMessage="" TableName="User" UpdateSuccessedMessage="">
            <!--
        子控件如果放置一个_Custom属性，如<asp:TextBox _Custom="1"></asp:TextBox>
        则表示在Save时候，不会自动把值存入数据库

        列表类型的控件，如listbox、dropdownlist,如果设置_sql="select id,name from table"，则自动从数据库绑定项
        -->
            <div class="dataEditor_container">
                <div class="dataEditor_title">
                    <div style="color:white;margin-top:10px;margin-left:10px;font-size:12px;font-weight:bold;">
                        <%=EntityEditor1.CurrentActionDescription %>
                        <script lang="ja">

                       document.write(document.title);
                   </script>
                    </div>
                </div>
                <div class="dataEditor_content">
                    <div id="EntityEditor1_Item_Role" class="dataEditor_item" style="height:33px;">
                        <span class="dataEditor_item_caption">角色： </span><span class="dataEditor_item_control">
                        <asp:DropDownList ID="lst_Role" runat="server" _Caption="角色" _DataField="Role"  CssClass="dataEditor_txt">
                            
                            <asp:ListItem Value="开发人员">开发人员</asp:ListItem>
                            <asp:ListItem Value="客户端测试人员">客户端测试人员</asp:ListItem>
                            <asp:ListItem Value="数据库设计师">数据库设计师</asp:ListItem>
                            <asp:ListItem Value="管理员">管理员</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div id="EntityEditor1_Item_Name" class="dataEditor_item" style="height:33px;">
                        <span class="dataEditor_item_caption">Name： </span><span class="dataEditor_item_control">
                        <asp:TextBox ID="txt_Name" runat="server" _Caption="Name" _DataField="Name" CssClass="dataEditor_txt" onblur="this.className='dataEditor_txt';" onfocus="this.className='dataEditor_txt_focus';"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="vd_Name" runat="server"  ValidationGroup="EntityEditor1" ControlToValidate="txt_Name" CssClass="vd" Display="Dynamic" ErrorMessage="请填写Name"></asp:RequiredFieldValidator>
                        </span>
                    </div>
                    <div id="EntityEditor1_Item_Password" class="dataEditor_item" style="height:33px;">
                        <span class="dataEditor_item_caption">Password： </span><span class="dataEditor_item_control">
                        <asp:TextBox ID="txt_Password" runat="server" _Caption="Password" _DataField="Password" CssClass="dataEditor_txt" onblur="this.className='dataEditor_txt';" onfocus="this.className='dataEditor_txt_focus';"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="vd_Password"  ValidationGroup="EntityEditor1" runat="server" ControlToValidate="txt_Password" CssClass="vd" Display="Dynamic" ErrorMessage="请填写Password"></asp:RequiredFieldValidator>
                        </span>
                    </div>
                </div>
                <div class="dataEditor_footer" style="text-align:center;">
                    <input class="page_button" onclick="_EntityEditor_Save('EntityEditor1')" style="width:90px;" type="button" value="保 存" />
                    <input class="page_button" onclick="_EntityEditor_Cancel('EntityEditor1')" style="width:90px;margin-left:10px;" type="button" value="取消/返回" />
                </div>
            </div>
            <script lang="ja">

    function _dataEditor_onload() {
        var divs = document.getElementsByClassName("dataEditor_container"); 
        for (var i = 0 ; i < divs.length ; i++) {
            divs[i].style.marginLeft =( document.documentElement.clientWidth - divs[i].offsetWidth)/2 + "px";
            divs[i].style.marginTop = "50px";
            divs[i].style.visibility = "visible";
        }

    }
    if (window.addEventListener)
        window.addEventListener("load", _dataEditor_onload, false);
    else
        window.attachEvent("onload", _dataEditor_onload);
</script>
        </way:EntityEditor>
    </form>
</body>
</html>
