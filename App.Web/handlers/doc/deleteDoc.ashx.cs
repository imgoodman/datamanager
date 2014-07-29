using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;

namespace App.Web.handlers.doc
{
    /// <summary>
    /// Summary description for deleteDoc
    /// </summary>
    public class deleteDoc : IHttpHandler
    {
        DocService ds = new DocService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int docinstanceid = Convert.ToInt32(context.Request.Form["docinstanceid"]);
            int userid = Convert.ToInt32(context.Request.Form["userid"]);
            var r = ds.deleteDocInstance(docinstanceid, userid);
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