using System;
using System.Activities;
using System.Activities.Statements;
using System.Collections.Generic;

namespace OrderProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            Order myOrder = new Order
            {
                OrderID = 1,
                Description = "Need some stuff",
                ShippingMethod = "2ndDay",
                TotalWeight = 100
            };

            // Add some OrderItem objects
            myOrder.Items.Add(new OrderItem
            {
                OrderItemID = 1,
                Quantity = 1,
                ItemCode = "12345",
                Description = "Widget"
            });

            myOrder.Items.Add(new OrderItem
            {
                OrderItemID = 2,
                Quantity = 3,
                ItemCode = "12346",
                Description = "Gadget"
            });

            myOrder.Items.Add(new OrderItem
            {
                OrderItemID = 3,
                Quantity = 2,
                ItemCode = "12347",
                Description = "Super Widget"
            });

            // create dictionary with input arguments for the workflow
            IDictionary<string, object> input = new Dictionary<string, object> 
            {
                { "OrderInfo" , myOrder }
            };

            // execute the workflow
            IDictionary<string, object> output
                = WorkflowInvoker.Invoke(new OrderWF(), input);

            // Get the TotalAmount returned by the workflow
            decimal total = (decimal)output["TotalAmount"];
            Console.WriteLine("Workflow returned ${0} for my order total", total);
            
            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }
    }
}
