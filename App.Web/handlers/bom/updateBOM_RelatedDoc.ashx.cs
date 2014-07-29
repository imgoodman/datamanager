using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;

namespace App.Web.handlers.bom
{
    /// <summary>
    /// Summary description for updateBOM_RelatedDoc
    /// </summary>
    public class updateBOM_RelatedDoc : IHttpHandler
    {
        BOMService bms = new BOMService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int olddocid = Convert.ToInt32(context.Request.Form["olddocid"]);
            int newdocid = Convert.ToInt32(context.Request.Form["newdocid"]);
            int bomid = Convert.ToInt32(context.Request.Form["id"]);
            int id = bms.updateBOM_RelatedDoc(bomid, olddocid, newdocid);
            context.Response.Write(id);
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