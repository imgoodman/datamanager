using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using Newtonsoft.Json;

namespace App.Web.handlers.bom
{
    /// <summary>
    /// Summary description for list
    /// </summary>
    public class list : IHttpHandler
    {
        BOMService bms = new BOMService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int pageindex = Convert.ToInt32(context.Request.Form["pageIndex"]);
            string bomname = context.Request.Form["bomname"];
            string istemp = context.Request.Form["istemp"];
            var r = bms.list(pageindex, bomname, istemp);
            context.Response.Write(JsonConvert.SerializeObject(r));
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