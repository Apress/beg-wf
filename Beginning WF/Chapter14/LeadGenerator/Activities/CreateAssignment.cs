using System;
using System.Activities;
using System.Activities.Tracking;
using System.Linq;
using System.Data.Linq;
using System.Transactions;

namespace LeadGenerator
{
    /*****************************************************/
    // This custom activity creates an Assignment class
    // using the input parameters (LeadID and AsignedTo).
    /*****************************************************/
    public sealed class CreateAssignment : NativeActivity
    {
        public InArgument<int> LeadID { get; set; }
        public InArgument<string> AssignedTo { get; set; }

        protected override void Execute(NativeActivityContext context)
        {

            // Get the connection string
            DBExtension ext = context.GetExtension<DBExtension>();
            if (ext == null)
                throw new InvalidProgramException("No connection string available");

            // Create a data context
            LeadDataDataContext dc = new LeadDataDataContext(ext.ConnectionString);

            // Enlist on the current transaction
            RuntimeTransactionHandle rth = new RuntimeTransactionHandle();
            rth = context.Properties.Find(rth.ExecutionPropertyName)
                as RuntimeTransactionHandle;
            if (rth != null)
            {
                Transaction t = rth.GetCurrentTransaction(context);

                // Open the connection, if necessary
                if (dc.Connection.State == System.Data.ConnectionState.Closed)
                    dc.Connection.Open();

                dc.Connection.EnlistTransaction(t);
            }

            // Create an Assignment class and populate its properties
            Assignment a = new Assignment();
            dc.Assignments.InsertOnSubmit(a);

            a.WorkflowID = context.WorkflowInstanceId;
            a.LeadID = LeadID.Get(context);
            a.DateAssigned = DateTime.Now;
            a.AssignedTo = AssignedTo.Get(context);
            a.Status = "Assigned";
            a.DateDue = DateTime.Now + TimeSpan.FromDays(5);

            dc.SubmitChanges();
        }
    }
}
