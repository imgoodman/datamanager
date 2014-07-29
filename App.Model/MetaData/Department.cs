using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Model
{
    public class Department
    {
        public int ID { get; set; }
        public string DeptName { get; set; }
        public int FatherDepartmentID { get; set; }
        public int State { get; set; }
    }
}
