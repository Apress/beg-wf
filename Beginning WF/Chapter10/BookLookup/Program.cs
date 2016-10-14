using System;
using System.Linq;
using System.Activities;
using System.Activities.Statements;
using System.Collections.Generic;
using BookLookup.ServiceReference1;

namespace BookLookup
{
    class Program
    {
        static void Main(string[] args)
        {
            // create dictionary with input arguments for the workflow
            IDictionary<string, object> input = new Dictionary<string, object> 
            {
                { "Author" , "Margaret Mitchell" },
                { "Title" , "Gone with the Wind" },
                { "ISBN" , "1234567890123" }
            };

            // execute the workflow
            IDictionary<string, object> output =
                WorkflowInvoker.Invoke(new Workflow1(), input);

            BookInfo[] l = output["BookList"] as BookInfo[];
            if (l != null)
            {
                foreach (BookInfo i in l)
                {
                    Console.WriteLine("{0}: {1}, {2}", i.Title, i.status, i.InventoryID);
                }
            }
            else
                Console.WriteLine("No items were found");

            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }
    }
}
