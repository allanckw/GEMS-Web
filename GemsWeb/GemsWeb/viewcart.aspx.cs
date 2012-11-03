using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Globalization;
using GemsWeb.Controllers;

namespace GemsWeb
{
    public partial class viewcart : System.Web.UI.Page
    {
        protected void Page_Load(System.Object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                bool authenticated = true;
                int domain = int.Parse(Session["Domain"].ToString());
                if (domain >= 2)
                    authenticated = false;

                if (!authenticated)
                    Response.Redirect("~/Error403.aspx");

            }
            // if the cart identifier is not passed in the request string
            if (Request.QueryString.Count == 0)
            {
                this.btnPayPal.Visible = false;
                this.gvCarts.Visible = false;
            }
        }



        /// <summary>
        /// creating a record about the payment request
        /// </summary>
        /// <param name="cart_id">the cart identifier</param>
        /// <param name="cost">the total cost of the cart</param>
        /// <returns>the identifier of the request_id request</returns>
        protected int CreatePaymentRequest(string cart_id, decimal cost)
        {

            string xmlFile = Server.MapPath("~/App_Data/PaymentRequests.xml");
            XmlDocument doc = new XmlDocument();
            CultureInfo ci = new CultureInfo("en-us");

            XmlTextReader reader = default(XmlTextReader);

            if (File.Exists(xmlFile))
            {
                reader = new XmlTextReader(xmlFile);
                reader.Read();
            }
            else
            {
                Carts.CreateXml(xmlFile, "Requests");
                reader = new XmlTextReader(xmlFile);
                reader.Read();
            }

            doc.Load(reader);
            reader.Close();

            // getting a unique request identifier
            XmlNodeList nodes = doc.GetElementsByTagName("Request");
            int request_id = 0;
            if (nodes.Count != 0)
            {
                request_id = Carts.GetIdentity(nodes, "request_id");
            }
            else
            {
                request_id = 0;
            }

            // creating a new element containing information about the payment request
            XmlElement myrequest = doc.CreateElement("Request");
            myrequest.SetAttribute("request_id", request_id.ToString());
            myrequest.SetAttribute("cart_id", cart_id);
            myrequest.SetAttribute("price", cost.ToString(ci));
            myrequest.SetAttribute("request_date", DateTime.Now.ToString(ci));
            doc.DocumentElement.AppendChild(myrequest);

            try
            {
                doc.Save(xmlFile);
            }
            catch (Exception ex)
            {
                //Carts.WriteFile("Error in ViewCart.CreatePaymentRequest(): " + ex.Message);
                return -1;
            }

            return request_id;
        }

        protected void gvCarts_DataBound(object sender, System.EventArgs e)
        {
            try
            {
                // adding a footer with information about the total cost of goods in the cart
                CultureInfo ci = new CultureInfo("en-us");
                GridViewRow footer = gvCarts.FooterRow;
                footer.Cells[0].ColumnSpan = 2;
                footer.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                footer.Cells.RemoveAt(1);
                footer.Cells[0].Text = "Total amount for payment: " + CalculateTotalAmount().ToString("C", ci);
            }
            catch (Exception ex)
            {
                //Carts.WriteFile("Error in ViewCart.gvCarts_DataBound(): " + ex.Message);
            }
        }

        /// <summary>    
        /// getting the total cost of goods in the cart
        /// </summary>
        /// <returns>the total cost of goods in the cart</returns>
        protected decimal CalculateTotalAmount()
        {
            decimal total = 0;
            CultureInfo ci = new CultureInfo("en-us");
            try
            {
                foreach (GridViewRow row in this.gvCarts.Rows)
                {
                    decimal price = decimal.Parse(row.Cells[2].Text, ci);
                    total += price;
                }
                Session["TotalExpense"] = total;
                return total;
            }
            catch (Exception ex)
            {
                //Carts.WriteFile("Error in ViewCart.CalculateTotalAmount(): Input string was not in a correct format");
                return 0;
            }
        }

        protected void btnPayPal_Click(object sender, ImageClickEventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-us");
            // getting the total cost of the cart
            decimal cost = CalculateTotalAmount();
            if (cost == 0)
            {
                Response.Redirect("~/default.aspx");
            }

            try
            {
                Session["Amount"] = cost.ToString(ci);

                // creating a record about the payment request
                int request_id = CreatePaymentRequest(Request.QueryString["cart_id"].ToString(), cost);
                if (request_id != -1)
                {
                    Session["request_id"] = request_id.ToString();
                }
                else
                {
                    return;
                }

            }
            catch (Exception ex)
            {
                //Carts.WriteFile("Error in ViewCart.ibPayPal_Click(): " + ex.Message);
                Response.Redirect("~/default.aspx");
            }

            Response.Redirect("~/PayPal.aspx");
        }
    }
}