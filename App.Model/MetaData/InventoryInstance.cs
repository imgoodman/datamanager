using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
    public class InventoryInstance : BaseModel
    {
        public bool IsExpired { get; set; }
        public int ID { get; set; }
        public int InventoryID { get; set; }
        public List<InventoryInstanceDocs> InstanceDocs { get; set; }
    }
}
