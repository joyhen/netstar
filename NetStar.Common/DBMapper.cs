using System;
using System.Collections.Concurrent;
using System.Dynamic;
using System.Reflection;

namespace NetStar.Common
{
    internal static class DBMapper<T> where T : class
    {
        private static readonly ConcurrentDictionary<string, PropertyInfo> PropertyMap
            = new ConcurrentDictionary<string, PropertyInfo>();

        static DBMapper()
        {
            var dicProperName = typeof(T).GetProperties();

            foreach (var p in dicProperName)
                PropertyMap.TryAdd(p.Name.ToLower(), p);
        }

        public static void Map(ExpandoObject source, T destination)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (destination == null)
                throw new ArgumentNullException("destination");

            foreach (var kv in source)
            {
                PropertyInfo p;
                if (PropertyMap.TryGetValue(kv.Key.ToLower(), out p))
                {
                    var propType = p.PropertyType;
                    if (kv.Value == null)
                    {
                        if (!propType.IsByRef && propType.Name != "Nullable`1")
                        {
                            throw new ArgumentException("not nullable");
                        }
                    }
                    p.SetValue(destination, kv.Value == DBNull.Value ? default(T) : kv.Value, null);
                }
            }
        }
    }

    //...

}