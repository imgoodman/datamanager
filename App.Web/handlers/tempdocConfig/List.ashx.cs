using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using Newtonsoft.Json;

namespace App.Web.handlers.tempdocConfig
{
    /// <summary>
    /// Summary description for List
    /// </summary>
    public class List : IHttpHandler
    {
        TempDocConfigService dcs = new TempDocConfigService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int pageIndex = Convert.ToInt32(context.Request.Form["pageIndex"]);
            var r = dcs.getDocuments(pageIndex);
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