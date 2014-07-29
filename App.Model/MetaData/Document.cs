using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
    public class Document : BasicDoc
    {
        
        public bool IsExpired { get; set; }
        public string Description { get; set; }
        public List<DocumentAttr> Attrs { get; set; }
    }
}
