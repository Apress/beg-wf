using System;
using System.Linq;
using System.Activities;
using ServiceLayer.Extensions;
using UserTasks.Extensions;

namespace ServiceLayer.Activities
{

    public sealed class UpdateRequest : CodeActivity
    {
        // Define the input arguments 
        public InArgument<Guid> RequestKey { get; set; }
        public InArgument<string> ActionTaken { get; set; }
        public InArgument<string> RouteNext { get; set; }
        public OutArgument<Request> Request { get; set; }

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

            // Update the Request record
            r.ActionTaken = ActionTaken.Get(context);
            r.RouteNext = RouteNext.Get(context);

            PersistRequest persist = context.GetExtension<PersistRequest>();
            persist.AddRequest(r);

            context.SetValue(Request, r);
        }
    }
}
