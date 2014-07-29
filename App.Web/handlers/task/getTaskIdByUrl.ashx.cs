using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;

namespace App.Web.handlers.task
{
    /// <summary>
    /// Summary description for getTaskIdByUrl
    /// </summary>
    public class getTaskIdByUrl : IHttpHandler
    {
        TaskService ts = new TaskService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string url = context.Request.Form["url"];
            var r = ts.getTaskIDByUrl(url);
            context.Response.Write(r);
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