using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class SubmitRequest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
        }
        else
        {
            if (Session["LastRequest"] != null)
            {
                string s = ((Guid)Session["LastRequest"]).ToString();
                if (s != null && s.Length > 1)
                    lblInstruction.Text = "Request " + s + " was submitted";
            }
        }

        MembershipUser user = Membership.GetUser();
        if (user != null)
        {
            txtName.Text = user.UserName;
            txtEmail.Text = user.Email;
        }
    }

    protected void btnAddItem_Click(object sender, ImageClickEventArgs e)
    {
        ServiceReference1.ProcessRequestClient serv = new ServiceReference1.ProcessRequestClient();
        Guid g = Guid.NewGuid();
        serv.SubmitRequest(g, txtName.Text, cbCategory.Text, txtComment.Text, txtEmail.Text);

        Session["LastRequest"] = g;
        lblInstruction.Text = "Request " + g.ToString() + " was submitted";

        // Clear the form
        txtName.Text = "";
        txtEmail.Text = "";
        txtComment.Text = "";

        MembershipUser user = Membership.GetUser();
        if (user != null)
        {
            txtName.Text = user.UserName;
            txtEmail.Text = user.Email;
        }
    }
}