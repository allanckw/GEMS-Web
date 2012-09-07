﻿using System;
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

            int eventID  = int.Parse(Request.QueryString["EventID"]);
            EventClient evClient = new EventClient();
            Events event_ = evClient.GetEvent(eventID);
            try
            {
               

                this.hypRegister.NavigateUrl = "~/Register.aspx?EventID=" + eventID.ToString() + "&Name=" + event_.Name;

                RegistrationClient client = new RegistrationClient();
                Publish publish = client.ViewPublish(eventID);

                client.Close();
                menuEvent.Visible = true;
                this.mvTab.Visible = true;
                lbleventname.Text = event_.Name;
                lbleventdate.Text = event_.StartDateTime.ToString("dd MMM yyyy");
                lbleventstarttime.Text = event_.StartDateTime.ToString("HH:mm");
                lbleventendtime.Text = event_.EndDateTime.ToString("HH:mm");
                lbleventdescription.Text = event_.Description;
                hypeventwebsite.Text = event_.Website;
                hypeventwebsite.NavigateUrl = event_.Website;
                if (publish != null)
                {
                    lbleventpublishinfo.Text = publish.Remarks;
                }
                else
                {
                    lbleventpublishinfo.Text = "";

                }
                //Day dependent now
                //Guest[] guests = event_.Guests;

                //gvGuest.DataSource = guests;
                //gvGuest.DataBind();

                //Program[] programs = event_.Programs;
                //gvProgram.DataSource = programs;
                //gvProgram.DataBind();


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

        protected void menuEvent_MenuItemClick(object sender, MenuEventArgs e)
        {
            //mvTab.ActiveViewIndex = int.Parse(e.Item.Value);
            //Load Program/Guest as accordingly
        }

    }

}