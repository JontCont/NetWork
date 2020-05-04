<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Gobang.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
     <title>網頁五子棋</title>
    <script type="text/javascript" src="chk5.js"></script>
    <script type="text/javascript">
        var G;//繪圖物件(對應於canvas物件)
        var S; //XML通訊物件
        var A;//棋盤陣列(19X19)
        var Q;//下棋次序控制變數(true:輪到自己;false:輪到對手)
        function init() {
            S = new XMLHttpRequest();//建立通訊物件實體
            G = C.getContext("2d"); //建立繪圖物件(對應於canvas物件id=C))
            reset();//重設(清除)棋盤
            setInterval("Listen()", 250);//啟動定時監看對手下棋訊息
        }
        //重設棋盤
        function reset() {
            G.clearRect(0, 0, 570, 570); //清除棋盤(舊棋子圖案)
            A = new Array(19);//重設棋盤陣列
            for (var i = 0; i < 19 ; i++) {//宣告19個一維陣列
                A[i] = new Array(19);//成員數目19
                for (var j = 0; j < 19; j++) { A[i][j] = 0; }//設初值皆為0
            }
            Msg.innerHTML = "下棋囉！";//顯示可以下棋的訊息
            Q = true;//解除下棋鎖定
        }
        //點擊棋盤
        function md() {
            if (Q == false) return; //還沒輪到你下棋
            var x = Math.round((event.offsetX - 15) / 30) //棋格水平座標
            var y = Math.round((event.offsetY - 15) / 30) //棋格垂直座標
            if (A[x][y] != 0) return; //該處已有棋子，不能下(返回)
            url = "Default.aspx?A=" + x + "," + y;//設定回傳網址與下棋位置
            S.open("GET", url, true);//開啟連結
            S.send();//傳送資料
            chess(x, y, 1);//己方下黑棋
        }
        //定時監聽接收訊息執行繪圖動作
        function Listen() {
            if (document.getElementById("H").value == "") return;//無訊息
            var q = document.getElementById("H").value;//取得訊息
            document.getElementById("H").value = ""; //清除訊息，避免重複處理
            if (q == "X") {//重設棋盤
                reset();//重設
            } else {//對手下棋
                var p = q.split(","); //拆解訊息
                var x = parseInt(p[0]);//X座標
                var y = parseInt(p[1]);//Y座標
                chess(x, y, -1);//對手下白棋
            }
        }
        //下棋處理副程序
        function chess(x, y, st) {
            A[x][y] = st;//填寫棋格狀態(1:我方；-1:對手)
            switch (st) {
                case 1://我方下棋
                    plt(x, y, "black");//畫黑棋
                    Q = false;//鎖定棋盤不能再下
                    if (chk5(x, y, 1)) {//黑棋連線我方勝
                        Msg.innerHTML = "你贏了！";//勝負訊息
                    } else {//勝負未分
                        Msg.innerHTML = "換對手下...";//下棋序提示訊息
                    }
                    break;
                case -1://對手下棋
                    plt(x, y, "white");//畫白棋
                    if (chk5(x, y, -1)) {//白棋連線敵方勝
                        Msg.innerHTML = "你輸了！";//勝負訊息
                    } else {//未分勝負
                        Msg.innerHTML = "到你了！";//下棋序提示訊息
                        Q = true;//換我方下棋
                    }
                    break;
            }
        }
        //畫棋子的程式，i,j為座標(數字)，k為顏色(文字)
        function plt(i, j, k) {
            var x = i * 30 + 15;//繪圖X位置
            var y = j * 30 + 15;//繪圖Y位置
            G.beginPath(); //宣告路徑開始
            G.arc(x, y, 13, 0, Math.PI * 2, true); //設定圓心、半徑、角度與方向
            G.closePath(); //宣告路徑結束
            G.fillStyle = k//填黑或白色
            G.fill(); //執行填滿
            G.stroke(); //畫路徑框線
        }
    </script>
</head>
<body onload="init()">
    <form id="form1" runat="server">
<div style="text-align: center">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Timer ID="Timer1" runat="server" Interval="250" OnTick="Timer1_Tick">
                    </asp:Timer>
                    <asp:HiddenField ID="H" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <div id="Msg" style="width: 570px; text-align: center;">
            </div>
            <div style="width: 570px; height: 570px; background-image: url('bg.gif');">
                <canvas id="C" width="570" height="570" onmousedown="md()"></canvas>
            </div>
            <p style="width: 570px; text-align: center;">
                我是：<asp:TextBox ID="txtMe" runat="server" OnTextChanged="txtMe_TextChanged"></asp:TextBox>
                在跟：<asp:TextBox ID="txtTo" runat="server" OnTextChanged="txtTo_TextChanged"></asp:TextBox>
                玩&nbsp;
            <asp:Button ID="Button1" runat="server" Text="重玩" OnClick="Button1_Click" />
            </p>
        </div>
    </form>
</body>
</html>
