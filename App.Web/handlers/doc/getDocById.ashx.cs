using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using Newtonsoft.Json;

namespace App.Web.handlers.doc
{
    /// <summary>
    /// Summary description for getDocById
    /// </summary>
    public class getDocById : IHttpHandler
    {
        DocService ds = new DocService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int docinstanceid = Convert.ToInt32(context.Request.Form["docinstanceid"]);
            var r = ds.getDocInstanceById(docinstanceid);
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