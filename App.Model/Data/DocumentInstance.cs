using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
    public class DocumentInstance:BaseModel
    {
        public int ID { get; set; }
        public Document Document { get; set; }
    }
}
