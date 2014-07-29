using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using Newtonsoft.Json;

namespace App.Web.handlers.dashboard
{
    /// <summary>
    /// Summary description for getTopCreators
    /// </summary>
    public class getTopCreators : IHttpHandler
    {
        DashService ds = new DashService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var r = ds.getTopCreators();
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