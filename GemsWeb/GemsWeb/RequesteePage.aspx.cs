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
    public partial class RequesteePage : System.Web.UI.Page
    {
        #region "Page Global Function"

        private Requestee RequesteeUser()
        {
            Requestee r;
            try
            {
                 r = (Requestee)Session["ReQuestEE"];
            }
            catch (Exception)
            {
                return null;
            }
            return r;
        }

        protected string RequestorName(string s)
        {
            AdministrationClient adClient = new AdministrationClient();
            string uploader = adClient.GetUserName(s);
            adClient.Close();
            return uploader;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                bool authenticated = true;
                int domain = int.Parse(Session["Domain"].ToString());
                if (domain != 2)
                    authenticated = false;

                if (!authenticated)
                    Response.Redirect("~/Error403.aspx");



                ddlStatus.Items.Clear();
                ddlStatus.DataSource = Enum.GetNames(typeof(RequestStatus));
                ddlStatus.DataBind();
                ddlStatus.SelectedIndex = 0;

                ddlRequesteeStatus.Items.Clear();
                ddlRequesteeStatus.DataSource = Enum.GetNames(typeof(RequestStatus));
                ddlRequesteeStatus.DataBind();

                dpFrom.Enabled = false;
                dpTo.Enabled = false;
                RequestEvents(DateTime.Now.AddMonths(-1), DateTime.Now);

            }
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
            Clear();
            RequestEvents(dpFrom.CalDate, dpTo.CalDate);
            //btnRequestNew_Click(sender, e);
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

            List<Request> requests;
            try
            {
                RequestClient client = new RequestClient();
                requests = client.ViewRequests(RequesteeUser(), start, end, requestStatus, viewAll).ToList<Request>();
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


            txtFrmWho.Text = RequestorName(request.Requestor);
            txtRequestTitle.Text = request.Title;
            txtRequestDesc.Text = request.Description;
            for (int i = 0; i < ddlRequesteeStatus.Items.Count; i++)
            {
                if (ddlRequesteeStatus.Items[i].Value==request.Status.ToString())
                {
                    ddlRequesteeStatus.SelectedIndex = i;
                    break;
                }
            }

            hypLnkFileUrl.Visible = false;
            if (request.URL.Trim().Length>0)
            {
                hypLnkFileUrl.Visible = true;
                hypLnkFileUrl.NavigateUrl = request.URL;
            }

            if (request.Logs.Count() > 0)
            {
                lblRequestLogLabel.Visible = true;
                gvRequestLog.Visible = true;
                gvRequestLog.DataSource = request.Logs;
                gvRequestLog.PageIndex = page;
                gvRequestLog.DataBind();
            }
        }

        protected void gvRequestLog_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            int requestID = int.Parse(lstRequest.SelectedValue);
            retRequest(requestID, e.NewPageIndex);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (hidRequestID.Value == "")
                return;
            RequestClient client = new RequestClient();
            int requestID = int.Parse(hidRequestID.Value);
            RequestStatus requestStatus = (RequestStatus)Enum.Parse(typeof(RequestStatus), ddlRequesteeStatus.SelectedValue);
            try
            {
                client.ChangeStatus(RequesteeUser(), requestID, requestStatus, txtRemarks.Text.Trim());
                retRequest(requestID);
                Alert.Show("Request Status Updated Successfully!");
            }
            catch (Exception ex)
            {
                Alert.Show("Error Updating the request with message: " + ex.Message);
                //throw;
            }
            finally
            {
                client.Close();
            }
            
        }

        protected void Clear()
        {
            txtFrmWho.Text = "";
            txtRemarks.Text = "";
            txtRequestDesc.Text = "";
            txtRequestTitle.Text = "";
            lblRequestLogLabel.Visible = false;
            gvRequestLog.Visible = false;
            ddlRequesteeStatus.SelectedIndex = 0;
        }
    }
}