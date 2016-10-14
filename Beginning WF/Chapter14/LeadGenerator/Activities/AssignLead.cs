using System;
using System.Activities;
using System.Configuration;
using System.Activities.Tracking;
using System.Linq;
using System.Data.Linq;
using System.Transactions;

namespace LeadGenerator
{
    /*****************************************************/
    // This custom activity assigns Lead to the specified
    // person (AssignedTo parameter). The updated Lead is
    // returned in the output parameter.
    /*****************************************************/
    public sealed class AssignLead : NativeActivity
    {
        public InArgument<string> AssignedTo { get; set; }
        public OutArgument<Lead> Lead { get; set; }

        protected override void Execute(NativeActivityContext context)
        {

            // Get the connection string
            DBExtension ext = context.GetExtension<DBExtension>();
            if (ext == null)
                throw new InvalidProgramException("No connection string available");

            // Query the Lead table
            LeadDataDataContext dc = new LeadDataDataContext(ext.ConnectionString);
            dc.Refresh(RefreshMode.OverwriteCurrentValues, dc.Leads);
            Lead l = dc.Leads.SingleOrDefault<Lead>
                (x => x.WorkflowID == context.WorkflowInstanceId);

            if (l == null)
                throw new InvalidProgramException
                    ("The Lead was not found in the database");

            l.AssignedTo = AssignedTo.Get(context);
            l.Status = "Assigned";

            // Enlist on the current transaction
            RuntimeTransactionHandle rth = new RuntimeTransactionHandle();
            rth = context.Properties.Find(rth.ExecutionPropertyName)
                as RuntimeTransactionHandle;
            if (rth != null)
            {
                Transaction t = rth.GetCurrentTransaction(context);
                dc.Connection.EnlistTransaction(t);
            }

            dc.SubmitChanges();

            // Store the request in the OutArgument
            Lead.Set(context, l);
        }
    }
}
