using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
    public class BOMDocument:BasicDoc
    {
        public List<BOMDocumentAttr> RelatedDocAttrs { get; set; }
    }
}
