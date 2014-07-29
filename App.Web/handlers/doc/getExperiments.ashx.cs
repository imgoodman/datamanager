using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using Newtonsoft.Json;

namespace App.Web.handlers.doc
{
    /// <summary>
    /// Summary description for getExperiments
    /// </summary>
    public class getExperiments : IHttpHandler
    {
        DocService dcs = new DocService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string nameStart = context.Request.Form["nameStart"];
            int maxRows = Convert.ToInt32(context.Request.Form["maxRows"]);
            var r = dcs.getExperiments(nameStart, maxRows);
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