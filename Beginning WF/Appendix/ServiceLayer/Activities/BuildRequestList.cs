using System;
using System.Collections.Generic;
using System.Linq;
using System.Activities;
using UserTasks;
using UserTasks.Extensions;

namespace ServiceLayer.Activities
{

    public sealed class BuildRequestList : CodeActivity
    {
        public InArgument<UserTasks.QueueInstance[]> QueueInstanceList { get; set; }
        public OutArgument<Request[]> RequestList { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // Get the connection string
            DBConnection ext = context.GetExtension<DBConnection>();
            if (ext == null)
                throw new InvalidProgramException("No connection string available");

            RequestDataContext dc = new RequestDataContext(ext.ConnectionString);

            // Get the list of QueueInstances
            UserTasks.QueueInstance[] qiList = QueueInstanceList.Get(context);
            if (qiList != null && qiList.Count() > 0)
            {
                // Build a list of Request objects
                Request[] rList = new Request[qiList.Count()];
                int i = 0;
                foreach (UserTasks.QueueInstance qi in QueueInstanceList.Get(context))
                {
                    Request r = dc.Requests.SingleOrDefault(x => x.QueueInstanceKey == qi.QueueInstanceKey);
                    rList[i++] = r;
                }

                RequestList.Set(context, rList);
            }
        }
    }
}
