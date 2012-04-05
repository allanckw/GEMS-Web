using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GemsWeb.CustomControls;
using GemsWeb.Controllers;
using evmsService.entities;

namespace GemsWeb
{
    //Reference: http://www.codeproject.com/Articles/26589/Dynamically-add-and-remove-user-controls;
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
          
            

            //for (int i = 0; i < lf.Count<Field>(); i++)
            //{
            //    Field f = lf[i];
            //    Label templabel = new Label();
            //    TextBox temptxt = new TextBox();

            //    templabel.Text = f.FieldLabel;

            //    templabel.Width = 100;
            //    temptxt.Width = 100;

            //    if (f.FieldName.ToLower().IndexOf("email") != -1)
            //    {
            //        templabel.Text = templabel.Text + ":::@";
            //    }
            //    if (f.IsRequired)
            //        templabel.Text = templabel.Text + ":::TRUE";
            //    //temptxt.Text = "TRUE";

            //    PanelRegistration.Controls.Add(templabel);
            //    PanelRegistration.Controls.Add(new TextBox());
            //    PanelRegistration.Controls.Add(new LiteralControl("<br />"));
            //}
            ////  }

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

        private int eventID;
        private void AddRegistrationField()
        {
            //Determine which control fired the postback event. 
            Control c = GetPostBackControl(Page);

            //Be sure everything in the placeholder control is cleared out
            this.phRegister.Controls.Clear();

            int ControlID = 0;

            String strEvent = Request.QueryString["EventID"];
            int intEvent;
            String EventName = Request.QueryString["Name"];
            lblEventName.InnerText = EventName;
            bool AcceptedEvent = int.TryParse(strEvent, out intEvent);
           
            if (!AcceptedEvent)
            {
                Response.Redirect("Error.aspx?errormsg=Invalid Event");
            }
            eventID = intEvent;
            Field[] lf = null;
            try
            {
                EvmsServiceClient client = new EvmsServiceClient();
                lf = client.ViewField(intEvent);
                // client.ViewField
                client.Close();
            }
            catch (Exception ex)
            {
                Response.Redirect("~/Default.aspx");
            }
            if (lf.Count() == 0)
                Alert.Show("Error, Organizer did not Set Fields Hence You Can Not Register", true, "Default.aspx");

            //Since these are dynamic user controls, re-add them every time the page loads.
            for (int i = 0; i < lf.Count(); i++)
            {
                Field f = lf[i];
                RegistrationField regField = (RegistrationField)LoadControl("CustomControls\\RegistrationField.ascx");

                //regField.ID = "uc" + ControlID;
                regField.ID = f.FieldID.ToString();

                regField.IsEmailField = false;
                regField.IsRequired = false;
                //if (i == 0)
                    regField.TextBoxValue = "";

               // if (i == 1)
                    regField.FieldLabelString = f.FieldLabel + ": ";
                    

                    if (f.FieldName.ToLower().IndexOf("email") != -1)
                    {
                        regField.IsEmailField = true;
                    }
                    if (f.IsRequired)
                        regField.IsRequired = true;
                    //temptxt.Text = "TRUE";

                //Finally, add the user control to the panel
                this.phRegister.Controls.Add(regField);

                //Increment the control id for the next round through the loop
                ControlID += 1;
            }
        }

        private void sendRequest(List<TupleOfintstring> fieldAnswers)
        {
            try
            {
                
                EvmsServiceClient client = new EvmsServiceClient();
                client.RegisterParticipant(eventID,fieldAnswers.ToArray());
                // client.ViewField
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
                List<TupleOfintstring> lstFieldAnswer = new List<TupleOfintstring>();
                foreach (Control c in this.phRegister.Controls)
                {
                    
                    if (c is RegistrationField)
                    {
                        TupleOfintstring fieldAnswer = new TupleOfintstring();

                        RegistrationField regField = ((RegistrationField)c);

                        int FieldID = int.Parse(regField.ID);
                        String FieldText = regField.TextBoxValue;

                        fieldAnswer.m_Item1 = FieldID;
                        fieldAnswer.m_Item2 = FieldText;

                        lstFieldAnswer.Add(fieldAnswer);
                    }
                }

                if(lstFieldAnswer.Count != 0)
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

