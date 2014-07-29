using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Model;
using System.Data;
using System.Xml;

namespace App.Dll
{
    public class BOMService
    {
        private static readonly string TABLENAME_BOM = "BOM";
        private static readonly string TABLENAME_BOMMainDocAttr = "BOMMainDocAttr";
        private static readonly string TABLENAME_BOMRelatedDoc = "BOMRelatedDoc";
        private static readonly string TABLENAME_BOMRelatedDocAttr = "BOMRelatedDocAttr";

        private static readonly string XML_FILE_PATH = @"data\";

        private string sql = "";

        BaseClass bc = new BaseClass();

        public BOM bom = new BOM();
        public BasicBOM getBasicBOM(int id)
        {
            sql = "select id,name,maindocid from " + TABLENAME_BOM + " where id=" + id;
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            return new BasicBOM { ID = id, Name = dt.Rows[0]["name"].ToString(), MainDocId=dt.Rows[0]["maindocid"].ToString() };
        }
        public List<BasicBOM> getAllBasicBOMs()
        {
            List<BasicBOM> rs = new List<BasicBOM>();
            sql = "select * from " + TABLENAME_BOM + " where isdeleted='false'";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                rs.Add(getBasicBOM(int.Parse(dr["id"].ToString())));
            }
            return rs;
        }
        //check bom basic
        public bool checkBOM_Basic()
        {
            sql = "select id from " + TABLENAME_BOM + " where name='" + bom.Name + "' and isdeleted='false'";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        public int addBOM_Basic()
        {
            if (checkBOM_Basic())
                return 0;

            sql = "insert into " + TABLENAME_BOM + " (name,description,istemp,creatorid,createtime,lastmodifierid,lastmodifytime) values ('" + bom.Name + "','" + bom.Description + "','true'," + bom.Creator.ID + ",'" + DateTime.Now + "'," + bom.Creator.ID + ",'" + DateTime.Now + "')";
            bc.RunSqlTransaction(sql);
            sql = "select id from " + TABLENAME_BOM + " where name='" + bom.Name + "' and isdeleted='false'";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
                return int.Parse(dt.Rows[0]["id"].ToString());
            else
                return -1;
        }
        public bool checkUpdateBOM_Basic()
        {
            sql = "select id from " + TABLENAME_BOM + " where name='" + bom.Name + "' and isdeleted='false' and id<>" + bom.ID;
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        public bool updateBOM_Basic()
        {
            if (checkUpdateBOM_Basic())
                return false;

            sql = "update " + TABLENAME_BOM + " set name='" + bom.Name + "', lastmodifierid=" + bom.LastModifier.ID + ", lastmodifytime='" + DateTime.Now + "' where id=" + bom.ID;
            return bc.RunSqlTransaction(sql) == 0 ? true : false;
        }
        public bool updateBOM_MainDoc()
        {
            sql = "update " + TABLENAME_BOM + " set maindocid=" + bom.MainDoc.ID + ",lastmodifierid=" + bom.LastModifier.ID + ",lastmodifytime='" + DateTime.Now + "' where id=" + bom.ID;
            return bc.RunSqlTransaction(sql) == 0 ? true : false;
        }
        //check bom exist
        public bool checkBOM()
        {
            sql = "select id from " + TABLENAME_BOM + " where name='" + bom.Name + "' and maindocid=" + bom.MainDoc.ID + " and isdeleted='false'";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        //add bom,-1 error, 0 repeat, otherwise, success
        public int addBOM()
        {
            if (checkBOM())
            {
                return 0;
            }

            sql = "insert into " + TABLENAME_BOM + " (name,maindocid,description,istemp,creatorid,createtime,lastmodifierid,lastmodifytime) values ('" + bom.Name + "'," + bom.MainDoc.ID + ",'true','" + bom.Creator.ID + "','" + DateTime.Now + "','" + bom.Creator.ID + "','" + DateTime.Now + "')";
            bc.RunSqlTransaction(sql);
            sql = "select id from " + TABLENAME_BOM + " where name='" + bom.Name + "' and maindocid=" + bom.MainDoc.ID + " and isdeleted='false'";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
                return int.Parse(dt.Rows[0]["id"].ToString());
            else
                return -1;
        }
        //delete bom
        public bool deleteBOM()
        {
            sql = "update " + TABLENAME_BOM + " set isdeleted='true' where id=" + bom.ID;
            return bc.RunSqlTransaction(sql) == 0 ? true : false;
        }
        //activate bom
        public bool activateBOM()
        {
            sql = "update " + TABLENAME_BOM + " set istemp='false' where id=" + bom.ID;
            return bc.RunSqlTransaction(sql) == 0 ? true : false;
        }
        //update bom
        public bool updateBOM()
        {
            sql = "update " + TABLENAME_BOM + " set name='" + bom.Name + "',MainDocID=" + bom.MainDoc.ID + ",description='" + bom.Description + "',lastmodifierid=" + bom.LastModifier.ID + ",lastmodifytime='" + DateTime.Now + "'";
            return bc.RunSqlTransaction(sql) == 0 ? true : false;
        }
        //get bom
        SysMethod.DBUserManager us = new SysMethod.DBUserManager();
        public BOM getBOM()
        {
            sql = "select * from " + TABLENAME_BOM + " where id=" + bom.ID;
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            DataRow dr = dt.Rows[0];
            bom.Name = dr["name"].ToString();
            bom.CreateTime = Convert.ToDateTime(dr["createtime"].ToString());
            bom.Creator = us.getUserByID(int.Parse(dr["creatorid"].ToString()));
            bom.Description = dr["description"].ToString();
            bom.IsTemp = Convert.ToBoolean(dr["istemp"].ToString());
            bom.FilePath = dr["filepath"].ToString();
            if (!string.IsNullOrEmpty(dr["maindocid"].ToString()))
            {
                bom.MainDoc = new BOMDocument();
                bom.MainDoc.ID = int.Parse(dr["MainDocID"].ToString());
                bom.MainDoc = getBOM_MainDocument();
            }
            else
                bom.MainDoc = null;
            bom.RelatedDocs = new List<BOMDocument>();
            sql = "select * from " + TABLENAME_BOMRelatedDoc + " where bomid=" + bom.ID + " and isdeleted='false'";
            dt = BaseClass.mydataset(sql).Tables[0];
            foreach (DataRow dr2 in dt.Rows)
            {
                bom.RelatedDocs.Add(getBOM_RelatedDocument(int.Parse(dr2["id"].ToString()), int.Parse(dr2["RelatedDocID"].ToString())));
            }
            return bom;

        }
        //get bom document
        DocConfigService dcs = new DocConfigService();
        BOMDocument getBOM_MainDocument()
        {
            var basicdoc = dcs.getBasicDocById(bom.MainDoc.ID);
            BOMDocument bdoc = new BOMDocument { DocName = basicdoc.DocName, ID = basicdoc.ID };
            sql = "select * from " + TABLENAME_BOMMainDocAttr + " where bomid=" + bom.ID + " and isdeleted='false' ";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            bdoc.RelatedDocAttrs = new List<BOMDocumentAttr>();
            foreach (DataRow dr in dt.Rows)
            {
                var basicattr = dcs.getAttrById(int.Parse(dr["maindocattrid"].ToString()));
                BOMDocumentAttr attr = new BOMDocumentAttr { ID = basicattr.ID, AttrName = basicattr.AttrName };
                attr.Surname = dr["surname"].ToString();
                bdoc.RelatedDocAttrs.Add(attr);
            }
            return bdoc;
        }
        //get related document
        BOMDocument getBOM_RelatedDocument(int rel_id, int docid)
        {
            var basicdoc = dcs.getBasicDocById(docid);
            BOMDocument bdoc = new BOMDocument { ID = docid, DocName = basicdoc.DocName };
            sql = "select * from " + TABLENAME_BOMRelatedDocAttr + " where BOMDocID=" + rel_id + " and isdeleted='false'";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            bdoc.RelatedDocAttrs = new List<BOMDocumentAttr>();
            foreach (DataRow dr in dt.Rows)
            {
                var basicattr = dcs.getAttrById(int.Parse(dr["DocAttrID"].ToString()));
                BOMDocumentAttr attr = new BOMDocumentAttr { ID = basicattr.ID, AttrName = basicattr.AttrName };
                attr.Surname = dr["surname"].ToString();
                bdoc.RelatedDocAttrs.Add(attr);
            }
            return bdoc;
        }


        //add main doc attr
        bool checkBOM_MainDocAttr()
        {
            sql = "select id from " + TABLENAME_BOMMainDocAttr + " where bomid=" + bom.ID + " and mainDocAttrID=" + bom.MainDoc.RelatedDocAttrs[0].ID + " and isdeleted='false'";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                //exist and update its sruname
                //sql = "update "+TABLENAME_BOMMainDocAttr+" set surname="+bom.MainDoc.RelatedDocAttrs[0].Surname+" where id="+dt.Rows[0]["id"].ToString();
                //bc.RunSqlTransaction(sql);
                return true;
            }
            else
                return false;
        }
        //clear main doc attrs
        public bool clearMainDocAttr()
        {
            sql = "delete from " + TABLENAME_BOMMainDocAttr + " where bomid=" + bom.ID;
            return bc.RunSqlTransaction(sql) == 0 ? true : false;
        }
        public int addBOM_MainDocAttr()
        {
            if (checkBOM_MainDocAttr())
                return 0;

            sql = "insert into " + TABLENAME_BOMMainDocAttr + " (bomid,MainDocAttrID,surname,creatorid,createtime,lastmodifierid,lastmodifytime) values (" + bom.ID + "," + bom.MainDoc.RelatedDocAttrs[0].ID + ",'" + bom.MainDoc.RelatedDocAttrs[0].Surname + "'," + bom.MainDoc.RelatedDocAttrs[0].Creator.ID + ",'" + DateTime.Now + "'," + bom.MainDoc.RelatedDocAttrs[0].Creator.ID + ",'" + DateTime.Now + "')";
            bc.RunSqlTransaction(sql);
            sql = "select id from " + TABLENAME_BOMMainDocAttr + " where bomid=" + bom.ID + " and MainDocAttrID=" + bom.MainDoc.RelatedDocAttrs[0].ID + " and isdeleted='false'";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            return int.Parse(dt.Rows[0]["id"].ToString());
        }



        //check related doc
        public bool checkBOM_RelatedDoc()
        {
            sql = "select id from " + TABLENAME_BOMRelatedDoc + " where bomid=" + bom.ID + " and RelatedDocID=" + bom.RelatedDocs[0].ID + " and isdeleted='false'";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        //add related doc
        public int addBOM_RelatedDoc()
        {
            if (checkBOM_RelatedDoc())
                return 0;

            sql = "insert into " + TABLENAME_BOMRelatedDoc + " (bomid,RelatedDocID,creatorid,createtime,lastmodifierid,lastmodifytime) values (" + bom.ID + "," + bom.RelatedDocs[0].ID + "," + bom.RelatedDocs[0].Creator.ID + ",'" + DateTime.Now + "'," + bom.RelatedDocs[0].Creator.ID + ",'" + DateTime.Now + "')";
            bc.RunSqlTransaction(sql);
            sql = "select id from " + TABLENAME_BOMRelatedDoc + " where bomid=" + bom.ID + " and RelatedDocID=" + bom.RelatedDocs[0].ID + " and isdeleted='false' ";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
                return int.Parse(dt.Rows[0]["id"].ToString());
            else
                return -1;
        }
        //delete related doc
        public bool deleteBOM_RelatedDoc()
        {
            sql = "update " + TABLENAME_BOMRelatedDoc + " set isdeleted='true' where bomid=" + bom.ID + " and RelatedDocID=" + bom.RelatedDocs[0].ID;
            return bc.RunSqlTransaction(sql) == 0 ? true : false;
        }
        //clear related doc attr
        public bool clearBOM_RelatedDocAttr()
        {
            sql = "delete from " + TABLENAME_BOMRelatedDocAttr + " where BOMDocID=" + bom.RelatedDocs[0].ID;
            return bc.RunSqlTransaction(sql) == 0 ? true : false;
        }
        //add related doc attr
        public int addBOM_RelatedDocAttr()
        {
            sql = "insert into " + TABLENAME_BOMRelatedDocAttr + " (BOMDocID,docattrid,surname,creatorid,createtime,lastmodifierid,lastmodifytime) values (" + bom.RelatedDocs[0].ID + "," + bom.RelatedDocs[0].RelatedDocAttrs[0].ID + ",'" + bom.RelatedDocs[0].RelatedDocAttrs[0].Surname + "'," + bom.RelatedDocs[0].RelatedDocAttrs[0].Creator.ID + ",'" + DateTime.Now + "'," + bom.RelatedDocs[0].RelatedDocAttrs[0].Creator.ID + ",'" + DateTime.Now + "')";
            bc.RunSqlTransaction(sql);
            sql = "select id from " + TABLENAME_BOMRelatedDocAttr + " where bomdocid=" + bom.RelatedDocs[0].ID + " and docattrid=" + bom.RelatedDocs[0].RelatedDocAttrs[0].ID + " and isdeleted='false'";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
                return int.Parse(dt.Rows[0]["id"].ToString());
            else
                return -1;
        }

        public BOMDocumentAttr getMainDocAttr(int bomid, int attrid)
        {
            sql = "select * from " + TABLENAME_BOMMainDocAttr + " where bomid=" + bomid + " and maindocattrid=" + attrid + " and isdeleted='false' ";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                return new BOMDocumentAttr { ID = attrid, Surname = dr["surname"].ToString() };
            }
            else
                return null;
        }
        public BOMDocumentAttr getRelatedDocAttr(int bomid, int reldocid, int attrid)
        {
            sql = "select id from " + TABLENAME_BOMRelatedDoc + " where bomid=" + bomid + " and relateddocid=" + reldocid + " and isdeleted='false' ";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string bomdocid = dt.Rows[0]["id"].ToString();
                sql = "select * from " + TABLENAME_BOMRelatedDocAttr + " where bomdocid=" + bomdocid + " and isdeleted='false' and docattrid=" + attrid;
                dt = BaseClass.mydataset(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    return new BOMDocumentAttr { ID = attrid, Surname = dr["surname"].ToString() };
                }
                else
                    return null;
            }
            else
                return null;
        }
        public int updateBOM_RelatedDoc(int bomid, int olddocid, int newdocid)
        {
            sql = "select id from " + TABLENAME_BOMRelatedDoc + " where bomid=" + bomid + " and relateddocid=" + olddocid + " and isdeleted='false'";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string id = dt.Rows[0]["id"].ToString();
                sql = "select id from " + TABLENAME_BOMRelatedDoc + " where bomid=" + bomid + " and relateddocid=" + newdocid + " and id<>" + id + " and isdeleted='false'";
                dt = BaseClass.mydataset(sql).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    sql = "update " + TABLENAME_BOMRelatedDoc + " set relateddocid=" + newdocid + " where id=" + id;
                    bc.RunSqlTransaction(sql);
                    return int.Parse(id);
                }
                else
                    return 0;
            }
            else
                return 0;
        }
        public List<BOM> list(int pageindex, string bomname, string istemp)
        {
            int pagesize = Convert.ToInt32(MyExtension.getAppValue("DocPageCount"));
            int skipnum = pageindex * pagesize;
            sql = "select top " + pagesize + " id from " + TABLENAME_BOM + " where id not in (select top " + skipnum + " id from " + TABLENAME_BOM + " where isdeleted='false' " + (string.IsNullOrEmpty(bomname) ? "" : "and lower(name) like '%" + bomname.ToLower() + "%'") + (istemp == "2" ? "" : " and istemp='" + (istemp == "1" ? "true" : "false") + "'") + " order by name asc) and isdeleted='false' " + (string.IsNullOrEmpty(bomname) ? "" : " and lower(name) like '%" + bomname.ToLower() + "%'") + (istemp == "2" ? "" : " and istemp='" + (istemp == "1" ? "true" : "false") + "'") + " order by name asc";
            List<BOM> rs = new List<BOM>();
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    bom = new BOM();
                    bom.ID = int.Parse(dr["id"].ToString());
                    rs.Add(getBOM());
                }
            }
            return rs;
        }
        SysCommon sc = new SysCommon();
        DocService docserv = new DocService();
        public int getTotal(string istemp)
        {
            return sc.GetRecordCount(TABLENAME_BOM, " isdeleted='false'" + (istemp == "2" ? "" : " and istemp='" + (istemp == "1" ? "true" : "false") + "'"));
        }


        #region 显示清单实例数据
        //返回清单数据数量
        public int getBOM_Total(int bomid, List<DocumentAttr> attrs)
        {
            string ids = getBOMMainInstanceIDs(bomid, attrs);
            if (ids != "")
            {
                string[] idArray = ids.Split(',');
                return idArray.Length;
            }
            else
                return 0;
        }
        //返回指定页数和查询条件的清单数据
        public string getPagedIds(int pageIndex,int bomid)
        {
            int pageSize = int.Parse(MyExtension.getAppValue("DocPageCount"));
            sql = "select top "+pageSize+" id from docinstance where isdeleted='false' and docid in (select maindocid from "+TABLENAME_BOM+" where id="+bomid+") and id not in (select top "+pageIndex*pageSize+" id from docinstance where isdeleted='false' and docid in (select maindocid from "+TABLENAME_BOM+" where id="+bomid+") order by id asc) order by id asc";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            string ids = "";
            foreach (DataRow dr in dt.Rows)
            {
                ids += dr["id"].ToString() + ",";
            }
            if (!string.IsNullOrEmpty(ids))
                ids = ids.Remove(ids.Length - 1);
            return ids;
        }
        public List<Model.Data.BOMInstance> getBOMInstance(int pageIndex, int bomid, List<DocumentAttr> attrs)
        {
            int pageSize = int.Parse(MyExtension.getAppValue("DocPageCount"));
            int skipNum = pageSize * pageIndex;
            List<Model.Data.BOMInstance> rs = new List<Model.Data.BOMInstance>();
            //string ids = getBOMMainInstanceIDs(bomid, attrs);
            string ids = getPagedIds(pageIndex, bomid);
            if (ids != "")
            {
                string[] idArray = ids.Split(',');
                BOMDocument main_doc = getMainDocument(bomid);
                List<BOMDocument> related_docs = getRelatedDocument(bomid);
                foreach (string id in idArray)
                {
                    rs.Add(getBOMInatanceByMainInstanceId(int.Parse(id), main_doc, related_docs));
                }
                /*if (idArray.Length <= skipNum)
                {
                    return rs;
                }
                else
                {
                    int startIndex = skipNum;
                    int endIndex = ((idArray.Length > (skipNum + pageSize) ? (startIndex + 9) : (idArray.Length - 1)));
                    BOMDocument main_doc = getMainDocument(bomid);
                    List<BOMDocument> related_docs = getRelatedDocument(bomid);
                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        rs.Add(getBOMInatanceByMainInstanceId(int.Parse(idArray[i]), main_doc, related_docs));
                    }
                }*/
            }
            return rs;
        }

        //返回指定主实例以及其相关实例的数据
        public Model.Data.BOMInstance getBOMInatanceByMainInstanceId(int id, BOMDocument main_doc, List<BOMDocument> related_docs)
        {
            Model.Data.BOMInstance r = new Model.Data.BOMInstance();
            var main_doc_instance = docserv.getDocInstanceById(id);
            r.MainDoc = new BasicDoc { ID = main_doc_instance.Document.ID, DocName = main_doc_instance.Document.DocName };
            //var main_doc_data = new Model.Data.BOMDataItem();
            //main_doc_data.DocInstanceID = id;
            //main_doc_data.RelatedDocAttr = new List<DocumentAttr>();
            //foreach (var main_doc_attr in main_doc.RelatedDocAttrs)
            //{
            //    main_doc_data.RelatedDocAttr.Add(new DocumentAttr { ID = main_doc_attr.ID, Value = main_doc_instance.Document.Attrs.First(p1 => p1.ID == main_doc_attr.ID).TranValue });
            //}
            r.MainDocData = getBOMDataFromInstanceData(main_doc_instance, main_doc);

            //set related docinstance attrs
            r.RelatedDocDatas = new List<Model.Data.BOMRelatedDataItem>();
            foreach (var related_doc in related_docs)
            {
                string ids = getRelatedInstanceIds(main_doc_instance, related_doc.ID.ToString());
                if (ids != "")
                {
                    Model.Data.BOMRelatedDataItem related_instance_data = new Model.Data.BOMRelatedDataItem();
                    related_instance_data.RelatedDoc = new BasicDoc { ID = related_doc.ID, DocName = related_doc.DocName };
                    related_instance_data.RelatedDocInstances = new List<Model.Data.BOMDataItem>();
                    string[] idArray = ids.Split(',');
                    foreach (string id_related_instance in idArray)
                    {
                        var related_instance = docserv.getDocInstanceById(int.Parse(id_related_instance));
                        related_instance_data.RelatedDocInstances.Add(getBOMDataFromInstanceData(related_instance, related_doc));
                    }
                    r.RelatedDocDatas.Add(related_instance_data);
                }
            }
            return r;
        }
        //根据实例原始数据，显示清单数据
        public Model.Data.BOMDataItem getBOMDataFromInstanceData(DocumentInstance instance, BOMDocument bomdoc)
        {
            Model.Data.BOMDataItem r = new Model.Data.BOMDataItem();
            r.DocInstanceID = instance.ID;
            r.RelatedDocAttr = new List<DocumentAttr>();
            foreach (var attr in bomdoc.RelatedDocAttrs)
            {
                var val = "ValueNA";
                var attr_val = instance.Document.Attrs.Where(p1 => p1.ID == attr.ID);
                if (attr_val.Count() > 0)
                    val = attr_val.First().TranValue;
                r.RelatedDocAttr.Add(new DocumentAttr { ID = attr.ID, TranValue = val });
            }
            return r;
        }
        //得到主文档及属性
        public BOMDocument getMainDocument(int bomid)
        {
            BOMDocument r = new BOMDocument();
            sql = "select maindocid from " + TABLENAME_BOM + " where id=" + bomid;
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            r.ID = int.Parse(dt.Rows[0]["maindocid"].ToString());
            r.DocName = dcs.getBasicDocById(r.ID).DocName;
            r.RelatedDocAttrs = new List<BOMDocumentAttr>();
            sql = "select * from " + TABLENAME_BOMMainDocAttr + " where bomid=" + bomid + " and isdeleted='false'";
            dt = BaseClass.mydataset(sql).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                //var attr = dcs.getAttrById(int.Parse(dr["maindocattrid"].ToString()));
                r.RelatedDocAttrs.Add(new BOMDocumentAttr { ID = int.Parse(dr["maindocattrid"].ToString()), Surname = dr["surname"].ToString() });
            }
            return r;
        }
        //得到相关文档及属性
        public List<BOMDocument> getRelatedDocument(int bomid)
        {
            List<BOMDocument> rs = new List<BOMDocument>();
            sql = "select * from " + TABLENAME_BOMRelatedDoc + " where bomid=" + bomid + " and isdeleted='false'";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                BOMDocument r = new BOMDocument();
                r.ID = int.Parse(dr["relateddocid"].ToString());
                r.DocName = dcs.getBasicDocById(r.ID).DocName;
                r.RelatedDocAttrs = new List<BOMDocumentAttr>();
                sql = "select * from " + TABLENAME_BOMRelatedDocAttr + " where bomdocid=" + dr["id"].ToString() + " and isdeleted='false'";
                DataTable dt2 = BaseClass.mydataset(sql).Tables[0];
                foreach (DataRow dr2 in dt2.Rows)
                {
                    r.RelatedDocAttrs.Add(new BOMDocumentAttr { ID = int.Parse(dr2["docattrid"].ToString()), Surname = dr2["surname"].ToString() });
                }
                rs.Add(r);
            }
            return rs;
        }
        //得到（满足条件）的主实例的所有id集合
        public string getBOMMainInstanceIDs(int bomid, List<DocumentAttr> mainInstanceAttrs)
        {
            string ids = "";
            sql = "select maindocid from " + TABLENAME_BOM + " where id=" + bomid;
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            string maindocid = dt.Rows[0]["maindocid"].ToString();
            string relatedDocids = "";
            sql = "select relateddocid from " + TABLENAME_BOMRelatedDoc + " where isdeleted='false' and bomid=" + bomid;
            dt = BaseClass.mydataset(sql).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                relatedDocids += dr["relateddocid"].ToString() + ",";
            }
            if (relatedDocids != "")
                relatedDocids = relatedDocids.Remove(relatedDocids.Length - 1);

            string[] relatedDocIdArray = relatedDocids.Split(',');
            //检查主实例的属性查询条件
            string conditionalIDs = "";
            if (mainInstanceAttrs.Count > 0)
            {
                //有查询条件
                conditionalIDs = getConditionalInstanceIds(mainInstanceAttrs);
                sql = "select id from docinstance where docid=" + maindocid + " and isdeleted='false' and id in (" + conditionalIDs + ") order by id asc";
            }
            else
            {
                //无查询条件
                sql = "select id from docinstance where docid=" + maindocid + " and isdeleted='false' order by id asc";
            }
            dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var main_doc_instance = docserv.getDocInstanceById(int.Parse(dr["id"].ToString()));
                    var has_related_instance = false;
                    foreach (string related_doc_id in relatedDocIdArray)
                    {
                        if (has_related_instance)
                            break;

                        string related_doc_instance_ids = getRelatedInstanceIds(main_doc_instance, related_doc_id);
                        if (related_doc_instance_ids != "")
                            has_related_instance = true;
                    }
                    if (has_related_instance)
                        ids += dr["id"].ToString() + ",";
                }
            }
            if (ids != "")
                ids = ids.Remove(ids.Length - 1);

            return ids;
        }
        //得到符合查询条件的实例id集合
        public string getConditionalInstanceIds(List<DocumentAttr> attrs)
        {
            string ids = "";
            foreach (var attr in attrs)
            {
                sql = "select docinstanceid from docinstance" + docserv.getAttrValueTableName(attr) + " where docattrid=" + attr.ID + " and lower(value)='" + attr.Value.ToLower() + "' and isdeleted='false' " + (ids == "" ? "" : " and docinstanceid in (" + ids + ")") + " order by docinstanceid asc";
                DataTable dt = BaseClass.mydataset(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ids += dr["docinstanceid"].ToString() + ",";
                    }
                    if (ids != "")
                        ids = ids.Remove(ids.Length - 1);
                }
            }
            return ids;
        }
        //得到与主实例相关的某个文档实例的id集合
        public string getRelatedInstanceIds(Model.DocumentInstance mainInstance, string relatedDocID)
        {
            string ids = "";
            sql = "select * from DocAttrRelate_Tab where sdocid=" + mainInstance.Document.ID + " and rdocid=" + relatedDocID;
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //根据关联属性,匹配实例
                    var relatedAttr = dcs.getAttrById(int.Parse(dr["rattrid"].ToString()));
                    sql = "select distinct docinstanceid from docinstance" + docserv.getAttrValueTableName(relatedAttr) + " where docinstanceid in (select id from docinstance where docid=" + relatedDocID + " and isdeleted='false') and value='" + mainInstance.Document.Attrs.First(p1 => p1.ID == int.Parse(dr["sattrid"].ToString())).Value + "' and docattrid=" + relatedAttr.ID;
                    DataTable dt2 = BaseClass.mydataset(sql).Tables[0];
                    if (dt2.Rows.Count > 0)
                    {
                        foreach (DataRow dr2 in dt2.Rows)
                        {
                            ids += dr2["docinstanceid"].ToString() + ",";
                        }
                        break;
                    }
                }
            }
            if (ids != "")
                ids = ids.Remove(ids.Length - 1);
            return ids;
        }
        #endregion

        #region export data into xml file
        public string getMainDocInstanceIDs(string docid)
        {
            string ids = "";
            sql = "select id from DocInstance where docid="+docid+" and isdeleted='false' ";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            foreach (DataRow dr in dt.Rows)
                ids += dr["id"].ToString() + ",";
            if (!string.IsNullOrEmpty(ids))
                ids = ids.Remove(ids.Length - 1);
            return ids;
        }

        public string exportDataIntoXML(BasicBOM basicbom, string folderpath)
        {
            //string ids = getBOMMainInstanceIDs(basicbom.ID, new List<DocumentAttr>() { });
            string ids = getMainDocInstanceIDs(basicbom.MainDocId);
            int pageSize = int.Parse(MyExtension.getAppValue("DocPageCount"));
            string str = Guid.NewGuid().ToString();
            string fileName = "bom_" + basicbom.ID + "_" + str + ".xml";
            string savedFilePath = folderpath + fileName;
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode docNode = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(docNode);

            XmlElement root = xmlDoc.CreateElement("Datas");
            xmlDoc.AppendChild(root);

            XmlElement generatetime = xmlDoc.CreateElement("GenerateTime");
            generatetime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            root.AppendChild(generatetime);

            XmlElement bomele = xmlDoc.CreateElement("BOM");
            bomele.SetAttribute("ID", basicbom.ID.ToString());
            bomele.SetAttribute("Name", basicbom.Name);
            root.AppendChild(bomele);

            XmlElement totalnum = xmlDoc.CreateElement("TotalItem");

            if (ids != "")
            {
                string[] idArray = ids.Split(',');
                totalnum.InnerText = idArray.Length.ToString();
                root.AppendChild(totalnum);

                int totalPageNum = idArray.Length / pageSize;
                for (int i = 0; i <= totalPageNum; i++)
                {
                    XmlElement data = xmlDoc.CreateElement("Data");
                    data.SetAttribute("Page", i.ToString());
                    root.AppendChild(data);

                    var instance_items = getBOMInstance(i, basicbom.ID, new List<DocumentAttr>() { });
                    //var instance_items=

                    foreach (var bom_instance_item in instance_items)
                    {

                        XmlElement item = xmlDoc.CreateElement("Item");
                        data.AppendChild(item);

                        XmlElement maindoc_ele = xmlDoc.CreateElement("MainDoc");
                        item.AppendChild(maindoc_ele);

                        maindoc_ele.SetAttribute("DocID", bom_instance_item.MainDoc.ID.ToString());
                        maindoc_ele.SetAttribute("InstanceID", bom_instance_item.MainDocData.DocInstanceID.ToString());

                        XmlElement main_attrs = xmlDoc.CreateElement("AttrValues");
                        maindoc_ele.AppendChild(main_attrs);

                        foreach (var attr in bom_instance_item.MainDocData.RelatedDocAttr)
                        {
                            XmlElement attr_ele = xmlDoc.CreateElement("AttrItem");
                            main_attrs.AppendChild(attr_ele);

                            attr_ele.SetAttribute("ID", attr.ID.ToString());
                            attr_ele.SetAttribute("Value", attr.TranValue);
                        }

                        XmlElement relateddoc_ele = xmlDoc.CreateElement("RelatedDocs");
                        item.AppendChild(relateddoc_ele);

                        foreach (var related_instance in bom_instance_item.RelatedDocDatas)
                        {
                            XmlElement RelatedDocItem_ele = xmlDoc.CreateElement("RelatedDocItem");
                            relateddoc_ele.AppendChild(RelatedDocItem_ele);
                            RelatedDocItem_ele.SetAttribute("DocID", related_instance.RelatedDoc.ID.ToString());

                            XmlElement InstanceDatas_ele = xmlDoc.CreateElement("InstanceDatas");
                            RelatedDocItem_ele.AppendChild(InstanceDatas_ele);

                            foreach (var related_instance_item in related_instance.RelatedDocInstances)
                            {
                                XmlElement InstanceItem_ele = xmlDoc.CreateElement("InstanceItem");
                                InstanceDatas_ele.AppendChild(InstanceItem_ele);
                                InstanceItem_ele.SetAttribute("InstanceID", related_instance_item.DocInstanceID.ToString());

                                XmlElement AttrValues_ele = xmlDoc.CreateElement("AttrValues");
                                InstanceItem_ele.AppendChild(AttrValues_ele);

                                foreach (var attr_item in related_instance_item.RelatedDocAttr)
                                {
                                    XmlElement rel_attr_item = xmlDoc.CreateElement("AttrItem");
                                    AttrValues_ele.AppendChild(rel_attr_item);
                                    rel_attr_item.SetAttribute("ID", attr_item.ID.ToString());
                                    rel_attr_item.SetAttribute("Value", attr_item.TranValue);
                                }
                            }
                        }

                    }
                }
            }
            else
            {
                totalnum.InnerText = "0";
                root.AppendChild(totalnum);
            }
            sql = "update " + TABLENAME_BOM + " set filepath='" + fileName + "' where id=" + basicbom.ID;
            bc.RunSqlTransaction(sql);
            xmlDoc.Save(savedFilePath);
            return str;
        }
        public string getFilePath(int id)
        {
            DataTable dt = BaseClass.mydataset("select filepath from " + TABLENAME_BOM + " where id=" + id).Tables[0];
            return dt.Rows[0]["filepath"].ToString();
        }

        public List<Model.Data.BOMInstance> getInstancesFromXML(int bomid, int pageindex, string folderpath)
        {
            List<Model.Data.BOMInstance> rs = new List<Model.Data.BOMInstance>();
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(folderpath + getFilePath(bomid));
            XmlNode nodes = xmlDoc.SelectSingleNode("/Datas/Data[@Page=" + pageindex + "]");
            if (!object.Equals(nodes, null))
            {
                foreach (XmlNode node in nodes.ChildNodes)
                {
                    Model.Data.BOMInstance r = new Model.Data.BOMInstance();

                    XmlNode main_node = node.ChildNodes[0];
                    r.MainDoc = new BasicDoc { ID = int.Parse(main_node.Attributes["DocID"].InnerText) };
                    r.MainDocData = new Model.Data.BOMDataItem();
                    r.MainDocData.DocInstanceID = int.Parse(main_node.Attributes["InstanceID"].InnerText);
                    r.MainDocData.RelatedDocAttr = new List<DocumentAttr>();
                    foreach (XmlNode main_attr_node in main_node.ChildNodes[0].ChildNodes)
                    {
                        r.MainDocData.RelatedDocAttr.Add(new DocumentAttr { ID = int.Parse(main_attr_node.Attributes["ID"].InnerText), TranValue = main_attr_node.Attributes["Value"].InnerText });
                    }

                    XmlNode relate_node = node.ChildNodes[1];
                    r.RelatedDocDatas = new List<Model.Data.BOMRelatedDataItem>();
                    foreach (XmlNode doc_node in relate_node.ChildNodes)
                    {
                        Model.Data.BOMRelatedDataItem relate_data = new Model.Data.BOMRelatedDataItem();
                        relate_data.RelatedDoc = new BasicDoc { ID = int.Parse(doc_node.Attributes["DocID"].InnerText) };

                        relate_data.RelatedDocInstances = new List<Model.Data.BOMDataItem>();

                        foreach (XmlNode instance_node in doc_node.ChildNodes[0].ChildNodes)
                        {
                            Model.Data.BOMDataItem rel_instance_data = new Model.Data.BOMDataItem();
                            rel_instance_data.DocInstanceID = int.Parse(instance_node.Attributes["InstanceID"].InnerText);
                            rel_instance_data.RelatedDocAttr = new List<DocumentAttr>();
                            foreach (XmlNode attr_node in instance_node.ChildNodes[0].ChildNodes)
                            {
                                rel_instance_data.RelatedDocAttr.Add(new DocumentAttr { ID = int.Parse(attr_node.Attributes["ID"].InnerText), TranValue = attr_node.Attributes["Value"].InnerText });
                            }

                            relate_data.RelatedDocInstances.Add(rel_instance_data);
                        }
                        r.RelatedDocDatas.Add(relate_data);
                    }

                    rs.Add(r);
                }
            }
            return rs;
        }
        public string getTotalFromXML(int bomid, string folderpath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(folderpath + getFilePath(bomid));
            XmlNode node = xmlDoc.SelectSingleNode("/Datas/TotalItem");
            return node.InnerText + "_" + xmlDoc.SelectSingleNode("/Datas/GenerateTime").InnerText;
        }
        #endregion

        #region export data into excel file
        public void exportDataIntoExcel(int bomid, string folderpath, string filename)
        {
            bom.ID = bomid;
            bom = getBOM();
            Microsoft.Office.Interop.Excel._Application excel = null;
            Microsoft.Office.Interop.Excel._Workbook wb = null;
            Microsoft.Office.Interop.Excel._Worksheet ws = null;
            Microsoft.Office.Interop.Excel.Range range = null;

            excel = new Microsoft.Office.Interop.Excel.Application();
            excel.DisplayAlerts = false;
            excel.Visible = false;
            excel.SheetsInNewWorkbook = 1;

            wb = excel.Workbooks.Add();
            ws = wb.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
            ws.Name = bom.Name;

            //create header of doc
            (ws.Cells[1, 1] as Microsoft.Office.Interop.Excel.Range).Value2 = "序号";

            (ws.get_Range((object)ws.Cells[1, 1], (object)ws.Cells[2, 1]) as Microsoft.Office.Interop.Excel.Range).Merge(Type.Missing);
            (ws.get_Range((object)ws.Cells[1, 1], (object)ws.Cells[2, 1]) as Microsoft.Office.Interop.Excel.Range).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
            (ws.get_Range((object)ws.Cells[1, 1], (object)ws.Cells[2, 1]) as Microsoft.Office.Interop.Excel.Range).Font.Bold = true;
            (ws.Cells[1, 2] as Microsoft.Office.Interop.Excel.Range).Value2 = bom.MainDoc.DocName;

            if (bom.MainDoc.RelatedDocAttrs.Count > 1)
            {
                (ws.get_Range((object)ws.Cells[1, 2], (object)ws.Cells[1, 1 + bom.MainDoc.RelatedDocAttrs.Count]) as Microsoft.Office.Interop.Excel.Range).Merge(Type.Missing);
                (ws.get_Range((object)ws.Cells[1, 2], (object)ws.Cells[1, 1 + bom.MainDoc.RelatedDocAttrs.Count]) as Microsoft.Office.Interop.Excel.Range).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                (ws.get_Range((object)ws.Cells[1, 2], (object)ws.Cells[1, 1 + bom.MainDoc.RelatedDocAttrs.Count]) as Microsoft.Office.Interop.Excel.Range).Font.Bold = true;
            }
            int columnIndex = bom.MainDoc.RelatedDocAttrs.Count + 2;
            foreach (var rel_doc in bom.RelatedDocs)
            {
                (ws.Cells[1, columnIndex] as Microsoft.Office.Interop.Excel.Range).Value2 = rel_doc.DocName;
                if (rel_doc.RelatedDocAttrs.Count > 1)
                {
                    (ws.get_Range((object)ws.Cells[1, columnIndex], (object)ws.Cells[1, columnIndex + rel_doc.RelatedDocAttrs.Count - 1]) as Microsoft.Office.Interop.Excel.Range).Merge(Type.Missing);
                }
                columnIndex += rel_doc.RelatedDocAttrs.Count;
            }
            //header of attrs
            int docIndex = 0;
            int[] totalDocs = new int[bom.RelatedDocs.Count + 1];
            int attrIndex = 0;
            int[] totalAttrs = new int[columnIndex - 2];

            totalDocs[docIndex] = bom.MainDoc.ID;
            docIndex++;

            foreach (var attr in bom.MainDoc.RelatedDocAttrs)
            {
                (ws.Cells[2, attrIndex + 2] as Microsoft.Office.Interop.Excel.Range).Value2 = attr.Surname;
                totalAttrs[attrIndex] = attr.ID;
                attrIndex++;
            }
            foreach (var rel_doc in bom.RelatedDocs)
            {
                totalDocs[docIndex] = rel_doc.ID;
                docIndex++;

                foreach (var attr in rel_doc.RelatedDocAttrs)
                {
                    (ws.Cells[2, attrIndex + 2] as Microsoft.Office.Interop.Excel.Range).Value2 = attr.Surname;
                    totalAttrs[attrIndex] = attr.ID;
                    attrIndex++;
                }
            }

            //string ids = getBOMMainInstanceIDs(bomid, new List<DocumentAttr>() { });
            //string ids = getMainDocInstanceIDs(bom.MainDoc.ID.ToString());
            string ids = getMainDocInstanceIDs(bom.MainDoc.ID.ToString());
            int pageSize = int.Parse(MyExtension.getAppValue("DocPageCount"));
            int rowIndex = 3;
            if (ids != "")
            {
                string[] idArray = ids.Split(',');
                int totalPageNum = idArray.Length / pageSize;
                for (int i = 0; i <= totalPageNum; i++)
                {
                    var instance_items = getBOMInstance(i, bomid, new List<DocumentAttr>() { });

                    foreach (var bom_instance_item in instance_items)
                    {
                        //main doc attrs
                        (ws.Cells[rowIndex, 1] as Microsoft.Office.Interop.Excel.Range).Value2 = (rowIndex - 2).ToString();
                        foreach (var attr in bom_instance_item.MainDocData.RelatedDocAttr)
                        {
                            for (int attr_index = 0; attr_index < totalAttrs.Length; attr_index++)
                            {
                                if (attr.ID == totalAttrs[attr_index])
                                {
                                    (ws.Cells[rowIndex, attr_index + 2] as Microsoft.Office.Interop.Excel.Range).Value2 = attr.TranValue;
                                    break;
                                }
                            }
                        }
                        foreach (var rel_doc in bom_instance_item.RelatedDocDatas)
                        {
                            for (int doc_index = 0; doc_index < totalDocs.Length; doc_index++)
                            {
                                if (totalDocs[doc_index] == rel_doc.RelatedDoc.ID)
                                {
                                    for (int attr_index = 0; attr_index < totalAttrs.Length; attr_index++)
                                    {
                                        if (rel_doc.RelatedDocInstances[0].RelatedDocAttr.Where(p1 => p1.ID == totalAttrs[attr_index]).Count() > 0)
                                        {
                                            string str = "";
                                            foreach (var real_instance in rel_doc.RelatedDocInstances)
                                            {
                                                foreach (var real_attr in real_instance.RelatedDocAttr)
                                                {
                                                    if (real_attr.ID == totalAttrs[attr_index])
                                                    {
                                                        str += real_attr.TranValue + ";";
                                                    }
                                                }
                                            }
                                            (ws.Cells[rowIndex, attr_index + 2] as Microsoft.Office.Interop.Excel.Range).Value2 = str;

                                        }
                                    }
                                    break;
                                }
                            }

                        }
                        rowIndex++;
                    }
                }
            }
            string totalFileName = folderpath + @"excel\bom_" + bomid + "_" + filename + ".xlsx";
            wb.SaveAs(totalFileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            wb.Close(false, Type.Missing, Type.Missing);
            excel.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            range = null;
            ws = null;
            wb = null;
            excel = null;
            GC.Collect();

        }
        #endregion
    }
}
