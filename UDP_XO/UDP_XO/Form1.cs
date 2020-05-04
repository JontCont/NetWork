using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace UDP_XO
{
    public partial class Form1 : Form
    {
        UdpClient U;//UDP監聽器
        Thread Th;//監聽執行緒
        bool T = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false; //忽略跨執行緒操作的錯誤
            Th = new Thread(Listen); //建立新執行緒
            Th.Start(); //啟動監聽執行緒
            Label2.Text += " " + MyIP();//顯示本機IP於標題列
            //B1~B8按鍵共用B0的事件副程序
            this.B1.Click += new System.EventHandler(this.B0_Click);
            this.B2.Click += new System.EventHandler(this.B0_Click);
            this.B3.Click += new System.EventHandler(this.B0_Click);
            this.B4.Click += new System.EventHandler(this.B0_Click);
            this.B5.Click += new System.EventHandler(this.B0_Click);
            this.B6.Click += new System.EventHandler(this.B0_Click);
            this.B7.Click += new System.EventHandler(this.B0_Click);
            this.B8.Click += new System.EventHandler(this.B0_Click);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Th.Abort(); //關閉監聽執行緒
                U.Close(); //關閉監聽器
            }
            catch
            {
                //如果監聽執行緒沒開會出現錯誤，程式跳到此處執行，
                //此處不寫程式就是忽略錯誤，程式繼續執行的意思！
            }
        }
        //監聽副程序
        private void Listen()
        {
            int Port = int.Parse("8000"); //設定監聽用的通訊埠
            Label5.Text = "My Port : " + Port;
            U = new UdpClient(Port); //建立UDP監聽器
            IPEndPoint EP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port); //建立監聽端點資訊
            while (true) //持續監聽的無限迴圈→有訊息(True)就處理，無訊息就等待！
            {
                byte[] B = U.Receive(ref EP);//接收網路訊息
                string A = Encoding.Default.GetString(B);//翻譯為棋盤狀態字串
                if (chk(A)) MessageBox.Show("你贏了！","訊息");//如果有連線
                char[] C = A.ToCharArray();//拆解字串為字元陣列
                for (int i = 0; i < 9; i++)
                {
                    switch (C[i])//XO互換
                    {
                        case 'O':
                            C[i] = 'X'; break;
                        case 'X':
                            C[i] = 'O'; break;
                    }
                    //取得"Bi"物件的參考
                    Button D = (Button)this.Controls["B" + i.ToString()];//取得棋格物件
                    D.Tag = C[i];//註記圈叉
                    switch (C[i])//繪製圈叉
                    {
                        case '_'://空格
                            D.BackgroundImage = Properties.Resources.E;
                            break;
                        case 'X'://畫叉
                            D.BackgroundImage = Properties.Resources.X;
                            break;
                        case 'O'://畫圈
                            D.BackgroundImage = Properties.Resources.O;
                            break;
                    }
                }
                T = true;//切換下棋序
            }
        }
        //檢查圈圈是否連線
        private bool chk(string A)
        {
            char[] C = A.ToCharArray();//拆解為字元陣列
            if (C[0] == 'O' && C[1] == 'O' && C[2] == 'O') { return true; }
            if (C[3] == 'O' && C[4] == 'O' && C[5] == 'O') { return true; }
            if (C[6] == 'O' && C[7] == 'O' && C[8] == 'O') { return true; }
            if (C[0] == 'O' && C[3] == 'O' && C[6] == 'O') { return true; }
            if (C[1] == 'O' && C[4] == 'O' && C[7] == 'O') { return true; }
            if (C[2] == 'O' && C[5] == 'O' && C[8] == 'O') { return true; }
            if (C[0] == 'O' && C[4] == 'O' && C[8] == 'O') { return true; }
            if (C[2] == 'O' && C[4] == 'O' && C[6] == 'O') { return true; }
            return false;//未連線
        }
        //找出本機IP
        private string MyIP()
        {
            string hn = Dns.GetHostName();
            IPAddress[] ip = Dns.GetHostEntry(hn).AddressList; //取得本機IP陣列
            foreach (IPAddress it in ip)
            {
                if (it.AddressFamily == AddressFamily.InterNetwork)
                {
                    return it.ToString();//如果是IPv4回傳此IP字串
                }
            }
            return ""; //找不到合格IP回傳空字串
        }

        private void B0_Click(object sender, EventArgs e)
        {
            if (T == false) return;//未輪到下棋跳出副程序
            Button B = (Button)sender;//取得驅動事件的按鍵參考
            if (B.Tag.ToString() != "_") return; //不是空格跳出副程序
            B.BackgroundImage = Properties.Resources.O;//畫圈
            B.Tag = "O";//註記為"O"
            string A = "";//重建棋盤狀態字串(所有按鍵的Tag值)
            for (int i = 0; i < 9; i++)
            {
                A += this.Controls["B" + i.ToString()].Tag;
            }
            if (chk(A))
            {
                if (chk(A)) MessageBox.Show("你輸了！","訊息");//如果有連線
            }
            int Port = int.Parse(TextBox2.Text);//取得通訊埠
            UdpClient S = new UdpClient(TextBox1.Text, Port);//建立通訊物件
            byte[] K = Encoding.Default.GetBytes(A);//建立通訊資料陣列
            S.Send(K, K.Length);//送出訊息
            S.Close();//關閉通訊物件
            T = false;//輪到對手下棋
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= 8; i++)
            {
                Button D = (Button)this.Controls["B" + i.ToString()];//取得按鍵參考
                D.BackgroundImage = Properties.Resources.E;//空格影像
                D.Tag = "_";//註記為底線
                T = true;//可以下棋
            }
        }

    }
}
