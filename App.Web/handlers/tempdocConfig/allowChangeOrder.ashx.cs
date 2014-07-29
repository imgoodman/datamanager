using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Model;
using App.Dll;

namespace App.Web.handlers.tempdocConfig
{
    /// <summary>
    /// Summary description for allowChangeOrder
    /// </summary>
    public class allowChangeOrder : IHttpHandler
    {
        TempDocConfigService dcs = new TempDocConfigService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int docID = Convert.ToInt32(context.Request.Form["docID"]);
            int attrid = Convert.ToInt32(context.Request.Form["attrID"]);
            string action = context.Request.Form["action"];
            var r = dcs.allowChangeOrder(docID, dcs.getAttrById(attrid), action);
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