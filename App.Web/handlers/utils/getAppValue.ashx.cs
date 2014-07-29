using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.handlers.utils
{
    /// <summary>
    /// Summary description for getAppValue
    /// </summary>
    public class getAppValue : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var key = context.Request.Form["key"];
            context.Response.Write(App.Dll.MyExtension.getAppValue(key));
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