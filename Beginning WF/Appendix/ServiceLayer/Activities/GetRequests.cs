using System;
using System.Linq;
using System.Activities;
using System.Configuration;
using System.Web.Configuration;
using System.Collections.Generic;

namespace ServiceLayer
{
    public sealed class GetRequests : CodeActivity
    {
        // Define an activity input argument of type string
        public InArgument<string> QueueName { get; set; }
        public InArgument<bool> QC { get; set; }
        public InArgument<Guid> OperatorID { get; set; }
        public OutArgument<Request[]> RequestList { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // Open the config file and get the connection string
            Configuration config = WebConfigurationManager.OpenWebConfiguration("/RequestWeb");
            ConnectionStringsSection css =
                (ConnectionStringsSection)config.GetSection("connectionStrings");
            string connectionString = css.ConnectionStrings["Request"].ConnectionString;

            // Lookup the Queue
            RequestDataContext dc = new RequestDataContext(connectionString);

            Queue queue = dc.Queues.SingleOrDefault(x => x.QueueName == QueueName.Get(context));
            if (queue == null)
                throw new InvalidProgramException("The specified queue (" + QueueName.Get(context) + ") was not found");

            if (queue.AllowSelection)
            {
                IEnumerable<Request> q = dc.Requests
                    .Where(x => x.Queue.QueueName == QueueName.Get(context) &&
                                x.QC == QC.Get(context) &&
                                (x.AssignedOperator == null ||
                                 x.AssignedOperator == OperatorID.Get(context)))
                    .OrderBy(x => x.AssignedDate);

                if (q.Count() > 0)
                {
                    Request[] reqList = new Request[q.Count()];
                    int i = 0;
                    foreach (Request r in q)
                    {
                        reqList[i++] = r;
                    }

                    RequestList.Set(context, reqList);
                }
            }
            else
            {
                IEnumerable<Request> q = dc.Requests
                    .Where(x => x.Queue.QueueName == QueueName.Get(context) &&
                                x.QC == QC.Get(context) &&
                                (x.AssignedOperator == null ||
                                 x.AssignedOperator == OperatorID.Get(context)))
                    .OrderBy(x => x.AssignedDate)
                    .Take(1);

                if (q.Count() > 0)
                {
                    Request r = q.First<Request>();
                    r.AssignedOperator = OperatorID.Get(context);
                    dc.SubmitChanges();

                    Request[] reqList = new Request[1];
                    reqList[0] = r;

                    RequestList.Set(context, reqList);
                }
            }
        }
    }
}
