using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;


namespace App.Web.handlers.doc
{
    /// <summary>
    /// Summary description for updateExpCheck
    /// </summary>
    public class updateExpCheck : IHttpHandler
    {
        DocService docs = new DocService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var r = docs.updateExpCheck();
            context.Response.Write(r ? "1" : "");
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