using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GemsWeb.CustomControls;
using GemsWeb.Controllers;
using evmsService.entities;
using evmsService.Controllers;

namespace GemsWeb
{
    //Reference: http://www.codeproject.com/Articles/26589/Dynamically-add-and-remove-user-controls;
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            AddRegistrationField();
        }

        public Control GetPostBackControl(Page page)
        {
            Control control = null;

            string ctrlname = page.Request.Params.Get("__EVENTTARGET");
            if ((ctrlname != null) & ctrlname != string.Empty)
            {
                control = page.FindControl(ctrlname);
            }
            else
            {
                foreach (string ctl in page.Request.Form)
                {
                    Control c = page.FindControl(ctl);
                    if (c is System.Web.UI.WebControls.Button)
                    {
                        control = c;
                        break;
                    }
                }
            }
            return control;
        }

        private int EventID
        {
            get
            {
                int evID;
                bool success = int.TryParse(ViewState["EventID"].ToString(), out evID);
                if (success)
                {
                    return evID;
                }
                else
                {
                    return -1;
                }
            }

            set
            {
                ViewState["EventID"] = value;
            }
        }

        private void AddRegistrationField()
        {
            //Determine which control fired the postback event. 
            Control c = GetPostBackControl(Page);

            //Be sure everything in the placeholder control is cleared out
            this.phRegister.Controls.Clear();

            int ControlID = 0;

            String strEvent = Request.QueryString["EventID"];
            int intEvent;


            bool AcceptedEvent = int.TryParse(strEvent, out intEvent);

            if (!AcceptedEvent)
            {
                Alert.Show("Invalid Event", true);
            }

            String EventName = Request.QueryString["Name"];
            lblEventName.InnerText = EventName;

            EventID = intEvent;
            Field[] lf = null;
            try
            {
                RegistrationClient client = new RegistrationClient();
                lf = client.ViewField(intEvent);
                // client.ViewField
                client.Close();
            }
            catch (Exception ex)
            {
                Response.Redirect("~/default.aspx");
            }

            //Since these are dynamic user controls, re-add them every time the page loads.
            for (int i = 0; i < lf.Count(); i++)
            {
                Field f = lf[i];
                RegistrationField regField = (RegistrationField)LoadControl("CustomControls\\RegistrationField.ascx");

                regField.ID = f.FieldID.ToString();

                regField.IsEmailField = false;
                regField.IsRequired = false;
                regField.HelpText = f.Remarks;
                regField.TextBoxValue = "";

                regField.FieldLabelString = f.FieldLabel + ": ";

                int domain;
                bool success = int.TryParse(Session["Domain"].ToString(), out domain);

                if (f.FieldName.ToLower().IndexOf("email") != -1)
                {
                    regField.IsEmailField = true;
                    Session["mailID"] = i;

                    if (success)
                    {
                        if (domain == 0 || domain == 1 )
                            regField.TextBoxValue = Session["partiEmail"].ToString();
                    }
                }

                regField.IsRequired = f.IsRequired;

                //Finally, add the user control to the panel
                this.phRegister.Controls.Add(regField);

                //Increment the control id for the next round through the loop
                ControlID += 1;
            }
        }

        private void sendRequest(List<QuestionIDWithAnswer> fieldAnswers)
        {
            try
            {

                RegistrationClient client = new RegistrationClient();

                int mailID = int.Parse(Session["mailID"].ToString());

                string email = fieldAnswers[mailID].Answer;

                if (client.isEventRegistered(email, EventID))
                {
                    Alert.Show("You are already registered for the event", true);
                    return;
                }

                if (!client.isRegistered(email))
                {
                    MailHandler.sendVerifyMail(KeyGen.GeneratePwd(email), email);
                }

                client.RegisterParticipant(EventID, fieldAnswers.ToArray());
                client.Close();

                Response.Redirect("~/RegistrationSuccessful.aspx");
            }
            catch (Exception ex)
            {
                Alert.Show("Error Sending Registration Request, Please Try Again Later", false, "~/Register.aspx");
            }
        }

        protected void btnSignUp_Click(object sender, EventArgs e)
        {

            ccJoin.ValidateCaptcha(txtCaptcha.Text.Trim());
            if (ccJoin.UserValidated)
            {
                List<QuestionIDWithAnswer> lstFieldAnswer = new List<QuestionIDWithAnswer>();
                foreach (Control c in this.phRegister.Controls)
                {
                    if (c is RegistrationField)
                    {
                        QuestionIDWithAnswer fieldAnswer = new QuestionIDWithAnswer();

                        RegistrationField regField = ((RegistrationField)c);

                        int FieldID = int.Parse(regField.ID);
                        String FieldText = regField.TextBoxValue;

                        fieldAnswer.QuestionID = FieldID;
                        fieldAnswer.Answer = FieldText;

                        lstFieldAnswer.Add(fieldAnswer);
                    }
                }

                if (lstFieldAnswer.Count != 0)
                    sendRequest(lstFieldAnswer);
                else
                    Alert.Show("There is Nothing to Send");
            }
            else
            {
                Alert.Show("Invalid verification code, please try again");
                txtCaptcha.Text = "";
                txtCaptcha.Focus();
                return;
            }

        }

    }
}

