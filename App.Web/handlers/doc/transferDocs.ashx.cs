using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;

namespace App.Web.handlers.doc
{
    /// <summary>
    /// Summary description for transferDocs
    /// </summary>
    public class transferDocs : IHttpHandler
    {
        DocService dcs = new DocService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string docids = context.Request.Form["docids"];
            int recevierid = Convert.ToInt32(context.Request.Form["receiverid"]);
            var r = dcs.transferDocs(docids, recevierid);
            context.Response.Write(r ? "1" : "0");
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