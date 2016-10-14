using System;
using System.Linq;
using System.Activities;
using System.Configuration;
using System.Web.Configuration;
using System.Collections.Generic;

namespace ServiceLayer
{
    public sealed class LookupQueueStats : CodeActivity
    {
        // Define an activity input argument of type string
        public OutArgument<QueueDetail[]> QueueStats { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            // Open the config file and get the connection string
            Configuration config = WebConfigurationManager.OpenWebConfiguration("/RequestWeb");
            ConnectionStringsSection css =
                (ConnectionStringsSection)config.GetSection("connectionStrings");
            string connectionString = css.ConnectionStrings["Request"].ConnectionString;

            // Lookup the Queue
            RequestDataContext dc = new RequestDataContext(connectionString);

            var q = dc.Requests.Where(x => x.CurrentQueueID != null)
                .GroupBy(x => x.Queue.QueueName + "|" + x.QC.ToString());

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
                    string[] values = s.Split(delimiter, 2);

                    // Add a new queue to the stats array
                    QueueDetail det = new QueueDetail();
                    det.Key = group.Key;
                    det.QueueName = values[0];
                    det.QC = bool.Parse(values[1]);
                    det.Count = group.Count();
                    det.Oldest = group.Min(x => x.AssignedDate.Value);
                    queueStats[i++] = det;
                }

                // Store the results in the output arguments
                QueueStats.Set(context, queueStats);
            }
        }
    }
}
