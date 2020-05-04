<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebMSN.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Timer ID="Timer1" runat="server" Interval="2000" OnTick="Timer1_Tick">
                    </asp:Timer>
                    <asp:TextBox ID="TextBox1" runat="server" Rows="6" TextMode="MultiLine" Width="228px"></asp:TextBox>
                </ContentTemplate>
            </asp:UpdatePanel>
            <p>
                我是：<asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                &nbsp;要跟：<asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                &nbsp;
            </p>
            <p>
                講說：<asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="送出" OnClick="Button1_Click" Height="21px" />
            </p>
        </div>
    </form>
</body>
</html>
