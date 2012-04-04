using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GemsWeb
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Load today's Event
            }
        }

        protected void menuEvent_MenuItemClick(object sender, MenuEventArgs e)
        {
            mvTab.ActiveViewIndex = int.Parse(e.Item.Value);
            //Load Program/Guest as accordingly
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //Search and populate lstEvent
        }
    }
}