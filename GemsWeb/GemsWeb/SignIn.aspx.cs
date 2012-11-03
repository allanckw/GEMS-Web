using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using GemsWeb.Controllers;
using evmsService.Controllers;
using evmsService.entities;
using System.Web.Configuration;

namespace GemsWeb
{
    public partial class SignIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = ((ImageButton)Login1.FindControl("LoginButton")).UniqueID;


            if (!Page.IsPostBack)
            {
                DropDownList ddlDomain = (DropDownList)Login1.FindControl("ddlDomain");
                int mode;
                bool parseSuccess = int.TryParse(Request.QueryString["mode"], out mode);

                if (parseSuccess)
                {
                    ddlDomain.SelectedIndex = mode;
                }
                else
                {
                    if (Request.RawUrl.ToString().Contains("ReturnUrl"))
                    {
                        lblMsg.Visible = true;
                    }
                    else
                    {
                        lblMsg.Visible = false;
                    }
                    if (Request.RawUrl.ToString().ToLower().Contains("participantevents.aspx"))
                    {
                        ddlDomain.SelectedIndex = 0;
                    }
                }


            }
        }

        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
        {
            DropDownList ddlDomain = (DropDownList)Login1.FindControl("ddlDomain");

            int domainIndex = ddlDomain.SelectedIndex;
            bool auth = false;
            if (domainIndex == 0)
            {
                auth = AuthParticipant(Login1.UserName, Login1.Password);
            }
            else if (domainIndex == 1)
            {
                auth = AuthNUSNET(Login1.UserName, Login1.Password);
            }
            else if (domainIndex == 2)
            {
                auth = AuthRequestees(Login1.UserName, Login1.Password);
               
            }


            System.Threading.Thread.Sleep(500);

            if (auth)
            {
                Session["Domain"] = domainIndex;
                Session["Login"] = "1";
                Session["username"] = Login1.UserName.Trim();
                e.Authenticated = true;
                if (domainIndex == 0)
                {
                    Session["partiEmail"] = Login1.UserName.Trim();
                    FormsAuthentication.SetAuthCookie(Login1.UserName.Trim(), false);

                    Response.Redirect("~/ParticipantEvents.aspx");
                }
                else if (domainIndex == 1)
                {
                    FormsAuthentication.RedirectFromLoginPage(Login1.UserName.Trim(), false);
                    Session["partiEmail"] = Login1.UserName.Trim() + "@" + WebConfigurationManager.AppSettings["dom"];
                    //Redirect to NUSNET (Artefacts / Send Requests)
                }
                else if (domainIndex == 2)
                {
                    FormsAuthentication.SetAuthCookie(Login1.UserName.Trim(), false);
                    //Redirect to Requestee Page (Approval)
                    Response.Redirect("~/RequesteePage.aspx");
                }

                FormsAuthentication.RedirectFromLoginPage(Login1.UserName, true);
            }
            else
            {
                e.Authenticated = false;
                Session["Login"] = "0";
                Session["Domain"] = "-1";
            }

        }

        bool AuthParticipant(string username, string password)
        {
            return KeyGen.Authenticate(username, password);
        }

        bool AuthNUSNET(string username, string password)
        {
            AdministrationClient client = new AdministrationClient();

            Credentials c = new Credentials();
            c.UserID = Login1.UserName.Trim();
            c.Password = KeyGen.Encrypt(Login1.Password);
            try
            {
                User u = client.SecureAuthenticate(c);
                client.Close();
                Session["nusNETuser"] = u;
                client.Close();
                return true;
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message, false);
            }
            finally
            {
                client.Close();
            }
            return false;
        }

        bool AuthRequestees(string username, string password)
        {
            RequestClient reqClient = new RequestClient();

            try
            {
                Requestee r = reqClient.ValidateRequestee(username, password);
                reqClient.Close();
                Session["ReQuestEE"] = r;
                return true;
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message, false);
            }
            finally
            {
                reqClient.Close();
            }
            return false;
        }

    }
}