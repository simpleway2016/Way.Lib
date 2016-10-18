<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DllSetting.aspx.cs" Inherits="ECWeb.WebForm.Interfaces.DllSetting" %>

<%@ Register Assembly="AppLib" Namespace="AppLib.Controls" TagPrefix="way" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>类库引用路径</title>
</head>
<body>
    <form id="form1" runat="server">
           <way:EntityToolbar ID="RYToolbar1" runat="server">
             <div style="padding-bottom:10px;">
                 <input type="button" value="添加" onclick="<%=editor.InsertJS%>" class="page_button" />&nbsp;
                 <input type="button" value="删除" onclick="<%=grid.DeleteJS%>" class="page_button_red" />
            </div>
        </way:EntityToolbar>

        <way:EntityGridView ID="grid" runat="server" AutoGenerateColumns="False" DatabaseConfig="ECWeb.EJDB" IDFieldName="id" ShowHeaderWhenEmpty="True" SqlOrderby="path" SqlTermString="" TableName="DLLImport">
            <Columns>
                <way:EntityCheckboxColumn DataField="id">
                    <ItemStyle CssClass="border-bottom-chk" />
                </way:EntityCheckboxColumn>
                <way:EntityGridViewColumn DataField="path" HeaderText="类库文件路径" Statistical="False">
                    <ItemStyle CssClass="border-bottom" />
                </way:EntityGridViewColumn>
            </Columns>
        </way:EntityGridView>
        <way:EntityEditor ID="editor" runat="server" DatabaseConfig="ECWeb.EJDB" IDFieldName="id" InsertSuccessedMessage="" TableName="DLLImport" UpdateSuccessedMessage="" OnBeforeSave="editor_BeforeSave">
            <!--
        子控件如果放置一个_Custom属性，如<asp:TextBox _Custom="1"></asp:TextBox>
        则表示在Save时候，不会自动把值存入数据库

        列表类型的控件，如listbox、dropdownlist,如果设置_sql="select id,name from table"，则自动从数据库绑定项
        -->
            <div class="dataEditor_container">
                <div class="dataEditor_title">
                    <div style="color:white;margin-top:10px;margin-left:10px;font-size:12px;font-weight:bold;">
                        <%=editor.CurrentActionDescription %>
                        <script lang="ja">

                       document.write(document.title);
                   </script>
                    </div>
                </div>
                <div class="dataEditor_content">
                    <div id="editor_Item_path" class="dataEditor_item" style="height:33px;">
                        <span style="width:120px" class="dataEditor_item_caption">类库文件路径： </span><span class="dataEditor_item_control">
                        <asp:TextBox ID="txt_path" runat="server" _Caption="dll文件路径" _DataField="path" CssClass="dataEditor_txt" onblur="this.className='dataEditor_txt';" onfocus="this.className='dataEditor_txt_focus';"></asp:TextBox>
                            <input type="button" id="btnSelect" value="选择..." />
                        <asp:RequiredFieldValidator ID="vd_path" ValidationGroup="editor" runat="server" ControlToValidate="txt_path" CssClass="vd" Display="Dynamic" ErrorMessage="请填写dll文件路径"></asp:RequiredFieldValidator>
                        </span>
                    </div>
                </div>
                <div class="dataEditor_footer" style="text-align:center;">
                    <input class="page_button" onclick="_EntityEditor_Save('editor')" style="width:90px;" type="button" value="保 存" />
                    <input class="page_button" onclick="_EntityEditor_Cancel('editor')" style="width:90px;margin-left:10px;" type="button" value="取消/返回" />
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
