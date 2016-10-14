using System;
using System.Activities;
using System.Activities.Statements;
using System.Activities.Expressions;

namespace Chapter02
{
    class Program
    {
        static void Main(string[] args)
        {
            WorkflowInvoker.Invoke(CreateWorkflow());

            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }

        static Activity CreateWorkflow()
        {
            Variable<int> numberBells = new Variable<int>()
            {
                Name = "numberBells",
                Default = DateTime.Now.Hour
            };
            Variable<int> counter = new Variable<int>()
            {
                Name = "counter",
                Default = 1
            };

            return new Sequence()
            {
                DisplayName = "Main Sequence",
                Variables = { numberBells, counter },
                Activities =
                {
                    new WriteLine()
                    {
                        DisplayName = "Hello",
                        Text = "Hello, World!"
                    },
                    new If()
                    {
                        DisplayName = "Adjust for PM",
                        // Code to be added here in Level 2
                        Condition = ExpressionServices.Convert<bool>(env => numberBells.Get(env) > 12),
                        Then = new Assign<int>()
                        {
                            DisplayName = "Adjust Bells",
                            // Code to be added here in Level 3
                            To = new OutArgument<int>(numberBells),
                            Value = new InArgument<int>(env => numberBells.Get(env) - 12)
                        }
                    },
                    new While()
                    {
                        DisplayName = "Sound Bells",
                        // Code to be added here in Level 2
                        Condition = ExpressionServices.Convert<bool>
                            (env => counter.Get(env) <= numberBells.Get(env)),
                        Body = new Sequence()
                        {
                            DisplayName = "Sound Bell",
                            // Code to be added here in Level 3
                            Activities =
                            {
                                new WriteLine()
                                {
                                    Text = new InArgument<string>(env => counter.Get(env).ToString())
                                },
                                new Assign<int>()
                                {
                                    DisplayName = "Increment Counter",
                                    To = new OutArgument<int>(counter),
                                    Value = new InArgument<int>(env => counter.Get(env) + 1)
                                },
                                new Delay()
                                {
                                    Duration = TimeSpan.FromSeconds(1)
                                }
                            }
                        }
                    },
                    new WriteLine()
                    {
                        DisplayName = "Display Time",
                        Text = "The time is: " + DateTime.Now.ToString()
                    },
                    new If()
                    {
                        DisplayName = "Greeting",
                        // Code to be added here in Level 2
                        Condition = ExpressionServices.Convert<bool>(env => DateTime.Now.Hour >= 18),
                        Then = new WriteLine() { Text = "Good Evening" },
                        Else = new WriteLine() { Text = "Good Day" }
                    }
                }
            };
        }
    }
}
