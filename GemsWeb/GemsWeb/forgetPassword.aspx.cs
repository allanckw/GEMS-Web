using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web.Configuration;
using evmsService.entities;
using GemsWeb.Controllers;

namespace GemsWeb
{
    public partial class forgetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                checkSignedIn();
            }
        }

        private void checkSignedIn()
        {
            int login = 0;
            try
            {
                login = int.Parse(Session["Login"].ToString());
                if (login > 0)
                {
                    Alert.Show("You are already signed in!", true);
                }
            }
            catch (Exception)
            {
                Session["Login"] = "0";
                Session["Domain"] = "-1";
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ddlDomain.SelectedIndex == -1)
                return;

            if (ddlDomain.SelectedIndex == 0)
                retrievePartiPW();
            else
                retrieveRequesteePW();
        }

        private void retrievePartiPW()
        {
            RegistrationClient client = new RegistrationClient();

            if (client.isRegistered(txtEmail.Text.Trim()))
            {
                MailHandler.sendForgetPassword(KeyGen.GeneratePwd(txtEmail.Text.Trim()), txtEmail.Text.Trim(), 0);
            }
            else
            {
                Alert.Show("You are not registered in the system!");
            }

            client.Close();
        }

        private void retrieveRequesteePW()
        {
            RequestClient client = new RequestClient();
            try{
               string pw =  client.GetOtp(txtEmail.Text.Trim());

               MailHandler.sendForgetPassword(pw, txtEmail.Text.Trim(), 2);

            }catch(Exception ex){
                Alert.Show(ex.Message);
            }

            client.Close();
        }
    }
}