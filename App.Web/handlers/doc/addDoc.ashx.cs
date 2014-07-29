using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Model;
using App.Dll;

namespace App.Web.handlers.doc
{
    /// <summary>
    /// Summary description for addDoc
    /// </summary>
    public class addDoc : IHttpHandler
    {
        DocService ds = new DocService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int docid = int.Parse(context.Request.Form["docid"]);
            int userid = int.Parse(context.Request.Form["userid"]);
            var r = ds.addDoc(new DocumentInstance { Document = new Document { ID = docid }, Creator = new User { ID = userid } });
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