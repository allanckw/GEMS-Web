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


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                int eventID = int.Parse(Request.QueryString["EventID"]);
                EventClient evClient = new EventClient();
                Events event_;
                try
                {
                    event_ = evClient.GetEvent(eventID);
                }
                catch (Exception)
                {
                    Alert.Show("Event Does not exist, please try again", true);
                    return;
                }

                try
                {
                    this.hypRegister.NavigateUrl = "~/Register.aspx?EventID=" + eventID.ToString() + "&Name=" + event_.Name;

                    RegistrationClient client = new RegistrationClient();
                    Publish publish = client.ViewPublish(eventID);

                    client.Close();
                    menuEvent.Visible = true;
                    this.mvTab.Visible = true;
                    lbleventname.Text = event_.Name;
                    lbleventdate.Text = "From " + event_.StartDateTime.ToString("dd MMM yyyy") + " To " + event_.EndDateTime.ToString("dd MMM yyyy");
                    //lbleventstarttime.Text = event_.StartDateTime.ToString("HH:mm");
                    //lbleventendtime.Text = event_.EndDateTime.ToString("HH:mm");
                    lbleventdescription.Text = event_.Description;
                    hypeventwebsite.Text = event_.Website;
                    if (event_.Website == "http://")
                    {
                        hypeventwebsite.Visible = false;
                        lblWebsite.Visible = false;
                    }
                    hypeventwebsite.NavigateUrl = event_.Website;
                    if (publish != null)
                    {
                        lbleventpublishinfo.Text = publish.Remarks;
                    }
                    else
                    {
                        lbleventpublishinfo.Text = "";

                    }

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
            mvTab.ActiveViewIndex = int.Parse(e.Item.Value);
            //Load Program/Guest as accordingly
        }

        protected void ddlEventDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Load Program/Guest as accordingly to day
            int dayID = int.Parse(ddlEventDay.SelectedValue);
            EventClient evClient = new EventClient();
            EventDay evDay = evClient.GetDay(dayID);
            Guest[] guests = evDay.Guests;

            gvGuest.DataSource = guests;
            gvGuest.DataBind();

            Program[] programs = evDay.Programs;
            gvProgram.DataSource = programs;
            gvProgram.DataBind();
            evClient.Close();
        }

    }

}