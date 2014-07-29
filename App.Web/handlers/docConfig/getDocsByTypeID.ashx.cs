using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using Newtonsoft.Json;

namespace App.Web.handlers.docConfig
{
    /// <summary>
    /// Summary description for getDocsByTypeID
    /// </summary>
    public class getDocsByTypeID : IHttpHandler
    {
        DocConfigService dcs = new DocConfigService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int typeid = Convert.ToInt32(context.Request.Form["typeid"]);
            string cond = context.Request.Form["docCond"];
            var r = dcs.getDocsByTypeId(typeid,cond);
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