using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace App.Dll.DocMethod
{
    public class DocAttrRelate
    {
        BaseClass bc = new BaseClass();

        public DataTable getAttrsByDocID(int did)
        {
            return BaseClass.mydataset("select * from DocumentAttr where IsExpired='false' and DocID='" + did + "' order by VerticalOrder asc").Tables[0];
        }
        public bool isRelationValid(int rdocID, int rattrID, int sdocID, int sattrID)
        {
            string sql = "select * from DocAttrRelate_Tab where RDocID='" + rdocID + "' and RAttrID='" + rattrID + "' and SDocID='" + sdocID + "'";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
                return false;
            else
                return true; ;
        }
        public int AddRelation(int rdocID, int rattrID, int sdocID, int sattrID)
        {
            string sql = "insert into DocAttrRelate_Tab (RDocID,RAttrID,SDocID,SAttrID) values (" + rdocID + ",'" + rattrID + "','" + sdocID + "','" + sattrID + "')";
            if (bc.RunSqlTransaction(sql) != 0)
                return -1;
            else
            {
                return bc.intScalar("select top 1 ID from DocAttrRelate_Tab order by ID desc");
            }
        }
    }
}
