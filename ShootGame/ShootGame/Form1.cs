using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;//匯入網路通訊協定相關函數
using System.Net.Sockets;//匯入網路插座功能函數
using System.Threading;//匯入多執行緒功能函數
using System.Media;

namespace ShootGame
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
        bool Xbang;//拖曳球拍起點
        SoundPlayer player = new SoundPlayer();
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
                        for (int i = 0; i < M.Length; i++) ListBox1.Items.Add(M[i]); //加入名單
                        break;
                    case "3"://敵人移動槍枝
                        Q.Left = Panel1.Width - int.Parse(Str) - Q.Width; //左右顛倒
                        break;
                    case "4"://敵人開槍
                        Xbang = true; //樹立敵方開炮旗標
                        break;
                }
            }
        }

        //傳送訊息給 Server
        private void Send(string Str)
        {
            byte[] B = Encoding.Default.GetBytes(Str); //翻譯文字成Byte陣列
            T.Send(B, 0, B.Length, SocketFlags.None); //傳送訊息給伺服器
        }
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
                Th = new Thread(Listen)
                {
                    IsBackground = true //設定為背景執行緒
                }; //建立監聽執行緒
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Button1.Enabled == false)
            {
                Send("9" + User); //傳送自己的離線訊息給伺服器
                T.Close(); //關閉網路通訊器
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            foreach (Control c in Panel1.Controls)
            {
                string s = c.Tag.ToString();
                switch (s)
                {
                    case "B":
                        c.Top -= 5; //往上移動
                        if (c.Bottom < 0) c.Dispose(); //超出畫面子彈刪除
                        if (chkHit((Label)c, Q)) //如果擊中敵方飛機
                        {
                            c.Dispose(); //子彈刪除
                            Score.Text = (int.Parse(Score.Text) + 1).ToString(); //得分累加
                        }
                        break;
                    case "X":
                        c.Top += 5; //往下移動
                        if (c.Top > Panel1.Height) c.Dispose(); //超出畫面子彈刪除
                        break;
                }
            }
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            if (Xbang)//敵方開炮旗標豎起
            {
                XShot(); //繪製新砲火
                Xbang = false; //降下旗標
            }
        }
        //我的砲彈
        private void MyShot()
        {
            Label B = new Label
            {
                Tag = "B", //註記為我的子彈
                Width = 3,
                Height = 6,
                BackColor = Color.Red
            };//新子彈
            B.Left = P.Left + P.Width / 2 - B.Width / 2; //我的飛機中央
            B.Top = P.Top - B.Height; //貼齊機頭
            Panel1.Controls.Add(B); //加入表單的Panel1
        }
        //敵方砲彈
        private void XShot()
        {
            Label B = new Label
            {
                Tag = "X", //註記為我的子彈
                Width = 3,
                Height = 6,
                BackColor = Color.Gray
            };//新子彈
            B.Left = Q.Left + Q.Width / 2 - B.Width / 2; //我的飛機中央
            B.Top = Q.Top - B.Height; //貼齊機頭
            Panel1.Controls.Add(B); //加入表單的Panel1
        }
        //砲彈碰撞偵測程式
        private bool chkHit(Label B, PictureBox C)
        {
            if (B.Right < C.Left) return false; //子彈在物件之左(未碰撞)
            if (B.Left > C.Right) return false; //子彈在物件之右(未碰撞)
            if (B.Bottom < C.Top) return false; //子彈在物件之上(未碰撞)
            if (B.Top > C.Bottom) return false; //子彈在物件之下(未碰撞)
            return true;//已碰撞
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Form.CheckForIllegalCrossThreadCalls = false;
            Button2.Select(); //轉移焦點到Button2
            P.Left = 207;
            Q.Left = 207;
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Button2.Select();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Z:
                    if(P.Left>2)
                        P.Left -= 5; //左移
                    break;
                case Keys.X:
                     if(P.Left<467)
                        P.Left += 5; //右移
                    break;
                case Keys.Space:
                    MyShot(); //開槍
                    player.SoundLocation = Application.StartupPath + @"\\shoot.wav";
                    player.Play();
                    break;
            }
            TextBox4.Text = P.Left.ToString();
            if (ListBox1.SelectedIndex >= 0)//有選取遊戲對手，上線遊戲中
            {
                switch (e.KeyCode)
                {
                    case Keys.Z://移動飛機
                        Send("3" + P.Left.ToString() + "|" + ListBox1.SelectedItem); //傳送位置訊息
                        break;
                    case Keys.X://移動飛機
                        Send("3" + P.Left.ToString() + "|" + ListBox1.SelectedItem); //傳送位置訊息
                        break;
                    case Keys.Space://開槍
                        Send("4" + "S" + "|" + ListBox1.SelectedItem); //傳送開槍訊息
                        break;
                }
                Button2.Select(); //轉移焦點到Button2
            }
        }
    }
}
