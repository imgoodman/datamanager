using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
    public class BaseModel
    {
        public User Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public User LastModifier { get; set; }
        public DateTime LastModifyTime { get; set; }
    }
}
