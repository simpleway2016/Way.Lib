<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DatabaseEditor.aspx.cs" Inherits="ECWeb.WebForm.DatabaseEditor" %>

<%@ Register Assembly="AppLib" Namespace="AppLib.Controls" TagPrefix="way" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <way:EntityEditor ID="EntityEditor1" runat="server" DatabaseConfig="ECWeb.EJDB" IDFieldName="id" InsertSuccessedMessage="" TableName="Databases" OnBeforeSave="EntityEditor1_BeforeSave1" UpdateSuccessedMessage="修改成功!">
            <div class="m">
                <div id="EntityEditor1_Item_Name" class="hang h30">
                    <div class="title">
                        数据库名称：</div>
                    <asp:TextBox ID="txt_Name" runat="server" _Caption="Name" _DataField="Name" Width="96%" CssClass="input"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="d1" runat="server" ControlToValidate="txt_Name" CssClass="validator" ErrorMessage="请填写数据库名称" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>

               </div>
               
                <div id="EntityEditor1_Item_dbType" class="hang h30">
                    <div class="title">
                        数据库类型：</div>
                    <asp:DropDownList ID="selDBType" AutoPostBack="true" OnSelectedIndexChanged="selDBType_SelectedIndexChanged" runat="server" _Caption="数据库类型" Width="300px" _DataField="dbType" CssClass="input"></asp:DropDownList>
                </div>
                <div id="EntityEditor1_Item_conStr" class="hang h30">
                    <div class="title">
                        连接字符串：</div>
                    <asp:TextBox ID="txt_conStr" runat="server" _Caption="连接字符串" Text="server=;uid=sa;pwd=;database=" Width="96%" _DataField="conStr" CssClass="input"></asp:TextBox>
               <asp:RequiredFieldValidator ID="e2" ControlToValidate="txt_conStr" runat="server" CssClass="validator" ErrorMessage="请填写连接字符串" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>

                     </div>
                <div id="EntityEditor1_Item_dllPath" class="hang h30">
                    <div class="title">
                        dll生成文件夹：</div>
                    <asp:TextBox ID="txt_dllPath" runat="server" _Caption="dll生成文件夹" Width="96%" _DataField="dllPath" CssClass="input"></asp:TextBox>
                <input type="button" id="btnSelectFolder" value="选择..." />
                    <asp:RequiredFieldValidator ID="e3" ControlToValidate="txt_dllPath" runat="server" CssClass="validator" ErrorMessage="请填写dll生成文件夹" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>

                </div>
                 <div id="Div1" class="hang h30">
                    <div class="title">
                        NameSpace：</div>
                    <asp:TextBox ID="txtNamespace" runat="server" _Caption="NameSpace" _DataField="NameSpace" Width="96%" CssClass="input"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNamespace" CssClass="validator" ErrorMessage="请填写NameSpace" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>

               </div>
                <div class="hang h30">
                    <div class="title">
                    </div>
                    <asp:Button ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click" />
                </div>
            </div>
        </way:EntityEditor>
    </div>

        <input type="button" id="btnSelectFolder" style="display:none;"/>
        <input type="button" id="btnClose" style="display:none;"/>
    </form>
</body>
</html>
