using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Dapper;
using System.Data;
//using System.Data.SqlClient;
//using MySql.Data.MySqlClient;

namespace TestWork
{
    class Program
    {
        private enum AnEnum
        {
            A,
            B,
            C,
            D,
            E
        }

        //private static void DapperEnumValue(IDbConnection connection)
        //{
        //    // test passing as AsEnum, reading as int
        //    var v = (AnEnum)connection.QuerySingle<int>("select @v, @y, @z", new { v = AnEnum.B, y = (AnEnum?)AnEnum.B, z = (AnEnum?)null });
        //    Console.WriteLine(v.ToString());

        //    var args = new DynamicParameters();
        //    args.Add("v", AnEnum.B);
        //    args.Add("y", AnEnum.B);
        //    args.Add("z", null);
        //    v = (AnEnum)connection.QuerySingle<int>("select @v, @y, @z", args);
        //    Console.WriteLine(v.ToString());

        //    // test passing as int, reading as AnEnum
        //    var k = (int)connection.QuerySingle<AnEnum>("select @v, @y, @z", new { v = (int)AnEnum.B, y = (int?)(int)AnEnum.B, z = (int?)null });
        //    Console.WriteLine(k.ToString());

        //    args = new DynamicParameters();
        //    args.Add("v", (int)AnEnum.B);
        //    args.Add("y", (int)AnEnum.B);
        //    args.Add("z", null);
        //    k = (int)connection.QuerySingle<AnEnum>("select @v, @y, @z", args);
        //    Console.WriteLine(k.ToString());
        //}

        ////private static void TestDateTime(System.Data.Common.DbConnection connection)
        //private static void TestDateTime(IDbConnection connection)
        //{
        //    DateTime? now = DateTime.UtcNow;
        //    try { connection.Execute("DROP TABLE Persons"); }
        //    catch { }
        //    connection.Execute(@"CREATE TABLE Persons (id int not null, dob datetime null)");
        //    connection.Execute(@"INSERT Persons (id, dob) values (@id, @dob)", new { id = 7, dob = (DateTime?)null });
        //    connection.Execute(@"INSERT Persons (id, dob) values (@id, @dob)",
        //         new { id = 42, dob = now });

        //    var row = connection.QueryFirstOrDefault<Issue295Person>(
        //        "SELECT id, dob, dob as dob2 FROM Persons WHERE id=@id", new { id = 7 });
        //    if (row == null) Console.WriteLine("row is null");
        //    Console.WriteLine(row.Id);
        //    Console.WriteLine(row.DoB == null ? "DoB is null" : row.DoB.ToString());
        //    Console.WriteLine(row.DoB2 == null ? "DoB2 is null" : row.DoB2.ToString());
        //    Console.WriteLine(row.Id);

        //    row = connection.QueryFirstOrDefault<Issue295Person>(
        //        "SELECT id, dob FROM Persons WHERE id=@id", new { id = 42 });
        //    Console.WriteLine(row.Id);
        //    Console.WriteLine(row.DoB == null ? "DoB is null" : row.DoB.ToString());
        //    Console.WriteLine(row.DoB2 == null ? "DoB2 is null" : row.DoB2.ToString());
        //    Console.WriteLine(row.Id);
        //}

        class Issue295Person
        {
            public int Id { get; set; }
            public DateTime? DoB { get; set; }
            public DateTime? DoB2 { get; set; }
        }
        class Issue426_Test
        {
            public long Id { get; set; }
            public TimeSpan? Time { get; set; }
        }
        class SO36303462
        {
            public int Id { get; set; }
            public bool IsBold { get; set; }
        }

        //        public static void Issue295_NullableDateTime_MySql_Default()
        //        {
        //            using (var connection = Connection.GetMySqlConnection(true, false, false))
        //            {
        //                TestDateTime(connection);
        //            }
        //        }
        //        public static void Issue295_NullableDateTime_MySql_ConvertZeroDatetime()
        //        {
        //            using (var connection = Connection.GetMySqlConnection(true, true, false)) { TestDateTime(connection); }
        //        }

