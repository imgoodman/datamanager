using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
    public class InventoryType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int FatherID { get; set; }
        public int State { get; set; }
    }
}
