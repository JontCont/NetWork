using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace Gobang
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(570, 570);
            Graphics g = Graphics.FromImage(bmp);
            for (int i = 0; i < 19; i++)
            {
                g.DrawLine(Pens.Black, i * 30 + 15, 15, i * 30 + 15, 30 * 18 + 15);
            }
            for (int j = 0; j < 19; j++)
            {
                g.DrawLine(Pens.Black, 15, j * 30 + 15, 30 * 18 + 15, j * 30 + 15);
            }
            bmp.Save(Server.MapPath("bg.gif"));
        }
    }
}