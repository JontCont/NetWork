<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebDaub.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
     <title>網頁白板</title>
    <script type="text/javascript">
        var draw = false; //繪圖狀態
        var G; //繪圖物件
        var Q; //繪圖紀錄字串
        var S; //XML通訊物件

        //初始設定
        function init() {
            S = new XMLHttpRequest();//建立通訊物件
            G = m.getContext("2d"); //建立繪圖物件
            Listen(); //監聽遠端繪圖資訊
        }
        //監看遠端繪圖動作
        function Listen() {
            if (document.getElementById("H").value != "") {//遠端傳來繪圖動作
                var z = document.getElementById("H").value.split("/"); //拆解為座標陣列
                document.getElementById("H").value = ""; //清除資訊
                var p = z[0].split(","); //拆解起點的X與Y座標
                G.moveTo(p[0], p[1]); //設定繪圖起點
                for (var i = 1; i < z.length - 1; i++) {//依座標陣列連續繪製線段
                    var q = z[i].split(","); //拆解座標點的X與Y座標
                    G.lineTo(q[0], q[1]); //繪製線段
                }
                G.stroke(); //繪入畫布
            }
            setTimeout("Listen()", 200); //0.2秒之後再次檢視外來資訊
        }
        //開始繪圖
        function md() {
            G.moveTo(event.offsetX, event.offsetY); //繪圖起點
            draw = true; //進入繪圖狀態
            Q = event.offsetX + "," + event.offsetY + "/"; //紀錄繪圖起點
        }
        //繪圖中
        function mv() {
            if (draw) {//如為繪圖狀態
                G.lineTo(event.offsetX, event.offsetY); //定義終點
                G.stroke(); //繪圖
                Q += event.offsetX + "," + event.offsetY + "/"; //紀錄繪圖點座標
            }
        }
        //繪圖結束
        function mup() {
            draw = false; //結束繪圖狀態
            var url = "Default.aspx?A=" + Q;//設定回傳網址與資訊
            S.open("GET", url, true);//開啟連結
            S.send();//傳送資料到伺服端
        }
    </script>
</head>
<body onload="init()">
    <form id="form1" runat="server">
          <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Timer ID="Timer1" runat="server" Interval="500" OnTick="Timer1_Tick">
                </asp:Timer>
                <asp:HiddenField ID="H" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <p>
            我是：<asp:TextBox ID="TextBox1" runat="server" OnTextChanged="TextBox1_TextChanged"></asp:TextBox>
            畫給：<asp:TextBox ID="TextBox2" runat="server" Style="height: 19px" OnTextChanged="TextBox2_TextChanged"></asp:TextBox>
            看
        </p>
    </form>
        <canvas id="m" width="400" height="300" onmousedown="md()" onmousemove="mv()"
        onmouseup="mup()" style="border: thin solid #000000"></canvas>
</body>
</html>
