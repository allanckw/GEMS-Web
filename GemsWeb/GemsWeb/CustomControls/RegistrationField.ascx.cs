using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GemsWeb.CustomControls
{
    public partial class RegistrationField : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public bool IsEmailField
        {
            get { return this.regEmail.Enabled; }
            set { this.regEmail.Enabled = value; }
        }

        public bool IsRequired
        {
            get { return this.reqValidator.Enabled; }
            set { this.reqValidator.Enabled = value; }
        }

        public string FieldLabelString
        {
            get { return this.lblFieldName.Text.Trim(); }
            set { this.lblFieldName.Text = value; }
        }

        public string TextBoxValue
        {
            get { return this.txtFieldResult.Text.Trim(); }
            set { this.txtFieldResult.Text = value; }

        }

        public string HelpText
        {
            get
            {
                return this.txtFieldResult.ToolTip;
            }
            set
            {
                if (value != null)
                {
                    if (value.Length > 0)
                        this.txtFieldResult.ToolTip = value;
                }

            }
        }
    }
}