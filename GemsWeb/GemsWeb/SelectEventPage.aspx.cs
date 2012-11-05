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
using System.Data;

namespace GemsWeb
{
    public partial class SelectEventWorkspace : System.Web.UI.Page
    {
        #region "Page Global Function"
        private User NUSNetUser()
        {
            try
            {
                User u = (User)Session["nusNETuser"];
                return u;
            }
            catch (Exception ex)
            {
            }
            return null;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //To ByPass for NUSNetUser Temporary
            //AuthNUSNET("A0077307", "12345678");
            if (!Page.IsPostBack)
            {
                bool authenticated = true;

                int domain = int.Parse(Session["Domain"].ToString());
                if (domain >= 2)
                    authenticated = false;

                if (!authenticated)
                    Response.Redirect("~/Error403.aspx");

                dpFrom.Enabled = false;
                dpTo.Enabled = false;
                //lblNoEvents.Visible = false;
            }

            ScriptManager sc = (ScriptManager)Master.FindControl("ScriptManager1");

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

        protected void rptEvent_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RoleClient client = new RoleClient();
            // Execute the following logic for Items and Alternating Items.
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Events ev = (Events)e.Item.DataItem;
                //List<EnumFunctions> fx = client.GetRights(ev.EventID, u.UserID).ToList<EnumFunctions>();
                HyperLink hyperReq = (HyperLink)e.Item.FindControl("lnkRequest");
                hyperReq.Attributes.Add("onclick", "window.open('RequestPage.aspx?EventID=" + ev.EventID + "', 'Request Page','left=250px, top=245px, width=1100px, height=650px, location=no, directories=no, scrollbars=yes, status=no, resizable=no');return false;");
                //TODO: Turn on after enumfunctions for manage request is Up
                if (client.haveRightsTo(ev.EventID,NUSNetUser().UserID,EnumFunctions.Manage_Requests))//fx.Contains(EnumFunctions.Manage_Requests) || string.Compare(ev.Organizerid, u.UserID, true) == 0)
                {
                     // If can manage requests, visible
                    hyperReq.Visible = true;
                }
                else
                {
                    //if cannot off it
                    hyperReq.Visible = false;
                }

                HyperLink hyperArte = (HyperLink)e.Item.FindControl("lnkArtefact");
                if (client.haveRightsTo(ev.EventID,NUSNetUser().UserID,EnumFunctions.Manage_Artefacts))//string.Compare(ev.Organizerid, NUSNetUser().UserID, true) == 0)
                {
                    //if event organizer, go to the page that can edit folders
                    //hyperArte.NavigateUrl = "ArtefactBin.aspx?EventID=" + ev.EventID;
                    hyperArte.Attributes.Add("onclick", "window.open('ArtefactBin.aspx?EventID=" + ev.EventID + "', 'Artefact Bin','left=250px, top=245px, width=1100px, height=650px, location=no, directories=no, scrollbars=yes, status=no, resizable=no');return false;");
                }
                else
                {
                    //else go to the page that cannot edit folders
                    //hyperArte.NavigateUrl = "ArtefactWorkspace.aspx?EventID=" + ev.EventID;
                    hyperArte.Attributes.Add("onclick", "window.open('ArtefactWorkspace.aspx?EventID=" + ev.EventID + "', 'Artefact Workspace','left=250px, top=245px, width=1100px, height=650px, location=no, directories=no, scrollbars=yes, status=no, resizable=no');return false;");
                }


            }

            client.Close();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //ArtefactBinLink.Attributes.Clear();
            //DateTime start = DateTime.Now;
            //DateTime end = DateTime.Now;

            //int timeValue = int.Parse(rdlstFromDateRange.SelectedValue);
            //switch (timeValue)
            //{
            //    case 30:
            //        start = start.AddMonths(-1);
            //        break;
            //    case 90:
            //        start = start.AddMonths(-3);
            //        break;
            //    case 365:
            //        start = start.AddYears(-1);
            //        break;
            //    default:
            //        start = dpFrom.CalDate;
            //        break;
            //}

            //timeValue = int.Parse(rdlstToDateRange.SelectedValue);
            //switch (timeValue)
            //{
            //    case 30:
            //        end = end.AddMonths(1);
            //        break;
            //    case 90:
            //        end = end.AddMonths(3);
            //        break;
            //    case 365:
            //        end = end.AddYears(1);
            //        break;
            //    default:
            //        end = dpTo.CalDate;
            //        break;
            //}

            GetEventsForUser(dpFrom.CalDate, dpTo.CalDate);
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
                List<Events> userEvent = client.ViewEventsByDateAndTag(u, start, end, txtTag.Text.Trim()).ToList<Events>();
                client.Close();

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
    }
}