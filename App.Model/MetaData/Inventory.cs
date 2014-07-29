using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
    public class Inventory : BaseModel
    {
        public bool IsExpired { get; set; }
        public int ID { get; set; }
        public InventoryType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Document> Docs { get; set; }
    }
}
