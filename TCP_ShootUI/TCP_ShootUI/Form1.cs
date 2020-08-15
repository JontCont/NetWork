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

namespace TCP_ShootUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }




        #region Connect
        
        Socket T;
        Thread Th;
        
        private void Send(string Str)
        {
            try
            {
                byte[] B = Encoding.Default.GetBytes(Str); //翻譯文字成Byte陣列
                if (T != null) T.Send(B, 0, B.Length, SocketFlags.None); //傳送訊息給伺服器
            }
            catch { }
        }
        private void Connect(string IP, string Port, string Name)
        {
            Control.CheckForIllegalCrossThreadCalls = false; //忽略跨執行緒操作的錯誤
            try
            {
                IPEndPoint EP = new IPEndPoint(IPAddress.Parse(IP), int.Parse(Port));//建立伺服器端點資訊
                T = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                T.Connect(EP);
                Th = new Thread(Listen) { IsBackground = true };
                Th.Start();//開始監聽
                TxtSever.AppendText("(系統) : 已連線伺服器！ \r\n");
                Send("L" + Name);
            }
            catch
            {
                TxtSever.AppendText("(系統) : " + Name + " 無法連上伺服器！ \r\n");
                this.Close();
            }
        }
        private void Listen()
        {
            EndPoint ServerEP = (EndPoint)T.RemoteEndPoint;
            byte[] B = new byte[2048];
            int inLen = 0; //接收的位元組數目
            while (true)
            {
                try
                {
                    inLen = T.ReceiveFrom(B, ref ServerEP);//收聽資訊並取得位元組數
                }
                catch (Exception)
                {
                    T.Close();
                    listBox1.Items.Clear();//清除線上名單
                    MessageBox.Show("伺服器斷線了！", "注意", MessageBoxButtons.OK, MessageBoxIcon.Information);//顯示斷線
                    Exit_btn_Click(this, new EventArgs());
                    Th.Abort();//刪除執行緒
                }
                Recetor(B, inLen);
            }
        }
        private void Recetor(byte[] B, int inLen)
        {
            string Msg = Encoding.Default.GetString(B, 0, inLen); //解讀完整訊息
            string St = Msg.Substring(0, 1); //取出命令碼 (第一個字)
            string Str = Msg.Substring(1); //取出命令碼之後的訊息   
            switch (St)//依命令碼執行功能
            {
                case "L"://接收線上名單
                    listBox1.Items.Clear();
                    string[] M = Str.Split(','); //拆解名單成陣列
                    for (int i = 0; i < M.Length; i++)
                    {
                        listBox1.Items.Add(M[i]);
                    } //逐一加入名單
                    break;
                case "9": //接收離開玩家
                    TxtSever.AppendText("(系統) :" + Str + " 已離開伺服器！ \r\n");
                    listBox1.Items.Remove(Str);
                    break;
                case "3": TxtSever.AppendText("(私密)" + Str + "\r\n"); break;//私密訊息

                case "D": TxtSever.AppendText("(公開)" + Str + "\r\n"); break;//聊天室
                case "M": TxtPort.AppendText(Str + "\r\n"); break; //猜題未答對


            }
        } //接收ServerC回傳
        private void Con_btn_Click(object sender, EventArgs e)
        {
            Connect(TxtSever.Text,TxtPort.Text,TxtName.Text);
        }
        private void Create_btn_Click(object sender, EventArgs e)
        {
            FmServer fmServer = new FmServer();
            fmServer.Show();
        }
        #endregion


        private void Exit_btn_Click(object sender, EventArgs e)
        {
            try
            {
                Send("9" + TxtName); //傳送自己的離線訊息給伺服器
                if (T != null) T.Close(); //關閉網路通訊器  
                this.Close();
                Application.Exit();
            }
            catch
            { }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (Control c in panel3.Controls)
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
                            //Score.Text = (int.Parse(Score.Text) + 1).ToString(); //得分累加
                        }
                        break;
                    case "X":
                        c.Top += 5; //往下移動
                        if (c.Top > panel3.Height) c.Dispose(); //超出畫面子彈刪除
                        break;
                }
            }
        }

        //string User;//使用者
        bool Xbang;//拖曳球拍起點
        SoundPlayer player = new SoundPlayer();

        private bool chkHit(Label B, PictureBox C)
        {
            if (B.Right < C.Left) return false; //子彈在物件之左(未碰撞)
            if (B.Left > C.Right) return false; //子彈在物件之右(未碰撞)
            if (B.Bottom < C.Top) return false; //子彈在物件之上(未碰撞)
            if (B.Top > C.Bottom) return false; //子彈在物件之下(未碰撞)
            return true;//已碰撞
        }
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
            panel3.Controls.Add(B); //加入表單的Panel1
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (Xbang)//敵方開炮旗標豎起
            {
                XShot(); //繪製新砲火
                Xbang = false; //降下旗標
            }
        }
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
            panel3.Controls.Add(B); //加入表單的Panel1
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Form.CheckForIllegalCrossThreadCalls = false;
            //Button2.Select(); //轉移焦點到Button2
            P.Left = 180;
            Q.Left = 180;
            panel3.Width = 410;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (P.Left > 2)
                        P.Left -= 10; //左移
                    break;
                case Keys.Right:
                    if (P.Left < 355)
                        P.Left += 10; //右移
                    break;
                case Keys.Space:
                    MyShot(); //開槍
                    player.SoundLocation = Application.StartupPath + @"\\shoot.wav";
                    player.Play();
                    break;
            }
            if (listBox1.SelectedIndex >= 0)//有選取遊戲對手，上線遊戲中
            {
                switch (e.KeyCode)
                {
                    case Keys.Z://移動飛機
                        Send("3" + P.Left.ToString() + "|" + listBox1.SelectedItem); //傳送位置訊息
                        break;
                    case Keys.X://移動飛機
                        Send("3" + P.Left.ToString() + "|" + listBox1.SelectedItem); //傳送位置訊息
                        break;
                    case Keys.Space://開槍
                        Send("4" + "S" + "|" + listBox1.SelectedItem); //傳送開槍訊息
                        break;
                }
               // Button2.Select(); //轉移焦點到Button2
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Button2.Select();
        }
    }
}
