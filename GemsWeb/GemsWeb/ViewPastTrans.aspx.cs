using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using evmsService.entities;
using GemsWeb.Controllers;

namespace GemsWeb
{
    public partial class ViewPastTrans : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                dpFrom.Enabled = false;
                dpTo.Enabled = false;
                retPastTransaction(DateTime.Now.AddMonths(-1), DateTime.Now);
            }
        }

        private void retPastTransaction(DateTime start, DateTime end)
        {

            ParticipantsTransactionsClient ptClient = new ParticipantsTransactionsClient();
            try
            {
                ParticipantTransaction[] trans = ptClient.ViewParticipantsTransactions(ParticipantEmail(), start, end);
                gvPastTransaction.DataSource = trans;
                gvPastTransaction.DataBind();
            }
            catch (Exception)
            {
                Alert.Show("Error Retreiving Past Transaction!");
            }
            finally
            {
                ptClient.Close();
            }
        }

        private string ParticipantEmail()
        {
            string partiEmail = Session["partiEmail"].ToString();
            return partiEmail;
        }

        protected void rdlstFromDateRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            dpFrom.Enabled = true;
            if (rdlstFromDateRange.SelectedValue != "-1")
            {
                dpFrom.Enabled = false;
                switch (rdlstFromDateRange.SelectedValue)
                {
                    case "30":
                        dpFrom.CalDate = DateTime.Now.AddMonths(-1);
                        break;
                    case "90":
                        dpFrom.CalDate = DateTime.Now.AddMonths(-3);
                        break;
                    case "365":
                        dpFrom.CalDate = DateTime.Now.AddYears(-1);
                        break;
                    default:
                        break;
                }
            }
        }

        protected void rdlstToDateRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            dpTo.Enabled = true;
            if (rdlstToDateRange.SelectedValue == "0")
            {
                dpTo.CalDate = DateTime.Now;
                dpTo.Enabled = false;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            retPastTransaction(dpFrom.CalDate, dpTo.CalDate);
        }

        protected string getEventName(string eventID)
        {
            int eID = int.Parse(eventID);
            EventClient evClient = new EventClient();
            string evName = evClient.GetEventName(eID);
            evClient.Close();
            return evName;
        }

        protected string currencyFormat(string value)
        {
            decimal amt = decimal.Parse(value);
            return amt.ToString("C");
        }
    }
}