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
    public class DBInventoryType
    {
        BaseClass bc = new BaseClass();
        SysCommon SC = new SysCommon();
        private string TabelName = "InventoryType_Tab";


        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="a">待添加对象</param>
        /// <returns>新增成功，返回新增的ID；否则返回0</returns>
        public int Add(InventoryType a)
        {
            int id = 0;
            string sql = "insert into " + TabelName + " (Name,FatherID,State) values ('" + a.Name + "','" + a.FatherID + "','1')";
            bc.RunSqlTransaction(sql);
            string sqlselect = "select top 1 ID from " + TabelName + " where Name = '" + a.Name + "' and State<>'-1' ";
            id = bc.intScalar(sqlselect);
            return id;
        }

        /// <summary>
        /// 添加时候，验证是否已经存在
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public bool IsExistWhenAdd(InventoryType a)
        {
            string sql = "select  count(0) from " + TabelName + " where (Name='" + a.Name + "') and State<>'-1'";
            int count = bc.intScalar(sql);
            if (count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        ///  更新对象
        /// </summary>
        /// <param name="u">待更新对象</param>
        /// <returns>更新成功，返回true；否则返回false</returns>
        public bool Update(InventoryType u)
        {
            string sql = "update " + TabelName + " set Name='" + u.Name + "',FatherID='" + u.FatherID + "' where ID='" + u.ID + "'";
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
        public bool IsExistWhenUpdate(InventoryType u)
        {
            string sql = "select * from " + TabelName + " where (Name='" + u.Name + "') and State<>'-1' and id<>'" + u.ID + "'";
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
        public bool Delete(InventoryType d)
        {
            string sql = "update " + TabelName + " set State='-1' where id='" + d.ID + "'";
            if (bc.RunSqlTransaction(sql) == 0)
                return true;
            else
                return false;
        }

        public bool DeleteByID(int id, int LastModifierID)
        {
            string sql = "update " + TabelName + " set State='-1' where id='" + id + "'";
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
                int ID = bc.intScalar("select ID from " + TabelName + " where ID='" + ppp[i] + "' ");
                sql = " update " + TabelName + " set State='-1' where ID='" + ID + "'";
                bc.RunSqlTransaction(sql);
            }
        }

        /// <summary>
        /// 返回对应id的对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InventoryType GetItemByID(int id)
        {
            string sql = "select * from " + TabelName + " where id='" + id + "' and state<>'-1'";
            DataSet ds = BaseClass.mydataset(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                InventoryType d = new InventoryType();
                d.ID = id;
                d.Name = dr["Name"].ToString();
                d.FatherID = dr["FatherID"].ToString().ToInt();
                d.State = dr["State"].ToString().ToInt();
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
        public List<InventoryType> GetPagedItems(string sqlwhere, string orderstr, int pageIndex, int pageSize)
        {
            List<InventoryType> dlist = new List<InventoryType>();
            string sql = SC.GetPagedDataStr(TabelName, "ID", sqlwhere, orderstr, pageIndex, pageSize);
            DataSet ds = BaseClass.mydataset(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                    dlist.Add(GetItemByID(int.Parse(dr["ID"].ToString())));
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
        public List<InventoryType> GetAllItems(string sqlwhere, string orderstr)
        {
            List<InventoryType> dlist = new List<InventoryType>();
            string sql = SC.GetAllDataStr(TabelName, "ID", sqlwhere, orderstr);
            DataSet ds = BaseClass.mydataset(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                    dlist.Add(GetItemByID(int.Parse(dr["ID"].ToString())));
                return dlist;
            }
            else
                return null;
        }

    }
}
