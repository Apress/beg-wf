using System;
using System.Linq;
using System.Activities;
using System.Activities.Tracking;
using UserTasks.Extensions;

namespace UserTasks.Activities
{
    public sealed class CreateQueueInstance : CodeActivity
    {
        public InArgument<string> QueueName { get; set; }
        public InArgument<string> SubQueueName { get; set; }
        public OutArgument<Guid> QueueInstanceKey { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // Get the connection string
            DBConnection ext = context.GetExtension<DBConnection>();
            if (ext == null)
                throw new InvalidProgramException("No connection string available");

            // Lookup the Queue
            UserTasksDataContext dc = new UserTasksDataContext(ext.ConnectionString);
            Queue q = dc.Queues.SingleOrDefault(x => x.QueueName == QueueName.Get(context));
            if (q == null)
                throw new InvalidProgramException("The specified queue (" + QueueName.Get(context) + ") was not found");
            
            SubQueue s = dc.SubQueues
                .SingleOrDefault(x => x.QueueID == q.QueueID &&
                                 x.SubQueueName == SubQueueName.Get(context));
            if (s == null)
                throw new InvalidProgramException("The specified sub-queue (" + SubQueueName.Get(context) + ") was not found");

            // Create and initialize a QueueInstance object
            QueueInstance qi = new QueueInstance();
            qi.QueueInstanceKey = Guid.NewGuid();
            qi.CurrentSubQueueID = s.SubQueueID;
            qi.CreateDate = DateTime.UtcNow;
            qi.InstanceID = context.WorkflowInstanceId;

            // Setup the initial values
            qi.AssignedDate = null;
            qi.AssignedOperatorID = null;
            qi.QC = false;
            qi.Priority = null;

            // Insert the Request record
            PersistQueueInstance persist = context.GetExtension<PersistQueueInstance>();
            persist.AddQueueInstance(qi);

            // Return the QueueInstance object
            QueueInstanceKey.Set(context, qi.QueueInstanceKey);

            CustomTrackingRecord userRecord = new CustomTrackingRecord("Start")
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
