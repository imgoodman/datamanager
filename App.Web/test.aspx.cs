using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.Dll;
using System.IO;

namespace App.Web
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //产生新的清单数据文件
            string dirpath = HttpRuntime.AppDomainAppPath + @"data\";

            //clearBOMdatas(dirpath);
            BOMService bms = new BOMService();
            var boms = bms.getAllBasicBOMs();
            foreach (var bom in boms)
            {
                var filename=bms.exportDataIntoXML(bom, dirpath);
                bms.exportDataIntoExcel(bom.ID, dirpath, filename);
            }
        }
        private void clearBOMdatas(string dirpath)
        {
            DirectoryInfo dir = new DirectoryInfo(dirpath);
            foreach (FileInfo file in dir.GetFiles("*.xml"))
            {
                file.Delete();
            }
        }

        protected void btnCreator_Click(object sender, EventArgs e)
        {
            DocService ds = new DocService();
            ds.correctCreator();
        }
    }
}