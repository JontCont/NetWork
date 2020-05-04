using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;//匯入集合物件功能

namespace WebChat
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (Button1.Text == "登入") //嘗試登入
            {
                if (TextBox2.Text != "")//有寫名字
                {
                    Session["Me"] = TextBox2.Text;//記錄登入名稱
                    Application.Lock(); //鎖定網站公用變數
                    Hashtable L = (Hashtable)Application["L"]; //取得線上名單
                    L.Add(Session["Me"], DateTime.Now); //加自己到線上名單(名稱，時間)
                    Application.UnLock(); //解除鎖定
                    Button1.Text = "登出"; //已登入顯示功能為登出
                }
            }
            else
            {
                Application.Lock(); //鎖定網站公用變數
                Hashtable L = (Hashtable)Application["L"]; //取得線上名單
                L.Remove(Session["Me"]); //移除名單中的我
                Application.UnLock(); //解除鎖定
                Session["Me"] = null; //清除登入名稱
                Button1.Text = "登入"; //已登出顯示功能為登入
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (!(Session["Me"] == null))//已登入
            {
                Application.Lock(); //鎖定網站公用變數
                Queue Q = (Queue)Application["Q"]; //取得目前發言內容
                Q.Enqueue(TextBox2.Text + ":" + TextBox3.Text); //加入我的發言
                while (Q.Count > 5) //保存五筆資料
                {
                    Q.Dequeue(); //刪除最舊的資料
                }
                Application.UnLock(); //解除鎖定
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            //更新發言
            Queue Q = (Queue)Application["Q"]; //取得目前發言內容
            TextBox1.Text = ""; //清除看板
            foreach (var i in Q)
            {
                TextBox1.Text += i + "\r\n"; //一一顯示留言
            }
            //更新線上名單
            Application.Lock(); //鎖定網站公用變數
            Hashtable L = (Hashtable)Application["L"]; //取得線上名單
            if (!(Session["Me"] == null))//已登入
            {
                if (L[Session["Me"]] == null)//如果我不在名單內
                {
                    L.Add(Session["Me"], DateTime.Now); //重新登入我自己
                }
                else
                {
                    L[Session["Me"]] = DateTime.Now; //打卡更新時間
                }
            }
            Application.UnLock(); //解除鎖定
            ListBox1.Items.Clear(); //清除名單顯示
                                    //在ListBox中一一顯示名單
            foreach (var i in L.Keys)
            {
                ListBox1.Items.Add(i.ToString());
            }
        }
    }
}