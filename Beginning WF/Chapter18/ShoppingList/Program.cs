using System;
using System.Collections.Generic;
using System.Activities;
using System.Activities.Statements;
using System.Activities.Expressions;

namespace ShoppingList
{
    class Program
    {
        static void Main(string[] args)
        {
            WorkflowInvoker.Invoke(CollectionWF());

            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }

        private static Activity CollectionWF()
        {
            // myList is a collection of ListItem objects
            Variable<ICollection<ListItem>> myList = 
                new Variable<ICollection<ListItem>>()
            {
                Name = "MyList",
                Default = new LambdaValue<ICollection<ListItem>>(env => new List<ListItem>())
            };

            return new Sequence
            {
                Variables = { myList },
                Activities = 
                {
                    new WriteLine 
                    {
                        Text = "Workflow starting..." 
                    },
                    new AddToCollection<ListItem> 
                    {
                        Collection = myList,
                        Item = new LambdaValue<ListItem>(env => new ListItem("Milk", 1, 3.99m, 2, ""))
                    },
                    new AddToCollection<ListItem> 
                    {
                        Collection = myList,
                        Item = new LambdaValue<ListItem>(env => new ListItem("Bread", 2, 2.95m, 1, 
                            "Get 100% Whole Wheat, if possible"))
                    },
                    new AddToCollection<ListItem> 
                    {
                        Collection = myList,
                        Item = new LambdaValue<ListItem>(env => new ListItem("Cheese", 1, 1.75m, 4, ""))
                    },
                    new AddToCollection<ListItem> 
                    {
                        Collection = myList,
                        Item = new LambdaValue<ListItem>(env => new ListItem("Ice Cream", 4, 5.75m, 5, ""))
                    },
                    new PrintList()
                    {
                        Budget = 0m,
                        Collection = myList
                    },
                    new SortCollection 
                    {
                        Collection = myList
                    },
                    new PrintList()
                    {
                        Budget = 0m,
                        Collection = myList
                    },
                    new If 
                    {
                        Condition = new ExistsInCollection<ListItem>()
                        {
                            Collection = myList,
                            Item = new LambdaValue<ListItem>(env => new ListItem("Ice Cream"))
                        },
                        Then = new WriteLine 
                        {
                            Text = "You don't really need Ice Cream?"
                        }
                    },
                    new WriteLine 
                    {
                        Text = "Removing Ice Cream..." 
                    },
                    new RemoveFromCollection<ListItem>() 
                    {
                        Collection = myList,
                        Item = new LambdaValue<ListItem>(env => new ListItem("Ice Cream"))
                    },
                    new PrintList()
                    {
                        Budget = 0m,
                        Collection = myList
                    },
                    new ClearCollection<ListItem>() 
                    {
                        Collection = myList
                    },			            
                    new PrintList()
                    {
                        Budget = 0m,
                        Collection = myList
                    },
                    new WriteLine 
                    {
                        Text = "Workflow ended" 
                    }
                }
            };
        }
    }

    //-------------------------------------------
    // The ListItem class defines the items that
    // are stored in the collection
    //-------------------------------------------
    public class ListItem
    {
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int Priority { get; set; }
        public string Comments { get; set; }

        public ListItem(string description, int quantity, decimal unitPrice,
            int priority, string comments)
        {
            Description = description;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Priority = priority;
            Comments = comments;
        }

        public ListItem(string description)
        {
            Description = description;
        }

        // The Equals() method must be overridden 
        // to enable a search using the description
        public override bool Equals(object obj)
        {
            ListItem i = obj as ListItem;
            if (i == null)
                return false;
            else
            {
                if (i.Description == this.Description)
                    return true;
                else
                    return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

}
