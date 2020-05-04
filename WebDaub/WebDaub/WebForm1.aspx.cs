using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebDaub
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string Q = Request.QueryString["A"];//嘗試取得網頁繪圖訊息
            if (Q != null && Session["to"] != null)  //有訊息且有設定對手
            {
                Application[Session["to"].ToString()] = Q; //送出訊息給對話者
            }
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
            Session["me"] = TextBox1.Text;
        }

        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {
            Session["to"] = TextBox2.Text;
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            if (Session["me"] == null) return; //未設定身分
            if (Application[Session["me"].ToString()] == null) return; //無訊息
            H.Value = Application[Session["me"].ToString()].ToString(); //接收資訊送至網頁
            Application[Session["me"].ToString()] = null; //清除訊息
        }
    }
}