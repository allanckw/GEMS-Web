using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Configuration;
using GemsWeb.Controllers;

namespace GemsWeb
{
    public partial class IPNHandler : System.Web.UI.Page
    {
        private string business = WebConfigurationManager.AppSettings["BusinessEmail"];
        private string currency_code = WebConfigurationManager.AppSettings["CurrencyCode"];
        static DataSet requests = new DataSet();

        protected void Page_Load(System.Object sender, System.EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-us");
            string requestsFile = Server.MapPath("~/App_Data/PaymentRequests.xml");
            requests.Clear();

            if (File.Exists(requestsFile))
            {
                requests.ReadXml(requestsFile);
            }
            else
            {
                Carts.CreateXml(requestsFile, "Requests");
                requests.ReadXml(requestsFile);
            }

           

            string strFormValues = Encoding.ASCII.GetString(Request.BinaryRead(Request.ContentLength));
            dynamic strNewValue = null;

            // getting the URL to work with
            string URL = null;
            bool sandbox = Boolean.Parse(WebConfigurationManager.AppSettings["UseSandbox"]);
            if (sandbox)
            {
                URL = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            }
            else
            {
                URL = "https://www.paypal.com/cgi-bin/webscr";
            }

            // Create the request back
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);


            // Set values for the request back
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            strNewValue = strFormValues + "&cmd=_notify-validate";
            req.ContentLength = strNewValue.Length;

            // Write the request back IPN strings
            StreamWriter stOut = new StreamWriter(req.GetRequestStream(), Encoding.ASCII);
            stOut.Write(strNewValue);
            stOut.Close();

            //send the request, read the response
            HttpWebResponse strResponse = (HttpWebResponse)req.GetResponse();
            Stream IPNResponseStream = strResponse.GetResponseStream();
            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader readStream = new StreamReader(IPNResponseStream, encode);

            Char[] buffer = new Char[257];
            // Reads 256 characters at a time.
            int count = readStream.Read(buffer, 0, 256);

            while (count > 0)
            {
                // Dumps the 256 characters to a string
                String IPNResponse = new String(buffer, 0, count);
                count = readStream.Read(buffer, 0, 256);
                string amount;
                try
                {
                    // getting the total cost of the goods in cart for an identifier of the request stored in the "custom"
                    // variable
                    amount = GetRequestPrice(Request["custom"].ToString());
                    if (string.IsNullOrEmpty(amount))
                    {
                        readStream.Close();
                        strResponse.Close();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    readStream.Close();
                    strResponse.Close();
                    return;
                }

                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                provider.NumberGroupSeparator = ",";
                provider.NumberGroupSizes = new int[] { 3 };

                // if the request is verified
                if (IPNResponse == "VERIFIED")
                {

                }
                else
                {
                  
                }
            }

            readStream.Close();
            strResponse.Close();
        }

        public static string GetIDCart(string request_id)
        {

            try
            {
                string expression = null;
                expression = "request_id = '" + request_id + "'";
                DataRow[] tempRow = requests.Tables[0].Select(expression);
                if (tempRow.Length == 1)
                {
                    return tempRow[0]["cart_id"].ToString();
                }
            }
            catch (Exception ex)
            {
                
            }

            return "";
        }

        public static string GetRequestPrice(string request_id)
        {
            try
            {
                string expression = null;
                expression = "request_id = '" + request_id + "'";
                DataRow[] tempRow = requests.Tables[0].Select(expression);
                if (tempRow.Length == 1)
                {
                    return tempRow[0]["price"].ToString();
                }

            }
            catch (Exception ex)
            {

            }

            return "";
        }
    }
}