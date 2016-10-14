using System;
using System.Activities;
using System.Collections.Generic;

namespace ShoppingList
{
    public sealed class SortCollection : CodeActivity
    {
        public InOutArgument<ICollection<ListItem>> Collection { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            ICollection<ListItem> tempList =
                this.Collection.Get<ICollection<ListItem>>(context);

            if (tempList.Count > 0)
            {

                List<ListItem> sortedList = new List<ListItem>(tempList);
                ItemComparer c = new ItemComparer();
                sortedList.Sort(c as IComparer<ListItem>);

                Collection.Set(context, sortedList as ICollection<ListItem>);
            }
        }
    }

    public class ItemComparer : IComparer<ListItem>
    {
        public int Compare(ListItem x, ListItem y)
        {
            // Handle null arguments
            if (x == null && y == null)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;

            // Perform comparison based on the priority
            if (x.Priority == y.Priority)
                return 0;
            if (x.Priority > y.Priority)
                return 1;
            else
                return -1;
        }
    }
}
