using System;
using System.Linq;
using System.Activities;
using System.Configuration;
using UserTasks.Extensions;


namespace UserTasks.Activities
{

    public sealed class LoadQueueInstance : CodeActivity
    {
        public InArgument<Guid> QueueInstanceKey { get; set; }
        public OutArgument<QueueInstance> QueueInstance { get; set; }
        public OutArgument<string> ConnectionString { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // Get the connection string
            DBConnection ext = context.GetExtension<DBConnection>();
            if (ext == null)
                throw new InvalidProgramException("No connection string available");

            UserTasksDataContext dc = new UserTasksDataContext(ext.ConnectionString);

            // Lookup the QueueInstance
            dc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, dc.QueueInstances);
            QueueInstance qi = dc.QueueInstances.SingleOrDefault(x => x.QueueInstanceKey == QueueInstanceKey.Get(context));
            if (qi == null)
                throw new InvalidProgramException("The specified request (" + QueueInstanceKey.Get(context) + ") was not found");

            QueueInstance.Set(context, qi);
            ConnectionString.Set(context, ext.ConnectionString);
        }
    }
}
