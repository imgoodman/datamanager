using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;

namespace App.Web.handlers.doc
{
    /// <summary>
    /// Summary description for updateAttr
    /// </summary>
    public class updateAttr : IHttpHandler
    {
        DocService ds = new DocService();
        DocConfigService dcs = new DocConfigService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int docinstanceid = Convert.ToInt32(context.Request.Form["docinstanceid"]);
            int attrid = Convert.ToInt32(context.Request.Form["attrID"]);
            int userid = Convert.ToInt32(context.Request.Form["userid"]);
            string value = context.Request.Form["attrVal"];
            var attr = dcs.getAttrById(attrid);
            attr.Value = value;
            var r = ds.updateAttr(docinstanceid, attr,userid);
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