using System;
using System.Activities;
using System.Activities.Statements;
using System.IO;

namespace LeadGenerator
{
    /*****************************************************/
    // This file contains the definition of the EnterLead
    // workflow
    /*****************************************************/
    public sealed class EnterLead : Activity
    {
        // Define the input and output arguments
        public InArgument<string> ContactName { get; set; }
        public InArgument<string> ContactPhone { get; set; }
        public InArgument<string> Interests { get; set; }
        public InArgument<string> Notes { get; set; }
        public InArgument<string> ConnectionString { get; set; }
        public InArgument<int> Rating { get; set; }
        public InArgument<TextWriter> Writer { get; set; }

        public EnterLead()
        {
            // Define the variables used by this workflow
            Variable<Lead> lead = new Variable<Lead> { Name = "lead" };

            // Define the SendRequest workflow
            this.Implementation = () => new Sequence
            {
                DisplayName = "EnterLead",
                Variables = { lead },
                Activities = 
                {
                    new CreateLead
                    {
                        ContactName = new InArgument<string>
                            (env => ContactName.Get(env)),
                        ContactPhone = new InArgument<string>
                            (env => ContactPhone.Get(env)),
                        Interests = new InArgument<string>
                            (env => Interests.Get(env)),
                        Notes = new InArgument<string>(env => Notes.Get(env)),
                        ConnectionString = new InArgument<string>
                            (env => ConnectionString.Get(env)),
                        Lead = new OutArgument<Lead>(env => lead.Get(env)),
                    },
                    new WriteLine
                    {
                        Text = new InArgument<string>
                            (env => "Lead received [" + Rating.Get(env).ToString() 
                             + "]; waiting for assignment"),
                        TextWriter = new InArgument<TextWriter> (env => Writer.Get(env))
                    },
                    new InvokeMethod
                    {
                        TargetType = typeof(ApplicationInterface),
                        MethodName = "NewLead",
                        Parameters =
                        {
                            new InArgument<Lead>(env => lead.Get(env))
                        }
                    },
                    new WaitForInput<Lead>
                    {
                        BookmarkName = "GetAssignment",
                        Input = new OutArgument<Lead>(env => lead.Get(env))
                    },
                    new WriteLine
                    {
                        Text = new InArgument<string>
                            (env => "Lead assigned [" + Rating.Get(env).ToString() 
                             + "] to " + lead.Get(env).AssignedTo),
                        TextWriter = new InArgument<TextWriter> (env => Writer.Get(env))
                    }
                }
            };
        }
    }
}
