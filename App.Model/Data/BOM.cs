using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
    public class BOM:BasicBOM
    {
        
        public string Description { get; set; }
        public bool IsTemp { get; set; }
        public BOMDocument MainDoc { get; set; }
        public List<BOMDocument> RelatedDocs { get; set; }
    }
}
