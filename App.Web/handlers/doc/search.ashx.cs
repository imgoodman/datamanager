using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Model;
using App.Dll;
using Newtonsoft.Json;

namespace App.Web.handlers.doc
{
    /// <summary>
    /// Summary description for search
    /// </summary>
    public class search : IHttpHandler
    {
        DocConfigService dcs = new DocConfigService();
        DocService ds = new DocService();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int pageIndex = Convert.ToInt32(context.Request.Form["pageIndex"]);
            int docid = Convert.ToInt32(context.Request.Form["docID"]);
            int userid = int.Parse(context.Request.Form["userid"]);
            string attrids = context.Request.Form["attrIds"];
            string attrVals = context.Request.Form["attrVals"];
            string[] idArray = attrids.Split('$');
            string[] valArray = attrVals.Split('$');
            List<DocumentAttr> attrs = new List<DocumentAttr>();
            for (int i = 0; i < idArray.Length; i++)
            {
                if (idArray[i] != "0")
                {
                    var attr = dcs.getAttrById(int.Parse(idArray[i]));
                    attr.Value = valArray[i].ToLower();
                    if (attr.AttrType == AttrType.Date)
                    {
                        string dFrom = valArray[i].Split('_')[0];
                        string dTo = valArray[i].Split('_')[1];
                        attr.Value = (dFrom == "NA" ? "" : dFrom);
                        attr.TranValue = (dTo == "NA" ? "" : dTo);
                    }
                    attrs.Add(attr);
                }
                else
                    userid = int.Parse(valArray[i]);
            }
            var r = ds.search(pageIndex, docid, userid, attrs);
            if (r.Count > 0)
            {
                int attrCount = MyExtension.ToInt(MyExtension.getAppValue("DisplayDocAttrCount"));
                if (r.First().Document.Attrs.Count > attrCount)
                {
                    foreach (var item in r)
                    {
                        item.Document.Attrs.RemoveRange(attrCount, item.Document.Attrs.Count - attrCount);
                    }
                }
            }
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