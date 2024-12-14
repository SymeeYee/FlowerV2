using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AsgV3
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string selectedDestination = ddlDestination.SelectedValue;
            if (!string.IsNullOrEmpty(selectedDestination))
            {
                lblMessage.Text = "You have selected: " + selectedDestination;
            }
            else
            {
                lblMessage.Text = "Please select a destination.";
            }

        }
    }
}


       
