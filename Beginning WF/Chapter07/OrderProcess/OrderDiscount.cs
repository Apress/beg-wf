using System;
using System.Collections.Generic;

namespace OrderProcess
{
    public static class OrderDiscount
    {
        public static decimal ComputeDiscount(Order o, decimal total)
        {
            // Count the number of items ordered
            int count = 0;
            foreach (OrderItem i in o.Items)
            {
                count += i.Quantity;
            }

            // Determine the discount percentage
            decimal pct = 0;
            if (total > 500)
                pct = (decimal)0.20;
            if (total > 200)
                pct = (decimal)0.15;
            if (total > 100)
                pct = (decimal)0.10;

            // Calculate the discount amount
            decimal discount = total * pct;

            // Subtract a dollar for every item ordered
            discount -= (decimal)count;

            // Make sure it’s not less than zero
            if (discount < 0)
                discount = 0;

            Console.WriteLine("Discount computed: ${0}", discount.ToString());
            return discount;
        }
    }
}
