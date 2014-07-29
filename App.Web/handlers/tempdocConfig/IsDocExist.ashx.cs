using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;

namespace App.Web.handlers.tempdocConfig
{
    /// <summary>
    /// Summary description for IsDocExist
    /// </summary>
    public class IsDocExist : IHttpHandler
    {
        TempDocConfigService dcs = new TempDocConfigService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string name = context.Request.Form["DocName"];
            bool r = dcs.isDocExist(name);
            context.Response.Write(r ? "1" : "-1");
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