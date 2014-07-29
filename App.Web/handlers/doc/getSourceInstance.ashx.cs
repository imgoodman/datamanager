using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;

namespace App.Web.handlers.doc
{
    /// <summary>
    /// Summary description for getSourceInstance
    /// </summary>
    public class getSourceInstance : IHttpHandler
    {
        DocService dcs = new DocService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string rInstID = context.Request.Form["rInstID"];
            string rAttrID = context.Request.Form["rAttrID"];
            var id = dcs.getSourceInstanceID(int.Parse(rInstID), int.Parse(rAttrID));
            context.Response.Write(id);
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