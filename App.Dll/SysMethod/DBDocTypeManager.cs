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
    public class DBDocTypeManager
    {
        BaseClass bc = new BaseClass();
        SysCommon SC = new SysCommon();
        //得到父对象列表
        public DataTable GetParentTypeDt()
        {
            string sql = "select ID,TypeName from DocumentType_Tab where state<>'-1' order by TypeName asc";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            return dt;
        }
        //得到对应父对象下子对象列表
        public DataTable GetBrotherTypeDt(string ParentTypeID)
        {
            string sql = "select ID,TypeName from DocumentType_Tab where FatherTypeID='" + ParentTypeID + "' and state<>'-1' order by TypeName asc";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            return dt;
        }
        public void InsertToDt(string TypeName, string ParentTypeID)
        {
            string sql = "";
            sql += " insert into DocumentType_Tab(TypeName,FatherTypeID) values('" + TypeName + "','" + ParentTypeID + "')";
            bc.RunSqlTransaction(sql);
        }
        public void UpdateTypeManager(string TypeName, string ParentTypeID, string ID)
        {
            string sql = "";
            sql += " update DocumentType_Tab set TypeName='" + TypeName + "' where ID='" + ID + "'";

            bc.RunSqlTransaction(sql);
        }
        public void DeleteDt(string IDs)
        {
            string sql = " update DocumentType_Tab set State='-1' where ID in(" + IDs + ")";
            bc.RunSqlTransaction(sql);
        }
        public int GetRecordCount(string ParentTypeID)
        {
            if (ParentTypeID != "0")
                return SC.GetRecordCount("DocumentType_Tab", "FatherTypeID='" + ParentTypeID + "' and state<>'-1'");
            else
                return SC.GetRecordCount("DocumentType_Tab", "state<>'-1'");
        }
        public DataTable GetDt(string ParentTypeID)
        {
            string sql = "";
            if (ParentTypeID != "0")
                sql = "select ID,TypeName,(case FatherTypeID when '0' then '' else (select TypeName from DocumentType_Tab C2 where C2.ID=DocumentType_Tab.FatherTypeID) end) as FatherTypeName,FatherTypeID from DocumentType_Tab where FatherTypeID='" + ParentTypeID + "' and state<>'-1' order by TypeName asc";
            else
                sql = "select ID,TypeName,(case FatherTypeID when '0' then '' else (select TypeName from DocumentType_Tab C2 where C2.ID=DocumentType_Tab.FatherTypeID) end) as FatherTypeName,FatherTypeID from DocumentType_Tab where state<>'-1' order by TypeName asc";
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            return dt;
        }
        public int AlreadyExist(string TypeName, string ParentTypeID)
        {
            int count = bc.intScalar("select count(0) from DocumentType_Tab where TypeName='" + TypeName + "' and FatherTypeID='" + ParentTypeID + "' and state<>'-1'");
            return count;
        }
        public int AlreadyExist(string TypeName, string ParentTypeID, string id)
        {
            int count = bc.intScalar("select count(0) from DocumentType_Tab where TypeName='" + TypeName + "' and FatherTypeID='" + ParentTypeID + "' and state<>'-1' and ID<>'" + id + "'");
            return count;
        }

        public string GetDeptName(string ID)
        {
            string DeptName = bc.ecScalar("select TypeName from DocumentType_Tab where ID='" + ID + "'");
            return DeptName;
        }
        public string GetFatherDeptID(string ID)
        {
            string FatherDeptID = bc.ecScalar("select FatherTypeID from DocumentType_Tab where ID='" + ID + "'");
            return FatherDeptID;
        }
        public int FatherIDCount(string ParentTypeID)
        {
            int count = bc.intScalar(" select count(0) from DocumentType_Tab where FatherTypeID='" + ParentTypeID + "' and state<>'-1'");
            return count;
        }
        public DocType getItemById(int id)
        {
            string sql = "select * from DocumentType_Tab where id=" + id;
            DataTable dt = BaseClass.mydataset(sql).Tables[0];
            return new DocType { ID = id, TypeName = dt.Rows[0]["typename"].ToString() };
        }
        public List<DocType> getTypes(int isVirtual)
        {
            DataTable dt = BaseClass.mydataset("select * from DocumentType_Tab where State<>'-1'"+(isVirtual==2 ? "" : " and isvirtual='"+isVirtual+"'")).Tables[0];
            if (dt.Rows.Count > 0)
            {
                List<DocType> types = new List<DocType>();
                foreach (DataRow dr in dt.Rows)
                {
                    types.Add(new DocType { ID = int.Parse(dr["ID"].ToString()), TypeName = dr["TypeName"].ToString(), FatherTypeID = int.Parse(dr["FatherTypeID"].ToString()), State = int.Parse(dr["State"].ToString()) });
                }
                return types;
            }
            else
                return null;
        }
        /// <summary>
        /// 得到某文档类型的所有子类型,返回list
        /// </summary>
        /// <param name="fathertypeID"></param>
        /// <returns></returns>
        public List<DocType> getChildTypes(int fathertypeID)
        {
            string ids = GetChildTypeIDs(fathertypeID);
            if (!string.IsNullOrEmpty(ids))
                ids = ids.Remove(ids.Length - 1);
            else
                return null;

            List<DocType> types = new List<DocType>();
            foreach (DataRow drType in BaseClass.mydataset("select * from DocumentType_Tab where State<>'-1' and ID in (" + ids + ")").Tables[0].Rows)
            {
                types.Add(new DocType { ID = int.Parse(drType["ID"].ToString()), TypeName = drType["TypeName"].ToString(), FatherTypeID = int.Parse(drType["FatherTypeID"].ToString()) });
            }
            return types;
        }
        /// <summary>
        /// 得到某文档类型的所有子类型ID，拼接为','分隔的字符串
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetChildTypeIDs(int id)
        {
            string ids = string.Empty;
            foreach (DataRow dr in BaseClass.mydataset("select ID from DocumentType_Tab where State<>'-1' and FatherTypeID='" + id + "'").Tables[0].Rows)
            {
                ids += dr["ID"].ToString() + ",";
                ids += GetChildTypeIDs(int.Parse(dr["ID"].ToString()));
            }
            return ids;
        }
    }
}
