using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using App.Dll;

namespace App.Web.Controls
{
    public partial class CommonFileUpload : System.Web.UI.UserControl
    {
        FileService fs = new FileService();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private string _serverFilePath;
        public string ServerFilePath { get { return HyperLink1.NavigateUrl; } }
        public string FileName
        {
            get
            {
                return lbAttachName.Text;
            }
            set
            {
                lbAttachName.Text = value.ToString();
                if (string.IsNullOrEmpty(value))
                {
                    HyperLink1.Text = HyperLink1.NavigateUrl = string.Empty;
                }
                else
                {
                    HyperLink1.Text = fs.getOriginalFileName(value);
                    HyperLink1.NavigateUrl = HttpContext.Current.Server.MapPath("../Avatar") + "//" + value;
                }
            }
        }

        public void Initialize()
        {
            this.FileName = "";
        }
        public void Save()
        {
            if (FileUpload1.HasFile)
            {
                string uploadPath = HttpContext.Current.Server.MapPath("../Avatar") + "//";
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                string extension = Path.GetExtension(FileUpload1.FileName);
                string realName = Path.GetFileNameWithoutExtension(FileUpload1.FileName);
                string guid = Guid.NewGuid().ToString();
                string filename = guid + extension;

                FileUpload1.SaveAs(uploadPath + filename);
                fs.add(filename, FileUpload1.FileName);
                this.FileName = filename;
            }
            else
                this.FileName = string.Empty;
        }
    }
}