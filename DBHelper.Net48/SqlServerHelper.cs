using System;
using System.Data;
using System.Data.SqlClient;

namespace DBHelper
{
    public class SqlServerHelper
    {
        #region 执行SQL语句或存储过程，返回影响的行数
        /// <summary>
        /// 执行不带参数的SQL语句，返回影响的行数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static int ExecuteSqlNonQuery(string connectionString, string strSQL)
        {
            return ExecuteSqlNonQuery(connectionString, strSQL, null);
        }

        /// <summary>
        /// 执行带参数的SQL语句，返回影响的行数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="strSQL">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSqlNonQuery(string connectionString, string strSQL, params SqlParameter[] sqlParameters)
        {
            return ExecuteNonQuery(connectionString, strSQL, CommandType.Text, sqlParameters);
        }

        /// <summary>
        /// 执行不带参数的存储过程，返回影响的行数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        public static int ExecuteProcedureNonQuery(string connectionString, string procName)
        {
            return ExecuteProcedureNonQuery(connectionString, procName, null);
        }

        /// <summary>
        /// 执行带参数的存储过程，返回影响的行数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="procName">存储过程名称</param>
        /// <param name="sqlParameters">存储过程参数</param>
        /// <returns></returns>
        public static int ExecuteProcedureNonQuery(string connectionString, string procName, params SqlParameter[] sqlParameters)
        {
            return ExecuteNonQuery(connectionString, procName, CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 执行SQL或者存储过程，返回影响的行数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="name">SQL语句或存储过程名称</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="sqlParameters">参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connectionString, string name, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(name, sqlConnection))
                {
                    try
                    {
                        command.CommandType = commandType;
                        if (sqlParameters != null)
                        {
                            foreach (SqlParameter parameter in sqlParameters)
                            {
                                if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                                    (parameter.Value == null))
                                {
                                    parameter.Value = DBNull.Value;
                                }
                                command.Parameters.Add(parameter);
                            }
                        }
                        int rows = command.ExecuteNonQuery();
                        return rows;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        command.Dispose();
                        sqlConnection.Close();
                    }
                }
            }
        }

        #endregion

        #region 执行SQL语句或存储过程,返回DataSet

        /// <summary>
        /// 执行不带参数的SQL语句，返回DataSet数据集
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="strSQL">SQL语句</param>
        /// <returns></returns>
        public static DataSet ExecuteSQLQuery(string connectionString, string strSQL)
        {
            return ExecuteSQLQuery(connectionString, strSQL, null);
        }

        /// <summary>
        /// 执行带参数的SQL语句，返回DataSet数据集
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="sqlParameters">参数</param>
        /// <returns></returns>
        public static DataSet ExecuteSQLQuery(string connectionString, string strSQL, params SqlParameter[] sqlParameters)
        {
            return ExecuteQuery(connectionString, strSQL, CommandType.Text, sqlParameters);
        }

        /// <summary>
        /// 执行不带参数的存储过程，返回DataSet数据集
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        public static DataSet ExecuteProcedureQuery(string connectionString, string procName)
        {
            return ExecuteProcedureQuery(connectionString, procName, null);
        }

        /// <summary>
        /// 执行带参数的存储过程，返回DataSet数据集
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="sqlParameters">参数</param>
        /// <returns></returns>
        public static DataSet ExecuteProcedureQuery(string connectionString, string procName, params SqlParameter[] sqlParameters)
        {
            return ExecuteQuery(connectionString, procName, CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 执行SQL或者存储过程，返回DataSet数据集
        /// </summary>
        /// <param name="name">SQL语句或存储过程名称</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="sqlParameters">参数</param>
        /// <returns></returns>
        public static DataSet ExecuteQuery(string connectionString, string name, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                //command初始化
                using (SqlCommand command = new SqlCommand(name, sqlConnection))
                {
                    try
                    {
                        //指定command类型
                        command.CommandType = commandType;
                        //指定command参数
                        if (sqlParameters != null)
                        {
                            foreach (SqlParameter parameter in sqlParameters)
                            {
                                if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                                    (parameter.Value == null))
                                {
                                    parameter.Value = DBNull.Value;
                                }
                                command.Parameters.Add(parameter);
                            }
                        }
                        DataSet dataSet = new DataSet();
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.SelectCommand = command;
                        adapter.Fill(dataSet);
                        return dataSet;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        command.Dispose();
                        sqlConnection.Close();
                    }
                }
            }
        }
        #endregion
    }
}
