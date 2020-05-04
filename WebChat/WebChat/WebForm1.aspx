<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebChat.WebForm1" %>

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
                        <br />
                        線上名單<br />
                        <asp:ListBox ID="ListBox1" runat="server" Width="100px"></asp:ListBox>
                        <br />
                        <br />
                        聊天看板<br />
                        <asp:TextBox ID="TextBox1" runat="server" Rows="7" TextMode="MultiLine" Width="200px"></asp:TextBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <p>
                    我是：<asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                    <asp:Button ID="Button1" runat="server" Text="登入" OnClick="Button1_Click" />
                </p>
                <p>
                    想說：<asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                    <asp:Button ID="Button2" runat="server" Text="發言" OnClick="Button2_Click" />
                </p>
            <br />

    </div>
    </form>
</body>
</html>
