using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;

namespace App.Web.handlers.bom
{
    /// <summary>
    /// Summary description for getbomtotal
    /// </summary>
    public class getbomtotal : IHttpHandler
    {
        BOMService bms = new BOMService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int bomid = Convert.ToInt32(context.Request.Form["id"]);
            var r = bms.getBOM_Total(bomid, new List<Model.DocumentAttr>() { });
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