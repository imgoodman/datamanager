using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using App.Dll;

namespace App.Web.ajax.file
{
    /// <summary>
    /// Summary description for fileUpload
    /// </summary>
    public class fileUpload : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "utf-8";

            HttpPostedFile file = context.Request.Files["Filedata"];
            string uploadPath =
                HttpContext.Current.Server.MapPath("../../Avatar") + "//";

            if (file != null)
            {
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                //file.SaveAs(uploadPath + file.FileName);
                string extension=Path.GetExtension(file.FileName);
                string realName = Path.GetFileNameWithoutExtension(file.FileName);
                string guid = Guid.NewGuid().ToString();
                string filename=guid+extension;
                /*string filename1 = "104-"+guid  + extension;
                string filename2 = "30-"+guid + extension;
                string filename3 = "200-"+guid + extension;
                string filename4 = "100-" + guid + extension;
                string filename5 = "40-" + guid + extension;*/
                file.SaveAs(uploadPath + filename);
                FileService fs = new FileService();
                fs.add(filename, file.FileName);
                /*if (ImageExtensions.Contains(extension.ToLower()))
                {
                    CreateThumbnail(uploadPath + filename, 30, 30, uploadPath + filename2);
                    CreateThumbnail(uploadPath + filename, 104, 104, uploadPath + filename1);
                    CreateThumbnail(uploadPath + filename, 200, 200, uploadPath + filename3);
                    CreateThumbnail(uploadPath + filename, 100, 100, uploadPath + filename4);
                    CreateThumbnail(uploadPath + filename, 40, 40, uploadPath + filename5);
                }*/
                //下面这句代码缺少的话，上传成功后上传队列的显示不会自动消失
                context.Response.Write(file.FileName+"_"+ filename);
            }
            else
            {
                context.Response.Write("0");
            }
        }
        public void CreateThumbnail(string filename, int desiredWidth, int desiredHeight, string outFilename)
        {
            using (System.Drawing.Image img = System.Drawing.Image.FromFile(filename))
            {
                float widthRatio = (float)img.Width / (float)desiredWidth;
                float heightRatio = (float)img.Height / (float)desiredHeight;
                // Resize to the greatest ratio
                float ratio = heightRatio > widthRatio ? heightRatio : widthRatio;
                int newWidth = Convert.ToInt32(Math.Floor((float)img.Width / ratio));
                int newHeight = Convert.ToInt32(Math.Floor((float)img.Height / ratio));
                using (System.Drawing.Image thumb = img.GetThumbnailImage(newWidth, newHeight, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailImageAbortCallback), IntPtr.Zero))
                {
                    thumb.Save(outFilename);
                }
            }
        }

        public static bool ThumbnailImageAbortCallback()
        {
            return true;
        }
        public string[] ImageExtensions = new string[] {".jpg",".gif",".png" };
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}