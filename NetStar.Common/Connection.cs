using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data.SqlClient;

namespace NetStar.Common
{
    internal class Connection
    {

        public Connection()
        {
            DbType = DBEnum.DbType.MYSQL;
        }
        public Connection(string connstr)
        {
            ConnectionString = connstr;
            DbType = DBEnum.DbType.MYSQL;
        }
        public Connection(DBEnum.DbType dbtype = DBEnum.DbType.MYSQL)
        {
            DbType = dbtype;
        }

        public DBEnum.DbType DbType;
        public bool OpenDB = true;
        public string ConnectionString { set; get; }

        public System.Data.IDbConnection GetSqlConnection()
        {
            if (DbType == DBEnum.DbType.MSSQL)
                return GetSqlServerConnection();
            if (DbType == DBEnum.DbType.MSSQL)
                return GetMySqlConnection();

            return GetMySqlConnection();
        }

        /// <summary>
        /// 创建SqlServer对象实例
        /// </summary>
        private SqlConnection GetSqlServerConnection()
        {
            var connection = new SqlConnection(ConnectionString ?? GetConfigConnectionString());
            if (OpenDB) connection.Open();
            return connection;
        }
        /// <summary>
        /// 创建MySqlConnection对象实例
        /// </summary>
        /// <param name="convertZeroDatetime"></param>
        /// <param name="allowZeroDatetime"></param>
        private MySqlConnection GetMySqlConnection(bool convertZeroDatetime = false, bool allowZeroDatetime = false)
        {
            var connectionBuilder = new MySqlConnectionStringBuilder(ConnectionString ?? GetConfigConnectionString());
            connectionBuilder.AllowZeroDateTime = allowZeroDatetime;
            connectionBuilder.ConvertZeroDateTime = convertZeroDatetime;

            var connection = new MySqlConnection(connectionBuilder.ConnectionString);
            if (OpenDB) connection.Open();
            return connection;
        }

        private const string user = "root";
        private const string pwd = "123123";
        private const string ipadd = "127.0.0.1";

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        private string GetConfigConnectionString()
        {
            string key = NetStar.Config.KeyCenter.KeyMySQLConnection;
            string defaultconstr = string.Format("Database=netstar;Data Source={2};User Id={0};Password={1};", user, pwd, ipadd);

            ConnectionStringSettings consetting = ConfigurationManager.ConnectionStrings[key];
            return consetting != null ? (consetting.ConnectionString ?? defaultconstr) : defaultconstr;
        }
    }
}