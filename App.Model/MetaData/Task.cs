using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
    public class Task
    {
        public int ID { get; set; }
        public string TaskName { get; set; }
        public int ParentID { get; set; }
        public int OrderID { get; set; }
        public string Url { get; set; }
        public int TaskLevel { get; set; }
    }
}
