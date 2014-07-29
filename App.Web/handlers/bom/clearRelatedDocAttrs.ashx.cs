using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;

namespace App.Web.handlers.bom
{
    /// <summary>
    /// Summary description for clearRelatedDocAttrs
    /// </summary>
    public class clearRelatedDocAttrs : IHttpHandler
    {
        BOMService bms = new BOMService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int bomdocid = Convert.ToInt32(context.Request.Form["id"]);
            bms.bom.RelatedDocs = new List<Model.BOMDocument> { new Model.BOMDocument { ID = bomdocid } };
            var r = bms.clearBOM_RelatedDocAttr();
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