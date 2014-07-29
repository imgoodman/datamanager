using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using Newtonsoft.Json;

namespace App.Web.handlers.bom
{
    /// <summary>
    /// Summary description for getitembyid
    /// </summary>
    public class getitembyid : IHttpHandler
    {
        BOMService bms = new BOMService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int id = Convert.ToInt32(context.Request.Form["id"]);
            bms.bom.ID = id;
            var r = bms.getBOM();
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