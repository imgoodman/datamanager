using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using Newtonsoft.Json;

namespace App.Web.handlers.bom
{
    /// <summary>
    /// Summary description for getRelateddocattr
    /// </summary>
    public class getRelateddocattr : IHttpHandler
    {
        BOMService bms = new BOMService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int bomid = Convert.ToInt32(context.Request.Form["id"]);
            int reldocid = Convert.ToInt32(context.Request.Form["docid"]);
            int attrid = Convert.ToInt32(context.Request.Form["attrid"]);
            var r = bms.getRelatedDocAttr(bomid,reldocid,attrid);
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