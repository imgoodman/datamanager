using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
    public class InventoryInstanceDocs
    {
        public bool IsExpired { get; set; }
        public int ID { get; set; }
        public int DocID { get; set; }
        public string DocInstanceIDs { get; set; }
    }
}
