using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace App.Dll.SysMethod
{
    public class DBRoleManager
    {
        BaseClass bc = new BaseClass();
        SysCommon SC = new SysCommon();

        public DataTable GetPagedData(string orderStr, int pageIndex, int pageSize)
        {
            DataTable dt = SC.GetPagedData("RoleManage_Tab", "ID ,RoleName", "State<>'-1'", orderStr, pageIndex, pageSize);
            return dt;
        }
        public int GetRecordCount()
        {
            int recordCount = SC.GetRecordCount("RoleManage_Tab", "State<>'-1'");
            return recordCount;
        }
        public void UpdateRole(string taskList, int roleID)
        {
            string sqltxt = "update RoleManage_Tab set TaskList='" + taskList + "' where ID='" + roleID + "'";
            bc.RunSqlTransaction(sqltxt);
        }
        public DataSet GetTasks()
        {
            return BaseClass.mydataset("SELECT * FROM Task_Tab order by OrderID asc");
        }

        public DataTable GetDt()
        {
            string strsql = "select * from RoleManage_Tab where State<>'-1'";
            DataTable mytable = BaseClass.mydataset(strsql).Tables[0];
            return mytable;
        }
        public void InsertToDt(string dataRoleName)
        {
            string strsql = "insert into RoleManage_Tab (RoleName) Values ('" + dataRoleName + "')";
            bc.RunSqlTransaction(strsql);
        }
        public void UpdateDt(int RoleID, string NewRoleName)
        {
            string strsql = "update RoleManage_Tab set RoleName='" + NewRoleName + "' where ID='" + RoleID + "' ";
            bc.RunSqlTransaction(strsql);
        }
        public void DeleteDt(string dataIDAll)
        {
            string strsql = "update RoleManage_Tab set State='-1' where ID in (" + dataIDAll + ")";
            bc.RunSqlTransaction(strsql);
        }
        public bool IsExist(string name)
        {
            if (bc.intScalar("select count(0) from RoleManage_Tab where RoleName='" + name + "' and State<>'-1'") > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool IsExist(string name, int id)
        {
            if (bc.intScalar("select count(0) from RoleManage_Tab where RoleName='" + name + "' and State<>'-1' and ID<>'" + id + "'") > 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}
