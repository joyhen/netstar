using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace NetStar.Common.ADO
{
    /// <summary>
    /// 数据库操作
    /// </summary>
    internal sealed class DBHelp
    {
        /// <summary>
        /// SqlCommand命令终止执行操作时间（秒）
        /// </summary>
        private const int CommandTimeoutSecond = 5;
        private const string user = "root";
        private const string pwd = "123123";
        private const string ipadd = "127.0.0.1";

        private static string constr = string.Empty;
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        private static string ConnectionString
        {
            get
            {
                if (!string.IsNullOrEmpty(constr)) return constr;

                string key = NetStar.Config.KeyCenter.KeyMySQLConnection;
                string defaultconstr = string.Format("Database=netstar;Data Source={2};User Id={0};Password={1};", user, pwd, ipadd);

                ConnectionStringSettings consetting = ConfigurationManager.ConnectionStrings[key];
                constr = (consetting != null) ? (consetting.ConnectionString ?? defaultconstr) : defaultconstr;

                return constr;
            }
        }

        /// <summary>
        /// 执行SQL语句，返回影响的记录数（SELECT语句此方法不可行）
        /// </summary>
        public int ExecuteSql(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlTransaction tran = connection.BeginTransaction();

                using (SqlCommand cmd = new SqlCommand(SQLString, connection, tran))
                {
                    try
                    {
                        cmd.CommandTimeout = CommandTimeoutSecond;
                        int rows = cmd.ExecuteNonQuery();
                        tran.Commit();
                        //return Math.Max(rows, 1);
                        return rows == -1 ? 1 : rows; //-1
                    }
                    catch //(Exception ex)
                    {
                        tran.Rollback();
                        return -1;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        /// <summary>
        /// 执行带参数的SQL语句，返回影响的记录数
        /// </summary>
        public int ExecuteSql(string SQLString, bool isProcedure = false, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlTransaction tran = connection.BeginTransaction();

                using (SqlCommand cmd = new SqlCommand(SQLString, connection, tran))
                {
                    try
                    {
                        cmd.CommandTimeout = CommandTimeoutSecond;
                        cmd.CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text;
                        cmd.Parameters.AddRange(cmdParms);

                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        tran.Commit();

                        //return Math.Max(rows, 1);
                        return rows == -1 ? 1 : rows; //-1
                    }
                    catch
                    {
                        tran.Rollback();
                        return -1;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        /// <summary>
        /// 执行多条sql语句
        /// </summary>
        public int ExecuteSql(List<String> SQLStringList)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlTransaction tran = connection.BeginTransaction();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandTimeout = CommandTimeoutSecond;
                cmd.Transaction = tran;

                try
                {
                    int count = 0;
                    foreach (var x in SQLStringList)
                    {
                        cmd.CommandText = x;
                        var temp = cmd.ExecuteNonQuery();
                        //count += Math.Max(temp, 1); //may be should fix it in something conditions
                        count += (temp == -1 ? 1 : temp); //-1
                    }

                    tran.Commit();
                    return count;
                }
                catch
                {
                    tran.Rollback();
                    return -1;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 获取数据集
        /// </summary>
        public DataSet GetDataSet(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                SqlTransaction tran = connection.BeginTransaction();
                SqlCommand cmd = new SqlCommand(SQLString, connection, tran);
                cmd.CommandTimeout = CommandTimeoutSecond;

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    var ds = new DataSet();
                    try
                    {
                        da.Fill(ds);
                        return ds;
                    }
                    catch
                    {
                        return ds;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        /// <summary>
        /// 获取数据集
        /// </summary>
        public DataSet GetDataSet(string SQLString, bool isProcedure = false, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                SqlTransaction tran = connection.BeginTransaction();
                SqlCommand cmd = new SqlCommand(SQLString, connection, tran);
                cmd.CommandTimeout = CommandTimeoutSecond;
                cmd.CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text;
                cmd.Parameters.AddRange(cmdParms);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    var ds = new DataSet();
                    try
                    {
                        da.Fill(ds);
                        cmd.Parameters.Clear();
                        return ds;
                    }
                    catch
                    {
                        return ds;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 获取数据表
        /// </summary>
        public DataTable GetDataTable(string SQLString, int tbIndex = 0)
        {
            var ds = GetDataSet(SQLString);

            var tbs = ds.Tables;
            if (tbs != null && tbs.Count > 0)
                return tbs[tbIndex];

            return null;
        }
        /// <summary>
        /// 获取数据表
        /// </summary>
        public DataTable GetDataTable(string SQLString, int tbIndex = 0, bool isProcedure = false, params SqlParameter[] cmdParms)
        {
            var ds = GetDataSet(SQLString, isProcedure, cmdParms);

            var tbs = ds.Tables;
            if (tbs != null && tbs.Count > 0)
                return tbs[tbIndex];

            return null;
        }

        //...

    }
}
