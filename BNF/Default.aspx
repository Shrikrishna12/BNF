<%@ Page Language="C#"  Title ="Authentication Page" AutoEventWireup="true" Inherits="_Default" Codebehind="Default.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 166px;
        }
    </style>
</head>
<body >
    <form id="form1" runat="server">
    <div>
    
        <table style="width:100%;">
            <tr>
                <td class="style1">
    
                    &nbsp;</td>
                <td align="left" valign="top">
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                    <br />
                    <asp:Label ID="lblMessage" runat="server" Font-Bold="False" Font-Italic="False" 
                        Font-Names="Arial" Text="This is a sample page to login..."></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style1">
    
                    &nbsp;</td>
                <td>
    
                    <img src="Images/Sample1.gif" /> </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style1">
                    &nbsp;</td>
                <td align="center">
    
        <asp:Button ID="cmdPost" runat="server" onclick="cmdPost_Click" 
                        Text="Click here to log in" />
                </td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
