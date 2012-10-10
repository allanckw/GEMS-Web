using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GemsWeb
{
    public partial class Main : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string userid = (string)Session["username"];
            int login = 0;
            try
            {
                login = int.Parse(Session["L6446"].ToString());
                if (login == 0)
                {
                    pnl1.Visible = true;
                }
                else
                {
                    pnl1.Visible = false;

                }
            }
            catch (Exception )
            {
                Session["L6446"] = "0";
            }

            pnl2.Visible = !pnl1.Visible;
          
        }
    }
}