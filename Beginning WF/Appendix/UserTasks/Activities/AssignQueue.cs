using System;
using System.Linq;
using System.Activities;
using System.Activities.Tracking;
using System.Configuration;
using UserTasks.Extensions;

namespace UserTasks.Activities
{

    public sealed class AssignQueue : CodeActivity
    {
        public InArgument<Guid> QueueInstanceKey { get; set; }
        public InArgument<string> QueueName { get; set; }
        public InArgument<string> SubQueueName { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // Get the connection string
            DBConnection ext = context.GetExtension<DBConnection>();
            if (ext == null)
                throw new InvalidProgramException("No connection string available");

            UserTasksDataContext dc = new UserTasksDataContext(ext.ConnectionString);

            Queue q = null;
            SubQueue sq = null;
            int queueID = 0;
            int subQueueID = 0;

            if (QueueName.Get(context) != "" && SubQueueName.Get(context) != "None")
            {
                // Lookup the queue and subqueue
                q = dc.Queues.SingleOrDefault(x => x.QueueName == QueueName.Get(context));
                if (q == null)
                    throw new InvalidProgramException("The specified queue (" + QueueName.Get(context) + ") was not found");

                sq = dc.SubQueues.SingleOrDefault(x => x.QueueID == q.QueueID &&
                                                       x.SubQueueName == SubQueueName.Get(context));
                if (sq == null)
                    throw new InvalidProgramException("The specified subqueue (" +
                        QueueName.Get(context) + " - " +
                        SubQueueName.Get(context) + ") was not found");

                queueID = q.QueueID;
                subQueueID = sq.SubQueueID;
            }

            // Lookup the QueueInstance
            dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, dc.QueueInstances);
            QueueInstance qi = dc.QueueInstances.SingleOrDefault(x => x.QueueInstanceKey == QueueInstanceKey.Get(context));
            if (qi == null)
                throw new InvalidProgramException("The specified request (" + QueueInstanceKey.Get(context) + ") was not found");

            // Assign the QueueInstance to this subqueue
            if (sq != null)
                qi.CurrentSubQueueID = sq.SubQueueID;
            else
                qi.CurrentSubQueueID = null;

            qi.AssignedDate = null;
            qi.AssignedOperatorID = null;
            qi.QC = false;
            qi.Priority = null;

            // Update the QueueInstance record
            PersistQueueInstance persist = context.GetExtension<PersistQueueInstance>();
            persist.AddQueueInstance(qi);

            // Add a custom track record
            CustomTrackingRecord userRecord = new CustomTrackingRecord("Route")
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
