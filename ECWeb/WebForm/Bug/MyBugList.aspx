<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyBugList.aspx.cs" Inherits="ECWeb.WebForm.Bug.MyBugList" %>

<%@ Register Assembly="AppLib" Namespace="AppLib.Controls" TagPrefix="way" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style>
        .red {
        background-color:#fba5a5;
        }
         .blue {
        background-color:#9ccfe5;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <script lang="ja">
            function bind() {
                document.getElementById("<%=btnRebind.ClientID%>").click();
            }
            function setHandler(id, clientid) {
                var list = eval("js_" + clientid);
                document.getElementById("<%=hidName.ClientID%>").value = list.getText();
            if (list.getText() == "") {
                alert("请选择负责人");
                return false;
            }
            return true;
        }
    </script>
        <div style="display: none;">
            <asp:Button runat="server" ID="btnRebind" OnClick="btnRebind_Click" />
        </div>
        <asp:HiddenField ID="hidName" runat="server" />
        <way:JSDataSource ID="userListSource" runat="server"></way:JSDataSource>
        <way:EntityGridView ID="EntityGridView1" runat="server" AutoGenerateColumns="False" DatabaseConfig="ECWeb.EJDB_Check" IDFieldName="id" ShowHeaderWhenEmpty="True" SqlOrderby="Status,SubmitTime" SqlTermString="" TableName="MyBugList" AspNetPager="Pager1" OnCellDataBound="EntityGridView1_CellDataBound" OnRowCommand="EntityGridView1_RowCommand" OnRowDataBound="EntityGridView1_RowDataBound">
            <Columns>
                 <way:EntityGridViewColumn DataField="id" HeaderText="编号" Statistical="False">
                    <ItemStyle CssClass="border-bottom" />
                </way:EntityGridViewColumn>
                <way:EntityGridViewColumn DataField="SubmitUserID" HeaderText="提交者" Statistical="False" KeyIDField="id" KeyTableName="User" KeyTextField="Name">
                    <ItemStyle CssClass="border-bottom" />
                </way:EntityGridViewColumn>
                 <way:EntityGridViewColumn DataField="Title" HeaderText="标题" Statistical="False">
                    <ItemStyle CssClass="border-bottom" />
                </way:EntityGridViewColumn>
                <way:EntityGridViewColumn DataField="SubmitTime" HeaderText="提交时间" Statistical="False">
                    <ItemStyle CssClass="border-bottom" />
                </way:EntityGridViewColumn>
                <way:EntityGridViewColumn DataField="HandlerID" HeaderText="负责人" Statistical="False" KeyIDField="id" KeyTableName="User" KeyTextField="Name">
                    <ItemStyle CssClass="border-bottom" />
                </way:EntityGridViewColumn>
                <way:EntityGridViewColumn DataField="LastDate" HeaderText="最后反馈时间" Statistical="False">
                    <ItemStyle CssClass="border-bottom" />
                </way:EntityGridViewColumn>
                <way:EntityGridViewColumn DataField="FinishTime" HeaderText="处理完毕时间" Statistical="False">
                    <ItemStyle CssClass="border-bottom" />
                </way:EntityGridViewColumn>
                <way:EntityGridViewColumn DataField="Status" HeaderText="当前状态" Statistical="False">
                    <ItemStyle CssClass="border-bottom" />
                </way:EntityGridViewColumn>
                <asp:TemplateField HeaderText="操作">
                    <ItemStyle CssClass="border-bottom" />
                    <ItemTemplate>
                        <way:TextBoxList ID="selHandler" JSDataSource="userListSource" Visible="false" runat="server"></way:TextBoxList>
                        <asp:Button runat="server" CommandName="setHandler" Visible="false" ID="btnSetHandler" Text="设置负责人"/>
                        
                        <input type='button' runat="server" id="btnView" style="display:none;" _type='发起者' value='查看'/>
                        <asp:Button runat="server" CommandName="del" Visible="false" OnClientClick="return confirm('确定删除吗？')" ID="btnDelete" Text="删除"/>
                        
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </way:EntityGridView>
        <way:Pager ID="Pager1" runat="server"></way:Pager>
    </form>
</body>
</html>
