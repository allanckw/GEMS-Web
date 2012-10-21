﻿using System;
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
                }
                else
                {
                    pnl1.Visible = false;

                }
            }
            catch (Exception )
            {
                Session["Login"] = "0";
            }

            pnl2.Visible = !pnl1.Visible;
          
        }

        protected void Signout_Click(object sender, EventArgs e)
        {
            Carts.LoadCart(Session["partiEmail"].ToString().ToString());
            Carts.DeleteUserCart(Session["partiEmail"].ToString().ToString());
            Session["Login"] = "0";
            Session["Domain"] = "-1";
            Session["partiEmail"] = "";
            Session["username"] = "";
            FormsAuthentication.SignOut();
            Response.Redirect("~/default.aspx");
        }

    }
}