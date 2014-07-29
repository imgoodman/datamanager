using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
    public class BasicBOM:BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string MainDocId { get; set; }
        public string FilePath { get; set; }
    }
}
