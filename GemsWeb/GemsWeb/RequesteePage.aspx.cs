using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using evmsService.entities;
using GemsWeb.Controllers;
using System.Web.Security;

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


        protected string RequestorEmail(string s)
        {
            AdministrationClient adClient = new AdministrationClient();
            string uploader = adClient.GetUserEmail(s);
            adClient.Close();
            return uploader;
        }
        #endregion

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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int domain = -1;
                try
                {
                    domain = int.Parse(Session["Domain"].ToString());
                }
                catch (Exception)
                {
                    domain = -1;
                }

                if (domain != 2)
                    Response.Redirect("~/Error404.aspx");//authenticated = false;

                btnApprove.Visible = false;
                btnReject.Visible = false;
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
                Alert.Show("Error Retreiving List of Request from Server: " + ex.Message, false, "~/Default.aspx");
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
                if (ddlRequesteeStatus.Items[i].Value == request.Status.ToString())
                {
                    ddlRequesteeStatus.SelectedIndex = i;
                    break;
                }
            }

            if (request.Status == RequestStatus.Pending)
            {
                btnApprove.Visible = true;
                btnReject.Visible = true;
                txtRemarks.Text = "";
                txtRemarks.ReadOnly = false;
            }
            else
            {
                btnApprove.Visible = false;
                btnReject.Visible = false;
                txtRemarks.Text = request.Remark;
                txtRemarks.ReadOnly = true;
            }

            hypLnkFileUrl.Visible = false;
            if (request.URL.Trim().Length > 0)
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


        private void UpdateRequestFromRequestee(RequestStatus requestStatus)
        {
            if (hidRequestID.Value == "")
                return;
            RequestClient client = new RequestClient();
            int requestID = int.Parse(hidRequestID.Value);
            Request r = client.GetRequest(requestID);
            try
            {
                client.ChangeStatus(RequesteeUser(), r.RequestID, requestStatus, txtRemarks.Text.Trim());
                retRequest(requestID);
                MailHandler.sendRequestorMail(txtRequestTitle.Text, RequestorEmail(r.Requestor),
                    txtRemarks.Text.Trim(), requestStatus.ToString(), RequesteeUser().TargetEmail);
                if (requestStatus==RequestStatus.Approved)
                {
                    Alert.Show("The Request has been successfully approved!");
                }
                else
                    Alert.Show("The Request has been successfully rejected!");
                
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
            Clear();
            RequestEvents(dpFrom.CalDate, dpTo.CalDate);
        }

        protected void Clear()
        {
            txtFrmWho.Text = "";
            txtRemarks.Text = "";
            txtRequestDesc.Text = "";
            txtRequestTitle.Text = "";
            txtRemarks.ReadOnly = false;
            lblRequestLogLabel.Visible = false;
            gvRequestLog.Visible = false;
            //ddlRequesteeStatus.SelectedIndex = 0;
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {

            UpdateRequestFromRequestee(RequestStatus.Approved);
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            UpdateRequestFromRequestee(RequestStatus.Rejected);
        }
    }
}