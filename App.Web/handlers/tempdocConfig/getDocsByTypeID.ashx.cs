using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using Newtonsoft.Json;

namespace App.Web.handlers.tempdocConfig
{
    /// <summary>
    /// Summary description for getDocsByTypeID
    /// </summary>
    public class getDocsByTypeID : IHttpHandler
    {
        TempDocConfigService dcs = new TempDocConfigService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int typeid = Convert.ToInt32(context.Request.Form["typeid"]);
            var r = dcs.getDocsByTypeId(typeid);
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