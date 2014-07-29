using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Dll;
using System.Data;

namespace App.Dll
{
    public class TaskService
    {
        BaseClass bc = new BaseClass();
        string sql = "";
        public int getTaskIDByUrl(string url)
        {
            sql = "select * from task_tab where url='" + url + "' and parentid='0'";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            if (dt.Rows.Count > 0)
                return Convert.ToInt32(dt.Rows[0]["id"].ToString());
            else
                return 0;
        }
        public int getMaxOrderOfRoot()
        {
            sql = "select * from task_tab where parentid=0 order by orderid desc";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            return Convert.ToInt32(dt.Rows[0]["orderid"].ToString()) + 1;
        }
        public int addTask(string taskname, string url, int tasklevel, int parentid)
        {
            sql = "insert into task_tab (taskname,parentid,orderid,url,tasklevel) values ('" + taskname + "'," + parentid + "," + getMaxOrderOfRoot() + ",'" + url + "'," + tasklevel + ")";
            bc.RunSqlTransaction(sql);
            sql = "select * from task_tab where taskname='" + taskname + "' ";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            return Convert.ToInt32(dt.Rows[0]["id"].ToString());
        }
        public bool deleteTasksByDocID(int docid)
        {
            sql = "delete from task_tab where url like '%?ID=" + docid + "'";
            return (bc.RunSqlTransaction(sql) == 0 ? true : false);
        }
    }
}
