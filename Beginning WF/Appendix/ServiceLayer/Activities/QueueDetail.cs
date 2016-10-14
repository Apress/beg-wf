using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceLayer
{
    public class QueueDetail
    {
        public string Key { get; set; }
        public string QueueName { get; set;  }
        public bool QC { get; set; }
        public int Count { get; set; }
        public DateTime Oldest { get; set; }
    }
}