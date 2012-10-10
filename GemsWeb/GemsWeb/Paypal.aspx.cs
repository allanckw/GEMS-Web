using System.Web.Configuration;
using System;

namespace GemsWeb
{
    public partial class Paypal : System.Web.UI.Page
    {
        protected string cmd = "_xclick";
        protected string business = WebConfigurationManager.AppSettings["BusinessEmail"].ToString();
        protected string item_name;
        protected string amount;
        protected string return_url = WebConfigurationManager.AppSettings["ReturnUrl"].ToString();
        protected string notify_url = WebConfigurationManager.AppSettings["NotifyUrl"].ToString();
        protected string cancel_url = WebConfigurationManager.AppSettings["CancelPurchaseUrl"].ToString();
        protected string currency_code = WebConfigurationManager.AppSettings["CurrencyCode"].ToString();
        protected string no_shipping = "1";
        protected string URL;
        protected string request_id;

        protected string rm;

        private void Page_Load(System.Object sender, System.EventArgs e)
        {
            // determining the URL to work with depending on whether sandbox or a real PayPal account should be used
            bool sandbox = Convert.ToBoolean(WebConfigurationManager.AppSettings["UseSandbox"]);
            if (sandbox)
            {
                URL = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            }
            else
            {
                URL = "https://www.paypal.com/cgi-bin/webscr";
            }

            //This parameter determines the was information about successfull transaction will be passed to the script
            // specified in the return_url parameter.
            // "1" - no parameters will be passed.
            // "2" - the POST method will be used.
            // "0" - the GET method will be used. 
            // The parameter is "0" by deault.
            bool sendToReturnURL = false;
            sendToReturnURL = Convert.ToBoolean(WebConfigurationManager.AppSettings["SendToReturnURL"]);
            if (sendToReturnURL)
            {
                rm = "2";
            }
            else
            {
                rm = "1";
            }

            // the total cost of the cart
            amount = Session["Amount"].ToString();
            // the identifier of the payment request
            request_id = Session["request_id"].ToString();

            item_name = "Payment for Events - NUS GEMS";
        }

    }
}