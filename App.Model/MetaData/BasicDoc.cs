using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
    public class BasicDoc:BaseModel
    {
        public int ID { get; set; }
        public DocType DocType { get; set; }
        public string DocName { get; set; }
    }
}
