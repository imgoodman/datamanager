using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using Newtonsoft.Json;

namespace App.Web.handlers.bom
{
    /// <summary>
    /// Summary description for getmaindocattr
    /// </summary>
    public class getmaindocattr : IHttpHandler
    {
        BOMService bms = new BOMService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int bomid = Convert.ToInt32(context.Request.Form["id"]);
            int attrid = Convert.ToInt32(context.Request.Form["attrid"]);
            var item = bms.getMainDocAttr(bomid,attrid);
            context.Response.Write(JsonConvert.SerializeObject(item));
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