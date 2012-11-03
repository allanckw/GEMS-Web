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
    public partial class Event : System.Web.UI.Page
    {

        //TODO, Put radio buttons to set the date
        //1 month, 3 months this year..
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                int eventID = int.Parse(Request.QueryString["EventID"]);
                EventClient evClient = new EventClient();
                Events event_ = evClient.GetEvent(eventID);
                try
                {
                    this.hypRegister.NavigateUrl = "~/Register.aspx?EventID=" + eventID.ToString() + "&Name=" + event_.Name;

                    RegistrationClient client = new RegistrationClient();
                    Publish publish = client.ViewPublish(eventID);
                    client.Close();

                    menuEvent.Visible = true;
                    mvTab.Visible = true;

                    lbleventname.Text = event_.Name;
                    lbleventdate.Text = "From " + event_.StartDateTime.ToString("dd MMM yyyy") + " To " 
                        + event_.EndDateTime.ToString("dd MMM yyyy");

                    lbleventdescription.Text = event_.Description;
                    hypeventwebsite.Text = event_.Website;

                    if (event_.Website.Length == 0)
                    {
                        hypeventwebsite.Visible = false;
                        lblWebsite.Visible = false;
                    }
                    else
                    {
                        hypeventwebsite.NavigateUrl = event_.Website;
                    }

                    if (publish != null)
                    {
                        lbleventpublishinfo.Text = publish.Remarks;
                        lblPublish.Visible = true;

                        if (publish.PaymentAMount > 0)
                        {
                            lblpaymentinfo.Text = "$" + publish.PaymentAMount.ToString("0.00");
                        }
                        else
                        {
                            lblpaymentinfo.Text = "Event is Free! :)";
                        }
                    }
                    else
                    {
                        lbleventpublishinfo.Text = "";
                        lblPublish.Visible = false;
                        lblpaymentinfo.Text = "";
                    }

                    lblpaymentinfo.Visible = lblPublish.Visible;
                    lblpaymentinfo.Visible = lblpayment.Visible;

                    ddlEventDay.DataSource = evClient.GetDays(event_.EventID);
                    ddlEventDay.DataValueField = "DayID";
                    ddlEventDay.DataTextField = "StartDateTime";

                    ddlEventDay.DataBind();
                    ddlEventDay_SelectedIndexChanged(this.ddlEventDay, new EventArgs());
                    evClient.Close();

                    if (publish == null || (publish.StartDateTime > DateTime.Now || publish.EndDateTime < DateTime.Now))
                    {
                        this.hypRegister.Visible = false;
                    }
                    else
                    {
                        hypRegister.Visible = true;
                    }


                }
                catch (Exception ex)
                {
                    Alert.Show("Error Retreiving List of Events from Server", false, "~/Default.aspx");
                    return;
                }
            }
        }

        protected void menuEvent_MenuItemClick(object sender, MenuEventArgs e)
        {
            mvProgramGuest.ActiveViewIndex = int.Parse(e.Item.Value);
            //Load Program/Guest as accordingly
        }

        protected void ddlEventDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Load Program/Guest as accordingly to day
            int dayID = int.Parse(ddlEventDay.SelectedValue);
            EventClient evClient = new EventClient();
            EventDay evDay = evClient.GetDay(dayID);
            List<Guest> guests = evDay.Guests.ToList<Guest>();

            rtpGuest.DataSource = guests;
            rtpGuest.DataBind();


            List<Program> programs = evDay.Programs.ToList<Program>();
            rptProgramme.DataSource = programs;
            rptProgramme.DataBind();
            evClient.Close();
        }

        protected string DateTimeToCustomString(DateTime d)
        {
            return d.ToString("hh:mm tt");
        }
    }

}