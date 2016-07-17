using System;
using System.Linq;
using System.Collections.Generic;

namespace NetStar.Common
{
    using Dapper;
    using NetStar.Config;

    public class MySqlHelp
    {
        /// <summary>
        /// 获取单个对象
        /// </summary>
        public T GetEntity<T>(string sql, object param) where T : IBaseEntity
        {
            var connection = new Connection();
            var dbConnection = connection.GetSqlConnection();

            try
            {
                //Query, QueryFirst, QuerySingle, QuerySingleOrDefault
                var result = dbConnection.QueryFirstOrDefault<T>(sql, param);
                return result;
            }
            catch (Exception ex)
            {
                DBLog.Log(ex);
                return default(T);
            }
            finally
            {
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }
        }

        public T GetEntityWithDynamic<T>(string sql, Dictionary<string, object> dynamicParam) where T : IBaseEntity
        {
            DynamicParameters args = null;

            if (dynamicParam != null && dynamicParam.Count > 0)
            {
                args = new DynamicParameters();
                foreach (var arg in dynamicParam)
                    args.Add(arg.Key, arg.Value);
            }

            return GetEntity<T>(sql, args);
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <example>Execute(@"INSERT Persons (id, dob) values (@id, @dob)", new { id = 7, dob = (DateTime?)null });</example>
        public int Execute(string sql, object param)
        {
            var connection = new Connection();
            var dbConnection = connection.GetSqlConnection();

            try
            {
                var result = dbConnection.Execute(sql, param);
                return result;
            }
            catch (Exception ex)
            {
                DBLog.Log(ex);
                return 0;
            }
            finally
            {
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }
        }

        //...

    }
}