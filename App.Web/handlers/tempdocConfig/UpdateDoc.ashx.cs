using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;

namespace App.Web.handlers.tempdocConfig
{
    /// <summary>
    /// Summary description for UpdateDoc
    /// </summary>
    public class UpdateDoc : IHttpHandler
    {
        TempDocConfigService dcs = new TempDocConfigService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int id = Convert.ToInt32(context.Request.Form["id"]);
            int docType = Convert.ToInt32(context.Request.Form["docType"]);
            string docName = context.Request.Form["docName"];
            string desc = context.Request.Form["desc"];
            int userid = Convert.ToInt32(context.Request.Form["userid"]);
            var r = dcs.UpdateDoc(new Model.Document { ID = id, DocType=new Model.DocType{ID=docType}, DocName = docName, Description = desc, LastModifier = new Model.User { ID = userid } });
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