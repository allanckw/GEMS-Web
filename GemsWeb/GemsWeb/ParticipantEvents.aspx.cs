using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml;
using evmsService.Controllers;
using GemsWeb.Controllers;
using System.Diagnostics;

namespace GemsWeb
{
    public partial class ParticipantEvents : System.Web.UI.Page
    {
        //TODO, Put radio buttons to set the date
        //1 month, 3 months this year..
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                bool authenticated = true;
                txtEmail.Text = Session["partiEmail"].ToString();
                lblNoEvents.Visible = false;

                int domain = int.Parse(Session["Domain"].ToString());
                if (domain >= 2)
                    authenticated = false;

                if (!authenticated)
                    Response.Redirect("~/Error403.aspx");
            }

            System.Web.UI.ScriptManager sc = (System.Web.UI.ScriptManager)Master.FindControl("ScriptManager1");

            if (sc != null)
            {
                sc.RegisterPostBackControl(btnSearch);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (dpFrom.CalDate > dpTo.CalDate)
            {
                loadData(dpTo.CalDate, dpFrom.CalDate);
            }
            else
            {
                loadData(dpFrom.CalDate, dpTo.CalDate);
            }

            if (string.Compare(Session["partiEmail"].ToString(), txtEmail.Text.Trim(), true) != 0)
            {
                Session["partiEmail"] = txtEmail.Text.Trim();
            }
        }

        private void loadData(DateTime from, DateTime to)
        {
            RegistrationClient regClient = new RegistrationClient();
            try
            {
                List<ParticipantEvent> unPaidEvents = regClient.ParticipantViewEvents(txtEmail.Text.Trim(), from, to, false).ToList<ParticipantEvent>();
                List<ParticipantEvent> paidEvents = regClient.ParticipantViewEvents(txtEmail.Text.Trim(), from, to, true).ToList<ParticipantEvent>();

                regClient.Close();


                if (paidEvents.Count == 0 && unPaidEvents.Count == 0)
                {
                    pnlEvents.Visible = false;
                }
                else
                {
                    gvUnpaid.DataSource = unPaidEvents;
                    gvPaid.DataSource = paidEvents;

                    gvPaid.DataBind();
                    gvUnpaid.DataBind();

                    pnlEvents.Visible = true;
                }
                lblNoEvents.Visible = !pnlEvents.Visible;

            }
            catch (Exception ex)
            {
                lblNoEvents.Visible = true;
                pnlEvents.Visible = false;
            }
        }

        protected void gv_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            GridView gv = (GridView)sender;
            gv.PageIndex = e.NewPageIndex;
            btnSearch_Click(btnSearch, new EventArgs());
            gv.DataBind();
        }

        protected bool AddToBasket(string cart_id, string eventID, string price, string eventName)
        {

            string xmlFile = Server.MapPath("~/App_Data/Carts.xml");
            XmlDocument doc = new XmlDocument();
            XmlTextReader reader = default(XmlTextReader);

            if (File.Exists(xmlFile))
            {
                reader = new XmlTextReader(xmlFile);
                reader.Read();
            }
            else
            {
                Carts.CreateXml(xmlFile, "Carts");
                reader = new XmlTextReader(xmlFile);
                reader.Read();
            }

            doc.Load(reader);
            reader.Close();

            // getting a unique number of the rec_id record.
            XmlNodeList nodes = doc.GetElementsByTagName("Cart");

            if (Carts.isDuplicateEvent(nodes, eventID, GetCartID()))
            {
                Alert.Show("You have already added the event to cart!");
            }
            else
            {
                int rec_id;
                if (nodes.Count != 0)
                {
                    rec_id = Carts.GetIdentity(nodes, "rec_id");
                }
                else
                {
                    rec_id = 1;
                }

                // creating a new XML element (adding an item in a cart)
                XmlElement cart = doc.CreateElement("Cart");
                cart.SetAttribute("rec_id", rec_id.ToString());
                cart.SetAttribute("cart_id", cart_id);
                cart.SetAttribute("Event_ID", eventID);
                cart.SetAttribute("Event_Name", eventName);
                cart.SetAttribute("price", price);
                cart.SetAttribute("quantity", "1");
                doc.DocumentElement.AppendChild(cart);

                try
                {
                    doc.Save(xmlFile);
                    Alert.Show("Added to cart, click on view cart to make payment", false);
                }
                catch (Exception ex)
                {
                    Response.Write("Error in Default.AddToBasket(): " + ex.Message);
                    Alert.Show("An error has occured, please try again", false);
                    return false;
                }
            }
            return true;
        }

        protected void gvGoods_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            // handling the event of the "Add To Basket" button pressure
            if (e.CommandName == "AddToBasket")
            {
                int index;
                bool isParsed = int.TryParse(e.CommandArgument.ToString(), out index);


                // Checking whether the quantity of goods added to the cart is correct
                this.Validate();
                // adding goods to the cart
                if (isParsed)
                {
                    AddToBasket(GetCartID(), GetItemID(index), GetPrice(index), GetEventName(index));
                }
            }
        }

        protected string GetCartID()
        {
            return txtEmail.Text.Trim();
        }

        protected string GetItemID(int index)
        {
            return this.gvUnpaid.DataKeys[index].Value.ToString();
        }

        protected string GetEventName(int index)
        {
            return this.gvUnpaid.Rows[index].Cells[1].Text;
        }

        protected string GetPrice(int index)
        {
            CultureInfo ci = new CultureInfo("en-us");
            return this.gvUnpaid.Rows[index].Cells[2].Text.ToString(ci);
        }

        protected void btnViewCart_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("~/ViewCart.aspx" + "?cart_id=" + GetCartID());
        }


    }
}