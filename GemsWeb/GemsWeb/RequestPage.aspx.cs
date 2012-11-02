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
    public partial class RequestPage : System.Web.UI.Page
    {

        #region "Page Global Function"
        private User NUSNetUser()
        {
            try
            {
                User u = (User)Session["nusNETuser"];
                return u;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        private int EventID()
        {
            int eventID = int.Parse(Request.QueryString["EventID"]);
            return eventID;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ddlStatus.Items.Clear();
                ddlStatus.DataSource = Enum.GetNames(typeof(RequestStatus));
                ddlStatus.DataBind();
                ddlStatus.Items.Add("ALL");
                ddlStatus.SelectedIndex = ddlStatus.Items.Count - 1;
                dpFrom.Enabled = false;
                dpTo.Enabled = false;
                getEventSummary();
                RequestEvents(DateTime.Now.AddMonths(-1), DateTime.Now);
            }

            //System.Web.UI.ScriptManager sc = (System.Web.UI.ScriptManager)Master.FindControl("ScriptManager1");

            //if (sc != null)
            //{
            //    sc.RegisterPostBackControl(btnSearch);
            //    sc.RegisterPostBackControl(btnRequestSave);
            //    sc.RegisterPostBackControl(btnRequestCancel);
            //}
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
            RequestEvents(dpFrom.CalDate, dpTo.CalDate);
            btnRequestNew_Click(sender, e);
        }

        private void RequestEvents(DateTime start, DateTime end)
        {
            bool viewAll = true;
            RequestStatus requestStatus;
            try
            {
                requestStatus = (RequestStatus)Enum.Parse(typeof(RequestStatus), ddlStatus.SelectedValue);
                viewAll = false;
            }
            catch (Exception)
            {
                requestStatus = (RequestStatus)Enum.Parse(typeof(RequestStatus), ddlStatus.Items[0].Value);
            }

            Request[] requests;
            try
            {
                RequestClient client = new RequestClient();
                requests = client.ViewRequestViaRequester(EventID(), NUSNetUser(), start, end, requestStatus, viewAll);
                client.Close();

                lstRequest.Items.Clear();

                if (requests.Count() > 0)
                {
                    lstRequest.DataSource = requests;
                    lstRequest.DataTextField = "Title";
                    lstRequest.DataValueField = "RequestID";
                    lstRequest.DataBind();
                }
            }
            catch (Exception ex)
            {
                Alert.Show("Error Retreiving List of Request from Server", false, "~/Default.aspx");
                return;
            }


        }

        private void getEventSummary()
        {
            Events event_;
            EventClient client = new EventClient();
            event_ = client.GetEvent(EventID());
            lblEventName.Text = event_.Name;
            lblFrmDate.Text = event_.StartDateTime.ToString("dd MMM yyyy");
            lblToDate.Text = event_.EndDateTime.ToString("dd MMM yyyy");
            txtEventDesc.Text = event_.Description;
            client.Close();
        }

        protected void lstRequest_SelectedIndexChanged(object sender, EventArgs e)
        {
            hidRequestID.Value = "";
            if (lstRequest.SelectedIndex == -1)
                return;

            int requestID = int.Parse(lstRequest.SelectedValue);
            retRequest(requestID);
        }

        private void retRequest(int RequestID, int page = 0)
        {
            Request request;
            hidRequestID.Value = RequestID.ToString();
            RequestClient client = new RequestClient();
            request = client.GetRequest(RequestID);
            client.Close();

            if (txtToWho.Text.Trim() == "")
            {
                txtToWho.Text = request.TargetEmail;
                txtRequestTitle.Text = request.Title;
                txtRequestDesc.Text = request.Description;
                txtFileUrl.Text = request.URL;
            }
            //txtToWho.Text = request.TargetEmail;
            //txtRequestTitle.Text = request.Title;
            //txtRequestDesc.Text = request.Description;
            //txtFileUrl.Text = request.URL;

            txtToWho.ReadOnly = true;
            txtRequestTitle.ReadOnly = true;
            lblRequestLogLabel.Visible = false;
            gvRequestLog.DataSource = null;
            gvRequestLog.DataBind();

            if (request.Logs.Count() > 0)
            {
                lblRequestLogLabel.Visible = true;
                gvRequestLog.Visible = true;
                gvRequestLog.DataSource = request.Logs;
                gvRequestLog.PageIndex = page;
                gvRequestLog.DataBind();
            }
        }

        protected void btnRequestNew_Click(object sender, EventArgs e)
        {
            hidRequestID.Value = "";
            lstRequest.SelectedIndex = -1;
            txtFileUrl.Text = "";
            txtRequestTitle.Text = "";
            txtRequestDesc.Text = "";
            txtToWho.Text = "";
            txtToWho.ReadOnly = false; ;
            txtRequestTitle.ReadOnly = false;
            lblRequestLogLabel.Visible = false;
            gvRequestLog.Visible = false;
        }

        protected void btnRequestCancel_Click(object sender, EventArgs e)
        {
            if (hidRequestID.Value == "")
            {
                return;
            }
            else
            {
                RequestClient client = new RequestClient();
                //call cancel request
                int requestID = int.Parse(hidRequestID.Value);
                try
                {
                    client.CancelRequest(NUSNetUser(), requestID);
                    retRequest(requestID);
                    Alert.Show("Request Cancelled!");
                }
                catch (Exception)
                {
                    Alert.Show("Request Failed to cancel!");
                }
                finally
                {
                    client.Close();
                }
            }
        }

        protected void btnRequestSave_Click(object sender, EventArgs e)
        {
            if (hidRequestID.Value == "")
            {
                //Add New Request
                RequestClient client = new RequestClient();
                try
                {
                    client.CreateNewRequest(NUSNetUser(), EventID(), txtToWho.Text.Trim(), txtRequestDesc.Text.Trim(), txtFileUrl.Text.Trim(), txtRequestTitle.Text.Trim());
                    Alert.Show("Request Created Successfully!");
                    btnRequestNew_Click(sender, e);
                    RequestEvents(dpFrom.CalDate, dpTo.CalDate);
                }
                catch (Exception)
                {
                    Alert.Show("Error Creating Request");
                    //throw;
                }
                finally
                {
                    client.Close();
                }
            }
            else
            {
                int requestID = int.Parse(hidRequestID.Value);
                RequestClient client = new RequestClient();
                try
                {
                    client.EditRequest(NUSNetUser(), requestID, txtRequestDesc.Text.Trim(), txtFileUrl.Text.Trim());
                    retRequest(requestID);
                    Alert.Show("Request updated Successfully!");
                }
                catch (Exception)
                {
                    Alert.Show("Error Updating Request");
                    //throw;
                }
                finally
                {
                    client.Close();
                }
            }
        }

        protected void gvRequestLog_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            int requestID = int.Parse(lstRequest.SelectedValue);
            retRequest(requestID, e.NewPageIndex);
        }
    }
}