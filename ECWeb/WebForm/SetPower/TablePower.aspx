<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TablePower.aspx.cs" Inherits="ECWeb.WebForm.SetPower.TablePower" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TreeView ID="TreeView1" runat="server" ImageSet="Simple" PopulateNodesFromClient="False" ShowLines="True"></asp:TreeView>
    </div>
         <asp:Button ID="btnOK" CssClass="page_button" runat="server" Text="确定" OnClick="btnOK_Click" />
    </form>
</body>
</html>
