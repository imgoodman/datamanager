using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using Newtonsoft.Json;

namespace App.Web.handlers.doc
{
    /// <summary>
    /// Summary description for getExpCheck
    /// </summary>
    public class getExpCheck : IHttpHandler
    {
        DocService docs = new DocService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int pageIndex = Convert.ToInt32(context.Request.Form["pageIndex"]);
            var r = docs.getExpCheck(pageIndex);
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