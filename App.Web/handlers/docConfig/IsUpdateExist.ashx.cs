using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;

namespace App.Web.handlers.docConfig
{
    /// <summary>
    /// Summary description for IsUpdateExist
    /// </summary>
    public class IsUpdateExist : IHttpHandler
    {
        DocConfigService dcs = new DocConfigService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int id = Convert.ToInt32(context.Request.Form["ID"]);
            string name = context.Request.Form["docName"];
            var r = dcs.isDocUpdateExist(new Model.Document { ID = id, DocName = name });
            context.Response.Write(r ? "1" : "0");
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