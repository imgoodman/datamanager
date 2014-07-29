using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.Dll;
using System.Xml;
using System.IO;

namespace App.Web.BOM
{
    public partial class StaInstance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        BOMService bms = new BOMService();
        protected void btnExport_Click(object sender, EventArgs e)
        {
            string folderpath = Server.MapPath(@"\data\");
            int bomid = Convert.ToInt32(Request.Params["id"]);
            bms.exportDataIntoExcel(bomid, folderpath, "test");

            /*bms.bom.ID = bomid;
            var bom = bms.getBOM();

            //ready information
            string attach = "attachment;filename=" +HttpUtility.UrlEncode(bom.Name + ".xls",System.Text.Encoding.UTF8);
            Response.ClearContent();
            Response.Charset = "GB2312";
            Response.AddHeader("Content-Disposition", attach);
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/ms-excel";            
            

            //create header of doc
            Response.Write("序号" + "\t");
            Response.Write(bom.MainDoc.DocName + "\t");
            int totalAttrs = 0;
            if (bom.MainDoc.RelatedDocAttrs.Count > 1)
            {
                for (int i = 0; i < bom.MainDoc.RelatedDocAttrs.Count - 1; i++)
                    Response.Write("\t");
            }
            totalAttrs = bom.MainDoc.RelatedDocAttrs.Count;
            foreach (var rel_doc in bom.RelatedDocs)
            {
                Response.Write(rel_doc.DocName + "\t");
                if (rel_doc.RelatedDocAttrs.Count > 1)
                {
                    for (int i = 0; i < rel_doc.RelatedDocAttrs.Count - 1; i++)
                    {
                        Response.Write("\t");
                    }
                }
                totalAttrs += rel_doc.RelatedDocAttrs.Count;
            }
            Response.Write("\n");
            //create header of attr
            Response.Write("\t");
            int[] attrs = new int[totalAttrs];
            int attrIndex = 0;
            foreach (var attr in bom.MainDoc.RelatedDocAttrs)
            {
                Response.Write(attr.AttrName + "\t");
                attrs[attrIndex] = attr.ID;
                attrIndex++;
            }
            foreach (var rel_doc in bom.RelatedDocs)
            {
                foreach (var attr in rel_doc.RelatedDocAttrs)
                {
                    Response.Write(attr.AttrName + "\t");
                    attrs[attrIndex] = attr.ID;
                    attrIndex++;
                }
            }
            Response.Write("\n");
            string filepath = bms.getFilePath(bomid);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(folderpath + filepath);


            Response.End();*/
        }
    }
}