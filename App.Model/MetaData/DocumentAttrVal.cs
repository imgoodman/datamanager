﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
    public class DocumentAttrVal:BaseModel
    {
        public int ID { get; set; }
        public string AttrValue { get; set; }
        public bool IsExpired { get; set; }
    }
}
