using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using Newtonsoft.Json;

namespace App.Web.handlers.docConfig
{
    /// <summary>
    /// Summary description for List
    /// </summary>
    public class List : IHttpHandler
    {
        DocConfigService dcs = new DocConfigService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int pageIndex = Convert.ToInt32(context.Request.Form["pageIndex"]);
            string docTypeID = context.Request.Form["docTypeID"];
            string docName = context.Request.Form["docName"];
            var r = dcs.getDocuments(pageIndex,docTypeID,docName);
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