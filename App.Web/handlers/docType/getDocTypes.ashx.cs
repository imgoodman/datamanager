using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using App.Dll;
using App.Model;

namespace App.Web.handlers.docType
{
    /// <summary>
    /// Summary description for getDocTypes
    /// </summary>
    public class getDocTypes : IHttpHandler
    {
        Dll.SysMethod.DBDocTypeManager dts = new Dll.SysMethod.DBDocTypeManager();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int b = Convert.ToInt32(context.Request.Form["isVirtual"]);
            var r = dts.getTypes(b);
            var docTypejson = new List<DeptJSON>();
            foreach (var item in r)
            {
                docTypejson.Add(new DeptJSON { id = ("d" + item.ID.ToString()), parent = (item.FatherTypeID.ToString() == "0" ? "#" : ("d" + item.FatherTypeID.ToString())), text = item.TypeName });
            }
            context.Response.Write(JsonConvert.SerializeObject(docTypejson));
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