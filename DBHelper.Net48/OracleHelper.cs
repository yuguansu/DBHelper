using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;


namespace DBHelper
{
    public class OracleHelper
    {
        #region 执行查询，返回DataTable对象-----------------------
        /// <summary>
        /// 执行存储过程，无参，返回DataTable
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        public static DataTable GetTableProc(string connectionString,string procName)
        {
            return (GetTable(connectionString,procName, null, CommandType.StoredProcedure));
        }
        /// <summary>
        /// 执行存储过程，有参，返回DataTable
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="procName">存储过程名称</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns></returns>
        public static DataTable GetTableProc(string connectionString, string procName, OracleParameter[] parameters)
        {
            return (GetTable(connectionString, procName, parameters, CommandType.StoredProcedure));
        }
        /// <summary>
        /// 执行SQL语句，无参，返回DataTable
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="strSQL">完整SQL语句</param>
        /// <returns></returns>
        public static DataTable GetTableSQL(string connectionString, string strSQL)
        {
            return GetTableSQL(connectionString, strSQL, null);
        }
        /// <summary>
        /// 执行SQL语句，有参，返回DataTable
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="strSQL">完整SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns></returns>
        public static DataTable GetTableSQL(string connectionString, string strSQL, OracleParameter[] parameters)
        {
            return GetTable(connectionString, strSQL, parameters, CommandType.Text);
        }
        /// <summary>
        /// 执行查询语句或存储过程，返回DataTable对象
        /// </summary>
        /// <param name="strSQL">sql语句</param>
        /// <param name="pas">参数数组</param>
        /// <param name="cmdtype">Command类型</param>
        /// <returns>DataTable对象</returns>
        public static DataTable GetTable(string connectionString, string strSQL, OracleParameter[] pas, CommandType cmdtype)
        {
            DataTable dt = new DataTable(); ;
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleDataAdapter da = new OracleDataAdapter(strSQL, conn);
                da.SelectCommand.CommandType = cmdtype;
                if (pas != null)
                {
                    da.SelectCommand.Parameters.AddRange(pas);
                }
                da.Fill(dt);
            }
            return dt;
        }
        #endregion

