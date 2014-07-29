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

        //��ͬ��״̬
        public string CondtionState = "State<>'-1' and State<>'-2' and State<>'-3'";

        //�õ���ҳ���ݣ���������Ϊ[����],��ȡ�ֶ��ַ���,Լ������,�����ַ���,��ǰҳ��,ÿҳ��ʾ��
        public DataTable GetPagedData(string tableName, string selectStr, string sqlWhere, string orderStr, int pageIndex, int pageSize)
        {
            int aa = (pageIndex - 1) * pageSize;
            DataTable dt = BaseClass.mydataset("select top " + pageSize + " " + selectStr + " from " + tableName + " where " + sqlWhere + " and ID not in (select top " + aa + " ID from " + tableName + " where " + sqlWhere + " order by " + orderStr + ") order by " + orderStr).Tables[0];
            return dt;
        }

        //�õ���¼��
        public int GetRecordCount(string tableName, string sqlWhere)
        {
            string sql = "select count(0) from " + tableName + " where " + sqlWhere;
            int recordCount = bc.intScalar(sql);
            return recordCount;
        }

        //��һ�ֵõ���ҳ���ݵķ�ʽ�����ڶ���ѯʱ������
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
            //����ͨ����ɾ�����з��ʸ��е���Ϣ
            dt.AcceptChanges();
            return dt;
        }
        public string GetFatherProdTypeID(string ProdGroupID)//����Ϊͨ�ö���ID,��û�и������󣬷���0��
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
            string subject = "�ʺ����żƻ�����ϵͳ֪ͨ";
            string body = "";
            if (toId == "0")
            {
                //toId = bc.ecScalar("select ProjectManagerID from Contract_Tab where ID='" + contID + "'");
                toId = "2";
            }
            string contNumber = bc.ecScalar("select SubProjName from SubProject_Tab where ID in (select SubProjectID from Contract_Tab where ID='" + contID + "')");
            string userName = bc.ecScalar("select Name from Users_Tab where ID='" + toId + "'");
            string fromName = bc.ecScalar("select Name from Users_Tab where ID='" + fromId + "'");
            body = userName + "������<br/>���ʺ����żƻ�����ϵͳ�Ļ��" + contNumber + " ��Ҫ���Ľ�һ�����������������¼ϵͳ���в�����лл����һ�������ߣ�" + fromName;

            string sqltxt = "insert into Email_Tab (FromID,ToID,Subject,Body,CreateTime) values ('" + fromId + "','" + toId + "','" + subject + "','" + body + "','" + DateTime.Now + "')";
            bc.RunSqlTransaction(sqltxt);
        }
        /// <summary>
        /// ��δ֪����ֵǿ��ת���� int ���͡����ֵ���� int�������� 0��
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

        //�õ���ҳ���ݣ���������Ϊ[����],��ȡ�ֶ��ַ���,Լ������,�����ַ���,��ǰҳ��,ÿҳ��ʾ��
        public string GetPagedDataStr(string tableName, string selectStr, string sqlWhere, string orderStr, int pageIndex, int pageSize)
        {
            if (sqlWhere == string.Empty)
                sqlWhere = "1=1";
            int aa = (pageIndex - 1) * pageSize;
            return "select top " + pageSize + " " + selectStr + " from " + tableName + " where " + sqlWhere + " and ID not in (select top " + aa + " ID from " + tableName + " where " + sqlWhere + " order by " + orderStr + ") order by " + orderStr;
        }

        //�õ�����ҳ����
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

        //�õ���¼��
        public string GetRecordCountStr(string tableName, string sqlWhere)
        {
            if (sqlWhere == string.Empty)
                sqlWhere = "1=1";
            return "select count(0) from " + tableName + " where " + sqlWhere;
        }

    }
}
