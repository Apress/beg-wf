using System;
using System.Linq;
using System.Activities;
using System.Configuration;
using ServiceLayer.Extensions;
using UserTasks.Extensions;

namespace ServiceLayer.Activities
{

    public sealed class CreateRequest : CodeActivity
    {
        public InArgument<string> RequestType { get; set; }
        public InArgument<string> UserName { get; set; }
        public InArgument<string> UserEmail { get; set; }
        public InArgument<string> Comment { get; set; }
        public InArgument<Guid> QueueInstanceKey { get; set; }
        public InArgument<Guid> RequestKey { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // Get the connection string
            DBConnection ext = context.GetExtension<DBConnection>();
            if (ext == null)
                throw new InvalidProgramException("No connection string available");

            RequestDataContext dc = new RequestDataContext(ext.ConnectionString);

            // Create and initialize a Request object
            Request r = new Request();
            r.UserName = UserName.Get(context);
            r.UserEmail = UserEmail.Get(context);
            r.RequestType = RequestType.Get(context);
            r.Comment = Comment.Get(context);
            r.CreateDate = DateTime.UtcNow;
            r.RequestKey = RequestKey.Get(context);
            r.QueueInstanceKey = QueueInstanceKey.Get(context);

            // Insert the Request record
            PersistRequest persist = context.GetExtension<PersistRequest>();
            persist.AddRequest(r);
        }
    }
}
