using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace Gobang
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string A = Request.QueryString["A"];//取得己方下棋訊息
            if (A != null && Session["To"] != null)  //有訊息且有設定對手
            {
                Application[Session["To"].ToString()] = A; //送出訊息給對手
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            if (Session["Me"] == null) return; //未設定身分
            if (Application[Session["Me"].ToString()] == null) return; //無訊息
            H.Value = Application[Session["Me"].ToString()].ToString(); //接收資訊送至網頁
            Application[Session["Me"].ToString()] = null; //清除訊息
        }

        protected void txtMe_TextChanged(object sender, EventArgs e)
        {
            Session["Me"] = txtMe.Text;
        }

        protected void txtTo_TextChanged(object sender, EventArgs e)
        {
            Session["To"] = txtTo.Text;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            H.Value = "X"; //清除己方棋盤(送訊息到己方網頁)
            if (Session["To"] != null)  //有設定對手
            {
                Application[Session["To"].ToString()] = "X"; //發訊息要求對手重設棋盤
            }
        }
    }
}