using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
    public class DocumentAttr : BaseModel
    {
        public int ID { get; set; }
        public string AttrName { get; set; }
        public AttrType AttrType { get; set; }
        public string Value { get; set; }
        public string TranValue { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
        public bool IsSearch { get; set; }
        public bool IsRepeat { get; set; }
        public bool IsExpired { get; set; }
        public int VerticalOrder { get; set; }
        public int HorizontalOrder { get; set; }
        public int RelateAttrID { get; set; }
        public List<DocumentAttrVal> AttrVals { get; set; }
    }
    public enum AttrType
    {
        Text = 1,
        RichText = 2,
        Date = 3,
        Person = 4,
        MultiPerson = 5,
        File = 6,
        EnumVal=7,
        Obj=8
    }
}
