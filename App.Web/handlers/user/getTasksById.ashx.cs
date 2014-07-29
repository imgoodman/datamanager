using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using Newtonsoft.Json;

namespace App.Web.handlers.user
{
    /// <summary>
    /// Summary description for getTasksById
    /// </summary>
    public class getTasksById : IHttpHandler
    {
        Dll.SysMethod.DBUserManager bus = new Dll.SysMethod.DBUserManager();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int userid = Convert.ToInt32(context.Request.Form["userid"]);
            var r = bus.getTasksByUserID(userid);
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