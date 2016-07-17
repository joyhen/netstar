using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetStar.Common.ADO
{
    /// <summary>
    /// 数据库操作
    /// <remarks>【注】:未做sql过滤处理</remarks>
    /// </summary>
    public class DBQuery
    {
        private const int DefaultRows = 100; //默认取100条数据
        private static DBHelp db = new DBHelp();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, PropertyInfo[]>
           DynamicObjectProperties = new ConcurrentDictionary<RuntimeTypeHandle, PropertyInfo[]>();

        public DBQuery() { }

        private static PropertyInfo[] GetObjectProperties<T>()
        {
            var type = typeof(T);
            var key = type.TypeHandle;

            PropertyInfo[] queryPts = null;
            if (!DynamicObjectProperties.TryGetValue(key, out queryPts))
            {
                queryPts = type.GetProperties();
                DynamicObjectProperties.TryAdd(key, queryPts);
            }

            return queryPts;
        }

        private string ParamFormat(object param, bool iswhere = true)
        {
            var queryWhere = param.GetType().GetProperties().Select(
                                 x => string.Format(iswhere ? "{0}={1}" : "{0}='{1}'",
                                                    x.Name,
                                                    x.GetValue(param, null))
                             );
            return string.Join(iswhere ? " AND " : ",", queryWhere);
        }

        /// <summary>
        /// 获取多个对象
        /// </summary>
        public List<T> GetMulti<T>(object where, string order = null) where T : class, new()
        {
            var type = typeof(T);
            var sql = new StringBuilder();

            if (where != null)
            {
                sql.AppendFormat("SELECT {0} FROM {1} WHERE {2}",
                     string.Join(",", GetObjectProperties<T>().Select(x => x.Name)),
                     type.Name,
                     ParamFormat(where));
            }
            else
                sql.AppendFormat("SELECT TOP {0} * FROM {1}", DefaultRows, type.Name);

            if (!string.IsNullOrWhiteSpace(order))
                sql.AppendFormat(" ORDER BY{0}", order);

            var dt = db.GetDataTable(sql.ToString());
            try { return dt.MapTo<T>(); }
            catch { return null; }
        }

        /// <summary>
        /// 获取单个对象
        /// </summary>
        public T GetSingle<T>(object where, string order = null) where T : class, new()
        {
            var query = GetMulti<T>(where, order);
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        public bool Delete<T>(object where) where T : class, new()
        {
            if (where == null) return false;

            var sql = string.Format("DELETE {0} WHERE {1}", typeof(T).Name, ParamFormat(where));
            return db.ExecuteSql(sql) > 0;
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        public bool Update<T>(object param, long id) where T : class, new()
        {
            var sql = string.Format("Update {0} SET {1} WHERE Id={2}", typeof(T).Name, ParamFormat(param, false), id);
            return db.ExecuteSql(sql) > 0;
        }

        //...

    }
}