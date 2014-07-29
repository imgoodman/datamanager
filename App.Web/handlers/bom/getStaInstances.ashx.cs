using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using Newtonsoft.Json;

namespace App.Web.handlers.bom
{
    /// <summary>
    /// Summary description for getStaInstances
    /// </summary>
    public class getStaInstances : IHttpHandler
    {
        BOMService bms = new BOMService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int id = Convert.ToInt32(context.Request.Form["id"]);
            int pageindex = Convert.ToInt32(context.Request.Form["pageindex"]);
            string folderpath = HttpRuntime.AppDomainAppPath + @"data\";
            var r = bms.getInstancesFromXML(id, pageindex,folderpath);
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