using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
    public class DocType
    {
        public int ID { get; set; }
        public string TypeName { get; set; }
        public int FatherTypeID { get; set; }
        public int State { get; set; }
    }
}
