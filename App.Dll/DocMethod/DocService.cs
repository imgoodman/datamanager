using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Model;
using System.Data;

namespace App.Dll
{
    public class DocService
    {
        BaseClass bc = new BaseClass();
        DocConfigService dcs = new DocConfigService();
        string sql = string.Empty;
        public int addDoc(DocumentInstance doc)
        {
            try
            {
                DateTime dt = DateTime.Now;
                sql = "insert into docinstance (docid,creatorid,createtime,lastmodifierid,lastmodifytime,isdeleted) values (" + doc.Document.ID + "," + doc.Creator.ID + ",'" + dt + "'," + doc.Creator.ID + ",'" + dt + "','false')";
                bc.RunSqlTransaction(sql);
                sql = "select * from docinstance where docid=" + doc.Document.ID + " and createtime='" + dt + "'";
                DataTable dTab = BaseClass.mydataset(sql).Tables[0];
                return int.Parse(dTab.Rows[0]["id"].ToString());
            }
            catch
            {
                return -1;
            }
        }
        public int addAttr(DocumentAttr attr, int docInstanceID, int userid)
        {
            try
            {
                sql = "insert into docinstance" + getAttrValueTableName(attr);
                sql += "(docinstanceid,docattrid,value,creatorid,createtime,lastmodifierid,lastmodifytime,isdeleted) values (" + docInstanceID + "," + attr.ID + ",'" + attr.Value + "'," + userid + ",'" + DateTime.Now + "'," + userid + ",'" + DateTime.Now + "','false')";
                bc.RunSqlTransaction(sql);
                sql = "select * from docinstance" + getAttrValueTableName(attr);
                sql += " where docinstanceid=" + docInstanceID + " and docattrid=" + attr.ID + " and isdeleted='false'";
                DataTable dt = BaseClass.mydataset(sql).Tables[0];
                return int.Parse(dt.Rows[0]["id"].ToString());
            }
            catch
            {
                return -1;
            }
        }
        public List<DocumentInstance> getDocInstances(int pageIndex, int docid, int userid)
        {
            int pageSize = MyExtension.ToInt(MyExtension.getAppValue("DocPageCount"));
            sql = "select top " + pageSize + " id from docinstance where id not in ( select top " + pageSize * pageIndex + " id from docinstance where isdeleted='false' and docid=" + docid + (userid == 0 ? "" : " and creatorid=" + userid) + " order by createtime desc) and isdeleted='false' and docid=" + docid + (userid == 0 ? "" : " and creatorid=" + userid) + " order by createtime desc";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                List<DocumentInstance> r = new List<DocumentInstance>();
                foreach (DataRow dr in dt.Rows)
                {
                    r.Add(getDocInstanceById(int.Parse(dr["id"].ToString())));
                }
                return r;
            }
            else
                return null;
        }
        FileService fs = new FileService();
        Dll.SysMethod.DBUserManager bus = new SysMethod.DBUserManager();
        public DocumentInstance getDocInstanceById(int id)
        {
            sql = "select * from docinstance where id=" + id;
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            DocumentInstance r = new DocumentInstance();
            r.ID = id;
            r.Document = dcs.getDocumentById(int.Parse(dt.Rows[0]["docid"].ToString()));
            r.Creator = bus.getUserByID(int.Parse(dt.Rows[0]["creatorid"].ToString()));
            foreach (var attr in r.Document.Attrs)
            {
                sql = "select * from docinstance" + getAttrValueTableName(attr);
                sql += " where docinstanceid=" + id + " and docattrid=" + attr.ID;
                dt = BaseClass.mydataset(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["creatorid"].ToString() != r.Creator.ID.ToString())
                    {
                        sql = "update docinstance" + getAttrValueTableName(attr)+" set creatorid="+r.Creator.ID+" where id="+dt.Rows[0]["id"].ToString();
                        bc.RunSqlTransaction(sql);
                    }
                    attr.Value = dt.Rows[0]["value"].ToString();

                    switch (attr.AttrType)
                    {
                        case AttrType.Text:
                        case AttrType.RichText:
                            attr.TranValue = attr.Value;
                            break;
                        case AttrType.Date:
                            if (!string.IsNullOrEmpty(attr.Value))
                                attr.TranValue = DateTime.Parse(attr.Value).ToString("yyyy-MM-dd");
                            else
                                attr.TranValue = MyExtension.getAppValue("ValueNA");
                            break;
                        case AttrType.File:
                            if (!string.IsNullOrEmpty(attr.Value))
                                attr.TranValue = fs.getOriginalFileName(attr.Value);
                            else
                                attr.TranValue = MyExtension.getAppValue("ValueNA");
                            break;
                        case AttrType.MultiPerson:
                            if (!string.IsNullOrEmpty(attr.Value))
                            {
                                foreach (string userid in attr.Value.Split(','))
                                {
                                    attr.TranValue += bus.getUserByID(int.Parse(userid)).Name + ",";
                                }
                                if (!string.IsNullOrEmpty(attr.TranValue))
                                    attr.TranValue = attr.TranValue.Remove(attr.TranValue.Length - 1);
                            }
                            else
                                attr.TranValue = MyExtension.getAppValue("ValueNA");
                            break;
                        case AttrType.Person:
                            if (!string.IsNullOrEmpty(attr.Value))
                                attr.TranValue = bus.getUserByID(int.Parse(attr.Value)).Name;
                            else
                                attr.TranValue = MyExtension.getAppValue("ValueNA");
                            break;
                        case AttrType.EnumVal:
                            if (!string.IsNullOrEmpty(attr.Value))
                                attr.TranValue = dcs.getAttrValueById(int.Parse(attr.Value)).AttrValue;
                            else
                                attr.TranValue = MyExtension.getAppValue("ValueNA");
                            break;
                    }
                }
                else
                {
                    attr.Value = "";
                    attr.TranValue = MyExtension.getAppValue("ValueNA");
                }
            }
            return r;
        }
        public bool updateAttr(int docInstanceId, DocumentAttr attr, int userid)
        {
            string tabName = getAttrValueTableName(attr);
            sql = "select * from docinstance" + tabName + " where docinstanceid=" + docInstanceId + " and docattrid=" + attr.ID;
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                sql = "update docinstance" + tabName;
                sql += " set value='" + attr.Value + "',lastmodifierid=" + userid + ",lastmodifytime='" + DateTime.Now + "' where docinstanceid=" + docInstanceId + " and docattrid=" + attr.ID;
                return bc.RunSqlTransaction(sql) == 0 ? true : false;
            }
            else
                return addAttr(attr, docInstanceId, userid) != -1 ? true : false;
        }
        public string getAttrValueTableName(DocumentAttr attr)
        {
            switch (attr.AttrType)
            {
                case AttrType.Date:
                    return "dateattr ";
                case AttrType.File:
                    return "fileattr";
                case AttrType.MultiPerson:
                    return "multipersonattr";
                case AttrType.Person:
                    return "personattr";
                case AttrType.RichText:
                    return "richtextattr";
                case AttrType.Text:
                    return "attr";
                case AttrType.EnumVal:
                    return "enumattr";
                default:
                    return "attr";
            }
        }
        public bool deleteDocInstance(int id, int userid)
        {
            var r = getDocInstanceById(id);
            foreach (var attr in r.Document.Attrs)
            {
                sql = "update docinstance" + getAttrValueTableName(attr) + " set isdeleted='true', lastmodifierid=" + userid + ", lastmodifytime='" + DateTime.Now + "' where docinstanceid=" + id + " and docattrid=" + attr.ID;
                bc.RunSqlTransaction(sql);
            }
            sql = "update docinstance set isdeleted='true' where id=" + id;
            return bc.RunSqlTransaction(sql) == 0 ? true : false;
        }
        public List<DocumentInstance> search(int pageIndex, int docid, int userid, List<DocumentAttr> attrs)
        {
            int pageSize = MyExtension.ToInt(MyExtension.getAppValue("DocPageCount"));
            string docInsIds = "";
            bool noJoin = false;
            foreach (var attr in attrs)
            {
                sql = "select docinstanceid from docinstance" + getAttrValueTableName(attr) + " where isdeleted='false' and docattrid=" + attr.ID + " " + (docInsIds == "" ? "" : " and docinstanceid in (" + docInsIds + ") ") + (userid == 0 ? "" : " and creatorid=" + userid);
                switch (attr.AttrType)
                {
                    case AttrType.Date:
                        if (attr.Value != "")
                            sql += " and value>='" + attr.Value + "' ";
                        if (attr.TranValue != "")
                            sql = " and value<='" + attr.TranValue + "' ";
                        break;
                    case AttrType.MultiPerson:
                        sql += " and lower(value) like '%" + attr.Value + "%'";
                        break;
                    case AttrType.Person:
                    case AttrType.EnumVal:
                        sql += " and value=" + attr.Value;
                        break;
                    case AttrType.Text:
                        sql += " and lower(value) like '%" + attr.Value + "%'";
                        break;
                }
                DataTable dtIDs = BaseClass.mydataset(sql).Tables[0];
                if (dtIDs.Rows.Count == 0)
                {
                    noJoin = true;
                    break;
                }
                else
                {
                    docInsIds = "";
                    foreach (DataRow dr in dtIDs.Rows)
                        docInsIds += dr["docinstanceid"].ToString() + ",";

                    docInsIds = docInsIds.Remove(docInsIds.Length - 1);
                }
            }
            if (!noJoin)
            {
                sql = "select top " + pageSize + " id from docinstance where isdeleted='false' and id not in ( select top " + pageSize * pageIndex + " id from docinstance where isdeleted='false' and docid=" + docid + (userid == 0 ? "" : " and creatorid=" + userid) + (docInsIds == "" ? "" : " and id in (" + docInsIds + ")") + " order by createtime desc) and isdeleted='false' and docid=" + docid + (userid == 0 ? "" : " and creatorid=" + userid) + (docInsIds == "" ? "" : " and id in (" + docInsIds + ")") + " order by createtime desc";
                DataTable dt = BaseClass.mydataset(sql).Tables[0];
                List<DocumentInstance> r = new List<DocumentInstance>();
                foreach (DataRow dr in dt.Rows)
                {
                    r.Add(getDocInstanceById(int.Parse(dr["id"].ToString())));
                }
                return r;
            }
            else
                return null;
        }
        public bool transferDocs(string docids, int receiverid)
        {
            try
            {
                string[] docidArray = docids.Split(',');
                foreach (string did in docidArray)
                {
                    sql = "update docinstance set creatorid=" + receiverid + " where id=" + did;
                    bc.RunSqlTransaction(sql);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public string getSourceInstanceID(int rInstID, int rAttrID)
        {
            try
            {
                //默认情况下 只有text属性才是可以关联其他对象的
                sql = "select * from docinstanceattr where docinstanceid=" + rInstID + " and docattrid=" + rAttrID;
                DataTable dt = BaseClass.mydataset(sql).Tables[0];
                string value = dt.Rows[0]["value"].ToString();

                string sourceIDs = string.Empty;

                sql = "select * from docattrrelate_tab where rattrid=" + rAttrID;
                dt = BaseClass.mydataset(sql).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    string sDocID = dr["sdocid"].ToString();
                    string sAttrID = dr["sattrid"].ToString();

                    sql = "select docinstanceid from docinstanceattr where docattrid=" + sAttrID + " and lower(value)='" + value.ToLower() + "'  and docinstanceid in (select id from docinstance where docid=" + sDocID + ")";
                    DataTable dt2 = BaseClass.mydataset(sql).Tables[0];
                    if (dt2.Rows.Count > 0)
                        sourceIDs += dt2.Rows[0]["docinstanceid"].ToString() + ",";
                }
                if (!string.IsNullOrEmpty(sourceIDs))
                    sourceIDs = sourceIDs.Remove(sourceIDs.Length - 1);
                return sourceIDs;
            }
            catch
            {
                return string.Empty;
            }
        }
        public List<BasicDoc> getAllBasicDocs(int isVirtual)
        {
            sql = "select * from document where isexpired='false' " + (isVirtual == 2 ? "" : " and typeid in (select id from documenttype_tab where isvirtual='" + isVirtual + "')") + "  order by docname asc";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            List<BasicDoc> r = new List<BasicDoc>();
            foreach (DataRow dr in dt.Rows)
            {
                r.Add(new BasicDoc { ID = int.Parse(dr["id"].ToString()), DocName = dr["docname"].ToString() });
            }
            return r;
        }
        public List<Experiment> getExperiments(string nameStart, int maxRows)
        {
            int expID = Convert.ToInt32(MyExtension.getAppValue("ExperimentID"));
            string attrName = MyExtension.getAppValue("ExpAttrName");
            string attrNameID = "";
            string attrValue = MyExtension.getAppValue("ExpAttrValue");
            string attrValueID = "";
            //find attrs of experient
            sql = "select * from documentattr where docid=" + expID;
            DataTable dtExps = BaseClass.mydataset(sql).Tables[0];
            foreach (DataRow dr in dtExps.Rows)
            {
                if (dr["attrname"].ToString() == attrName)
                    attrNameID = dr["id"].ToString();

                if (dr["attrname"].ToString() == attrValue)
                    attrValueID = dr["id"].ToString();
            }
            sql = "select * from docinstanceattr where docattrid in (" + attrNameID + "," + attrValueID + ") and lower(value) like '%" + nameStart.ToLower() + "%'";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            List<Experiment> r = new List<Experiment>();
            foreach (DataRow dr in dt.Rows)
            {
                Experiment exp = new Experiment();
                var docInst = getDocInstanceById(int.Parse(dr["docinstanceid"].ToString()));
                foreach (var attr in docInst.Document.Attrs)
                {
                    if (attr.AttrName == attrName)
                    {
                        exp.Code = attr.Value;
                    }
                    if (attr.AttrName == attrValue)
                    {
                        exp.Name = attr.Value;
                    }
                }
                r.Add(exp);
            }
            return r.OrderBy(p1 => p1.Code).OrderBy(p1 => p1.Name).Take(maxRows).ToList();
        }

        public int getTotal(int userid, List<DocumentAttr> attrs, int docid)
        {
            string docInsIds = "";
            bool noJoin = false;
            foreach (var attr in attrs)
            {
                sql = "select docinstanceid from docinstance" + getAttrValueTableName(attr) + " where isdeleted='false' and docattrid=" + attr.ID + " " + (docInsIds == "" ? "" : " and docinstanceid in (" + docInsIds + ") ") + (userid == 0 ? "" : " and creatorid=" + userid);
                switch (attr.AttrType)
                {
                    case AttrType.Date:
                        if (attr.Value != "")
                            sql += " and value>='" + attr.Value + "' ";
                        if (attr.TranValue != "")
                            sql = " and value<='" + attr.TranValue + "' ";
                        break;
                    case AttrType.MultiPerson:
                        sql += " and lower(value) like '%" + attr.Value + "%'";
                        break;
                    case AttrType.Person:
                    case AttrType.EnumVal:
                        sql += " and value=" + attr.Value;
                        break;
                    case AttrType.Text:
                        sql += " and lower(value) like '%" + attr.Value + "%'";
                        break;
                }
                DataTable dtIDs = BaseClass.mydataset(sql).Tables[0];
                if (dtIDs.Rows.Count == 0)
                {
                    noJoin = true;
                    break;
                }
                else
                {
                    docInsIds = "";
                    foreach (DataRow dr in dtIDs.Rows)
                        docInsIds += dr["docinstanceid"].ToString() + ",";

                    docInsIds = docInsIds.Remove(docInsIds.Length - 1);
                }
            }
            if (noJoin)
                return 0;
            else
            {
                sql = "select * from docinstance where isdeleted='false' and docid=" + docid + (userid == 0 ? "" : " and creatorid=" + userid) + (docInsIds == "" ? "" : " and id in (" + docInsIds + ")");
                return BaseClass.mydataset(sql).Tables[0].Rows.Count;
            }
        }
        //get files of a doc config
        public List<Model.MetaData.BasicData> getFileAttachesByDocID(int id)
        {
            sql = "select id,value from DocInstanceFileAttr where docinstanceid in (select id from DocInstance where docid=" + id + ") and isdeleted='false' and value<>'' ";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            List<Model.MetaData.BasicData> rs = new List<Model.MetaData.BasicData>();
            foreach (DataRow dr in dt.Rows)
            {
                string originalFileName = "N/A";
                sql = "select OriginalFileName from FileNameMapping where FileName='" + dr["value"].ToString() + "'";
                DataTable dt2 = BaseClass.mydataset(sql).Tables[0];
                if (dt2.Rows.Count > 0)
                    originalFileName = dt2.Rows[0]["OriginalFileName"].ToString();
                rs.Add(new Model.MetaData.BasicData { Name = dr["id"].ToString(), Value = dr["value"].ToString(), OrginalValue = originalFileName });
            }
            return rs;
        }
        public Model.MetaData.BasicData getFileAttachById(string id)
        {
            sql = "select id,value from DocInstanceFileAttr where id=" + id;
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            Model.MetaData.BasicData r = new Model.MetaData.BasicData();
            r.Name = id;
            r.Value = dt.Rows[0]["value"].ToString();
            sql = "select OriginalFileName from FileNameMapping where FileName='" + dt.Rows[0]["value"].ToString() + "'";
            DataTable dt2 = BaseClass.mydataset(sql).Tables[0];
            r.OrginalValue = dt2.Rows[0]["OriginalFileName"].ToString();
            return r;
        }

        public List<DocumentInstance> getExpCheck(int pageIndex)
        {
            List<DocumentInstance> rs = new List<DocumentInstance>();
            sql = "select top 15 docinstanceid from DocInstanceAttr where Value like '%(%' and DocAttrID in (select id from DocumentAttr where AttrName='所属试验' and IsExpired='false') and docinstanceid not in ( select top " + 15 * pageIndex + " docinstanceid from DocInstanceAttr where Value like '%(%' and DocAttrID in (select id from DocumentAttr where AttrName='所属试验' and IsExpired='false') order by docinstanceid asc) order by docinstanceid asc";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                rs.Add(getDocInstanceById(int.Parse(dr["docinstanceid"].ToString())));
            }
            return rs;
        }
        public int getExpCheckTotal()
        {
            return bc.intScalar("select count(0) from DocInstanceAttr where Value like '%(%' and DocAttrID in (select id from DocumentAttr where AttrName='所属试验' and IsExpired='false')");
        }
        public bool updateExpCheck()
        {
            try
            {
                sql = "select value,id from DocInstanceAttr where Value like '%(%' and DocAttrID in (select id from DocumentAttr where AttrName='所属试验' and IsExpired='false')";
                DataTable dt = BaseClass.mydataset(sql).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    var oldvalue = dr["value"].ToString();
                    if (oldvalue.Contains("(") || oldvalue.Contains("（"))
                    {
                        int symindex = 0;
                        if (oldvalue.Contains("("))
                            symindex = oldvalue.IndexOf("(");

                        if (oldvalue.Contains("（"))
                            symindex = oldvalue.IndexOf("（");
                        oldvalue = oldvalue.Substring(0, symindex);

                        sql = "update docinstanceattr set value='" + oldvalue + "' where id=" + dr["id"].ToString();
                        bc.RunSqlTransaction(sql);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void correctCreator()
        {
            sql = "select top 100 id,creatorid from docinstance where isdeleted='false' order by id desc";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                sql = "update docinstanceattr set creatorid=" + dr["creatorid"].ToString() + " where docinstanceid=" + dr["id"].ToString();
                bc.RunSqlTransaction(sql);
                sql = "update docinstancedateattr set creatorid=" + dr["creatorid"].ToString() + " where docinstanceid=" + dr["id"].ToString();
                bc.RunSqlTransaction(sql);
                sql = "update docinstanceenumattr set creatorid=" + dr["creatorid"].ToString() + " where docinstanceid=" + dr["id"].ToString();
                bc.RunSqlTransaction(sql);
                sql = "update docinstancefileattr set creatorid=" + dr["creatorid"].ToString() + " where docinstanceid=" + dr["id"].ToString();
                bc.RunSqlTransaction(sql);
                sql = "update docinstancemultipersonattr set creatorid=" + dr["creatorid"].ToString() + " where docinstanceid=" + dr["id"].ToString();
                bc.RunSqlTransaction(sql);
                sql = "update docinstancepersonattr set creatorid=" + dr["creatorid"].ToString() + " where docinstanceid=" + dr["id"].ToString();
                bc.RunSqlTransaction(sql);
                sql = "update docinstancerichtextattr set creatorid=" + dr["creatorid"].ToString() + " where docinstanceid=" + dr["id"].ToString();
                bc.RunSqlTransaction(sql);
            }
        }
        public int getBadCreators()
        {
            int total = 0;
            sql = "select id,creatorid from docinstance where isdeleted='false' order by id desc";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                sql = "select creatorid from docinstanceattr where docinstanceid=" + dr["id"].ToString();
                DataTable dt2 = BaseClass.mydataset(sql).Tables[0];
                if (dt2.Rows.Count > 0)
                    if (dt2.Rows[0]["creatorid"].ToString() != dr["creatorid"].ToString())
                        total++;

                if (total > 10)
                    break;
            }
            return total;
        }
    }
}