        //        public static void Issue426_SO34439033_DateTimeGainsTicks()
        //        {
        //            using (var connection = Connection.GetMySqlConnection(true, true, true))
        //            {
        //                try { connection.Execute("drop table Issue426_Test"); }
        //                catch { }
        //                try { connection.Execute("create table Issue426_Test (Id int not null, Time time not null)"); }
        //                catch { }

        //                const long ticks = 553440000000;
        //                const int Id = 426;

        //                var localObj = new Issue426_Test
        //                {
        //                    Id = Id,
        //                    Time = TimeSpan.FromTicks(ticks) // from code example
        //                };
        //                var effect = connection.Execute("replace into Issue426_Test values (@Id, @Time)", localObj);

        //                var dbObj = connection.Query<Issue426_Test>("select * from Issue426_Test where Id = @id", new { id = Id }).Single();
        //                Console.WriteLine("{0}-->{1}", dbObj.Id, Id);
        //                Console.WriteLine("{0}-->{1}", dbObj.Time.Value.Ticks, ticks);
        //            }
        //        }
        //        public static void SO36303462_Tinyint_Bools()
        //        {
        //            using (var connection = Connection.GetMySqlConnection(true, true, true))
        //            {
        //                try { connection.Execute("drop table SO36303462_Test"); }
        //                catch { }
        //                connection.Execute("create table SO36303462_Test (Id int not null, IsBold tinyint not null);");
        //                connection.Execute("insert SO36303462_Test (Id, IsBold) values (1,1);");
        //                connection.Execute("insert SO36303462_Test (Id, IsBold) values (2,0);");
        //                connection.Execute("insert SO36303462_Test (Id, IsBold) values (3,1);");

        //                var rows = connection.Query<SO36303462>("select * from SO36303462_Test").ToDictionary(x => x.Id);
        //                Console.WriteLine("{0} IsEqualTo {1}", rows.Count, 3);
        //                foreach (var r in rows)
        //                {
        //                    Console.WriteLine("idx:{2} key:{0} , value:{1}", r.Value.Id, r.Value.IsBold, r.Key);
        //                }
        //            }
        //        }

        //        public static void TestListOfAnsiStrings()
        //        {
        //            using (var connection = Connection.GetMySqlConnection(true))
        //            {
        //                var results = connection.Query<string>("select * from (select 'a' str union select 'b' union select 'c') X where str in @strings",
        //                    new
        //                    {
        //                        strings = new[] 
        //                        {
        //                            new DbString { IsAnsi = true, Value = "a" },
        //                            new DbString { IsAnsi = true, Value = "b" }
        //                        }
        //                    }).ToList();

        //                Console.WriteLine("results.Count:{0}", results.Count);
        //                results.Sort(); //默认比较器进行排序
        //                results.ForEach(x => Console.WriteLine(x));
        //            }
        //        }

        static void Main(string[] args)
        {
            //using (var connection = Connection.GetMySqlConnection())
            //{
            //    DapperEnumValue(connection);
            //}

            //Issue295_NullableDateTime_MySql_Default();
            //Issue295_NullableDateTime_MySql_ConvertZeroDatetime();

            //Issue426_SO34439033_DateTimeGainsTicks();

            //SO36303462_Tinyint_Bools();

            //TestListOfAnsiStrings();

            //            var mysql = new NetStar.Common.MySqlHelp();
            //            var param = new
            //            {
            //                username = "zzl",
            //                userpwd = "123123",
            //                realname = "李四",
            //                gender = 1,
            //                cellphone = "18855556666",
            //                adduser = "admin",
            //                addtime = DateTime.Now
            //            };

            //            var result = mysql.Execute(@"INSERT sys_user(
            //                                            username,
            //                                            userpwd,
            //                                            realname,
            //                                            gender,
            //                                            cellphone,
            //                                            adduser,
            //                                            addtime
            //                                        ) values (
            //                                            @username,
            //                                            @userpwd,
            //                                            @realname,
            //                                            @gender,
            //                                            @cellphone,
            //                                            @adduser,
            //                                            @addtime
            //                                        )", param);

            var kk = NetStar.Tools.CookiesHelp.GetCookiesDomain();

            var result = NetStar.Tools.JsonUtils.JsonSerializer(kk);
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}