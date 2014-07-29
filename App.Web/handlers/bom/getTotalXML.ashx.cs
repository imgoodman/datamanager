using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;

namespace App.Web.handlers.bom
{
    /// <summary>
    /// Summary description for getTotalXML
    /// </summary>
    public class getTotalXML : IHttpHandler
    {
        BOMService bms = new BOMService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int id = Convert.ToInt32(context.Request.Form["id"]);
            string folderpath = HttpRuntime.AppDomainAppPath + @"data\";
            var r = bms.getTotalFromXML(id, folderpath);
            context.Response.Write(r);
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