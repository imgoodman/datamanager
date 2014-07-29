using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using Newtonsoft.Json;

namespace App.Web.handlers.docConfig
{
    /// <summary>
    /// Summary description for getSearchAttrs
    /// </summary>
    public class getSearchAttrs : IHttpHandler
    {
        DocConfigService dcs = new DocConfigService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int docid = Convert.ToInt32(context.Request.Form["id"]);
            var r = dcs.getSearchAtts(docid).OrderByDescending(p1 => p1.VerticalOrder); ;
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