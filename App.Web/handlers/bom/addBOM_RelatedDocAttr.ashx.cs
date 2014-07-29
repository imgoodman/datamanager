using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using System.Web.SessionState;

namespace App.Web.handlers.bom
{
    /// <summary>
    /// Summary description for addBOM_RelatedDocAttr
    /// </summary>
    public class addBOM_RelatedDocAttr : IHttpHandler, IRequiresSessionState
    {
        BOMService bms = new BOMService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int id = Convert.ToInt32(context.Request.Form["id"]);
            int attrid = Convert.ToInt32(context.Request.Form["attrid"]);
            string surname = context.Request.Form["surname"];
            bms.bom.RelatedDocs = new List<Model.BOMDocument> { new Model.BOMDocument { ID = id } };
            bms.bom.RelatedDocs[0].RelatedDocAttrs = new List<Model.BOMDocumentAttr> { new Model.BOMDocumentAttr { ID = attrid, Surname = surname, Creator = new Model.User { ID = Convert.ToInt32(context.Session["userid"].ToString()) } } };
            var r = bms.addBOM_RelatedDocAttr();
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