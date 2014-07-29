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
    public class DBUserManager
    {
        BaseClass bc = new BaseClass();
        SysCommon SC = new SysCommon();
        private static readonly string ADDUSER = "INSERT INTO Users_Tab(UserName,Name,RoleID,Gender,DeptNameID,State,Password,Email,Mobile,Phone) VALUES";
        private static readonly string GETUSERS = "SELECT * FROM Users_Tab WHERE State<>'-1'";
        private static readonly string GETROLENAMES = "SELECT ID,RoleName FROM RoleManage_Tab";

        public void AddUser(string sUserName, string sName, string sRole, int sGender, string sDeptNameID, int sState, string sPassword, string sEmail, string sMobile, string sPhone)
        {
            string sqlstr = ADDUSER + "(" + "'" + sUserName + "'," + "'" + sName + "'," + "'" + sRole + "'," + "'" + sGender + "'," + "'" + sDeptNameID + "'" + ",'" + sState + "','" + sPassword + "','" + sEmail + "','" + sMobile + "','" + sPhone + "')";
            bc.RunSqlTransaction(sqlstr);
        }
        public void DeleteUser(int sID, int sState)
        {
            string sqlstr = "UPDATE Users_Tab SET State='" + sState + "'" + "WHERE ID='" + sID + "'";
            bc.RunSqlTransaction(sqlstr);
        }
        public void DeleteDt(string dataIDAll)
        {
            string[] ppp = dataIDAll.Split(',');
            string sql = "";
            for (int i = 0; i < ppp.Length; i++)
            {
                int ID = bc.intScalar("select ID from Users_Tab where ID='" + ppp[i] + "' ");
                sql = " update Users_Tab set State=-1 where ID='" + ID + "'";
                bc.RunSqlTransaction(sql);
            }
        }
        public void UpdateUser(Int32 sID, string sUserName, string sName, string sRole, int sGender, string sDeptName, string sEmail, string sMobile, string sPhone)
        {
            string UPDATEUSER = "UPDATE Users_Tab SET UserName='" + sUserName + "',Name='" + sName + "',RoleID='" + sRole + "',Gender='" + sGender + "',DeptNameID='" + sDeptName + "',Email='" + sEmail + "',Mobile='" + sMobile + "',Phone='" + sPhone + "' WHERE ID='" + sID + "'";
            bc.RunSqlTransaction(UPDATEUSER);
        }

        public DataTable GetRoleNames()
        {
            DataTable dt = BaseClass.mydataset(GETROLENAMES).Tables[0];
            return dt;
        }
        public int GetRecordCount()
        {
            int recordCount = SC.GetRecordCount("Users_Tab", "State<>'-1'");
            return recordCount;
        }
        public DataTable GetPagedData(string orderStr, int pageIndex, int pageSize)
        {
            DataTable dt = SC.GetPagedData("Users_Tab", " *,(case Gender when '1' then '男' when '-1' then '女' else '其它' end) as dataGender,(select DeptName from DeptManage_Tab where DeptManage_Tab.ID=Users_Tab.DeptNameID) as dataDeptName,(select RoleName from RoleManage_Tab where RoleManage_Tab.ID=Users_Tab.RoleID) as dataRoleName ", "State<>'-1'", orderStr, pageIndex, pageSize);
            return dt;
        }
        public bool IsExist(string username)
        {
            if (bc.intScalar("select count(0) from Users_Tab where UserName='" + username + "'and State<>'-1'") > 0)
                return true;
            else
                return false;
        }
        public bool IsMExist(string username, int id)
        {
            if (bc.intScalar("select count(0) from Users_Tab where UserName='" + username + "'and State<>'-1' and ID<>'" + id + "'") > 0)
                return true;
            else
                return false;
        }


        public int GetDeptID(int id)
        {
            int deptID = bc.intScalar("select DeptNameID from Users_Tab where ID='" + id + "' and State<>'-1'");
            return deptID;
        }
        public int GetRoleID(int id)
        {
            int deptID = bc.intScalar("select RoleID from Users_Tab where ID='" + id + "' and State<>'-1'");
            return deptID;
        }
        public void ResetPwd(string id, string newPwd)
        {
            string sql = "update Users_Tab set Password='" + newPwd + "' where ID='" + id + "'";
            bc.RunSqlTransaction(sql);
        }
        public List<User> getUsersByDepartment(int departID)
        {
            string ids = departID.ToString() + ",";
            ids += GetDeptIDs(departID);
            ids = ids.Remove(ids.Length - 1);
            List<User> users = new List<User>();
            foreach (DataRow drUser in BaseClass.mydataset("select * from Users_Tab where State<>'-1' and DeptNameID in (" + ids + ") order by UserName asc").Tables[0].Rows)
            {
                users.Add(new User { ID = int.Parse(drUser["ID"].ToString()), Name = drUser["Name"].ToString(), UserName = drUser["UserName"].ToString(), DepartmentID = int.Parse(drUser["DeptNameID"].ToString()) });
            }
            return users;
        }
        public string GetDeptIDs(int id)
        {
            string ids = string.Empty;
            foreach (DataRow dr in BaseClass.mydataset("select ID from DeptManage_Tab where State<>'-1' and FatherDeptID='" + id + "'").Tables[0].Rows)
            {
                ids += dr["ID"].ToString() + ",";
                ids += GetDeptIDs(int.Parse(dr["ID"].ToString()));
            }
            return ids;
        }
        public User getUserByID(int uid)
        {
            DataTable dt = BaseClass.mydataset("select top 1 * from Users_Tab where State<>'-1' and ID='" + uid + "'").Tables[0];
            if (dt.Rows.Count > 0)
            {
                return new User { ID = int.Parse(dt.Rows[0]["ID"].ToString()), Name = dt.Rows[0]["Name"].ToString(), UserName = dt.Rows[0]["UserName"].ToString(), IsAdmin=(object.Equals(dt.Rows[0]["isadmin"],null) ? false : Convert.ToBoolean(dt.Rows[0]["isadmin"].ToString())), DepartmentID = int.Parse(dt.Rows[0]["DeptNameID"].ToString()) };
            }
            else
                return null;
        }
        public User CheckLogin(string username, string pwd)
        {
            string encodePwd = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "MD5").ToLower().Substring(8, 16);
            DataTable dt = BaseClass.mydataset("select top 1 * from Users_Tab where State<>'-1' and UserName='" + username + "' and Password='" + encodePwd + "'").Tables[0];
            if (dt.Rows.Count > 0)
            {
                return new User { ID = int.Parse(dt.Rows[0]["ID"].ToString()), Name = dt.Rows[0]["Name"].ToString(), UserName = dt.Rows[0]["UserName"].ToString(), DepartmentID = int.Parse(dt.Rows[0]["DeptNameID"].ToString()) };
            }
            else
                return null;
        }
        public List<Task> getTasksByUserID(int uid)
        {
            string taskIDs = bc.ecScalar("select TaskList from RoleManage_Tab where State<>'-1' and ID=(select top 1 RoleID from Users_Tab where State<>'-1' and ID='" + uid + "')");
            if (!string.IsNullOrEmpty(taskIDs))
            {
                DataTable dt = BaseClass.mydataset("select * from Task_Tab where ID in (" + taskIDs + ") order by ParentID,OrderID asc").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    List<Task> tasklt = new List<Task>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        tasklt.Add(new Task { ID = int.Parse(dr["ID"].ToString()), TaskName = dr["TaskName"].ToString(), ParentID = int.Parse(dr["ParentID"].ToString()), OrderID = int.Parse(dr["OrderID"].ToString()), Url = dr["Url"].ToString(), TaskLevel = int.Parse(dr["TaskLevel"].ToString()) });
                    }
                    return tasklt;
                }
                else
                {
                    return null;
                }
            }
            else
                return null;
        }
        public int GetUserIDByUserName(string name)
        {
            return bc.intScalar("select top 1 id from users_tab where state<>'-1' and name='"+name+"'");
        }
    }
}
