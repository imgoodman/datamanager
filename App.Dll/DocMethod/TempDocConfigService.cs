using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using App.Model;

namespace App.Dll
{
    public class TempDocConfigService
    {
        BaseClass bc = new BaseClass();
        string sql = string.Empty;
        public bool isAccessAllowed(int id, int userid)
        {
            sql = "select * from tempdocument where id=" + id + " and creatorid=" + userid;
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            return dt.Rows.Count > 0 ? true : false;
        }
        public bool isAddAllowed(int userid)
        {
            return true;
        }
        public bool isDocExist(string name)
        {
            sql = "select * from document where docname='" + name + "' and isexpired='false'";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        public bool isDocUpdateExist(Document doc)
        {
            sql = "select * from tempdocument where isexpired='false' and docname='" + doc.DocName + "' and id<>" + doc.ID;
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        public int AddDoc(Document doc)
        {
            if (isDocExist(doc.DocName))
                return 0;

            sql = "insert into tempdocument (typeid,docname,isexpired,description,creatorid,createtime,lastmodifierid,lastmodifytime) values ("+doc.DocType.ID+",'" + doc.DocName + "','false','" + doc.Description + "','" + doc.Creator.ID + "','" + DateTime.Now.ToString() + "','" + doc.Creator.ID + "','" + DateTime.Now.ToString() + "')";
            if (bc.RunSqlTransaction(sql) == 0)
            {
                sql = "select id from tempdocument where docname='" + doc.DocName + "' and isexpired='false' ";
                DataTable dt = BaseClass.mydataset(sql).Tables[0];
                string id = dt.Rows[0]["ID"].ToString();
                //add tasks
                //TaskService ts = new TaskService();
                //int tid = ts.addTask(doc.DocName + "管理", "Doc/List.aspx?ID=" + id, 0, 0);
                //ts.addTask("新增" + doc.DocName, "Doc/Add.aspx?ID=" + id, 1, tid);
                //ts.addTask(doc.DocName + "列表", "Doc/List.aspx?ID=" + id, 1, tid);
                return int.Parse(id);
            }
            else
                return -1;
        }
        public bool UpdateDoc(Document doc)
        {
            sql = "update tempdocument set typeid="+doc.DocType.ID+", docname='" + doc.DocName + "',description='" + doc.Description + "',lastmodifierid='" + doc.LastModifier.ID + "',lastmodifytime='" + DateTime.Now.ToString() + "' where id=" + doc.ID;
            return (bc.RunSqlTransaction(sql) == 0 ? true : false);
        }
        public bool DisableDoc(Document doc)
        {
            sql = "update tempdocument set isexpired='true',lastmodifierid='" + doc.LastModifier.ID + "',lastmodifytime='" + DateTime.Now.ToString() + "' where id=" + doc.ID;
            bc.RunSqlTransaction(sql);
            //TaskService ts = new TaskService();
            //ts.deleteTasksByDocID(doc.ID);
            sql = "update tempdocumentattr set isexpired='true' where docid=" + doc.ID;
            return (bc.RunSqlTransaction(sql) == 0 ? true : false);
        }
        public List<Document> getDocuments(int pageIndex)
        {
            int pageSize = MyExtension.ToInt(MyExtension.getAppValue("DocPageCount"));
            sql = "select top " + pageSize + " id from tempdocument where id not in (select top " + pageIndex * pageSize + " id from tempdocument where isexpired='false' order by lastmodifytime desc) and isexpired='false' order by lastmodifytime desc";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                List<Document> docs = new List<Document>();
                foreach (DataRow dr in dt.Rows)
                    docs.Add(getDocumentById(int.Parse(dr["ID"].ToString())));

                return docs;
            }
            else
                return null;
        }
        SysMethod.DBDocTypeManager dts = new SysMethod.DBDocTypeManager();
        public Document getDocumentById(int id)
        {
            sql = "select * from tempdocument where id=" + id;
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                Document d = new Document();
                d.ID = id;
                d.DocType = dts.getItemById(int.Parse(dt.Rows[0]["typeid"].ToString()));
                d.DocName = dt.Rows[0]["DocName"].ToString();
                d.Description = dt.Rows[0]["Description"].ToString();
                d.Attrs = new List<DocumentAttr>();
                sql = "select * from tempdocumentattr where isexpired='false' and docid=" + id + " order by verticalorder asc, horizontalorder asc";
                dt = BaseClass.mydataset(sql).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    var attrItem = getAttrById(int.Parse(dr["ID"].ToString()));                    
                    d.Attrs.Add(attrItem);
                }
                return d;
            }
            else
                return null;
        }
        public DocumentAttr getAttrById(int id)
        {
            sql = "select * from tempdocumentattr where id=" + id;
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            DataRow dr = dt.Rows[0];
            DocumentAttr attrItem=new DocumentAttr { ID = int.Parse(dr["ID"].ToString()), AttrName = dr["AttrName"].ToString(), AttrType = getAttrType(dr["AttrType"].ToString()), IsRequired = Convert.ToBoolean(dr["IsRequired"].ToString()), IsSearch = Convert.ToBoolean(dr["IsSearch"].ToString()), VerticalOrder = Convert.ToInt32(dr["VerticalOrder"].ToString()), HorizontalOrder = Convert.ToInt32(dr["HorizontalOrder"].ToString()), Description = dr["Description"].ToString() };
            if (attrItem.AttrType == AttrType.EnumVal)
            {
                attrItem.AttrVals = new List<DocumentAttrVal>();
                sql = "select * from tempdocumentattrval where docattrid=" + id;
                DataTable dtVals = BaseClass.mydataset(sql).Tables[0];
                foreach (DataRow attrval in dtVals.Rows)
                {
                    attrItem.AttrVals.Add(new DocumentAttrVal { ID=Convert.ToInt32(attrval["ID"].ToString()), AttrValue=attrval["attrvalue"].ToString() });
                }
            }

            return attrItem; 
        }
        public AttrType getAttrType(string attrtype)
        {
            switch (attrtype)
            {
                case "Text":
                    return AttrType.Text;
                case "RichText":
                    return AttrType.RichText;
                case "Person":
                    return AttrType.Person;
                case "MultiPerson":
                    return AttrType.MultiPerson;
                case "Date":
                    return AttrType.Date;
                case "File":
                    return AttrType.File;
                case "EnumVal":
                    return AttrType.EnumVal;
                default:
                    return AttrType.Text;
            }
        }
        public bool IsAttrExist(Document docAttr)
        {
            sql = "select * from tempdocumentattr where docid='" + docAttr.ID + "' and attrname='" + docAttr.Attrs[0].AttrName + "' and isexpired='false' ";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            return dt.Rows.Count > 0 ? true : false;
        }
        public bool isUpdateAttrExist(Document docAttr)
        {
            sql = "select * from tempdocumentattr where docid='" + docAttr.ID + "' and attrname='" + docAttr.Attrs[0].AttrName + "' and id<>" + docAttr.Attrs[0].ID + " and isexpired='false'";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            return dt.Rows.Count > 0 ? true : false;
        }
        public int AddDocAttr(Document docAttr)
        {
            if (IsAttrExist(docAttr))
                return 0;

            sql = "insert into tempdocumentattr (docid,attrname,attrtype,description,isrequired,issearch,isexpired,verticalorder,horizontalorder,creatorid,createtime,lastmodifierid,lastmodifytime) values ('" + docAttr.ID + "','" + docAttr.Attrs[0].AttrName + "','" + docAttr.Attrs[0].AttrType + "','" + docAttr.Attrs[0].Description + "','" + docAttr.Attrs[0].IsRequired + "','" + docAttr.Attrs[0].IsSearch + "','false','" + docAttr.Attrs[0].VerticalOrder + "','0','" + docAttr.Attrs[0].Creator.ID + "','" + DateTime.Now.ToString() + "','" + docAttr.Attrs[0].Creator.ID + "','" + DateTime.Now.ToString() + "')";
            if (bc.RunSqlTransaction(sql) == 0)
            {
                sql = "select id from tempdocumentattr where docid='" + docAttr.ID + "' and attrname='" + docAttr.Attrs[0].AttrName + "' and isexpired='false'";
                DataTable dt = BaseClass.mydataset(sql).Tables[0];
                return int.Parse(dt.Rows[0]["ID"].ToString());
            }
            else
                return -1;
        }
        public bool deleteAttrValue(int id)
        {
            sql = "delete from tempdocumentattrval where id="+id;
            return (bc.RunSqlTransaction(sql)==0 ? true : false);
        }
        public int addAttrValue(int attrid, string val, int userid)
        {
            try
            {
                sql = "insert into tempdocumentattrval (docattrid,attrvalue,isexpired,creatorid,createtime,lastmodifierid,lastmodifytime) values (" + attrid + ",'" + val + "','false'," + userid + ",'" + DateTime.Now + "'," + userid + ",'" + DateTime.Now + "')";
                bc.RunSqlTransaction(sql);
                sql = "select id from tempdocumentattrval where docattrid=" + attrid + " and attrvalue='" + val + "'";
                DataTable dt = BaseClass.mydataset(sql).Tables[0];
                return Convert.ToInt32(dt.Rows[0]["id"].ToString());
            }
            catch
            {
                return -1;
            }
        }
        public bool updateAttrValue(DocumentAttrVal val)
        {
            sql = "update tempdocumentattrval set attrvalue='"+val.AttrValue+"',lastmodifierid="+val.LastModifier.ID+",lastmodifytime='"+DateTime.Now+"' where id="+val.ID;
            return (bc.RunSqlTransaction(sql) == 0 ? true : false);
        }
        public bool UpdateAttr(Document doc)
        {
            if (isUpdateAttrExist(doc))
                return false;

            sql = "update tempdocumentattr set attrname='" + doc.Attrs[0].AttrName + "',attrtype='" + doc.Attrs[0].AttrType + "',description='" + doc.Attrs[0].Description + "',isrequired='" + doc.Attrs[0].IsRequired + "',issearch='" + doc.Attrs[0].IsSearch + "',lastmodifierid='" + doc.Attrs[0].LastModifier.ID + "',lastmodifytime='" + DateTime.Now.ToString() + "' where id=" + doc.Attrs[0].ID;
            return bc.RunSqlTransaction(sql) == 0 ? true : false;
        }
        public bool UpdateAttrOrder(DocumentAttr doc)
        {
            sql = "update tempdocumentattr set verticalorder='" + doc.VerticalOrder + "',horizontalorderr='" + doc.HorizontalOrder + "' where id=" + doc.ID;
            return bc.RunSqlTransaction(sql) == 0 ? true : false;
        }
        public bool DisableAttr(DocumentAttr doc, int docid)
        {
            //分两种情况
            //第一：当前属性是单行，则升级后续属性即可；
            //第二：当前属性是所在行包含两个属性，则需要同时更新同行属性

            //升级后续属性
            sql = "select * from tempdocumentattr where docid=" + docid + " and verticalorder>" + doc.VerticalOrder + " and isexpired='false' order by verticalorder asc";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                sql = "update tempdocumentattr set verticalorder=" + (Convert.ToInt32(dr["VerticalOrder"].ToString()) - 1) + " where id=" + dr["id"].ToString();
                bc.RunSqlTransaction(sql);
            }
            //一行具有多个属性
            int count = sc.GetRecordCount("tempdocumentattr", " docid=" + docid + " and verticalorder=" + doc.VerticalOrder + " and isexpired='false'");
            if (doc.HorizontalOrder == 0 && count == 2)
            {
                //删除左边的属性了
                sql = "update tempdocumentattr set horizontalorder=0 where docid=" + docid + " and verticalorder=" + doc.VerticalOrder;
                bc.RunSqlTransaction(sql);
            }
            sql = "update tempdocumentattr set isexpired='true' where id=" + doc.ID;
            bc.RunSqlTransaction(sql);
            return true;
        }
        SysCommon sc = new SysCommon();
        public bool allowChangeOrder(int docID, DocumentAttr attr, string action)
        {
            bool r = true;
            //action是up or down
            //第一个不能升级，最后一个不能降级，两个在同级的不能升级或者降级
            int count = sc.GetRecordCount("tempdocumentattr", " docid='" + docID + "' and verticalorder=" + attr.VerticalOrder);
            if (count == 1)
            {
                //同一行只有一个元素
                if (attr.VerticalOrder == 0 && action == "up")
                {
                    //第一个属性
                    r = false;
                }
                else
                {
                    //是否最后一个属性
                    count = sc.GetRecordCount("tempdocumentattr", " docid='" + docID + "' and verticalorder>" + attr.VerticalOrder + " and isexpired='false'");
                    if (count == 0 && action == "down")
                    {
                        //最后一个
                        r = false;
                    }
                }
            }
            else
            {
                r = false;
            }
            return r;
        }
        public bool changeAttrOrder(DocumentAttr preAttr, DocumentAttr nextAttr)
        {
            sql = "update tempdocumentattr set verticalorder='" + nextAttr.VerticalOrder + "' where id=" + preAttr.ID;
            bc.RunSqlTransaction(sql);
            sql = "update tempdocumentattr set verticalorder='" + preAttr.VerticalOrder + "' where id=" + nextAttr.ID;
            bc.RunSqlTransaction(sql);
            return true;
        }
        public bool followAttr(DocumentAttr preAttr, DocumentAttr nextAttr)
        {
            return true;
        }

        public List<DocumentAttr> getSearchAtts(int docid)
        {
            List<DocumentAttr> r = new List<DocumentAttr>();
            sql = "select * from tempdocumentattr where issearch='true' and isexpired='false' and docid=" + docid;
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                r.Add(getAttrById(int.Parse(dr["id"].ToString())));
            }
            return r;
        }
        public List<BasicDoc> getBasicDocs()
        {
            sql = "select * from tempdocument where isexpired='false' order by docname asc";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                List<BasicDoc> r = new List<BasicDoc>();
                foreach (DataRow dr in dt.Rows)
                {
                    r.Add(new BasicDoc { ID=int.Parse(dr["id"].ToString()), DocName=dr["docname"].ToString() });
                }
                return r;
            }
            else
                return null;
        }
        public DocumentAttrVal getAttrValueById(int id)
        {
            sql = "select * from tempdocumentattrval where id="+id;
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            return new DocumentAttrVal { ID=id, AttrValue=dt.Rows[0]["attrvalue"].ToString() };
        }
        public List<BasicDoc> getDocsByTypeId(int typeid)
        {
            SysMethod.DBDocTypeManager dcm = new SysMethod.DBDocTypeManager();
            string typeids = dcm.GetChildTypeIDs(typeid);
            if (string.IsNullOrEmpty(typeids))
                typeids = typeid.ToString();
            else
                typeids += ","+typeid.ToString();
            sql = "select * from tempdocument where typeid in (" + typeids+")";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (!object.Equals(dt, null))
            {
                List<BasicDoc> r = new List<BasicDoc>();
                foreach (DataRow dr in dt.Rows)
                {
                    r.Add(new BasicDoc { ID = int.Parse(dr["id"].ToString()), DocName = dr["docname"].ToString() });
                }
                return r;
            }
            else
                return null;
        }
    }
}
