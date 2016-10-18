<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="databasePower.aspx.cs" Inherits="ECWeb.WebForm.SetPower.databasePower" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
   <div style="padding:5px;">
        <asp:CheckBoxList ID="chkList" runat="server"></asp:CheckBoxList>
        
    </div>
        <asp:Button ID="btnOK" CssClass="page_button" runat="server" Text="确定" OnClick="btnOK_Click" />
    </form>
</body>
</html>
