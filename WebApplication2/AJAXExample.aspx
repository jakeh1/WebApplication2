<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AJAXExample.aspx.cs" Inherits="WebApplication2.Views.AJAXExample" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Random Number Genrator</title>
    <link href="~/Content/MyStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <header>Platteville Bakery</header>
    <form id="form1" runat="server" >
        
        <asp:Label ID="heading_lable" runat="server" Text="Random Number Genrator" style="font-size: xx-large; font-style: normal"></asp:Label>
        <br />
        <br />
        <br />
        <br />
       
            
        
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
 
        <ContentTemplate>
            <br />
            <br />
            <asp:Label ID="Label1" runat="server" >Random Number Genrator</asp:Label>
            <br />
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Start" />
            <br />
            <br />
            <asp:Label ID="Label2" runat="server">Number</asp:Label>
            <br />
            <br />
        </ContentTemplate>
 
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
        </Triggers>
 
        </asp:UpdatePanel>
 
        <asp:Timer ID="Timer1" runat="server" Interval="3000" OnTick="Timer1_Tick">
        </asp:Timer>

       
        
    <footer>Copyright &copy 2021 by UW-Platteville</footer>     
       
    </form>
    </body>
</html>
