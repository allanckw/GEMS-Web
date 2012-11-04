using System;
using System.Web.Security;
using GemsWeb.Controllers;
namespace GemsWeb
{
    public partial class Main : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            MailHandler.url = Request.Url.ToString().Replace(Request.RawUrl.Replace("%2f", "/"), "");
            string userid = (string)Session["username"];
            int login = 0;
            try
            {
                login = int.Parse(Session["Login"].ToString());
                if (login == 0)
                {
                    pnl1.Visible = true;
                    hypHome.NavigateUrl = "~/default.aspx";
                }
                else
                {
                    int domain;
                    bool success = int.TryParse(Session["Domain"].ToString(), out domain);
                    if (success)
                    {
                        if (domain == 0 || domain == 1)
                            hypCart.NavigateUrl = "~/viewcart.aspx?" + Session["partiEmail"].ToString();

                        if (domain == 0)
                        {
                            this.plcNUSNET.Visible = false;
                        }
                        else if (domain == 1)
                        {
                            hypHome.NavigateUrl = "~/SelectEventPage.aspx";
                            this.plcNUSNET.Visible = true;
                        }
                        else if (domain == 2)
                        {
                            hypHome.NavigateUrl = "~/RequesteePage.aspx";
                            this.plcNUSNET.Visible = false;
                            this.plcParti.Visible = false;
                        }
                    }
                    pnl1.Visible = false;

                }
            }
            catch (Exception)
            {
                Session["Login"] = "0";
            }

            pnl2.Visible = !pnl1.Visible;

        }

        protected void Signout_Click(object sender, EventArgs e)
        {
            //Carts.LoadCart(Session["partiEmail"].ToString().ToString());
            //Carts.DeleteUserCart(Session["partiEmail"].ToString().ToString());
            Session["Login"] = "0";
            Session["Domain"] = "-1";
            Session["partiEmail"] = "";
            Session["username"] = "";
            Session["ReQuestEE"] = "";
            Session["nusNETuser"] = "";

            FormsAuthentication.SignOut();
            Response.Redirect("~/default.aspx");
        }

    }
}