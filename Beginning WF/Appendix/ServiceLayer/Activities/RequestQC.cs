using System;
using System.Linq;
using System.Activities;
using System.Configuration;
using System.Web.Configuration;
using ServiceLayer.Extensions;

namespace ServiceLayer
{

    public sealed class RequestQC : CodeActivity
    {
        // Define the input arguments 
        public InArgument<Guid> RequestKey { get; set; }
        public InArgument<string> Priority { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // Open the config file and get the connection string
            Configuration config = WebConfigurationManager.OpenWebConfiguration("/RequestWeb");
            ConnectionStringsSection css =
                (ConnectionStringsSection)config.GetSection("connectionStrings");
            string connectionString = css.ConnectionStrings["Request"].ConnectionString;

            // Lookup the Request
            RequestDataContext dc = new RequestDataContext(connectionString);
            Request r = dc.Requests.SingleOrDefault(x => x.RequestKey == RequestKey.Get(context));
            if (r == null)
                throw new InvalidProgramException("The specified request (" + RequestKey.Get(context) + ") was not found");

            r.QC = true;
            r.AssignedDate = DateTime.UtcNow;
            r.AssignedOperator = null;

            // Update the Request record
            PersistRequest persist = context.GetExtension<PersistRequest>();
            persist.AddRequest(r);
        }
    }
}
