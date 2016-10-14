using System;
using System.Linq;
using System.Activities;
using System.Configuration;
using System.Web.Configuration;
using ServiceLayer.Extensions;

namespace ServiceLayer
{

    public sealed class AssignRequest : CodeActivity
    {
        // Define the input arguments 
        public InArgument<Guid> RequestKey { get; set; }
        public InArgument<Guid> OperatorID { get; set; }
        public OutArgument<int> Result { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // Open the config file and get the connection string
            Configuration config = WebConfigurationManager.OpenWebConfiguration("/RequestWeb");
            ConnectionStringsSection css =
                (ConnectionStringsSection)config.GetSection("connectionStrings");
            string connectionString = css.ConnectionStrings["Request"].ConnectionString;

            // Lookup the Queue
            RequestDataContext dc = new RequestDataContext(connectionString);

            // Lookup the Request
            dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, dc.Requests);
            Request r = dc.Requests.SingleOrDefault(x => x.RequestKey == RequestKey.Get(context));
            if (r == null)
                throw new InvalidProgramException("The specified request (" + RequestKey.Get(context) + ") was not found");

            if (r.AssignedOperator != null)
            {
                if (r.AssignedOperator != OperatorID.Get(context))
                {
                    Result.Set(context, -1);
                    return;
                }
            }

            r.AssignedOperator = OperatorID.Get(context);
            r.AssignedDate = DateTime.UtcNow;

            // Update the Request record
            PersistRequest persist = context.GetExtension<PersistRequest>();
            persist.AddRequest(r);

            Result.Set(context, 0);
        }
    }
}
