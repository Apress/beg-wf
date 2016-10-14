using System;
using System.Linq;
using System.Activities;
using System.Configuration;
using System.Collections.Generic;
using UserTasks.Extensions;

namespace UserTasks.Activities
{

    public sealed class GetQueueInstances : CodeActivity
    {
        public InArgument<string> QueueKey { get; set; }
        public InArgument<Guid> OperatorKey { get; set; }
        public OutArgument<QueueInstance[]> QueueInstanceList { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // Get the connection string
            DBConnection ext = context.GetExtension<DBConnection>();
            if (ext == null)
                throw new InvalidProgramException("No connection string available");

            // Lookup the operator
            UserTasksDataContext dc = new UserTasksDataContext(ext.ConnectionString);
            OperatorConfig oc = dc.OperatorConfigs.SingleOrDefault(x => x.OperatorKey == OperatorKey.Get(context));
            if (oc == null)
            {
                oc = new OperatorConfig();
                oc.OperatorKey = OperatorKey.Get(context);
                oc.UnderEvaluation = false;
                oc.Frequency = 5;
                oc.NumberSinceLastEval = 0;

                dc.OperatorConfigs.InsertOnSubmit(oc);
                dc.SubmitChanges();
            }
        
            // Determine the Queue info
            char[] delimiter = { '|' };
            string[] values = QueueKey.Get(context).Split(delimiter, 3);

            // Lookup the queue and subqueue
            Queue q = dc.Queues.SingleOrDefault(x => x.QueueName == values[0]);
            if (q == null)
                throw new InvalidProgramException("The specified queue (" + values[0] + ") was not found");

            SubQueue sq = dc.SubQueues.SingleOrDefault(x => x.QueueID == q.QueueID &&
                                                   x.SubQueueName == values[1]);
            if (sq == null)
                throw new InvalidProgramException("The specified subqueue (" +
                    values[0] + " - " +
                    values[1] + ") was not found");

            bool bQC = bool.Parse(values[2]);

            if (sq.AllowSelection)
            {
                // Return all the available instances
                IEnumerable<QueueInstance> instances;
                
                if (bQC)
                {
                    instances = dc.QueueInstances
                        .Where(x => x.CurrentSubQueueID == sq.SubQueueID &&
                                x.QC == bQC &&
                                (x.AssignedOperatorID == null ||
                                 x.AssignedOperatorID == oc.OperatorConfigID))
                        .OrderBy(x => x.Priority.Value)
                        .OrderBy(x => x.CreateDate);
                }
                else
                {
                    instances = dc.QueueInstances
                        .Where(x => x.CurrentSubQueueID == sq.SubQueueID &&
                                    x.QC == bQC &&
                                   (x.AssignedOperatorID == null ||
                                    x.AssignedOperatorID == oc.OperatorConfigID))
                        .OrderBy(x => x.CreateDate);
                }

                if (instances.Count() > 0)
                {
                    QueueInstance[] qiList = new QueueInstance[instances.Count()];
                    int i = 0;
                    foreach (QueueInstance r in instances)
                    {
                        qiList[i++] = r;
                    }

                    QueueInstanceList.Set(context, qiList);
                }
            }
            else
            {
                // Return the oldest instance
                IEnumerable<QueueInstance> instances;
                if (bQC)
                {
                    instances = dc.QueueInstances
                        .Where(x => x.CurrentSubQueueID == sq.SubQueueID &&
                                    x.QC == bQC &&
                                   (x.AssignedOperatorID == null ||
                                    x.AssignedOperatorID == oc.OperatorConfigID))
                        .OrderBy(x => x.Priority.Value)
                        .OrderBy(x => x.CreateDate)
                        .Take(1);
                }
                else
                {
                    instances = dc.QueueInstances
                        .Where(x => x.CurrentSubQueueID == sq.SubQueueID &&
                                    x.QC == bQC &&
                                    (x.AssignedOperatorID == null ||
                                     x.AssignedOperatorID == oc.OperatorConfigID))
                        .OrderBy(x => (x.Priority.HasValue ? 1 : 0) * x.Priority.Value * (bQC ? 1 : 0))
                        .OrderBy(x => x.CreateDate)
                        .Take(1);
                }

                if (instances.Count() > 0)
                {
                    QueueInstance qi = instances.First<QueueInstance>();
                    qi.AssignedOperatorID = oc.OperatorConfigID;
                    dc.SubmitChanges();

                    QueueInstance[] qiList = new QueueInstance[1];
                    qiList[0] = qi;

                    QueueInstanceList.Set(context, qiList);
                }
            }
        }
    }
}
