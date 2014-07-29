using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using Newtonsoft.Json;
using App.Model;

namespace App.Web.handlers.dept
{
    /// <summary>
    /// Summary description for getDepts
    /// </summary>
    public class getDepts : IHttpHandler
    {
        Dll.SysMethod.DBDeptManager dms = new Dll.SysMethod.DBDeptManager();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var r = dms.getDepartments();
            var deptjson = new List<DeptJSON>();
            foreach (var item in r)
            {
                deptjson.Add(new DeptJSON { id =("d"+ item.ID.ToString()), parent = (item.FatherDepartmentID.ToString()=="0" ? "#" : ("d"+ item.FatherDepartmentID.ToString())), text = item.DeptName });
            }
            context.Response.Write(JsonConvert.SerializeObject(deptjson));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}