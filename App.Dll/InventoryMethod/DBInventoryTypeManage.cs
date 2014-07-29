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
    public class DBInventoryTypeManage
    {
        BaseClass bc = new BaseClass();
        SysCommon SC = new SysCommon();

        public void InsertToDt(string TypeName, string ParentTypeID)
        {
            string sql = "";
            sql += " insert into InventoryType_Tab(Name,FatherID) values('" + TypeName + "','" + ParentTypeID + "')";
            bc.RunSqlTransaction(sql);
        }
        public void UpdateTypeManager(string TypeName, string ParentTypeID, string ID)
        {
            string sql = "";
            sql += " update InventoryType_Tab set Name='" + TypeName + "',FatherID='" + ParentTypeID + "' where ID='" + ID + "'";

            bc.RunSqlTransaction(sql);
        }
        public void DeleteDt(string IDs)
        {
            string sql = " update InventoryType_Tab set State='-1' where ID in(" + IDs + ")";
            bc.RunSqlTransaction(sql);
        }
        public int GetRecordCount(string ParentTypeID)
        {
            if (ParentTypeID != "0")
                return SC.GetRecordCount("InventoryType_Tab", "FatherID='" + ParentTypeID + "' and state<>'-1'");
            else
                return SC.GetRecordCount("InventoryType_Tab", "state<>'-1'");
        }
        public DataTable GetDt(string ParentTypeID)
        {
            string sql = "";
            if (ParentTypeID != "0")
                sql = "select ID,Name,(case FatherID when '0' then '' else (select Name from InventoryType_Tab C2 where C2.ID=InventoryType_Tab.FatherID) end) as FatherName,FatherID from InventoryType_Tab where FatherID='" + ParentTypeID + "' and state<>'-1' order by Name asc";
            else
                sql = "select ID,Name,(case FatherID when '0' then '' else (select Name from InventoryType_Tab C2 where C2.ID=InventoryType_Tab.FatherID) end) as FatherName,FatherID from InventoryType_Tab where state<>'-1' order by Name asc";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            return dt;
        }
        public int AlreadyExist(string TypeName, string ParentTypeID)
        {
            int count = bc.intScalar("select count(0) from InventoryType_Tab where Name='" + TypeName + "' and FatherID='" + ParentTypeID + "' and state<>'-1'");
            return count;
        }
        public int AlreadyExist(string TypeName, string ParentTypeID, string id)
        {
            int count = bc.intScalar("select count(0) from InventoryType_Tab where Name='" + TypeName + "' and FatherID='" + ParentTypeID + "' and state<>'-1' and ID<>'" + id + "'");
            return count;
        }
        public string GetDeptName(string ID)
        {
            string DeptName = bc.ecScalar("select Name from InventoryType_Tab where ID='" + ID + "'");
            return DeptName;
        }
        public string GetFatherDeptID(string ID)
        {
            string FatherDeptID = bc.ecScalar("select FatherID from InventoryType_Tab where ID='" + ID + "'");
            return FatherDeptID;
        }
        public int FatherIDCount(string ParentTypeID)
        {
            int count = bc.intScalar(" select count(0) from InventoryType_Tab where FatherID='" + ParentTypeID + "' and state<>'-1'");
            return count;
        }

        /// <summary>
        /// 得到某类型的所有子类型ID，拼接为','分隔的字符串
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetChildTypeIDs(int id)
        {
            string ids = string.Empty;
            foreach (DataRow dr in BaseClass.mydataset("select ID from InventoryType_Tab where State<>'-1' and FatherID='" + id + "'").Tables[0].Rows)
            {
                ids += dr["ID"].ToString() + ",";
                ids += GetChildTypeIDs(int.Parse(dr["ID"].ToString()));
            }
            return ids;
        }
    }
}
