using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;

namespace App.Web.handlers.bom
{
    /// <summary>
    /// Summary description for getTotal
    /// </summary>
    public class getTotal : IHttpHandler
    {
        BOMService bms = new BOMService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string istemp = context.Request.Form["istemp"];
            int total = bms.getTotal(istemp);
            context.Response.Write(total);
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