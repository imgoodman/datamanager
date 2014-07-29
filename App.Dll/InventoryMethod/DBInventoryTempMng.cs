using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Web;
using App.Model;

namespace App.Dll.InventoryMethod
{
    public class DBInventoryTempMng
    {
        BaseClass bc = new BaseClass();
        SysCommon SC = new SysCommon();
        private string TabelName = "InventoryBasic_Temp_Tab";
        private string InventoryDocTab = "InventoryDocs_Temp_Tab";

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="a">待添加对象</param>
        /// <returns>新增成功，返回新增的ID；否则返回0</returns>
        public int Add(Inventory a)
        {
            int id = 0;
            string sql = "insert into " + TabelName + " (TypeID,Name,Description,IsExpired,CreatorID,CreateTime,LastModifierID,LastModifyTime) values ('" + a.Type.ID + "','" + a.Name + "','" + a.Description + "','0','" + a.Creator.ID + "','" + DateTime.Now + "','" + a.LastModifier.ID + "','" + DateTime.Now + "')";
            bc.RunSqlTransaction(sql);
            string sqlselect = "select top 1 ID from " + TabelName + " where Name = '" + a.Name + "' and IsExpired<>'1' ";
            id = bc.intScalar(sqlselect);
            return id;
        }

        /// <summary>
        /// 添加时候，验证是否已经存在
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public bool IsExistWhenAdd(Inventory a)
        {
            string sql = "select  count(0) from " + TabelName + " where (Name='" + a.Name + "') and IsExpired<>'1'";
            int count = bc.intScalar(sql);
            if (count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 添加清单对象的文档
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public bool AddInventoryDocs(Inventory a)
        {
            string sql = null;
            if (a.Docs != null)
            {
                //先删除数据库存在但未勾选的文档
                string selectedids = null;
                foreach (Document doc in a.Docs)
                {
                    selectedids += doc.ID + ",";
                }
                selectedids = selectedids.Remove(selectedids.Length - 1);
                sql = "update " + InventoryDocTab + " set IsExpired='1' where IsExpired='0' and InventoryID='" + a.ID + "' and DocID not in (" + selectedids + ")";
                if (bc.RunSqlTransaction(sql) == 0)
                {
                    foreach (Document doc in a.Docs)
                    {
                        sql = "select id from " + InventoryDocTab + " where IsExpired='0'  and InventoryID='" + a.ID + "' and DocID='" + doc.ID + "'";
                        if (string.IsNullOrEmpty(bc.ecScalar(sql)))//不存在该记录
                        {
                            sql = "insert into " + InventoryDocTab + " (InventoryID,DocID,IsExpired) values ('" + a.ID + "','" + doc.ID + "','0')";
                            if (bc.RunSqlTransaction(sql) != 0)
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }
                else
                    return false;
            }
            else
            {
                sql = "update " + InventoryDocTab + " set IsExpired='1' where InventoryID='" + a.ID + "'";
                if (bc.RunSqlTransaction(sql) == 0)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// 添加清单对象的文档及其属性
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        //public bool AddInventoryDocsandAttrs(Inventory a)
        //{
        //    //先删除该清单已有的文档
        //    string sql = "update InventoryDocs_Temp_Tab set IsExpired='1' where InventoryID='" + a.ID + "'";
        //    if (bc.RunSqlTransaction(sql) == 0)
        //    {
        //        if (a.Docs != null)
        //        {
        //            foreach (Document doc in a.Docs)
        //            {
        //                string attrIDs = null;
        //                if (doc.Attrs != null)
        //                {
        //                    foreach (var attr in doc.Attrs)
        //                    {
        //                        attrIDs += attr.ID + ",";
        //                    }
        //                }
        //                if (attrIDs != null)
        //                {
        //                    attrIDs = attrIDs.Remove(attrIDs.Length - 1);
        //                }
        //                sql = "insert into InventoryDocs_Temp_Tab (InventoryID,DocID,AttrIDs,IsExpired) values ('" + a.ID + "','" + doc.ID + "','" + attrIDs + "','0')";
        //                if (bc.RunSqlTransaction(sql) != 0)
        //                {
        //                    return false;
        //                }
        //            }
        //        }
        //        return true;
        //    }
        //    else
        //        return false;
        //}

        /// <summary>
        /// 添加清单对象的文档的属性
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public bool AddInventoryDocAttrs(Inventory a)
        {
            string sql = null;
            if (a.Docs != null)
            {
                foreach (var d in a.Docs)
                {
                    string attrIDs = null;
                    if (d.Attrs != null)
                    {
                        foreach (var attr in d.Attrs)
                        {
                            attrIDs += attr.ID + ",";
                        }
                    }
                    if (attrIDs != null)
                    {
                        attrIDs = attrIDs.Remove(attrIDs.Length - 1);
                    }
                    sql = "update " + InventoryDocTab + " set AttrIDs='" + attrIDs + "' where IsExpired='0' and InventoryID='" + a.ID + "' and DocID='" + d.ID + "'";
                    if (bc.RunSqlTransaction(sql) != 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        ///  更新对象
        /// </summary>
        /// <param name="u">待更新对象</param>
        /// <returns>更新成功，返回true；否则返回false</returns>
        public bool Update(Inventory u)
        {
            string sql = "update " + TabelName + " set TypeID='" + u.Type.ID + "',Name='" + u.Name + "',Description='" + u.Description + "',LastModifierID='" + u.LastModifier.ID + "' ,LastModifyTime='" + DateTime.Now + "' where ID='" + u.ID + "'";
            if (bc.RunSqlTransaction(sql) == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 修改时候，验证是否已经存在
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public bool IsExistWhenUpdate(Inventory u)
        {
            string sql = "select * from " + TabelName + " where (Name='" + u.Name + "') and IsExpired<>'1' and id<>'" + u.ID + "'";
            int count = bc.intScalar(sql);
            if (count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(Inventory d)
        {
            string sql = "update " + TabelName + " set IsExpired='1',LastModifierID='" + d.LastModifier.ID + "' ,LastModifyTime='" + DateTime.Now + "' where id='" + d.ID + "'";
            if (bc.RunSqlTransaction(sql) == 0)
                return true;
            else
                return false;
        }

        public bool DeleteByID(int id, int LastModifierID)
        {
            string sql = "update " + TabelName + " set IsExpired='1',LastModifierID='" + LastModifierID + "' ,LastModifyTime='" + DateTime.Now + "' where id='" + id + "'";
            if (bc.RunSqlTransaction(sql) == 0)
                return true;
            else
                return false;
        }

        public void DeleteDt(string dataIDAll)
        {
            string[] ppp = dataIDAll.Split(',');
            string sql = "";
            for (int i = 0; i < ppp.Length; i++)
            {
                int ID = ppp[i].ToInt();
                sql = " update " + TabelName + " set IsExpired='1',LastModifyTime='" + DateTime.Now + "' where ID='" + ID + "'";
                bc.RunSqlTransaction(sql);
            }
        }

        /// <summary>
        /// 返回对应id的对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Inventory GetInventoryByID(int id)
        {
            string sql = "select * from " + TabelName + " where id='" + id + "' and IsExpired='0'";
            DataSet ds = BaseClass.mydataset(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                Inventory d = new Inventory();
                d.ID = id;
                d.Type = new InventoryType();
                d.Type.ID = dr["TypeID"].ToString().ToInt();
                d.Name = dr["Name"].ToString();
                d.Description = dr["Description"].ToString();
                d.Creator = new User();
                d.Creator.ID = dr["CreatorID"].ToString().ToInt();
                d.CreateTime = DateTime.Parse(dr["CreateTime"].ToString());
                d.LastModifier = new User();
                d.LastModifier.ID = dr["LastModifierID"].ToString().ToInt();
                d.LastModifyTime = DateTime.Parse(dr["LastModifyTime"].ToString());
                d.IsExpired = dr["IsExpired"].ToBool();
                d.Docs = new List<Document>();
                d.Docs = GetInventroyDocsByID(id);
                return d;
            }
            else
                return null;
        }

        public List<Document> GetInventroyDocsByID(int id)
        {
            List<Document> dlist = new List<Document>();
            string sql = "select * from " + InventoryDocTab + " where InventoryID='" + id + "' and IsExpired='0'";
            DataSet ds = BaseClass.mydataset(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Document d = new Document();
                    d.ID = dr["DocID"].ToString().ToInt();
                    d.Attrs = new List<DocumentAttr>();
                    string attrIDs = dr["AttrIDs"].ToString();
                    if (!string.IsNullOrEmpty(attrIDs))
                    {
                        foreach (var i in attrIDs.Split(','))
                        {
                            DocumentAttr da = new DocumentAttr();
                            da.ID = i.ToInt();
                            d.Attrs.Add(da);
                        }
                    }
                    dlist.Add(d);
                }
                return dlist;
            }
            else
                return null;
        }

        public List<DocumentAttr> GetInventoryDocAttrsByDocID(int id, int docid)
        {
            List<DocumentAttr> datalist = new List<DocumentAttr>();
            string sql = "select * from " + InventoryDocTab + " where InventoryID='" + id + "' and docid='" + docid + "' and IsExpired='0'";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                string attrIDs = dr["AttrIDs"].ToString();
                if (!string.IsNullOrEmpty(attrIDs))
                {
                    foreach (var i in attrIDs.Split(','))
                    {
                        DocumentAttr da = new DocumentAttr();
                        da.ID = i.ToInt();
                        datalist.Add(da);
                    }
                }
                return datalist;
            }
            else
                return null;
        }

        public Inventory GetInventoryBasicByID(int id)
        {
            string sql = "select * from " + TabelName + " where id='" + id + "' and IsExpired='0'";
            DataSet ds = BaseClass.mydataset(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                Inventory d = new Inventory();
                d.ID = id;
                d.Type = new InventoryType();
                d.Type.ID = dr["TypeID"].ToString().ToInt();
                d.Name = dr["Name"].ToString();
                d.Description = dr["Description"].ToString();
                d.Creator = new User();
                d.Creator.ID = dr["CreatorID"].ToString().ToInt();
                d.CreateTime = DateTime.Parse(dr["CreateTime"].ToString());
                d.LastModifier = new User();
                d.LastModifier.ID = dr["LastModifierID"].ToString().ToInt();
                d.LastModifyTime = DateTime.Parse(dr["LastModifyTime"].ToString());
                d.IsExpired = dr["IsExpired"].ToBool();
                return d;
            }
            else
                return null;
        }

        /// <summary>
        /// 得到查询记录总数，一般用于绑定分页控件
        /// </summary>
        /// <param name="sqlwhere"></param>
        /// <returns></returns>
        public int GetRecordCount(string sqlWhere)
        {
            int recordCount = SC.GetRecordCount(TabelName, sqlWhere);
            return recordCount;
        }

        /// <summary>
        /// 返回分页的对象
        /// </summary>
        /// <param name="sqlwhere"></param>
        /// <param name="orderstr"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Inventory> GetPagedItems(string sqlwhere, string orderstr, int pageIndex, int pageSize)
        {
            List<Inventory> dlist = new List<Inventory>();
            string sql = SC.GetPagedDataStr(TabelName, "ID", sqlwhere, orderstr, pageIndex, pageSize);
            DataSet ds = BaseClass.mydataset(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                    dlist.Add(GetInventoryByID(int.Parse(dr["ID"].ToString())));
                return dlist;
            }
            else
                return null;
        }

        /// <summary>
        /// 返回不分页的对象
        /// </summary>
        /// <param name="sqlwhere"></param>
        /// <param name="orderstr"></param>
        /// <returns></returns>
        public List<Inventory> GetAllItems(string sqlwhere, string orderstr)
        {
            List<Inventory> dlist = new List<Inventory>();
            string sql = SC.GetAllDataStr(TabelName, "ID", sqlwhere, orderstr);
            DataSet ds = BaseClass.mydataset(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                    dlist.Add(GetInventoryByID(int.Parse(dr["ID"].ToString())));
                return dlist;
            }
            else
                return null;
        }

    }
}
