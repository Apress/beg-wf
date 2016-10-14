using System;
using System.Linq;
using System.Activities;
using System.Activities.Tracking;
using System.Configuration;
using UserTasks.Extensions;

namespace UserTasks.Activities
{

    public sealed class UnAssignQueueInstance : CodeActivity
    {
        public InArgument<Guid> QueueInstanceKey { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // Get the connection string
            DBConnection ext = context.GetExtension<DBConnection>();
            if (ext == null)
                throw new InvalidProgramException("No connection string available");

            // Lookup the QueueInstance
            UserTasksDataContext dc = new UserTasksDataContext(ext.ConnectionString);
            dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, dc.QueueInstances);
            QueueInstance qi = dc.QueueInstances.SingleOrDefault(x => x.QueueInstanceKey == QueueInstanceKey.Get(context));
            if (qi == null)
                throw new InvalidProgramException("The specified request (" + QueueInstanceKey.Get(context) + ") was not found");

            if (qi.AssignedOperatorID != null)
            {
                qi.AssignedOperatorID = null;
                qi.AssignedDate = null;

                // Update the QueueInstance record
                PersistQueueInstance persist = context.GetExtension<PersistQueueInstance>();
                persist.AddQueueInstance(qi);
            }

            // Add a custom track record
            CustomTrackingRecord userRecord = new CustomTrackingRecord("UnAssign")
            {
                Data = 
                {
                    {"QueueInstanceKey", qi.QueueInstanceKey},
                    {"SubQueueID", qi.CurrentSubQueueID},
                    {"QC", qi.QC}
                }
            };

            // Emit the custom tracking record
            context.Track(userRecord);
        }
    }
}
