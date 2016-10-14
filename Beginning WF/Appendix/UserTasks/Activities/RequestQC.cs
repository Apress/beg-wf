using System;
using System.Linq;
using System.Activities;
using System.Activities.Tracking;
using System.Configuration;
using UserTasks.Extensions;

namespace UserTasks.Activities
{

    public sealed class RequestQC : CodeActivity
    {
        public InArgument<Guid> QueueInstanceKey { get; set; }
        public InArgument<int> Priority { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // Get the connection string
            DBConnection ext = context.GetExtension<DBConnection>();
            if (ext == null)
                throw new InvalidProgramException("No connection string available");

            UserTasksDataContext dc = new UserTasksDataContext(ext.ConnectionString);
            
            // Lookup the QueueInstance
            dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, dc.QueueInstances);
            QueueInstance qi = dc.QueueInstances.SingleOrDefault(x => x.QueueInstanceKey == QueueInstanceKey.Get(context));
            if (qi == null)
                throw new InvalidProgramException("The specified request (" + QueueInstanceKey.Get(context) + ") was not found");

            qi.QC = true;
            qi.Priority = Priority.Get(context);
            qi.AssignedOperatorID = null;
            qi.AssignedDate = null;

            // Update the QueueInstance record
            PersistQueueInstance persist = context.GetExtension<PersistQueueInstance>();
            persist.AddQueueInstance(qi);

            // Add a custom track record
            CustomTrackingRecord userRecord = new CustomTrackingRecord("QC")
            {
                Data = 
                {
                    {"QueueInstanceKey", qi.QueueInstanceKey},
                    {"SubQueueID", qi.CurrentSubQueueID}
                }
            };

            // Emit the custom tracking record
            context.Track(userRecord);
        }
    }
}
