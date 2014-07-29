using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.Dll;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Text;

namespace App.Web.Doc
{
    public partial class Package : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnPackage_Click(object sender, EventArgs e)
        {
            try
            {
                Panel1.Visible = false;
                string docid = Request.Params["id"];
                if (!object.Equals(docid, null))
                {
                    DocConfigService docs = new DocConfigService();
                    var doc = docs.getBasicDocById(int.Parse(docid));

                    DocService dcs = new DocService();
                    string fileIds = fileids.Value;
                    if (!string.IsNullOrEmpty(fileIds))
                    {
                        //create temp folder
                        string dataFolder = Server.MapPath(@"\Avatar\");
                        DirectoryInfo di = new DirectoryInfo(dataFolder);
                        string tempFolder = "temp_" + doc.DocName + "_" + Guid.NewGuid().ToString();
                        DirectoryInfo tempDI = di.CreateSubdirectory(tempFolder);
                        fileIds = fileIds.Remove(fileIds.Length - 1);
                        string[] fidArray = fileIds.Split(',');
                        int fileIndex = 1;
                        foreach (string fid in fidArray)
                        {
                            var r = dcs.getFileAttachById(fid);
                            string sourFilePath = dataFolder + r.Value;
                            string targetFilePath = dataFolder + tempFolder + @"\" + fileIndex + "_" + r.OrginalValue;
                            if (File.Exists(sourFilePath))
                            {
                                File.Copy(sourFilePath, targetFilePath);
                                fileIndex++;
                            }
                        }
                        string err = "";
                        if (ZipFile(tempDI.ToString(), "", out err))
                        {
                            Panel1.Visible = true;
                            Label1.Text = "打包下载成功";
                            ResponseFile(tempDI.ToString() + ".zip");
                            
                            //delete temp file
                            //Directory.Delete(tempDI.ToString(), true);
                            //File.Delete(tempDI.ToString() + ".zip");
                            
                        }

                    }
                }
            }
            catch
            {
                Panel1.Visible = true;
                Label1.Text = "打包下载出错";
            }
        }
        /// <summary>
        /// 功能：压缩文件（暂时只压缩文件夹下一级目录中的文件，文件夹及其子级被忽略）
        /// </summary>
        /// <param name="dirPath">被压缩的文件夹夹路径</param>
        /// <param name="zipFilePath">生成压缩文件的路径，为空则默认与被压缩文件夹同一级目录，名称为：文件夹名+.zip</param>
        /// <param name="err">出错信息</param>
        /// <returns>是否压缩成功</returns>
        public bool ZipFile(string dirPath, string zipFilePath, out string err)
        {
            err = "";
            if (dirPath == string.Empty)
            {
                err = "要压缩的文件夹不能为空！";
                return false;
            }
            if (!Directory.Exists(dirPath))
            {
                err = "要压缩的文件夹不存在！";
                return false;
            }
            //压缩文件名为空时使用文件夹名＋.zip
            if (zipFilePath == string.Empty)
            {
                if (dirPath.EndsWith("\\"))
                {
                    dirPath = dirPath.Substring(0, dirPath.Length - 1);
                }
                zipFilePath = dirPath + ".zip";
            }

            try
            {
                string[] filenames = Directory.GetFiles(dirPath);
                using (ZipOutputStream s = new ZipOutputStream(File.Create(zipFilePath)))
                {
                    s.SetLevel(9);
                    byte[] buffer = new byte[4096];
                    foreach (string file in filenames)
                    {
                        ZipEntry entry = new ZipEntry(Path.GetFileName(file));
                        entry.IsUnicodeText = true;
                        entry.DateTime = DateTime.Now;
                        s.PutNextEntry(entry);
                        using (FileStream fs = File.OpenRead(file))
                        {
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                s.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }
                    }
                    s.Finish();
                    s.Close();
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
                return false;
            }
            return true;
        }
        protected void ResponseFile(string filename)
        {

            FileInfo file = new FileInfo(filename);//创建一个文件对象
            Response.Clear();//清除所有缓存区的内容
            Response.Charset = "GB2312";//定义输出字符集
            Response.ContentEncoding = Encoding.Default;//输出内容的编码为默认编码
            Response.AddHeader("Content-Disposition", "attachment;filename=" + file.Name);
            //添加头信息。为“文件下载/另存为”指定默认文件名称
            Response.AddHeader("Content-Length", file.Length.ToString());
            //添加头文件，指定文件的大小，让浏览器显示文件下载的速度 
            Response.WriteFile(file.FullName);// 把文件流发送到客户端
            Response.End();
        }
    }
}