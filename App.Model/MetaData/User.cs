using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public int DepartmentID { get; set; }
        public bool IsAdmin { get; set; }
    }
}
