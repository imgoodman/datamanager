using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using App.Model;

namespace App.Web.handlers.doc
{
    /// <summary>
    /// Summary description for addAttr
    /// </summary>
    public class addAttr : IHttpHandler
    {
        DocService ds = new DocService();
        DocConfigService dcs = new DocConfigService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int docinstanceid = int.Parse(context.Request.Form["docInstanceID"]);
            int attrid = int.Parse(context.Request.Form["attrID"]);
            int userid = Convert.ToInt32(context.Request.Form["userid"]);
            var attr = dcs.getAttrById(attrid);
            string value = context.Request.Form["attrVal"];
            attr.Value = value;
            var r = ds.addAttr(attr, docinstanceid,userid);
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