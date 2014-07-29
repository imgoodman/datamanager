using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Web;
using App.Model;

namespace App.Dll.SysMethod
{
    public class DBDeptManager
    {
        BaseClass bc = new BaseClass();
        SysCommon SC = new SysCommon();
        //得到父部门列表
        public DataTable GetParentDeptDt()
        {
            string sql = "select ID,DeptName from DeptManage_Tab where state<>'-1' order by DeptName asc";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            return dt;
        }
        //得到对应父部门下子部门列表
        public DataTable GetBrotherDeptDt(string ParentDeptID)
        {
            string sql = "select ID,DeptName from DeptManage_Tab where FatherDeptID='" + ParentDeptID + "' and state<>'-1' order by DeptName asc";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            return dt;
        }
        public void InsertToDt(string DeptName, string ParentDeptID)
        {
            string sql = "";
            sql += " insert into DeptManage_Tab(DeptName,FatherDeptID) values('" + DeptName + "','" + ParentDeptID + "')";
            bc.RunSqlTransaction(sql);
        }
        public void UpdateDeptManager(string DeptName, string ParentDeptID, string ID)
        {
            string sql = "";
            sql += " update DeptManage_Tab set DeptName='" + DeptName + "' where ID='" + ID + "'";

            bc.RunSqlTransaction(sql);
        }
        public void DeleteDt(string IDs)
        {
            string sql = " update DeptManage_Tab set State='-1' where ID in(" + IDs + ")";
            bc.RunSqlTransaction(sql);
        }
        public int GetRecordCount(string ParentDeptID)
        {
            if (ParentDeptID != "0")
                return SC.GetRecordCount("DeptManage_Tab", "FatherDeptID='" + ParentDeptID + "' and state<>'-1'");
            else
                return SC.GetRecordCount("DeptManage_Tab", "state<>'-1'");
        }
        public DataTable GetDt(string ParentDeptID)
        {
            string sql = "";
            if (ParentDeptID != "0")
                sql = "select ID,DeptName,(case FatherDeptID when '0' then '' else (select DeptName from DeptManage_Tab C2 where C2.ID=DeptManage_Tab.FatherDeptID) end) as FatherDeptName,FatherDeptID from DeptManage_Tab where FatherDeptID='" + ParentDeptID + "' and state<>'-1' order by DeptName asc";
            else
                sql = "select ID,DeptName,(case FatherDeptID when '0' then '' else (select DeptName from DeptManage_Tab C2 where C2.ID=DeptManage_Tab.FatherDeptID) end) as FatherDeptName,FatherDeptID from DeptManage_Tab where state<>'-1' order by DeptName asc";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            return dt;
        }
        public int AlreadyExist(string DeptName, string ParentDeptID)
        {
            int count = bc.intScalar("select count(0) from DeptManage_Tab where DeptName='" + DeptName + "' and FatherDeptID='" + ParentDeptID + "' and state<>'-1'");
            return count;
        }
        public int AlreadyExist(string DeptName, string ParentDeptID, string id)
        {
            int count = bc.intScalar("select count(0) from DeptManage_Tab where DeptName='" + DeptName + "' and FatherDeptID='" + ParentDeptID + "' and state=<>'-1' and ID<>'" + id + "'");
            return count;
        }

        public string GetDeptName(string ID)
        {
            string DeptName = bc.ecScalar("select DeptName from DeptManage_Tab where ID='" + ID + "'");
            return DeptName;
        }
        public string GetFatherDeptID(string ID)
        {
            string FatherDeptID = bc.ecScalar("select FatherDeptID from DeptManage_Tab where ID='" + ID + "'");
            return FatherDeptID;
        }
        public int FatherIDCount(string ParentDeptID)
        {
            int count = bc.intScalar(" select count(0) from DeptManage_Tab where FatherDeptID='" + ParentDeptID + "' and state<>'-1'");
            return count;
        }
        public List<Department> getDepartments()
        {
            DataTable dt = BaseClass.mydataset("select * from DeptManage_Tab where State<>'-1'").Tables[0];
            if (dt.Rows.Count > 0)
            {
                List<Department> departs = new List<Department>();
                foreach (DataRow dr in dt.Rows)
                {
                    departs.Add(new Department { ID = int.Parse(dr["ID"].ToString()), DeptName = dr["DeptName"].ToString(), FatherDepartmentID = int.Parse(dr["FatherDeptID"].ToString()), State = int.Parse(dr["State"].ToString()) });
                }
                return departs;
            }
            else
                return null;
        }
    }
}
