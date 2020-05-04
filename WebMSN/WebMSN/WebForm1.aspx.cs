using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebMSN
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            if (Application[TextBox2.Text] != null)
            {
                TextBox1.Text += Application[TextBox2.Text] + "\r\n"; //寫入看板
                Application[TextBox2.Text] = null; //刪除訊息
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string A = TextBox2.Text + "：" + TextBox4.Text; //發言者：發言
            TextBox1.Text += A + "\r\n"; //寫入看板
            Application[TextBox3.Text] = A;//發給收訊者
            TextBox4.Text = ""; //清除發言框
        }
    }
}