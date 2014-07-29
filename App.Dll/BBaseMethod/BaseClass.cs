using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace App.Dll
{
    public class BaseClass
    {
        #region ���ڵõ����ݿ�����
        public SqlConnection getconn()
        {
            string sqlConnectionStr = System.Configuration.ConfigurationManager.AppSettings["AppConnectionString"];//�õ���¼��Webconfig�е������ַ�����Ϣ
            SqlConnection sqlconn = new SqlConnection(sqlConnectionStr);//ʵ����һ��SQL����
            return sqlconn;//����ʵ�����������
        }
        #endregion

        #region ���ڷ������ݼ�
        public static DataSet mydataset(string FillSql)
        {
            BaseClass bc = new BaseClass();//ʵ�������ݲ���������BassClass
            SqlConnection con = bc.getconn();//���õõ����ݿ����ӵĺ����õ�һ������
            con.Open();//�����ݿ�����
            SqlDataAdapter adapter = new SqlDataAdapter(FillSql, con);//ʵ����һ�����������
            DataSet ds = new DataSet();//ʵ����һ�����ݼ���������¼���ݱ�
            try
            {
                adapter.Fill(ds);//�������������ݼ�����������ݲ���
            }
            catch (Exception ex)
            {
                //����Ϊ�쳣������䣬�漰��ϵͳ��־�������Ѷ�̫�󣬿��Ժ��Բ�������֮��������ǵ�������ݳ���ʱִ�е��׳��쳣��
                StringBuilder sb = new StringBuilder();
                sb.Append("�õ����ݼ�����ִ����䣺" + FillSql + "��������Դ��");
                StackTrace st = new StackTrace();
                foreach (StackFrame sf in st.GetFrames())
                {
                    if (sf.GetMethod().Name.Trim() != "EventArgFunctionCaller")
                    {
                        sb.Append("--->�ļ���:" + sf.GetMethod().ReflectedType.Name + ",");
                        sb.Append("������:" + sf.GetMethod().Name + "��");
                    }
                    else
                    {
                        break;
                    }
                }
                sb.Append("�������:" + ex.StackTrace);

                EventLog.WriteEntry("App.Dll", sb.ToString(), EventLogEntryType.Error);
                throw new Exception("�쳣(����001):" + ex.Message + "," + FillSql);
            }
            finally
            {
                con.Close();//�ر����ݿ�����
            }
            return ds;//���صõ������ݼ�
        }
        #endregion

        #region ���ڷ������ݿ��ѯ��һ������(string����)
        public string ecScalar(string FillSql)
        {
            SqlConnection con = this.getconn();//�õ�һ�����ݿ�����
            con.Open();//�����ݿ�����
            SqlCommand sm = new SqlCommand(FillSql, con);//ʵ����һ�����ݲ�����
            string str = "";//����һ�����ַ�����������¼������ݿ����ִ�к�����ݿ��еõ��ķ����ַ���
            try
            {
                str = sm.ExecuteScalar().ToString();//ִ�����ݿ�������õ����ݿ���һ�ַ���
            }
            catch (Exception e)
            {
                //�쳣����
                //throw new Exception("�쳣(����002):" + e.Message + "," + FillSql);
            }
            finally
            {
                con.Close();//�ر����ݿ����� 
            }
            return str;//���صõ����ַ���
        }
        #endregion

        #region ���ڷ������ݿ��ѯ��һ������(int����)
        public int intScalar(string FillSql)
        {
            //�˺���������ĵõ��ַ����ĺ�������һ�£�ֻ�Ƿ���ֵΪ��������
            SqlConnection con = this.getconn();
            con.Open();
            SqlCommand sm = new SqlCommand(FillSql, con);

            int str = 0;//����һ������������
            try
            {
                str = Convert.ToInt32(sm.ExecuteScalar());//ִ�����ݿ�����õ�һ��������
            }
            catch (Exception e)
            {
                //throw new Exception("�쳣(����003):" + e.Message + "," + FillSql);
            }
            finally { con.Close(); }

            return str;
        }
        #endregion

        #region SQL�������ַ�������
        public string SQLEncode(string sql)
        {
            //��������SQL����е������ַ�
            return sql.Trim().Replace("'", "''");
        }
        #endregion

        #region ִ������
        /// <summary>
        /// ִ������
        /// </summary>
        /// <param name="sql">���������</param>
        /// <returns>0:ִ�гɹ���1��ִ��ʧ�ܣ�2���ع�ʧ��</returns>
        public int RunSqlTransaction(string sql)
        {
            //�ú���Ϊ���ݿ�������õ���Ƶ����һ�������������޷���ֵ�����ݲ�������������ݣ�ɾ�����ݣ��������ݣ������������������ݲ�������Ϊ������ִ��ʧ�ܵ�ʱ�����ݿ��ع�������ǰ״̬���Ӷ���֤���ݿ����ݰ�ȫ
            int type = 0;//����һ������������¼�����ִ�н��
            SqlConnection myConnection = getconn();//�õ�һ�����ݿ�����
            myConnection.Open();//������

            SqlCommand myCommand = myConnection.CreateCommand();//�õ�һ�����ݿ������
            SqlTransaction myTrans;//����һ�����ݿ�ִ������

            //   Start   a   local   transaction   
            myTrans = myConnection.BeginTransaction();//��ʼִ������
            //   Must   assign   both   transaction   object   and   connection   
            //   to   Command   object   for   a   pending   local   transaction   
            myCommand.Connection = myConnection;
            myCommand.Transaction = myTrans;
            try
            {
                myCommand.CommandText = sql;
                myCommand.ExecuteNonQuery();
                myTrans.Commit();//�ύ����ִ�У��������ݿ�ִ�и�����
            }
            catch (Exception e)
            {
                type = 1;
                myTrans.Rollback();//ִ��ʧ�ܺ�Ļع����


                //����ͬ��Ϊ�쳣������䣬�漰��ϵͳ��־�������Ѷ�̫�󣬿��Ժ��Բ�������֮��������ǵ�������ݳ���ʱִ�е��׳��쳣��
                StringBuilder sb = new StringBuilder();
                sb.Append("ִ������ʧ�ܣ�ִ����䣺" + sql + "��������Դ��");
                StackTrace st = new StackTrace();
                foreach (StackFrame sf in st.GetFrames())
                {
                    if (sf.GetMethod().Name.Trim() != "EventArgFunctionCaller")
                    {
                        sb.Append("--->�ļ���:" + sf.GetMethod().ReflectedType.Name + ",");
                        sb.Append("������:" + sf.GetMethod().Name + "��");
                    }
                    else
                    {
                        break;
                    }
                }
                sb.Append("�������:" + e.StackTrace);

                EventLog.WriteEntry("App.Dll", sb.ToString(), EventLogEntryType.Error);
                throw new Exception("�쳣(����004):" + e.Message + "," + sql);
                //try
                //{

                //}
                //catch (SqlException ex)
                //{
                //    if (myTrans.Connection != null)
                //    {
                //        type = 2;
                //    } throw new Exception("�쳣(����005)");
                //}
            }
            finally
            {
                myConnection.Close();//�ر�����
            }
            return type;//��������ִ�н��
        }
        #endregion

    }
}
