using System;
using System.Collections.Generic;
using System.Activities;
using QCPolicy;

namespace SamplePolicy
{

    public class UpdateCounters : CodeActivity
    {
        public InOutArgument<ActivityConfig> ActivityData { get; set; }
        public InOutArgument<OperatorConfig> OperatorData { get; set; }
        public InArgument<bool> Review { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // Get the current data structures
            ActivityConfig a = ActivityData.Get(context);
            OperatorConfig o = OperatorData.Get(context);

            if (Review.Get(context))
            {
                a.ResetEval();
                o.ResetEval();
            }
            else
            {
                a.IncrementEvalCount();
                o.IncrementEvalCount();
            }

            // Return the updated data
            ActivityData.Set(context, a);
            OperatorData.Set(context, o);
        }
    }
}
