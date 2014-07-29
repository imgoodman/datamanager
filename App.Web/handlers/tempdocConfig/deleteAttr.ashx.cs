using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using App.Model;

namespace App.Web.handlers.tempdocConfig
{
    /// <summary>
    /// Summary description for deleteAttr
    /// </summary>
    public class deleteAttr : IHttpHandler
    {
        TempDocConfigService dcs = new TempDocConfigService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int attrid = Convert.ToInt32(context.Request.Form["attrID"]);
            int userid = Convert.ToInt32(context.Request.Form["userid"]);
            int docid = Convert.ToInt32(context.Request.Form["docID"]);
            var d = dcs.getAttrById(attrid);
            d.LastModifier = new User { ID = userid };
            var r = dcs.DisableAttr(d, docid);
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