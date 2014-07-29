using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using Newtonsoft.Json;

namespace App.Web.handlers.user
{
    /// <summary>
    /// Summary description for getUsers
    /// </summary>
    public class getUsers : IHttpHandler
    {
        Dll.SysMethod.DBUserManager ums = new Dll.SysMethod.DBUserManager();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int deptid =Convert.ToInt32(context.Request.Form["deptID"]);
            var r = ums.getUsersByDepartment(deptid);
            context.Response.Write(JsonConvert.SerializeObject(r));
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