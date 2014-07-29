using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;

namespace App.Web.handlers.doc
{
    /// <summary>
    /// Summary description for getExpCheckTotal
    /// </summary>
    public class getExpCheckTotal : IHttpHandler
    {
        DocService docs = new DocService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var r = docs.getExpCheckTotal();
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