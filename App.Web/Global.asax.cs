using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using App.Dll;
using System.IO;

namespace App.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            System.Timers.Timer t = new System.Timers.Timer(1000 * 60 * 60);//hao miao wei dan wei
            t.AutoReset = true;
            t.Enabled = true;
            t.Elapsed += new System.Timers.ElapsedEventHandler(Fun);
        }
        private void Fun(object sender, System.Timers.ElapsedEventArgs e)
        {
            //12 点开始重新生成
            int gen_time = int.Parse(Dll.MyExtension.getAppValue("BOM_GenerateTime"));
            if (DateTime.Now.Hour == gen_time)
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
        }
        private void clearBOMdatas(string dirpath)
        {
            DirectoryInfo dir = new DirectoryInfo(dirpath);
            foreach (FileInfo file in dir.GetFiles("*.xml"))
            {
                file.Delete();
            }
        }
        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}