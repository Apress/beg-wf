using System;
using System.Activities;
using System.Collections.Generic;

namespace ShoppingList
{
    public sealed class PrintList : CodeActivity
    {
        public InArgument<ICollection<ListItem>> Collection { get; set; }
        public InArgument<decimal> Budget { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            ICollection<ListItem> list =
                this.Collection.Get<ICollection<ListItem>>(context);

            if (list.Count == 0)
            {
                Console.WriteLine("The list is empty");
            }
            else
            {
                decimal total = 0m;
                decimal budget = Budget.Get(context);

                foreach (ListItem l in list)
                {
                    // See if this item will put us over budget
                    if (budget > 0 && total + (l.Quantity * l.UnitPrice) > budget)
                        break;

                    total += l.Quantity * l.UnitPrice;

                    Console.WriteLine("{0}: {1}, {2} @ ${3} [{4}]",
                        l.Priority.ToString(), l.Description,
                        l.Quantity.ToString(), l.UnitPrice, l.Comments);
                }

                Console.WriteLine("Total cost: ${0}", total.ToString());
                Console.WriteLine();
            }
        }
    }
}
