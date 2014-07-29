using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Model;
using System.Data;

namespace App.Dll
{
    public class DashService
    {
        public string sql = "";
        BaseClass bc = new BaseClass();
        public List<NameValuePair> getTopInstances()
        {
            List<NameValuePair> rs = new List<NameValuePair>();
            sql = "select id,docname from document where isexpired='false' order by docname asc";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                sql = "select count(0) from docinstance where isdeleted='false' and docid=" + dr["id"].ToString();
                int number = bc.intScalar(sql);
                if (number != 0)
                    rs.Add(new NameValuePair { Name =showString(dr["docname"].ToString(),5), Value = number.ToString(), Number = number });
            }
            return rs.OrderByDescending(p1 => p1.Number).Take(10).ToList();
        }
        public string showString(string str, int maxlength)
        {
            if (str.Length > maxlength)
                return str.Substring(0, maxlength)+"..";
            else
                return str;
        }
        public List<NameValuePair> getTopCreators()
        {
            List<NameValuePair> rs = new List<NameValuePair>();
            sql = "select id,name from users_tab where state='1'";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                sql = "select count(0) from docinstance where isdeleted='false' and creatorid=" + dr["id"].ToString();
                int number = bc.intScalar(sql);
                if (number != 0)
                    rs.Add(new NameValuePair { Name =showString(dr["name"].ToString(),5), Number = number, Value = number.ToString() });
            }
            return rs.OrderByDescending(p1 => p1.Number).Take(10).ToList();
        }
        DocService docs = new DocService();
        public List<DocumentInstance> getUnattachFiles(int pageIndex, string userid)
        {
            int pageSize = MyExtension.ToInt(MyExtension.getAppValue("DocPageCount"));
            List<DocumentInstance> rs = new List<DocumentInstance>();
            sql = "select top " + pageSize + " docinstanceid from DocInstanceFileAttr where value='' and docattrid in (select id from documentattr where attrtype='File' and isrequired='true' and isexpired='false') and creatorid=" + userid + " and docinstanceid not in (select top " + pageSize * pageIndex + " docinstanceid from docinstancefileattr where value='' and docattrid in (select id from documentattr where attrtype='File' and isrequired='true' and isexpired='false' ) and creatorid=" + userid + " )";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                rs.Add(docs.getDocInstanceById(int.Parse(dr["docinstanceid"].ToString())));
            }
            return rs;
        }
    }
}
