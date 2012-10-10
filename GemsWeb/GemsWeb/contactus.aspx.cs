using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GemsWeb.Controllers;


namespace GemsWeb
{
    public partial class contactus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ccJoin.ValidateCaptcha(txtCaptcha.Text.Trim());
            if (!ccJoin.UserValidated)
            {
                Alert.Show("Invalid verification code, please try again");
                txtCaptcha.Text = "";
                txtCaptcha.Focus();
                return;
            }

            try
            {
                MailHandler.sendContactEmail(txtEmail.Text.Trim(), txtContactName.Text.Trim(), txtMsg.Text.Trim(), RadNature.SelectedValue.ToString());
                Alert.Show("We will reply to your E-mail within the next 1-3 working days, please be patient. Thank you", true);
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message);
                //ErrorLog(ex.Message);
            }
        }
    }
}