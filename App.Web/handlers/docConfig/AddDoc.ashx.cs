using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using App.Model;

namespace App.Web.handlers.docConfig
{
    /// <summary>
    /// Summary description for AddDoc
    /// </summary>
    public class AddDoc : IHttpHandler
    {
        DocConfigService dcs = new DocConfigService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int docType = Convert.ToInt32(context.Request.Form["docType"]);
            string docName = context.Request.Form["docName"];
            string desc = context.Request.Form["desc"];
            int userid = Convert.ToInt32(context.Request.Form["userid"]);
            int id = dcs.AddDoc(new Document { DocType=new DocType{ ID=docType}, DocName = docName, Description = desc, Creator = new User { ID = userid } });
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