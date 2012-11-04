using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GemsWeb.CustomControls;
using GemsWeb.Controllers;
using evmsService.entities;


namespace GemsWeb
{
    public partial class _default : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                dpFrom.Enabled = false;
                dpTo.Enabled = false;
                RequestEvents(DateTime.Now, DateTime.Now.AddMonths(1));
            }
        }

        private void RequestEvents(DateTime start, DateTime end)
        {
            List<Events> events = new List<Events>();
            try
            {

                RegistrationClient client = new RegistrationClient();
                List<Events> arrEventPublish = client.ViewEventForPublish(start, end).ToList<Events>();
                if (txtTag.Text.Trim() != "")
                {
                    arrEventPublish = client.ViewEventForPublishByDateAndTag(start, end, txtTag.Text.Trim()).ToList<Events>();
                }

                client.Close();

                rtpEvent.DataSource = arrEventPublish;
                rtpEvent.DataBind();
                //lstEvent.DataSource = arrEventPublish;

                //lstEvent.DataValueField = "EventID";
                //lstEvent.DataTextField = "Name";

                //lstEvent.DataBind();

            }
            catch (Exception ex)
            {
                Alert.Show("Error Retreiving List of Events from Server", false, "~/Default.aspx");
                return;
            }

            
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //Search and populate lstEvent
            RequestEvents(dpFrom.CalDate, dpTo.CalDate);
        }

        //protected void btnGO_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("~/Event.aspx?EventID=" + lstEvent.SelectedValue);
        //}

        protected string DateToCustomString(DateTime eventDateTime)
        {
            return eventDateTime.ToString("dd MMM yyyy");
        }

        protected void rdlstToDateRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            dpTo.Enabled = true;
            if (rdlstToDateRange.SelectedValue != "-1")
                dpTo.Enabled = false;
            switch (rdlstToDateRange.SelectedValue)
            {
                case "30":
                    dpTo.CalDate = DateTime.Now.AddMonths(1);
                    break;
                case "90":
                    dpTo.CalDate = DateTime.Now.AddMonths(3);
                    break;
                case "365":
                    dpTo.CalDate = DateTime.Now.AddYears(1);
                    break;
                default:
                    break;
            }
        }

        protected void rdlstFromDateRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            dpFrom.Enabled = true;
            if (rdlstFromDateRange.SelectedValue != "-1")
            {
                dpFrom.Enabled = false;
                dpFrom.CalDate = DateTime.Now;
            }
        }
    }
}