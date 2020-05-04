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
//using Microsoft.VisualBasic.PowerPacks;//匯入VB向量繪圖功能
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.PowerPacks;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        private int[] _lastCustomColors = new int[16]; //color plate
        public Form1()
        {
            InitializeComponent();
            _lastCustomColors = new int[]  //color plate
            {
             ColorToInt(Color.Red),
             ColorToInt(Color.Blue),
             ColorToInt(Color.Gray)
            };
        }

        string[] ZZ = new string[3]; // for color plate RGB bands
        private static int ColorToInt(Color color)  //color plate
        {
            return (color.R) | (color.G << 8) | (color.G << 16);
        }

        //公用變數
        Socket T;//通訊物件
        Thread Th;//網路監聽執行緒
        string User;//使用者
       
        //登入伺服器
        private void button1_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; //忽略跨執行緒錯誤
            User = textBox3.Text;//使用者名稱
            string IP = textBox1.Text;//伺服器IP
            int Port = int.Parse(textBox2.Text);//伺服器Port
            //建立通訊物件，參數代表可以雙向通訊的TCP連線
            try
            {
                IPEndPoint EP = new IPEndPoint(IPAddress.Parse(IP), Port);//伺服器的連線端點資訊
                T = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                T.Connect(EP); //連上伺服器的端點EP(類似撥號給電話總機)
                Th = new Thread(Listen); //建立監聽執行緒
                Th.IsBackground = true; //設定為背景執行緒
                Th.Start(); //開始監聽
                textBox4.Text = "已連線伺服器！" + "\r\n";
                Send("0" + User);  //連線後隨即傳送自己的名稱給伺服器
            }
            catch (Exception)
            {
                textBox4.Text = "無法連上伺服器！" + "\r\n"; //連線失敗時顯示訊息
                return;
            }
            button1.Enabled = false; //讓連線按鍵失效，避免重複連線          
        }

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
                catch (Exception)//產生錯誤時
                {
                    T.Close();//關閉通訊器
                    listBox1.Items.Clear();//清除線上名單
                    MessageBox.Show("伺服器斷線了！");//顯示斷線
                    button1.Enabled = true;//連線按鍵恢復可用
                    Th.Abort();//刪除執行緒
                }
                Msg = Encoding.Default.GetString(B, 0, inLen); //解讀完整訊息
                St = Msg.Substring(0, 1);  //取出命令碼 (第一個字)
                Str = Msg.Substring(1);    //取出命令碼之後的訊息

                switch (St)//依命令碼執行功能
                {
                    case "L"://接收線上名單
                        listBox1.Items.Clear(); //清除名單
                        string[] M = Str.Split(','); //拆解名單成陣列
                        for (int i = 0; i < M.Length; i++)
                        {
                            listBox1.Items.Add(M[i]); //逐一加入名單
                        }
                        break;
                    case "1"://接收廣播訊息
                       textBox5.Text += "(公開)" + Str + "\r\n";//顯示訊息並換行
                       textBox1.SelectionStart = textBox1.Text.Length; //游標移到最後
                       textBox1.ScrollToCaret(); //捲動到游標位置
                        break;
                    case "2"://接收私密訊息
                        textBox5.Text += "(私密)" + Str + "\r\n";//顯示訊息並換行
                        textBox1.SelectionStart = textBox1.Text.Length;//游標移到最後
                        textBox1.ScrollToCaret();//捲動到游標位置
                        break;
                    case "3"://塗鴉牆訊息
                        string[] Q = Str.Split(':');//切割塗鴉牆訊息成陣列
                        textBox4.Text = Q[0] + "塗鴉"+ "\r\n";
                        
                //處理塗鴉動作
                        if(Q[1] == "C")
                        {D.Shapes.Clear();} // 清除畫面 
                        else // 繪圖
                        {
                         string[] Z = Q[1].Split('_');//切割顏色與座標資訊
                         string[] S = Z[1].Split('/');//切割座標點資訊
                         string[] Z1 = Z[0].Split('*'); //切割顏色RGB   
                         Point[] P = new Point[S.Length];//宣告座標點陣列
                         for (int i = 0; i < S.Length; i++)
                         {
                             string[] K = S[i].Split(',');//切割X與Y座標
                             P[i].X = int.Parse(K[0]);//定義第i點X座標
                             P[i].Y = int.Parse(K[1]);//定義第i點Y座標
                         }
                         for (int i = 0; i < P.Length - 1; i++)
                         {
                             LineShape L = new LineShape();//建立線段物件
                             L.StartPoint = P[i];//線段起點
                             L.EndPoint = P[i + 1];//線段終點

                            // 畫筆顏色 
                             int r = Convert.ToInt32(Z1[0]);
                             int g = Convert.ToInt32(Z1[1]);
                             int b = Convert.ToInt32(Z1[2]); 
                             L.BorderColor = Color.FromArgb(r, g, b);                            
                             L.Parent = D;//線段L加入畫布D(遠端使用者繪圖)
                         }
                        }
                        break;
                }
            }
        }

        //傳送訊息給 Server (Send Message to the Server)
        private void Send(string Str)
        {
            byte[] B = Encoding.Default.GetBytes(Str); //翻譯文字為Byte陣列
            T.Send(B, 0, B.Length, SocketFlags.None); //使用連線物件傳送資料給 Server
        }
        //繪圖相關變數宣告
        ShapeContainer C;//畫布物件(本機繪圖用)
        ShapeContainer D;//畫布物件(遠端繪圖用)
        Point stP;//繪圖起點
        string p;//筆畫座標字串

        private void button2_Click(object sender, EventArgs e)
        {
            C.Shapes.Clear(); // 清除本機畫面
            if (listBox1.SelectedItem != null)//選取傳送對象
            {
                Send("3" + User + ":" + "C" + p + "|" + listBox1.SelectedItem); // 發出清除訊息 命令碼"3"
            }           
        }
