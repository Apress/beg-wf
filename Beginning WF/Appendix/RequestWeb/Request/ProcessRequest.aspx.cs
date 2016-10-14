using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class ProcessRequest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            MembershipUser user = Membership.GetUser();
            if (user != null)
            {
                Session["OperatorKey"] = (Guid)user.ProviderUserKey;
            }

            ReloadQueues();
            lblRequests.Text = "Select which Queue to work";
            lblQC.Text = "QC Review";
            lblQC.Visible = false;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        QueueGrid.DataSource = Session["Queues"];
        QueueGrid.DataBind();
            
        lblRequests.Visible = true;

        if (Session["Requests"] != null)
        {
            RequestGrid.Visible = true;
            pnlSelection.Visible = true;
            RequestGrid.DataSource = Session["Requests"];
            RequestGrid.DataBind();
        }
        else
        {
            RequestGrid.Visible = false;
            pnlSelection.Visible = false;
        }
    }

    protected void QueueSelected(object sender, EventArgs e)
    {
        if (pnlRequests.Visible.Equals(false))
        {
            pnlRequests.Visible = true;
        }

        string key = (string)QueueGrid.SelectedValue;
        Session["QueueKey"] = key;
        lblQC.Visible = false;

        AssignNextRequest();
    }

    protected void RequestSelected(object sender, EventArgs e)
    {
        if (pnlRequests.Visible.Equals(false))
        {
            pnlRequests.Visible = true;
        }

        Guid requestKey = (Guid)RequestGrid.SelectedValue;
        Session["RequestKey"] = requestKey;

        ServiceReference1.ProcessRequestClient serv = new ServiceReference1.ProcessRequestClient();
        int rc = serv.AssignOperator(requestKey, (Guid)Session["OperatorKey"]);
        if (rc == 0)
        {
            ServiceReference1.Request r = serv.LoadRequest(requestKey);
            Session["Request"] = r;

            // Display the current values
            lblName.Text = r.UserName;
            lblEmail.Text = r.UserEmail;
            lblType.Text = r.RequestType;
            lblRequestInfo.Text = "Requests currently in the selected Queue";
            txtComment.Text = r.Comment;
            txtActionTaken.Text = r.ActionTaken;
            cbNextQueue.Text = r.RouteNext;

            if (r.QueueInstance.QC)
                lblQC.Visible = true;
            else
                lblQC.Visible = false;
        }
    }

    protected void CompleteRequest(object sender, EventArgs e)
    {
        ServiceReference1.Request r = (ServiceReference1.Request)Session["Request"];
        if (r != null)
        {
            ServiceReference1.ProcessRequestClient serv = new ServiceReference1.ProcessRequestClient();
            serv.CompleteRequest(r.RequestKey, txtActionTaken.Text, cbNextQueue.Text);

            // Clear the current values
            lblName.Text = "";
            lblEmail.Text = "";
            lblType.Text = "";
            lblRequestInfo.Text = "";
            txtComment.Text = "";
            txtActionTaken.Text = "";
            cbNextQueue.Text = "None";

            lblQC.Visible = false;

            // Reload the queue stats
            ReloadQueues();

            // Assign the next item in the queue
            AssignNextRequest();
        }
    }

    protected void UnassignRequest(object sender, EventArgs e)
    {
        ServiceReference1.Request r = (ServiceReference1.Request)Session["Request"];
        if (r != null)
        {
            ServiceReference1.ProcessRequestClient serv = new ServiceReference1.ProcessRequestClient();
            serv.UnassignRequest(r.RequestKey);

            // Clear the current values
            lblName.Text = "";
            lblEmail.Text = "";
            lblType.Text = "";
            lblRequestInfo.Text = "";
            txtComment.Text = "";
            txtActionTaken.Text = "";
            cbNextQueue.Text = "None";

            lblQC.Visible = false;

            // Reload the queue stats
            ReloadQueues();

            // Assign the next item in the queue
            //AssignNextRequest();
        }
    }

    private void ReloadQueues()
    {
        ServiceReference1.ProcessRequestClient serv = new ServiceReference1.ProcessRequestClient();

        ServiceReference1.QueueDetail[] q = serv.GetQueueStats();
        Session["Queues"] = q;
    }

    private void AssignNextRequest()
    {
        string queueKey = (string)Session["QueueKey"];
        char[] delimiter = { '|' };
        string[] values = queueKey.Split(delimiter, 3);

        Guid operatorKey = (Guid)Session["OperatorKey"];

        ServiceReference1.ProcessRequestClient serv = new ServiceReference1.ProcessRequestClient();

        ServiceReference1.Request[] reqList = serv.GetRequest(values[1], operatorKey, bool.Parse(values[2]));
        if (reqList != null && reqList.Count() == 1)
        {
            Session["Requests"] = null;

            ServiceReference1.Request r = reqList[0];
            Session["Request"] = r;

            int rc = serv.AssignOperator(r.RequestKey, operatorKey);
            if (rc == 0)
            {
                // Display the current values
                lblName.Text = r.UserName;
                lblEmail.Text = r.UserEmail;
                lblType.Text = r.RequestType;
                lblRequestInfo.Text = "Requests currently in the selected Queue";
                txtComment.Text = r.Comment;
                txtActionTaken.Text = r.ActionTaken;
                cbNextQueue.Text = r.RouteNext;

                if (r.QueueInstance.QC)
                    lblQC.Visible = true;
                else
                    lblQC.Visible = false;
            }
        }

        if (reqList != null && reqList.Count() > 1)
        {
            Session["Requests"] = reqList;
        }
    }

    protected void RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }
}