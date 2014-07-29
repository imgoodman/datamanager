using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using App.Model;

namespace App.Web.handlers.docConfig
{
    /// <summary>
    /// Summary description for ChangeAttrOrder
    /// </summary>
    public class ChangeAttrOrder : IHttpHandler
    {
        DocConfigService dcs = new DocConfigService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int upAttrid = Convert.ToInt32(context.Request.Form["upAttrid"]);
            int downAttrid = Convert.ToInt32(context.Request.Form["downAttrid"]);
            var upAttr = dcs.getAttrById(upAttrid);
            var downAttr = dcs.getAttrById(downAttrid);
            var r = dcs.changeAttrOrder(upAttr, downAttr);
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