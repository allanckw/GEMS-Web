using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GemsWeb
{
    public partial class RegistrationSuccessful : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int domain = -1;
            try
            {
                domain = int.Parse(Session["Domain"].ToString());
            }
            catch (Exception)
            {
                domain = -1;
            }

            if (domain != -1)
            {
                hypGoBack.NavigateUrl = "~/default.aspx";
            }
            else
            {
                hypGoBack.NavigateUrl = "~/ParticipantEvents.aspx";
            }
            //if (domain != 1)
            //    Response.Redirect("~/Error404.aspx");
        }
    }
}