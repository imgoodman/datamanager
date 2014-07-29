using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using System.Web.SessionState;

namespace App.Web.handlers.bom
{
    /// <summary>
    /// Summary description for addBOM_RelatedDoc
    /// </summary>
    public class addBOM_RelatedDoc : IHttpHandler,IRequiresSessionState
    {
        BOMService bms = new BOMService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int id = Convert.ToInt32(context.Request.Form["id"]);
            int docid = Convert.ToInt32(context.Request.Form["docid"]);
            bms.bom.ID = id;
            bms.bom.RelatedDocs = new List<Model.BOMDocument> {new Model.BOMDocument{ ID=docid, Creator=new Model.User{ID=Convert.ToInt32(context.Session["userid"].ToString())}} };
            var r = bms.addBOM_RelatedDoc();
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