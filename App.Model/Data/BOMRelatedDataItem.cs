using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model.Data
{
    public class BOMRelatedDataItem
    {
        public BasicDoc RelatedDoc { get; set; }
        public List<BOMDataItem> RelatedDocInstances { get; set; }
    }
}
