using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace TCP_Tennis
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //公用變數
        Socket T;//通訊物件
        Thread Th;//網路監聽執行緒
        string User;//使用者
        int mdx;//拖曳球拍起點
        int oX = 0;//球拍位置

        //監聽 Server 訊息 (Listening to the Server)
        private void Listen()
        {
            EndPoint ServerEP = (EndPoint)T.RemoteEndPoint; //Server 的 EndPoint
            byte[] B = new byte[1023]; //接收用的 Byte 陣列
            int inLen = 0; //接收的位元組數目
            string Msg; //接收到的完整訊息
            string St; //命令碼
            string Str; //訊息內容(不含命令碼)
            while (true)//無限次監聽迴圈
            {
                try
                {
                    inLen = T.ReceiveFrom(B, ref ServerEP);//收聽資訊並取得位元組數
                }
                catch (Exception)
                {
                    T.Close();//關閉通訊器
                    ListBox1.Items.Clear();//清除線上名單
                    MessageBox.Show("伺服器斷線了！");//顯示斷線
                    Button1.Enabled = true;//連線按鍵恢復可用
                    Th.Abort();//刪除執行緒
                }
                Msg = Encoding.Default.GetString(B, 0, inLen); //解讀完整訊息
                St = Msg.Substring(0, 1); //取出命令碼 (第一個字)
                Str = Msg.Substring(1); //取出命令碼之後的訊息   
                switch (St)//依命令碼執行功能
                {
                    case "L"://接收線上名單
                        ListBox1.Items.Clear(); //清除名單
                        string[] M = Str.Split(','); //拆解名單成陣列
                        for (int i = 0; i < M.Length; i++)
                        {
                            ListBox1.Items.Add(M[i]); //逐一加入名單
                        }
                        break;
                    case "7"://對手球拍移動訊息
                        H2.Left = G.Width - int.Parse(Str) - H2.Width; //鏡射之後的位置
                        break;
                    case "8"://球的位置同步訊息
                        string[] C = Str.Split(',');//切割訊號
                        Q.Left = G.Width - int.Parse(C[0]) - Q.Width; //左右鏡射位置
                        Q.Top = G.Height - Q.Height - int.Parse(C[1]); //上下鏡射位置
                        break;
                }
            }
        }
        //送出訊息
        private void Send(string Str)
        {
            byte[] B = Encoding.Default.GetBytes(Str); //翻譯文字成Byte陣列
            T.Send(B, 0, B.Length, SocketFlags.None); //傳送訊息給伺服器
        }
        //關閉視窗離線
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Button1.Enabled == false)//如果已經上線
            {
                Send("9" + User); //傳送自己的離線訊息給伺服器
                T.Close(); //關閉網路通訊器
            }
        }
        //開始拖曳球拍
        private void H1_MouseDown(object sender, MouseEventArgs e)
        {
            mdx = e.X; //拖曳起點
        }
        //拖曳球拍中
        private void H1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                int X = H1.Left + e.X - mdx; //試算拖曳終點位置
                if (X < 0) X = 0; //不能超出左邊界
                if (X > G.Width - H1.Width) X = G.Width - H1.Width; //不能超出右邊界
                H1.Left = X; //設定X為球拍座標
                if (ListBox1.SelectedIndex >= 0)//有選取遊戲對手，上線遊戲中
                {
                    if (oX != H1.Left)//球拍已移動
                    {
                        Send("7" + H1.Left.ToString() + "|" + ListBox1.SelectedItem); //傳送球拍位置訊息
                        oX = H1.Left;//紀錄球拍新位置
                    }
                }
            }
        }
        //控制球移動的程式
        private void Timer1_Tick(object sender, EventArgs e)
        {
            Point V = (Point)Q.Tag; //取出速度
            Q.Left += V.X; //移動X
            Q.Top += V.Y;  //移動Y
            chkHit(Q, G, true);   //檢查與處理球與內牆的碰撞
            chkHit(Q, H1, false); //檢查與處理球與自己球拍的碰撞
            chkHit(Q, H2, false); //檢查與處理球與對手球拍的碰撞
            if (ListBox1.SelectedIndex >= 0) //有選取遊戲對手，上線遊戲中
            { //傳送球的位置
                Send("8" + Q.Left.ToString() + "," + Q.Top.ToString() + "|" + ListBox1.SelectedItem);
            }
        }
        //碰撞檢查程式
        private bool chkHit(Label B, object C, bool inside)
        {
            Point V = (Point)B.Tag;//自物件的Tag屬性取出速度值(V.x, V.y)
            if (inside)//球與牆壁的碰撞偵測
            {
                Panel p = (Panel)C;
                if (B.Right > p.Width)//右牆碰撞
                {
                    V.X = -Math.Abs(V.X);
                    B.Tag = V;
                    return true;
                }
                if (B.Left < 0)//左牆碰撞
                {
                    V.X = Math.Abs(V.X);
                    B.Tag = V;
                    return true;
                }
                if (B.Bottom > p.Height)//地板碰撞
                {
                    V.X = -Math.Abs(V.X);
                    B.Tag = V;
                    return true;
                }
                if (B.Top < 0)//屋頂碰撞
                {
                    V.X = Math.Abs(V.X);
                    B.Tag = V;
                    return true;
                }
                return false;//未發生碰撞
            }
            else//求羽球拍的碰撞偵測
            {
                Label k = (Label)C;
                if (B.Right < k.Left) return false;//球在物件之左確定未碰撞
                if (B.Left > k.Right) return false;//球在物件之右確定未碰撞
                if (B.Bottom < k.Top) return false;//球在物件之上確定未碰撞
                if (B.Top > k.Bottom) return false;//球在物件之下確定未碰撞
                //    目標左側碰撞
                if (B.Right >= k.Left && (B.Right - k.Left) <= Math.Abs(V.X)) V.X = -Math.Abs(V.X);
                //    目標右側碰撞
                if (B.Left <= k.Right && (k.Right - B.Left) <= Math.Abs(V.X)) V.X = Math.Abs(V.X);
                //    目標底部碰撞
                if (B.Top <= k.Bottom && (k.Bottom - B.Top) <= Math.Abs(V.Y)) V.Y = Math.Abs(V.Y);
                //    目標頂部碰撞
                if (B.Bottom >= k.Top && (B.Bottom - k.Top) <= Math.Abs(V.Y)) V.Y = -Math.Abs(V.Y);
                B.Tag = V;//紀錄球速度(方向)
                return true;//回應有發生碰撞
            }
        }
        //啟動遊戲
        private void GO_Click(object sender, EventArgs e)
        {
            Timer1.Stop();
            Q.Left = 209;
            Q.Top= 224;
            Q.Tag = new Point(5, -5); //預設速度(往右上)
            Timer1.Start(); //開始移動
        }
        //連線伺服器
        private void Button1_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false; //忽略跨執行緒操作的錯誤
            User = TextBox3.Text;  //使用者名稱
            string IP = TextBox1.Text;//伺服器IP
            int Port = int.Parse(TextBox2.Text);  //伺服器Port
            try
            {
                IPEndPoint EP = new IPEndPoint(IPAddress.Parse(IP), Port);//建立伺服器端點資訊
                //建立TCP通訊物件
                T = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                T.Connect(EP); //連上Server的EP端點(類似撥號連線)
                Th = new Thread(Listen); //建立監聽執行緒
                Th.IsBackground = true; //設定為背景執行緒
                Th.Start(); //開始監聽
                TextBox4.Text = "已連線伺服器！" + "\r\n";
                Send("0" + User); //隨即傳送自己的 UserName 給 Server
                Button1.Enabled = false; //讓連線按鍵失效，避免重複連線
            }
            catch
            {
                TextBox4.Text = "無法連上伺服器！" + "\r\n";  //連線失敗時顯示訊息
            }
        }
    }
}
