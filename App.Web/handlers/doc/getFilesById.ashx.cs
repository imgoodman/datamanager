using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using Newtonsoft.Json;

namespace App.Web.handlers.doc
{
    /// <summary>
    /// Summary description for getFilesById
    /// </summary>
    public class getFilesById : IHttpHandler
    {
        DocService docs = new DocService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string id = context.Request.Form["id"];
            var r = docs.getFileAttachesByDocID(int.Parse(id));
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