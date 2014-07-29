using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model.Data
{
    public class BOMInstance
    {
        public BOMDataItem MainDocData { get; set; }
        public BasicDoc MainDoc { get; set; }
        public List<BOMRelatedDataItem> RelatedDocDatas { get; set; }
    }
}
