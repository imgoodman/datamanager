using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Model;
using App.Dll;

namespace App.Web.handlers.docConfig
{
    /// <summary>
    /// Summary description for updateAttrVal
    /// </summary>
    public class updateAttrVal : IHttpHandler
    {
        DocConfigService dcs = new DocConfigService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int id = Convert.ToInt32(context.Request.Form["id"]);
            string val = context.Request.Form["attrVal"];
            int userid = Convert.ToInt32(context.Request.Form["userid"]);
            var r = dcs.updateAttrValue(new DocumentAttrVal { ID = id, AttrValue = val, LastModifier = new User { ID=userid } });
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