using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GemsWeb.CustomControls;
using GemsWeb.Controllers;
using evmsService.entities;
using System.Text;

namespace GemsWeb
{
    public partial class SelectEventWorkspace : System.Web.UI.Page
    {
        //User u;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                dpFrom.Enabled = false;
                dpTo.Enabled = false;
                //lblNoEvents.Visible = false;
            }

            System.Web.UI.ScriptManager sc = (System.Web.UI.ScriptManager)Master.FindControl("ScriptManager1");

            if (sc != null)
            {
                sc.RegisterPostBackControl(btnSearch);
            }
        }

        protected void btnGO_Click(object sender, EventArgs e)
        {
            //if (lstEvent.SelectedIndex == -1)
            //    return;


            //string url = "~/ArtefactBin.aspx?EventID=" + lstEvent.SelectedValue;
            //Server.Transfer(url);
            //StringBuilder sb = new StringBuilder();
            //sb.Append("<script type = 'text/javascript'>");
            //sb.Append("window.open('");
            //sb.Append(url);
            //sb.Append("','','left=250px, top=245px, width=700px, height=450px, scrollbars=no, status=no, resizable=yes');return false;");
            //sb.Append("</script>");
            //ClientScript.RegisterStartupScript(this.GetType(),
            //        "script", sb.ToString());
            ////Response.Redirect("~/ArtefactBin.aspx?EventID=" + lstEvent.SelectedValue);
            //string url = "~/ArtefactBin.aspx?EventID=" + lstEvent.SelectedValue;
            //Response.Write("<SCRIPT language=\"javascript\">window.open('" + url + "','_blank','top=0,left=0,status=yes,resizable=yes,scrollbars=yes');</script>");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //ArtefactBinLink.Attributes.Clear();
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;

            int timeValue = int.Parse(rdlstFromDateRange.SelectedValue);
            switch (timeValue)
            {
                case 30:
                    start = start.AddMonths(-1);
                    break;
                case 90:
                    start = start.AddMonths(-3);
                    break;
                case 365:
                    start = start.AddYears(-1);
                    break;
                default:
                    start = dpFrom.CalDate;
                    break;
            }

            timeValue = int.Parse(rdlstToDateRange.SelectedValue);
            switch (timeValue)
            {
                case 30:
                    end = end.AddMonths(1);
                    break;
                case 90:
                    end = end.AddMonths(3);
                    break;
                case 365:
                    end = end.AddYears(1);
                    break;
                default:
                    end = dpTo.CalDate;
                    break;
            }

            GetEventsForUser(start, end);
        }

        private void GetEventsForUser(DateTime start, DateTime end)
        {
            List<Events> events = new List<Events>();

            try
            {
                User u = (User)Session["nusNETuser"];
                if (u == null)
                {
                    Alert.Show("Please Login First!", true, "SignIn.aspx");
                    return;
                }

                EventClient client = new EventClient();
                Events[] userEvent = client.ViewEventsByDateAndTag(u, start, end, txtTag.Text.Trim());
                if (txtTag.Text.Trim() != "")
                {
                    userEvent = client.ViewEventsbyDate(u, start, end);
                }

                client.Close();

                //lstEvent.DataSource = userEvent;

                //lstEvent.DataValueField = "EventID";
                //lstEvent.DataTextField = "Name";

                //lstEvent.DataBind();
                rtpEvent.DataSource = userEvent;
                rtpEvent.DataBind();
            }
            catch (Exception ex)
            {
                Alert.Show("Error Retreiving List of Events from Server", false, "~/Default.aspx");
                return;
            }
        }

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
                switch (rdlstFromDateRange.SelectedValue)
                {
                    case "30":
                        dpFrom.CalDate = DateTime.Now.AddMonths(-1);
                        break;
                    case "90":
                        dpFrom.CalDate = DateTime.Now.AddMonths(-3);
                        break;
                    case "365":
                        dpFrom.CalDate = DateTime.Now.AddYears(-1);
                        break;
                    default:
                        break;
                }
            }
        }

        //        Protected Function customStringtoDateS(ByVal s As String) As String
        //    Dim fD As New Date(CInt(s.ToString().Trim().Substring(0, 4)), _
        //   CInt(s.ToString().Trim().Substring(4, 2)), _
        //   CInt(s.ToString().Trim().Substring(6, 2)))

        //    Return fD.ToShortDateString()
        //End Function
        //protected void lstEvent_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (lstEvent.SelectedIndex == -1)
        //    {
        //        return;
        //    }

        //    ArtefactBinLink.Attributes.Add("onclick", "window.open('ArtefactBin.aspx?EventID=" + lstEvent.SelectedValue.ToString() + "', 'ArtefactBin','left=250px, top=245px, width=1100px, height=650px, directories=no, scrollbars=yes, status=no, resizable=no');return false;");
        //}

        //protected string EventID()
        //{
        //    if (lstEvent.SelectedIndex == -1)
        //        return "Nothing";

        //    return lstEvent.SelectedValue.ToString();
        //}
    }
}