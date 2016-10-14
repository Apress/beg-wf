using System;
using System.Collections.Generic;
using System.Activities;

namespace BookInventory
{
    /*****************************************************/
    // This custom activity creates a BookInfo array and 
    // uses the input parameters to "lookup" the matching 
    // items. The BookInfo array is returned in the output 
    // parameter.  
    /*****************************************************/
    public sealed class PerformLookup2 : CodeActivity
    {
        public InArgument<String> Title { get; set; }
        public InArgument<String> Author { get; set; }
        public InArgument<String> ISBN { get; set; }
        public OutArgument<BookInfo[]> BookList { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            string author = Author.Get(context);
            string title = Title.Get(context);
            string isbn = ISBN.Get(context);

            BookInfo[] l = new BookInfo[4];

            l[0] = new BookInfo(title, author, isbn, "Available");
            l[1] = new BookInfo(title, author, isbn, "CheckedOut");
            l[2] = new BookInfo(title, author, isbn, "Missing");
            l[3] = new BookInfo(title, author, isbn, "Available");
            BookList.Set(context, l);
        }
    }
}