using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Dll;
using System.Web.SessionState;

namespace App.Web.handlers.bom
{
    /// <summary>
    /// Summary description for addBOM_MainDocAttr
    /// </summary>
    public class addBOM_MainDocAttr : IHttpHandler, IRequiresSessionState
    {
        BOMService bms = new BOMService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int id = Convert.ToInt32(context.Request.Form["id"]);
            int attrid = Convert.ToInt32(context.Request.Form["attrid"]);
            string surname = context.Request.Form["surname"];
            bms.bom.ID = id;
            bms.bom.MainDoc = new Model.BOMDocument();
            bms.bom.MainDoc.RelatedDocAttrs = new List<Model.BOMDocumentAttr>() { new Model.BOMDocumentAttr { ID = attrid, Surname = surname, Creator=new Model.User{ID=int.Parse(context.Session["userid"].ToString())} } };
            var r = bms.addBOM_MainDocAttr();
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