//表單載入
        private void Form1_Load(object sender, EventArgs e)
        {
            C = new ShapeContainer();//建立畫布(本機繪圖用)
            pictureBox1.Controls.Add(C);//加入畫布C到表單
            D = new ShapeContainer();//建立畫布(遠端繪圖用)
            pictureBox1.Controls.Add(D);//加入畫布D到表單
            this.Text += " " + MyIP();//顯示本機IP於表單標題列
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
      
      private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (button1.Enabled == false) // 上線狀態
                {
                    Send("9" + User); //傳送自己的離線訊息給伺服器
                    T.Close(); //關閉專案相關所有執行緒
                }
            }
            catch
            {
                //如果監聽執行緒沒開會出現錯誤，程式跳到此處執行，
                //此處不寫程式就是忽略錯誤，程式繼續執行的意思！
            }
          
        }

        // Color Dialog
        private void button3_Click(object sender, EventArgs e)
        {
            using (ColorDialog dlgColor = new ColorDialog())
            {
                dlgColor.FullOpen = true;
                dlgColor.CustomColors = _lastCustomColors;
                if (dlgColor.ShowDialog() == DialogResult.OK)
                {
                    _lastCustomColors = dlgColor.CustomColors;
                    button2.BackColor = dlgColor.Color; //讀取色盤顏色給畫筆
                    ZZ[0] = button2.BackColor.R.ToString();
                    ZZ[1] = button2.BackColor.G.ToString();
                    ZZ[2] = button2.BackColor.B.ToString();
                }
            }
        }

       private void button4_Click(object sender, EventArgs e)
        {
            if (textBox6.Text == "") return; //未輸入訊息不傳送資料
            if (listBox1.SelectedIndex < 0)//未選取傳送對象(廣播)，命令碼：1
            {
                Send("1" + User + "公告：" + textBox6.Text);
            }
            else//有選取傳送對象(私密訊息)，命令碼：2
            {
                Send("2" + "來自" + User + ":" + textBox6.Text + "|" + listBox1.SelectedItem);
                textBox5.Text += "告訴" + listBox1.SelectedItem + "：" + textBox6.Text + "\r\n";
            }
            textBox6.Text = ""; //清除發言框
        }

       //本機端開始繪圖
       private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
       {
           stP = e.Location;//起點
           p = stP.X.ToString() + "," + stP.Y.ToString();//起點座標紀錄
       }

       //本機端繪圖中
       private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
       {
           if (e.Button == System.Windows.Forms.MouseButtons.Left)
           {
               LineShape L = new LineShape();//建立線段物件
               L.StartPoint = stP;//線段起點
               L.EndPoint = e.Location;//線段終點

               L.BorderColor = button2.BackColor; //設定畫筆顏色
               L.Parent = C;//線段加入畫布C
               stP = e.Location;//終點變起點
               p += "/" + stP.X.ToString() + "," + stP.Y.ToString();//持續紀錄座標
           }

       }

       //結束一筆劃, 送出繪圖動作
       private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
       {
           if (listBox1.SelectedItem != null)//選取傳送對象
           {
               p = ZZ[0] + "*" + ZZ[1] + "*" + ZZ[2] + "_" + p;
               Send("3" + User + ":" + p + "|" + listBox1.SelectedItem); // 傳送座標字串
           }   

       }
    }
}
