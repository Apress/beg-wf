using System;
using System.Activities;

namespace LeadGenerator
{
    /*****************************************************/
    // This custom activity assigns a Lead to the specified
    // person (AssignedTo parameter). The updated Lead is
    // returned in the output parameter.  
    /*****************************************************/
    public sealed class AssignLead : CodeActivity
    {
        public InArgument<string> AssignedTo { get; set; }
        public InOutArgument<Lead> Lead { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            Lead l = Lead.Get(context);
            l.AssignedTo = AssignedTo.Get(context);
            l.Status = "Assigned";

            PersistLead persist = context.GetExtension<PersistLead>();
            persist.AddLead(l, "Update");

            // Store the request in the OutArgument
            Lead.Set(context, l);
        }
    }
}
