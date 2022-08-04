using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DBHelper
{
    public class SqlServerHelper
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string connectionString = ConfigurationManager.ConnectionStrings["SQLSERVER_CONNECTION"].ToString();

        #region 执行SQL语句或存储过程，返回影响的行数

        /// <summary>
        /// 执行不带参数的SQL语句，返回影响的行数
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public static int ExecuteSqlNonQuery(string sqlString)
        {
            return ExecuteSqlNonQuery(sqlString, null);
        }

        /// <summary>
        /// 执行带参数的SQL语句，返回影响的行数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSqlNonQuery(string SQLString, params SqlParameter[] sqlParameters)
        {
            return ExecuteNonQuery(SQLString, CommandType.Text, sqlParameters);
        }

        /// <summary>
        /// 执行不带参数的存储过程，返回影响的行数
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        public static int ExecuteProcedureNonQuery(string procName)
        {
            return ExecuteProcedureNonQuery(procName, null);
        }

        /// <summary>
        /// 执行带参数的存储过程，返回影响的行数
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="sqlParameters">存储过程参数</param>
        /// <returns></returns>
        public static int ExecuteProcedureNonQuery(string procName,params SqlParameter[] sqlParameters)
        {
            return ExecuteNonQuery(procName, CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 执行SQL或者存储过程，返回影响的行数
        /// </summary>
        /// <param name="name">SQL语句或存储过程名称</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="sqlParameters">参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string name, CommandType commandType, params SqlParameter[] sqlParameters)
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
                    catch (Exception e)
                    {
                        throw e;
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
        /// <param name="sqlString">SQL语句</param>
        /// <returns></returns>
        public static DataSet ExecuteSQLQuery(string sqlString)
        {
            return ExecuteSQLQuery(sqlString, null);
        }

        /// <summary>
        /// 执行带参数的SQL语句，返回DataSet数据集
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="sqlParameters">参数</param>
        /// <returns></returns>
        public static DataSet ExecuteSQLQuery(string sqlString, params SqlParameter[] sqlParameters)
        {
            return ExecuteQuery(sqlString, CommandType.Text, sqlParameters);
        }

        /// <summary>
        /// 执行不带参数的存储过程，返回DataSet数据集
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        public static DataSet ExecuteProcedureQuery(string procName)
        {
            return ExecuteProcedureQuery(procName, null);
        }

        /// <summary>
        /// 执行带参数的存储过程，返回DataSet数据集
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="sqlParameters">参数</param>
        /// <returns></returns>
        public static DataSet ExecuteProcedureQuery(string procName,params SqlParameter[] sqlParameters)
        {
            return ExecuteQuery(procName, CommandType.StoredProcedure, sqlParameters);
        }

        /// <summary>
        /// 执行SQL或者存储过程，返回DataSet数据集
        /// </summary>
        /// <param name="name">SQL语句或存储过程名称</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="sqlParameters">参数</param>
        /// <returns></returns>
        public static DataSet ExecuteQuery(string name, CommandType commandType, params SqlParameter[] sqlParameters)
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
                    catch (Exception e)
                    {
                        throw e;
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
