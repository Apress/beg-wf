using System;
using System.Activities;
using System.Activities.Statements;
using System.Collections.Generic;
using QCPolicy;

namespace PolicySample
{
    class Program
    {
        static void Main(string[] args)
        {
            TransactionList l = new TransactionList();
            l.List.Add(new TransactionConfig(500.00m));
            l.List.Add(new TransactionConfig(300.00m));
            l.List.Add(new TransactionConfig(250.00m));
            l.List.Add(new TransactionConfig(1200.00m));
            l.List.Add(new TransactionConfig(2100.00m));
            l.List.Add(new TransactionConfig(1100.00m));

            IDictionary<string, object> input = new Dictionary<string, object> 
            {
                { "operatorData", new OperatorConfig(false, 2) },
                { "customerData", new CustomerConfig("Prospect") },
                { "activityData", new ActivityConfig(800, 2000, 3) },
                { "transactionList", l }
            };

            IDictionary<string, object> output
                = WorkflowInvoker.Invoke(new Workflow1(), input);

            Console.WriteLine();
            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }
    }
}
