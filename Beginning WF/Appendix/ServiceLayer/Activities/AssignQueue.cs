using System;
using System.Linq;
using System.Activities;
using System.Configuration;
using System.Web.Configuration;
using ServiceLayer.Extensions;

namespace ServiceLayer
{

    public sealed class AssignQueue : CodeActivity
    {
        // Define the input arguments 
        public InArgument<Guid> RequestKey { get; set; }
        public InArgument<string> QueueName { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // Open the config file and get the connection string
            Configuration config = WebConfigurationManager.OpenWebConfiguration("/RequestWeb");
            ConnectionStringsSection css =
                (ConnectionStringsSection)config.GetSection("connectionStrings");
            string connectionString = css.ConnectionStrings["Request"].ConnectionString;

            // Lookup the Queue
            RequestDataContext dc = new RequestDataContext(connectionString);
            Queue q = null;
            string queue = QueueName.Get(context);
            if (queue != null && queue.Length > 0 && queue != "None")
            {
                q = dc.Queues.SingleOrDefault(x => x.QueueName == QueueName.Get(context));
                if (q == null)
                    throw new InvalidProgramException("The specified queue (" + QueueName.Get(context) + ") was not found");
            }

            // Lookup the Request
            Request r = dc.Requests.SingleOrDefault(x => x.RequestKey == RequestKey.Get(context));
            if (r == null)
                throw new InvalidProgramException("The specified request (" + RequestKey.Get(context) + ") was not found");

            if (q != null)
            {
                r.CurrentQueueID = q.QueueID;
                r.AssignedDate = DateTime.UtcNow;
                r.AssignedOperator = null;
            }
            else
            {
                r.CurrentQueueID = null;
                r.AssignedDate = null;
                r.AssignedOperator = null;
            }

            // Update the Request record
            PersistRequest persist = context.GetExtension<PersistRequest>();
            persist.AddRequest(r);
        }
    }
}
