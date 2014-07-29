using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;

namespace App.Web.handlers.bom
{
    /// <summary>
    /// Summary description for deleteRelatedDoc
    /// </summary>
    public class deleteRelatedDoc : IHttpHandler
    {
        BOMService bms = new BOMService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int id = Convert.ToInt32(context.Request.Form["id"]);
            int docid = Convert.ToInt32(context.Request.Form["docid"]);
            bms.bom.ID = id;
            bms.bom.RelatedDocs = new List<Model.BOMDocument> { new Model.BOMDocument { ID = docid } };
            var r = bms.deleteBOM_RelatedDoc();
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