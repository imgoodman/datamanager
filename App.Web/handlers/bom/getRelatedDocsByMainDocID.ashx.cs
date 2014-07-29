using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using Newtonsoft.Json;

namespace App.Web.handlers.bom
{
    /// <summary>
    /// Summary description for getRelatedDocsByMainDocID
    /// </summary>
    public class getRelatedDocsByMainDocID : IHttpHandler
    {
        DocConfigService dcs = new DocConfigService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int id = Convert.ToInt32(context.Request.Form["id"]);
            var r = dcs.getRelatedDocsByDocID(id);
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