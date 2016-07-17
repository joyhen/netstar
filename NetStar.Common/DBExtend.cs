using System.Collections.Generic;
using System.Data;
using System.Dynamic;

namespace NetStar.Common
{
    public static class DBExtend
    {
        /// <summary>
        /// DataTable转对象
        /// </summary>
        public static List<T> MapTo<T>(this DataTable dt) where T : class, new()
        {
            if (dt == null) return null;
            if (dt.Rows.Count == 0) return new List<T>();

            var objects = new List<dynamic>();

            foreach (DataRow row in dt.Rows)
            {
                dynamic obj = new ExpandoObject();

                foreach (DataColumn column in dt.Columns)
                {
                    var x = (IDictionary<string, object>)obj;
                    x.Add(column.ColumnName, row[column.ColumnName]);
                }
                objects.Add(obj);
            }

            var retval = new List<T>();
            foreach (dynamic item in objects)
            {
                var o = new T();
                DBMapper<T>.Map(item, o);
                retval.Add(o);
            }

            return retval;
        }

        //...

    }
}