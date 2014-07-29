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
    public class DBInventoryInstanceMng
    {
        BaseClass bc = new BaseClass();
        SysCommon SC = new SysCommon();
        private string TabelName = "InventoryInstance_Tab";
        private string InstanceDocsTab = "InventoryInstanceDocs_Tab";

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="a">待添加对象</param>
        /// <returns>新增成功，返回新增的ID；否则返回0</returns>
        public int Add(InventoryInstance a)
        {
            int id = 0;
            string sql = "insert into " + TabelName + " (InventoryID,IsExpired,CreatorID,CreateTime,LastModifierID,LastModifyTime) values ('" + a.InventoryID + "','0','" + a.Creator.ID + "','" + DateTime.Now + "','" + a.LastModifier.ID + "','" + DateTime.Now + "')";
            bc.RunSqlTransaction(sql);
            string sqlselect = "select top 1 ID from " + TabelName + " where InventoryID = '" + a.InventoryID + "' and IsExpired<>'1' order by CreateTime desc";
            id = bc.intScalar(sqlselect);
            if (id > 0)
            {
                if (AddInstanceDocs(id, a.InstanceDocs))
                {
                    return id;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return 0;
            }
        }

        public bool AddInstanceDocs(int instanceID, List<InventoryInstanceDocs> a)
        {
            if (a != null)
            {
                foreach (var i in a)
                {
                    string sql = "insert into " + InstanceDocsTab + " (InstanceID,DocID,DocInstanceIDs,IsExpired) values ('" + instanceID + "','" + i.DocID + "','" + i.DocInstanceIDs + "','0')";
                    if (bc.RunSqlTransaction(sql) != 0)
                    {
                        return false;
                    }
                }
                return true;
            }
            return true;
        }

        /// <summary>
        ///  更新对象，不能修改清单对象，只修改文档实例
        /// </summary>
        /// <param name="u">待更新对象</param>
        /// <returns>更新成功，返回true；否则返回false</returns>
        public bool Update(InventoryInstance u)
        {
            string sql = "update " + TabelName + " set LastModifierID='" + u.LastModifier.ID + "' ,LastModifyTime='" + DateTime.Now + "' where ID='" + u.ID + "'";
            if (bc.RunSqlTransaction(sql) == 0)
            {
                if (u.InstanceDocs != null)
                {
                    foreach (var i in u.InstanceDocs)
                    {
                        sql = "update " + InstanceDocsTab + " set InstanceID='" + u.ID + "',DocID='" + i.DocID + "',DocInstanceIDs='" + i.DocInstanceIDs + "' where ID='" + u.ID + "'";
                        if (bc.RunSqlTransaction(sql) != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                return true;
            }
            else
                return false;
        }

        public bool UpdateInstanceDocs(InventoryInstanceDocs u)
        {
            string sql = "update " + InstanceDocsTab + " set DocInstanceIDs='" + u.DocInstanceIDs + "' where ID='" + u.ID + "'";
            if (bc.RunSqlTransaction(sql) != 0)
            {
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(InventoryInstance d)
        {
            string sql = "update " + TabelName + " set IsExpired='1',LastModifierID='" + d.LastModifier.ID + "' ,LastModifyTime='" + DateTime.Now + "' where id='" + d.ID + "'";
            if (bc.RunSqlTransaction(sql) == 0)
            {
                sql = "update " + InstanceDocsTab + " set IsExpired='1' where InstanceID='" + d.ID + "'";
                if (bc.RunSqlTransaction(sql) == 0)
                    return true;
            }
            return false;
        }

        public bool DeleteByID(int id, int LastModifierID)
        {
            string sql = "update " + TabelName + " set IsExpired='1',LastModifierID='" + LastModifierID + "' ,LastModifyTime='" + DateTime.Now + "' where id='" + id + "'";
            if (bc.RunSqlTransaction(sql) == 0)
            {
                sql = "update " + InstanceDocsTab + " set IsExpired='1' where InstanceID='" + id + "'";
                if (bc.RunSqlTransaction(sql) == 0)
                    return true;
            }
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
                if (bc.RunSqlTransaction(sql) == 0)
                {
                    sql = "update " + InstanceDocsTab + " set IsExpired='1' where InstanceID='" + ID + "'";
                    bc.RunSqlTransaction(sql);
                }
            }
        }

        /// <summary>
        /// 返回对应id的对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InventoryInstance GetInventoryInstanceByID(int id)
        {
            string sql = "select * from " + TabelName + " where id='" + id + "' and IsExpired='0'";
            DataSet ds = BaseClass.mydataset(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                InventoryInstance d = new InventoryInstance();
                d.ID = id;
                d.InventoryID = dr["InventoryID"].ToString().ToInt();
                d.Creator = new User();
                d.Creator.ID = dr["CreatorID"].ToString().ToInt();
                d.CreateTime = DateTime.Parse(dr["CreateTime"].ToString());
                d.LastModifier = new User();
                d.LastModifier.ID = dr["LastModifierID"].ToString().ToInt();
                d.LastModifyTime = DateTime.Parse(dr["LastModifyTime"].ToString());
                d.IsExpired = dr["IsExpired"].ToBool();
                d.InstanceDocs = GetInventroyDocsByID(id);
                return d;
            }
            else
                return null;
        }

        public List<InventoryInstanceDocs> GetInventroyDocsByID(int id)
        {
            List<InventoryInstanceDocs> dlist = new List<InventoryInstanceDocs>();
            string sql = "select * from " + InstanceDocsTab + " where InstanceID='" + id + "' and IsExpired='0'";
            DataSet ds = BaseClass.mydataset(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    InventoryInstanceDocs d = new InventoryInstanceDocs();
                    d.ID = dr["ID"].ToString().ToInt();
                    d.DocID = dr["DocID"].ToInt();
                    d.DocInstanceIDs = dr["DocInstanceIDs"].ToString();
                    d.IsExpired = false;
                    dlist.Add(d);
                }
                return dlist;
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
        public List<InventoryInstance> GetPagedItems(string sqlwhere, string orderstr, int pageIndex, int pageSize)
        {
            List<InventoryInstance> dlist = new List<InventoryInstance>();
            string sql = SC.GetPagedDataStr(TabelName, "ID", sqlwhere, orderstr, pageIndex, pageSize);
            DataSet ds = BaseClass.mydataset(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                    dlist.Add(GetInventoryInstanceByID(int.Parse(dr["ID"].ToString())));
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
        public List<InventoryInstance> GetAllItems(string sqlwhere, string orderstr)
        {
            List<InventoryInstance> dlist = new List<InventoryInstance>();
            string sql = SC.GetAllDataStr(TabelName, "ID", sqlwhere, orderstr);
            DataSet ds = BaseClass.mydataset(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                    dlist.Add(GetInventoryInstanceByID(int.Parse(dr["ID"].ToString())));
                return dlist;
            }
            else
                return null;
        }

    }
}
