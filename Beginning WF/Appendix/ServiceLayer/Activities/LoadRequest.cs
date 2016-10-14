using System;
using System.Linq;
using System.Activities;
using UserTasks.Extensions;

namespace ServiceLayer.Activities
{
    public sealed class LoadRequest : CodeActivity
    {
        public InArgument<Guid> RequestKey { get; set; }
        public OutArgument<Request> Request { get; set; }
        public OutArgument<Guid> QueueInstanceKey { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // Get the connection string
            DBConnection ext = context.GetExtension<DBConnection>();
            if (ext == null)
                throw new InvalidProgramException("No connection string available");

            // Lookup the Request
            RequestDataContext dc = new RequestDataContext(ext.ConnectionString);
            Request r = dc.Requests.SingleOrDefault(x => x.RequestKey == RequestKey.Get(context));
            if (r == null)
                throw new InvalidProgramException("The specified request (" + RequestKey.Get(context) + ") was not found");

            context.SetValue(Request, r);
            context.SetValue(QueueInstanceKey, r.QueueInstanceKey);
        }
    }
}
