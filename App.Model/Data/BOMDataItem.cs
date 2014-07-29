using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model.Data
{
    public class BOMDataItem
    {
        public int DocInstanceID { get; set; }
        public List<DocumentAttr> RelatedDocAttr { get; set; }
    }
}
