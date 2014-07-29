using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections.Generic;
using System.Text;

namespace App.Dll
{
    public class SysCommon
    {
        BaseClass bc = new BaseClass();

        //public void DisplayErrorMsg(Page page, string strMsg)
        //{
        //    //StringBuilder sb = new StringBuilder();
        //    //sb.Append("<script language='javascript'>");
        //    //sb.Append("alert('" + strMsg.Replace("'", "''") + " ');");
        //    //sb.Append("</script>");
        //    //page.Response.Write(sb.ToString());
        //    //sb = null;
        //    page.RegisterStartupScript("alert", "<script>alert('" + strMsg + "');</script>");
        //}

        //合同的状态
        public string CondtionState = "State<>'-1' and State<>'-2' and State<>'-3'";

        //得到分页数据，参数依次为[表名],提取字段字符串,约束条件,排序字符串,当前页码,每页显示数
        public DataTable GetPagedData(string tableName, string selectStr, string sqlWhere, string orderStr, int pageIndex, int pageSize)
        {
            int aa = (pageIndex - 1) * pageSize;
            DataTable dt = BaseClass.mydataset("select top " + pageSize + " " + selectStr + " from " + tableName + " where " + sqlWhere + " and ID not in (select top " + aa + " ID from " + tableName + " where " + sqlWhere + " order by " + orderStr + ") order by " + orderStr).Tables[0];
            return dt;
        }

        //得到记录数
        public int GetRecordCount(string tableName, string sqlWhere)
        {
            string sql = "select count(0) from " + tableName + " where " + sqlWhere;
            int recordCount = bc.intScalar(sql);
            return recordCount;
        }

        //另一种得到分页数据的方式：用于多表查询时的排序
        public DataTable GetPagedData_MutiTable(string tableName, string selectStr, string sqlWhere, string orderStr, int pageIndex, int pageSize)
        {
            DataTable dt = BaseClass.mydataset("select " + selectStr + " from " + tableName + " where " + sqlWhere + " order by " + orderStr).Tables[0];
            int recordFrom = (pageIndex - 1) * pageSize + 1;
            int recordTo = pageIndex * pageSize;

            if (dt.Rows.Count > recordTo)
            {
                for (int i = recordTo; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i].Delete();
                }
            }
            if (pageIndex > 1)
            {
                for (int j = 0; j < recordFrom - 1; j++)
                {
                    dt.Rows[j].Delete();
                }
            }
            //不能通过已删除的行访问该行的信息
            dt.AcceptChanges();
            return dt;
        }
        public string GetFatherProdTypeID(string ProdGroupID)//参数为通用对象ID,若没有父级对象，返回0；
        {
            string AncestorProdTypeID = "0";
            string ProdTypeID = bc.ecScalar(" select ProdTypeID from ProdGroup_Tab where ID='" + ProdGroupID + "'");
            string FatherProdTypeID = bc.ecScalar(" select FatherProdID from ProdTypeManager_Tab where ID='" + ProdTypeID + "'");
            if (FatherProdTypeID == "0")
            {
                return AncestorProdTypeID;
            }
            else
            {
                return FatherProdTypeID;
            }
        }
        public string GetAncestorProdTypeID(string ProdGroupID)
        {
            string ProdTypeID = bc.ecScalar(" select ProdTypeID from ProdGroup_Tab where ID='" + ProdGroupID + "'");
            string AncestorProdTypeID = GetFather(ProdTypeID);
            return AncestorProdTypeID;
        }
        protected string GetFather(string ProdTypeID)
        {

            string FatherProdTypeID = bc.ecScalar(" select FatherProdID from ProdTypeManager_Tab where ID='" + ProdTypeID + "'");
            if (FatherProdTypeID == "0")
            {
                return ProdTypeID;
            }
            else
            {
                string Father = GetFather(FatherProdTypeID);
                return Father;
            }
        }

        public void NewEmail(string fromId, string toId, string contID)
        {
            string subject = "适航部门计划管理系统通知";
            string body = "";
            if (toId == "0")
            {
                //toId = bc.ecScalar("select ProjectManagerID from Contract_Tab where ID='" + contID + "'");
                toId = "2";
            }
            string contNumber = bc.ecScalar("select SubProjName from SubProject_Tab where ID in (select SubProjectID from Contract_Tab where ID='" + contID + "')");
            string userName = bc.ecScalar("select Name from Users_Tab where ID='" + toId + "'");
            string fromName = bc.ecScalar("select Name from Users_Tab where ID='" + fromId + "'");
            body = userName + "，您好<br/>，适航部门计划管理系统的活动：" + contNumber + " 需要您的进一步操作，请您尽快登录系统进行操作，谢谢。上一步操作者：" + fromName;

            string sqltxt = "insert into Email_Tab (FromID,ToID,Subject,Body,CreateTime) values ('" + fromId + "','" + toId + "','" + subject + "','" + body + "','" + DateTime.Now + "')";
            bc.RunSqlTransaction(sqltxt);
        }
        /// <summary>
        /// 把未知类型值强制转换到 int 类型。如此值不是 int，将返回 0。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int ToInt32(object value)
        {
            if (value == null) return 0;
            int result;
            if (int.TryParse(value.ToString(), out result))
            {
                return result;
            }
            return 0;
        }

        //得到分页数据，参数依次为[表名],提取字段字符串,约束条件,排序字符串,当前页码,每页显示数
        public string GetPagedDataStr(string tableName, string selectStr, string sqlWhere, string orderStr, int pageIndex, int pageSize)
        {
            if (sqlWhere == string.Empty)
                sqlWhere = "1=1";
            int aa = (pageIndex - 1) * pageSize;
            return "select top " + pageSize + " " + selectStr + " from " + tableName + " where " + sqlWhere + " and ID not in (select top " + aa + " ID from " + tableName + " where " + sqlWhere + " order by " + orderStr + ") order by " + orderStr;
        }

        //得到不分页数据
        public string GetAllDataStr(string tableName, string selectStr, string sqlWhere, string orderStr)
        {
            if (sqlWhere == string.Empty)
            {
                sqlWhere = "1=1";
            }
            return "select " + selectStr + " from " + tableName + " where " + sqlWhere + " order by " + orderStr;
        }

        public string GetTopOneItemStr(string tableName, int id)
        {
            return "select top 1 * from " + tableName + " where ID=" + id;
        }

        //得到记录数
        public string GetRecordCountStr(string tableName, string sqlWhere)
        {
            if (sqlWhere == string.Empty)
                sqlWhere = "1=1";
            return "select count(0) from " + tableName + " where " + sqlWhere;
        }

    }
}