        #region 执行查询，返回DataSet对象-------------------------
        /// <summary>
        /// 执行存储过程，无参，返回DataSet
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        public static DataSet GetDataSetProc(string connectionString, string procName)
        {
            return (GetDataSet(connectionString, procName, null, CommandType.StoredProcedure));
        }
        /// <summary>
        /// 执行存储过程，有参，返回DataSet
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="procName">存储过程名称</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns></returns>
        public static DataSet GetDataSetProc(string connectionString, string procName, OracleParameter[] parameters)
        {
            return (GetDataSet(connectionString, procName, parameters, CommandType.StoredProcedure));
        }
        /// <summary>
        /// 执行SQL语句，无参，返回DataSet
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="strSQL">完整SQL语句</param>
        /// <returns></returns>
        public static DataSet GetDataSetSQL(string connectionString, string strSQL)
        {
            return GetDataSetSQL(connectionString, strSQL, null);
        }
        /// <summary>
        /// 执行SQL语句，有参，返回DataSet
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="strSQL">完整SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns></returns>
        public static DataSet GetDataSetSQL(string connectionString, string strSQL, OracleParameter[] parameters)
        {
            return GetDataSet(connectionString, strSQL, parameters, CommandType.Text);
        }
        /// <summary>
        /// 执行查询，返回DataSet对象
        /// </summary>
        /// <param name="strSQL">完整SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <param name="cmdtype">Command类型</param>
        /// <returns>DataSet对象</returns>
        public static DataSet GetDataSet(string connectionString, string strSQL, OracleParameter[] parameters, CommandType cmdtype)
        {
            DataSet dt = new DataSet();
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleDataAdapter da = new OracleDataAdapter(strSQL, conn);
                da.SelectCommand.CommandType = cmdtype;
                if (parameters != null)
                {
                    da.SelectCommand.Parameters.AddRange(parameters);
                }
                da.Fill(dt);
            }
            return dt;
        }
        #endregion

        #region 执行非查询存储过程和SQL语句-----------------------------
        /// <summary>
        /// 执行非查询的存储过程，无参，返回受影响行数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="ProcName">存储过程名称</param>
        /// <returns></returns>
        public static int ExcuteProc(string connectionString, string ProcName)
        {
            return ExcuteSQL(connectionString, ProcName, null, CommandType.StoredProcedure);
        }
        /// <summary>
        /// 执行非查询的存储过程，有参，返回受影响行数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="ProcName">存储过程名称</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns></returns>
        public static int ExcuteProc(string connectionString, string ProcName, OracleParameter[] parameters)
        {
            return ExcuteSQL(connectionString, ProcName, parameters, CommandType.StoredProcedure);
        }
        /// <summary>
        /// 执行非查询的SQL，无参，返回受影响行数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="strSQL">完整SQL语句</param>
        /// <returns></returns>
        public static int ExcuteSQL(string connectionString, string strSQL)
        {
            return ExcuteSQL(connectionString, strSQL, null);
        }
        /// <summary>
        /// 执行非查询的SQL，有参，返回受影响行数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="strSQL">完整SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns></returns>
        public static int ExcuteSQL(string connectionString, string strSQL, OracleParameter[] parameters)
        {
            return ExcuteSQL(connectionString, strSQL, parameters, CommandType.Text);
        }
        /// <summary>
        /// 执行非查询存储过程和SQL语句:增、删、改
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="strSQL">完整SQL语句</param>
        /// <param name="parameters">参数列表，没有参数填入null</param>
        /// <param name="cmdType">Command类型,Text,StoredProcedure</param>
        /// <returns>返回影响行数</returns>
        public static int ExcuteSQL(string connectionString, string strSQL, OracleParameter[] parameters, CommandType cmdType)
        {
            int i = 0;
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand(strSQL, conn);
                cmd.CommandType = cmdType;
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                conn.Open();
                i = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return i;
        }
        #endregion

        #region 执行查询返回第一行，第一列---------------------------------
        /// <summary>
        /// 返回第一行，第一列
        /// </summary>
        /// <param name="strSQL">完整SQL语句</param>
        /// <returns></returns>
        public static int ExcuteScalarSQL(string connectionString, string strSQL)
        {
            return ExcuteScalarSQL(connectionString, strSQL, null);
        }
        /// <summary>
        /// 执行查询返回第一行，第一列
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="strSQL">完整SQL语句</param>
        /// <param name="parameters">SQL参数列表</param>
        /// <returns></returns>
        public static int ExcuteScalarSQL(string connectionString, string strSQL, OracleParameter[] parameters)
        {
            return ExcuteScalarSQL(connectionString, strSQL, parameters, CommandType.Text);
        }
        public static int ExcuteScalarProc(string connectionString, string strSQL, OracleParameter[] parameters)
        {
            return ExcuteScalarSQL(connectionString, strSQL, parameters, CommandType.StoredProcedure);
        }
        /// <summary>
        /// 执行SQL语句，返回第一行，第一列
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="strSQL">要执行的SQL语句</param>
        /// <param name="parameters">参数列表，没有参数填入null</param>
        /// <returns>返回影响行数</returns>
        public static int ExcuteScalarSQL(string connectionString, string strSQL, OracleParameter[] parameters, CommandType cmdType)
        {
            int i = 0;
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand(strSQL, conn);
                cmd.CommandType = cmdType;
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                conn.Open();
                i = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }
            return i;
        }
        #endregion

        #region 查询获取单个值------------------------------------
        /// <summary>
        /// 调用不带参数的存储过程获取单个值
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="ProcName">存储过程名称</param>
        /// <returns></returns>
        public static object GetObjectByProc(string connectionString, string ProcName)
        {
            return GetObjectByProc(connectionString, ProcName, null);
        }
        /// <summary>
        /// 调用带参数的存储过程获取单个值
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="ProcName">存储过程名称</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns></returns>
        public static object GetObjectByProc(string connectionString, string ProcName, OracleParameter[] parameters)
        {
            return GetObject(connectionString, ProcName, parameters, CommandType.StoredProcedure);
        }
        /// <summary>
        /// 根据sql语句获取单个值
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="strSQL">完整SQL</param>
        /// <returns></returns>
        public static object GetObject(string connectionString, string strSQL)
        {
            return GetObject(connectionString, strSQL, null);
        }
        /// <summary>
        /// 根据sql语句 和 参数数组获取单个值
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="strSQL">完整SQL</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns></returns>
        public static object GetObject(string connectionString, string strSQL, OracleParameter[] parameters)
        {
            return GetObject(connectionString, strSQL, parameters, CommandType.Text);
        }

        /// <summary>
        /// 执行SQL语句，返回首行首列
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="strSQL">要执行的SQL语句</param>
        /// <param name="parameters">参数列表，没有参数填入null</param>
        /// <returns>返回的首行首列</returns>
        public static object GetObject(string connectionString, string strSQL, OracleParameter[] parameters, CommandType cmdtype)
        {
            object o;
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand(strSQL, conn);
                cmd.CommandType = cmdtype;
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                conn.Open();
                o = cmd.ExecuteScalar();
                conn.Close();
            }
            return o;
        }
        #endregion

        #region 查询获取DataReader------------------------------------
        /// <summary>
        /// 调用不带参数的存储过程，返回DataReader对象
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="procName">存储过程名称</param>
        /// <returns>DataReader对象</returns>
        public static OracleDataReader GetReaderByProc(string connectionString, string procName)
        {
            return GetReaderByProc(connectionString, procName, null);
        }
        /// <summary>
        /// 调用带有参数的存储过程，返回DataReader对象
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="procName">存储过程名</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>DataReader对象</returns>
        public static OracleDataReader GetReaderByProc(string connectionString, string procName, OracleParameter[] parameters)
        {
            return GetReader(connectionString, procName, parameters, CommandType.StoredProcedure);
        }
        /// <summary>
        /// 根据sql语句返回DataReader对象
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="strSQL">sql语句</param>
        /// <returns>DataReader对象</returns>
        public static OracleDataReader GetReader(string connectionString, string strSQL)
        {
            return GetReader(connectionString, strSQL, null);
        }
        /// <summary>
        /// 根据sql语句和参数返回DataReader对象
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="strSQL">sql语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>DataReader对象</returns>
        public static OracleDataReader GetReader(string connectionString, string strSQL, OracleParameter[] parameters)
        {
            return GetReader(connectionString, strSQL, parameters, CommandType.Text);
        }
        /// <summary>
        /// 查询SQL语句获取DataReader
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="strSQL">查询的SQL语句</param>
        /// <param name="parameters">参数列表，没有参数填入null</param>
        /// <returns>查询到的DataReader（关闭该对象的时候，自动关闭连接）</returns>
        public static OracleDataReader GetReader(string connectionString, string strSQL, OracleParameter[] parameters, CommandType cmdtype)
        {
            OracleDataReader sqldr;
            OracleConnection conn = new OracleConnection(connectionString);
            OracleCommand cmd = new OracleCommand(strSQL, conn);
            cmd.CommandType = cmdtype;
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }
            conn.Open();
            //CommandBehavior.CloseConnection的作用是如果关联的DataReader对象关闭，则连接自动关闭
            sqldr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return sqldr;
        }
        #endregion
        
        /*
        #region 批量插入数据---------------------------------------------

        /// <summary>
        /// 往数据库中批量插入数据
        /// </summary>
        /// <param name="sourceDt">数据源表</param>
        /// <param name="targetTable">服务器上目标表</param>
        public static void BulkToDB(DataTable sourceDt, string targetTable)
        {
            OracleConnection conn = new OracleConnection(strConn);
            SqlBulkCopy bulkCopy = new SqlBulkCopy(conn);   //用其它源的数据有效批量加载sql server表中
            bulkCopy.DestinationTableName = targetTable;    //服务器上目标表的名称
            bulkCopy.BatchSize = sourceDt.Rows.Count;   //每一批次中的行数

            try
            {
                conn.Open();
                if (sourceDt != null && sourceDt.Rows.Count != 0)
                    bulkCopy.WriteToServer(sourceDt);   //将提供的数据源中的所有行复制到目标表中
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                if (bulkCopy != null)
                    bulkCopy.Close();
            }

        }

        #endregion
    */

    }
}
