using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace App.Dll
{
    public class FileService
    {
        string sql = "";
        BaseClass bc = new BaseClass();
        public bool add(string filename, string originalfilename)
        {
            string ext = Path.GetExtension(originalfilename);
            sql = "insert into filenamemapping (filename,originalfilename,fileextension) values ('" + filename + "','" + originalfilename + "','" + ext + "')";
            return (bc.RunSqlTransaction(sql) == 0 ? true : false);
        }
        public string getOriginalFileName(string filename)
        {
            sql = "select * from filenamemapping where filename='"+filename+"'";
            return BaseClass.mydataset(sql).Tables[0].Rows[0]["originalfilename"].ToString();
        }
    }
}
