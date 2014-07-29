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
        #region 用于得到数据库连接
        public SqlConnection getconn()
        {
            string sqlConnectionStr = System.Configuration.ConfigurationManager.AppSettings["AppConnectionString"];//得到记录在Webconfig中的连接字符串信息
            SqlConnection sqlconn = new SqlConnection(sqlConnectionStr);//实例化一个SQL连接
            return sqlconn;//返回实例化后的连接
        }
        #endregion

        #region 用于返回数据集
        public static DataSet mydataset(string FillSql)
        {
            BaseClass bc = new BaseClass();//实例化数据操作常用类BassClass
            SqlConnection con = bc.getconn();//调用得到数据库连接的函数得到一个连接
            con.Open();//打开数据库连接
            SqlDataAdapter adapter = new SqlDataAdapter(FillSql, con);//实例化一个数据填充类
            DataSet ds = new DataSet();//实例化一个数据集，用来记录数据表
            try
            {
                adapter.Fill(ds);//利用填充类对数据集进行填充数据操作
            }
            catch (Exception ex)
            {
                //以下为异常处理语句，涉及到系统日志操作，难度太大，可以忽略不看，总之以下语句是当填充数据出错时执行的抛出异常的
                StringBuilder sb = new StringBuilder();
                sb.Append("得到数据集出错，执行语句：" + FillSql + "。错误来源：");
                StackTrace st = new StackTrace();
                foreach (StackFrame sf in st.GetFrames())
                {
                    if (sf.GetMethod().Name.Trim() != "EventArgFunctionCaller")
                    {
                        sb.Append("--->文件名:" + sf.GetMethod().ReflectedType.Name + ",");
                        sb.Append("方法名:" + sf.GetMethod().Name + "。");
                    }
                    else
                    {
                        break;
                    }
                }
                sb.Append("具体错误:" + ex.StackTrace);

                EventLog.WriteEntry("App.Dll", sb.ToString(), EventLogEntryType.Error);
                throw new Exception("异常(代码001):" + ex.Message + "," + FillSql);
            }
            finally
            {
                con.Close();//关闭数据库连接
            }
            return ds;//返回得到的数据集
        }
        #endregion

        #region 用于返回数据库查询的一个数据(string类型)
        public string ecScalar(string FillSql)
        {
            SqlConnection con = this.getconn();//得到一个数据库连接
            con.Open();//打开数据库连接
            SqlCommand sm = new SqlCommand(FillSql, con);//实例化一个数据操作类
            string str = "";//定义一个空字符串，用来记录随后数据库语句执行后从数据库中得到的返回字符串
            try
            {
                str = sm.ExecuteScalar().ToString();//执行数据库操作，得到数据库中一字符串
            }
            catch (Exception e)
            {
                //异常处理
                //throw new Exception("异常(代码002):" + e.Message + "," + FillSql);
            }
            finally
            {
                con.Close();//关闭数据库连接 
            }
            return str;//返回得到的字符串
        }
        #endregion

        #region 用于返回数据库查询的一个数据(int类型)
        public int intScalar(string FillSql)
        {
            //此函数跟上面的得到字符串的函数基本一致，只是返回值为整型数字
            SqlConnection con = this.getconn();
            con.Open();
            SqlCommand sm = new SqlCommand(FillSql, con);

            int str = 0;//定义一个整型数变量
            try
            {
                str = Convert.ToInt32(sm.ExecuteScalar());//执行数据库操作得到一个整型数
            }
            catch (Exception e)
            {
                //throw new Exception("异常(代码003):" + e.Message + "," + FillSql);
            }
            finally { con.Close(); }

            return str;
        }
        #endregion

        #region SQL中特殊字符串处理
        public string SQLEncode(string sql)
        {
            //用来处理SQL语句中的特殊字符
            return sql.Trim().Replace("'", "''");
        }
        #endregion

        #region 执行事务
        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="sql">批处理语句</param>
        /// <returns>0:执行成功；1：执行失败；2：回滚失败</returns>
        public int RunSqlTransaction(string sql)
        {
            //该函数为数据库操作中用的最频繁的一个，用来处理无返回值的数据操作，如插入数据，删除数据，更新数据，利用事务来进行数据操作是因为当事务执行失败的时候，数据库会回滚到操作前状态，从而保证数据库数据安全
            int type = 0;//定义一个变量用来记录事务的执行结果
            SqlConnection myConnection = getconn();//得到一个数据库连接
            myConnection.Open();//打开连接

            SqlCommand myCommand = myConnection.CreateCommand();//得到一个数据库操作类
            SqlTransaction myTrans;//定义一个数据库执行事务

            //   Start   a   local   transaction   
            myTrans = myConnection.BeginTransaction();//开始执行事务
            //   Must   assign   both   transaction   object   and   connection   
            //   to   Command   object   for   a   pending   local   transaction   
            myCommand.Connection = myConnection;
            myCommand.Transaction = myTrans;
            try
            {
                myCommand.CommandText = sql;
                myCommand.ExecuteNonQuery();
                myTrans.Commit();//提交事务执行，请求数据库执行该命令
            }
            catch (Exception e)
            {
                type = 1;
                myTrans.Rollback();//执行失败后的回滚语句


                //以下同样为异常处理语句，涉及到系统日志操作，难度太大，可以忽略不看，总之以下语句是当填充数据出错时执行的抛出异常的
                StringBuilder sb = new StringBuilder();
                sb.Append("执行事务失败，执行语句：" + sql + "。错误来源：");
                StackTrace st = new StackTrace();
                foreach (StackFrame sf in st.GetFrames())
                {
                    if (sf.GetMethod().Name.Trim() != "EventArgFunctionCaller")
                    {
                        sb.Append("--->文件名:" + sf.GetMethod().ReflectedType.Name + ",");
                        sb.Append("方法名:" + sf.GetMethod().Name + "。");
                    }
                    else
                    {
                        break;
                    }
                }
                sb.Append("具体错误:" + e.StackTrace);

                EventLog.WriteEntry("App.Dll", sb.ToString(), EventLogEntryType.Error);
                throw new Exception("异常(代码004):" + e.Message + "," + sql);
                //try
                //{

                //}
                //catch (SqlException ex)
                //{
                //    if (myTrans.Connection != null)
                //    {
                //        type = 2;
                //    } throw new Exception("异常(代码005)");
                //}
            }
            finally
            {
                myConnection.Close();//关闭连接
            }
            return type;//返回事务执行结果
        }
        #endregion

    }
}
