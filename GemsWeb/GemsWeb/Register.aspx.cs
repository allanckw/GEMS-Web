using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GemsWeb.CustomControls;
using GemsWeb.Controllers;

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


        private void AddRegistrationField()
        {
            //Determine which control fired the postback event. 
            Control c = GetPostBackControl(Page);

            //Be sure everything in the placeholder control is cleared out
            this.phRegister.Controls.Clear();

            int ControlID = 0;

            //Since these are dynamic user controls, re-add them every time the page loads.
            for (int i = 0; i <= 10; i++)
            {
                RegistrationField regField = (RegistrationField)LoadControl("CustomControls\\RegistrationField.ascx");

                regField.ID = "uc" + ControlID;


                regField.IsEmailField = false;
                regField.IsRequired = false;
                if (i == 0)
                    regField.TextBoxValue = "asd";

                if (i == 1)
                    regField.FieldLabelString = "name" + ": ";

                //Finally, add the user control to the panel
                this.phRegister.Controls.Add(regField);

                //Increment the control id for the next round through the loop
                ControlID += 1;
            }
        }

        protected void btnSignUp_Click(object sender, EventArgs e)
        {

            ccJoin.ValidateCaptcha(txtCaptcha.Text.Trim());
            if (ccJoin.UserValidated)
            {

                foreach (Control c in this.phRegister.Controls)
                {
                    if (c is RegistrationField)
                    {

                    }
                }
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

