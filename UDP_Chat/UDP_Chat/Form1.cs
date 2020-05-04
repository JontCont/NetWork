using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.Net;//匯入網路通訊協定相關函數
using System.Net.Sockets;//匯入網路插座功能函數
using System.Threading;//匯入多執行緒功能函數
using System.Collections;//匯入集合物件功能

namespace UDP_Chat
{
    public partial class Form1:MetroFramework.Forms.MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }
        //公用變數宣告
        UdpClient U;//宣告UDP通訊物件
        Thread Th;//宣告監聽用執行緒
        String MyName;//我的名稱
        ArrayList ips = new ArrayList();//線上客戶IP列表
        const short Port = 2013;//本程式使用的通訊埠(頻道)
        string BC;//廣播用IP

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false; //忽略跨執行緒操作的錯誤
            Label4.Text = "我的 IP: " + MyIP(); //顯示我的IP
            BC = IPAddress.Broadcast.ToString();//取得廣播用IP
        }
        //啟動監聽
        private void Button1_Click(object sender, EventArgs e)
        {
            if (TextBox1.Text == "")//未輸入姓名
            {
                MessageBox.Show("請輸入姓名！");
                return;
            }
            MyName = TextBox1.Text; //我的名稱
            ListBox1.Items.Clear(); //清除名單
            ips.Clear(); //清除線上人員IP陣列
            if (Button1.Text == "上線")//離線狀態
            {
                Th = new Thread(Listen); //建立監聽網路訊息的新執行緒
                Th.Start(); //啟動監聽執行緒
                Send(BC, "OnLine", ""); //公告上線訊息
                Button1.Text = "離線";
            }
            else//上線狀態
            {
                Send(BC, "OffLine", ""); //公告離線訊息
                Th.Abort(); //關閉監聽執行緒
                U.Close(); //關閉監聽器
                Button1.Text = "上線";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Th.Abort(); //關閉監聽執行緒
                U.Close(); //關閉監聽器
            }
            catch
            { }
        }
        //找出本機IP
        private string MyIP()
        {
            string hn = Dns.GetHostName();//取得本機電腦名稱
            IPAddress[] ip = Dns.GetHostEntry(hn).AddressList; //取得本機IP陣列(可能有多個)
            foreach (IPAddress it in ip)//列舉各個IP
            {
                if (it.AddressFamily == AddressFamily.InterNetwork)//如果是IPv4格式
                {
                    return it.ToString();//回傳此IP字串
                }
            }
            return ""; //找不到合格IP，回傳空字串
        }
        //發送訊息副程式
        private void Send(string ToIP, string msg, string toWhom)
        {
            //訊息格式：我是誰+IP+訊息+發給誰
            string A = MyName + ":" + MyIP() + ":" + msg + ":" + toWhom;
            byte[] B = Encoding.Default.GetBytes(A); //字串翻譯成位元組陣列
            UdpClient V = new UdpClient(ToIP, Port);//建立UDP通訊物件
            V.Send(B, B.Length);//發送資料
        }
        //監聽副程式
        private void Listen()
        {
            U = new UdpClient(Port); //建立UDP通訊物件
            IPEndPoint EP = new IPEndPoint(IPAddress.Parse(MyIP()), Port); //接聽端點(IP+Port)
            while (true)
            {
                byte[] B = U.Receive(ref EP); //訊息到達時讀取資訊到B陣列
                string A = Encoding.Default.GetString(B); //翻譯B陣列為字串A
                //切割訊息為：C[0]=發訊者；C[1]=IP；C[2]=訊息；C[3]=發訊對象
                string[] C = A.Split(':');
                switch (C[2])//依據訊息內容執行工作
                {
                    case "OnLine"://有人新上線的訊息
                        ListBox1.Items.Add(C[0]); //名稱加入列表
                        ips.Add(C[1]); //IP加入集合物件
                        if (C[0] != MyName) Send(C[1], "AddMe", C[0]); //回應我也在線上
                        break;
                    case "AddMe"://接收到誰也在線上的訊息
                        ListBox1.Items.Add(C[0]); //名稱加入列表
                        ips.Add(C[1]); //IP加入集合物件
                        break;
                    case "OffLine"://客戶離線的訊息
                        ListBox1.Items.Remove(C[0]);//移除名單
                        ips.Remove(C[1]);//移除IP
                        break;
                    default://一般訊息
                        if (C[3] == "")//公開訊息(無指定收訊者)
                        {
                            ListBox2.Items.Add(C[0] + "(公開):" + C[2]);//加入看板
                        }
                        else
                        {//私密訊息(有指定收訊者C[3])
                            ListBox2.Items.Add(C[0] + "(私密):" + C[2]);//加入看板
                        }
                        break;
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            ListBox1.ClearSelected(); //清除選取項目
        }

        private void TextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)//按下Enter
            {
                if (ListBox1.SelectedIndex < 0)//未選擇發訊對象
                {
                    Send(BC, TextBox2.Text, ""); //廣播訊息
                }
                else//發送私密訊息
                {
                    //Send(toIP, Message, toWhom)
                    Send(ips[ListBox1.SelectedIndex].ToString(), TextBox2.Text, ListBox1.SelectedItem.ToString());
                    ListBox2.Items.Add(MyName + "(私密):" + TextBox2.Text); //訊息寫入看板
                }
                TextBox2.Text = ""; //清除訊息填寫欄
            }
        }
    }
}
