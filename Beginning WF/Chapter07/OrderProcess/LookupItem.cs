using System;
using System.Activities;

namespace OrderProcess
{
    public sealed class LookupItem : CodeActivity
    {
        public InArgument<string> ItemCode { get; set; }
        public OutArgument<ItemInfo> Item { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            ItemInfo i = new ItemInfo();
            i.ItemCode = context.GetValue<string>(ItemCode);

            switch (i.ItemCode)
            {
                case "12345":
                    i.Description = "Widget";
                    i.Price = (decimal)10.0;
                    break;
                case "12346":
                    i.Description = "Gadget";
                    i.Price = (decimal)15.0;
                    break;
                case "12347":
                    i.Description = "Super Gadget";
                    i.Price = (decimal)25.0;
                    break;
            }

            context.SetValue(this.Item, i);
        }
    }
}
