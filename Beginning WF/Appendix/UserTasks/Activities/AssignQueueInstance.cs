using System;
using System.Linq;
using System.Activities;
using System.Activities.Tracking;
using System.Configuration;
using UserTasks.Extensions;

namespace UserTasks.Activities
{

    public sealed class AssignQueueInstance : CodeActivity
    {
        public InArgument<Guid> QueueInstanceKey { get; set; }
        public InArgument<Guid> OperatorKey { get; set; }
        public OutArgument<int> Result { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // Get the connection string
            DBConnection ext = context.GetExtension<DBConnection>();
            if (ext == null)
                throw new InvalidProgramException("No connection string available");

            // Lookup the operator
            UserTasksDataContext dc = new UserTasksDataContext(ext.ConnectionString);
            OperatorConfig oc = dc.OperatorConfigs.SingleOrDefault(x => x.OperatorKey == OperatorKey.Get(context));
            if (oc == null)
                throw new InvalidProgramException("The specified operator (" + OperatorKey.Get(context).ToString() + ") was not found");

            // Lookup the QueueInstance
            dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, dc.QueueInstances);
            QueueInstance qi = dc.QueueInstances.SingleOrDefault(x => x.QueueInstanceKey == QueueInstanceKey.Get(context));
            if (qi == null)
                throw new InvalidProgramException("The specified request (" + QueueInstanceKey.Get(context) + ") was not found");

            if (qi.AssignedOperatorID != null)
            {
                if (qi.AssignedOperatorID != oc.OperatorConfigID)
                {
                    Result.Set(context, -1);
                    return;
                }
            }

            qi.AssignedOperatorID = oc.OperatorConfigID;
            qi.AssignedDate = DateTime.UtcNow;

            // Update the QueueInstance record
            PersistQueueInstance persist = context.GetExtension<PersistQueueInstance>();
            persist.AddQueueInstance(qi);

            Result.Set(context, 0);

            // Add a custom track record
            CustomTrackingRecord userRecord = new CustomTrackingRecord("Assign")
            {
                Data = 
                {
                    {"QueueInstanceKey", qi.QueueInstanceKey},
                    {"OperatorKey", OperatorKey.Get(context)},
                    {"SubQueueID", qi.CurrentSubQueueID},
                    {"QC", qi.QC}
                }
            };

            // Emit the custom tracking record
            context.Track(userRecord);
        }
    }
}
