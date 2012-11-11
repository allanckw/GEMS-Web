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
    public partial class ExpressCheckoutSuccess : System.Web.UI.Page
    {
        private string business = WebConfigurationManager.AppSettings["BusinessEmail"];
        private string currency_code = WebConfigurationManager.AppSettings["CurrencyCode"];
        static DataSet requests = new DataSet();

        private void Page_Load(object sender, EventArgs e)
        {
            string partiMail = Session["partiEmail"].ToString();
            string requestUriString;
            CultureInfo provider = new CultureInfo("en-us");
            string requestsFile = this.Server.MapPath("~/App_Data/PaymentRequests.xml");

            requests.Clear();
            if (System.IO.File.Exists(requestsFile))
            {
                requests.ReadXml(requestsFile);
            }
            else
            {
                Carts.CreateXml(requestsFile, "Requests");
                requests.ReadXml(requestsFile);
            }

            string strFormValues = Encoding.ASCII.GetString(
                this.Request.BinaryRead(this.Request.ContentLength));

            // getting the URL to work with
            if (String.Compare(WebConfigurationManager.AppSettings["UseSandbox"].ToString(), "true", false) == 0)
            {
                requestUriString = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            }
            else
            {
                requestUriString = "https://www.paypal.com/cgi-bin/webscr";
            }

            // Create the request back
            HttpWebRequest request =
                (HttpWebRequest)WebRequest.Create(requestUriString);

            // Set values for the request back
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            string obj2 = strFormValues + "&cmd=_notify-validate";
            request.ContentLength = obj2.Length;

            // Write the request back IPN strings
            StreamWriter writer =
                new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
            writer.Write(RuntimeHelpers.GetObjectValue(obj2));
            writer.Close();

            //send the request, read the response
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            Encoding encoding = Encoding.GetEncoding("utf-8");
            StreamReader readStream = new StreamReader(responseStream, encoding);


            Char[] buffer = new Char[257];
            // Reads 256 characters at a time.
            int count = readStream.Read(buffer, 0, 256);
            while (count > 0)
            {
                // Dumps the 256 characters to a string

                string amount;
                string IPNResponse = new string(buffer, 0, count);
                count = readStream.Read(buffer, 0, 256);
                try
                {
                    // getting the total cost of the goods in cart for an identifier
                    // of the request stored in the "custom" variable

                    string x = this.Request["custom"].ToString();
                    amount = GetRequestPrice(x);
                    if (String.Compare(amount, "", false) == 0)
                    {

                        readStream.Close();
                        response.Close();
                        return;
                    }
                }
                catch (Exception ex)
                {

                    readStream.Close();
                    response.Close();
                    return;
                }

                NumberFormatInfo info2 = new NumberFormatInfo();
                info2.NumberDecimalSeparator = ".";
                info2.NumberGroupSeparator = ",";
                info2.NumberGroupSizes = new int[] { 3 };
                string tx = this.Request["txn_id"];

                // if the request is verified
                if (String.Compare(IPNResponse, "VERIFIED", false) == 0)
                {
                    try
                    {
                        savePaymentToDatabase(partiMail, tx);
                    }
                    catch (Exception ex)
                    {
                        Carts.LogFile(tx, ex.Message);
                    }
                }
                else
                {
                    Carts.LogFile(tx, "IPN Response was Invalid, Was Given: " + IPNResponse);
                }
            }
            readStream.Close();
            response.Close();
        }

        protected void savePaymentToDatabase(string email, string tx)
        {
            decimal paymentAmt = (decimal)Session["TotalExpense"];

            ParticipantsTransactionsClient client = new ParticipantsTransactionsClient();
            List<ParticipantTransaction> transList = new List<ParticipantTransaction>();

            DataSet dsCarts = Carts.LoadCart(email);
            foreach (DataRow dr in dsCarts.Tables[0].Rows)
            {
                ParticipantTransaction t = new ParticipantTransaction();
                t.Amount = decimal.Parse(dr["price"].ToString());
                t.Email = email;
                t.EventID = int.Parse(dr["Event_ID"].ToString());
                t.TransactionDateTime = DateTime.Now;
                t.TransactionID = tx;
                t.Remarks = "";
                transList.Add(t);
            }

            client.SaveTransaction(tx, transList.ToArray());
            Carts.DeleteUserCart(email);
            client.Close();
            MailHandler.sendPaymentReceivedMail(email, tx, paymentAmt);

        }

        public static string GetIDCart(string request_id)
        {

            string expression = null;
            expression = "request_id = '" + request_id + "'";
            DataRow[] tempRow = requests.Tables[0].Select(expression);
            if (tempRow.Length == 1)
            {
                return tempRow[0]["cart_id"].ToString();
            }
            return "";
        }

        public static string GetRequestPrice(string request_id)
        {

            string expression = null;
            expression = "request_id = '" + request_id + "'";
            DataRow[] tempRow = requests.Tables[0].Select(expression);
            if (tempRow.Length == 1)
            {
                return tempRow[0]["price"].ToString();
            }

            return "";
        }
    }
}