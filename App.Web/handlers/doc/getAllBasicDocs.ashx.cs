using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using Newtonsoft.Json;

namespace App.Web.handlers.doc
{
    /// <summary>
    /// Summary description for getAllBasicDocs
    /// </summary>
    public class getAllBasicDocs : IHttpHandler
    {
        DocService dcs = new DocService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int isVirtual = Convert.ToInt32(context.Request.Form["isVirtual"]);
            var r = dcs.getAllBasicDocs(isVirtual);
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