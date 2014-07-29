using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;

namespace App.Web.handlers.docConfig
{
    /// <summary>
    /// Summary description for AddAttrVal
    /// </summary>
    public class AddAttrVal : IHttpHandler
    {
        DocConfigService dcs = new DocConfigService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int attrid = Convert.ToInt32(context.Request.Form["attrid"]);
            string val = context.Request.Form["attrval"];
            int userid = Convert.ToInt32(context.Request.Form["userid"]);
            var r = dcs.addAttrValue(attrid, val, userid);
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