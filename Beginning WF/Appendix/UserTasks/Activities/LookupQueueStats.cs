using System;
using System.Linq;
using System.Activities;
using System.Configuration;
using UserTasks.Extensions;

namespace UserTasks.Activities
{

    public sealed class LookupQueueStats : CodeActivity
    {
        public InArgument<string> QueueName { get; set; }
        public OutArgument<QueueDetail[]> QueueStats { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // Get the connection string
            DBConnection ext = context.GetExtension<DBConnection>();
            if (ext == null)
                throw new InvalidProgramException("No connection string available");

            UserTasksDataContext dc = new UserTasksDataContext(ext.ConnectionString);

            var q = dc.QueueInstances
                .Where(x => x.CurrentSubQueueID != null &&
                            (QueueName.Get(context) == "" ||
                             x.SubQueue.Queue.QueueName == QueueName.Get(context)))
                .GroupBy(x => x.SubQueue.Queue.QueueName + "|" + 
                              x.SubQueue.SubQueueName + "|" +
                              x.QC.ToString());

            // Build the result array
            if (q.Count() > 0)
            {
                QueueDetail[] queueStats = new QueueDetail[q.Count()];
                int i = 0;
                foreach (var group in q)
                {

                    // Split the key into the queue name and QC value
                    string s = group.Key;
                    char[] delimiter = { '|' };
                    string[] values = s.Split(delimiter, 3);

                    // Add a new queue to the stats array
                    QueueDetail det = new QueueDetail();
                    det.Key = group.Key;
                    det.QueueName = values[0];
                    det.SubQueueName = values[1];
                    det.QC = bool.Parse(values[2]);
                    det.Count = group.Count();
                    det.Oldest = group.Min(x => x.CreateDate);
                    queueStats[i++] = det;
                }

                // Store the results in the output arguments
                QueueStats.Set(context, queueStats);
            }
        }
    }
}
