using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using App.Model;

namespace App.Web.handlers.docConfig
{
    /// <summary>
    /// Summary description for IsAttrUpdateExist
    /// </summary>
    public class IsAttrUpdateExist : IHttpHandler
    {
        DocConfigService dcs = new DocConfigService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int docID = Convert.ToInt32(context.Request.Form["docID"]);
            int attrid = Convert.ToInt32(context.Request.Form["attrID"]);
            string attrname = context.Request.Form["attrName"];
            var r = dcs.isUpdateAttrExist(new Document { ID = docID, Attrs = new List<DocumentAttr>() { new DocumentAttr { AttrName = attrname, ID = attrid } } });
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