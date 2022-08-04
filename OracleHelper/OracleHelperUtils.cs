﻿using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;


namespace OracleHelper
{
    public class OracleHelperUtils
    {
        //连接字符串
        static string strConn = ConfigurationManager.ConnectionStrings["oraConn"].ToString();
        
        #region 执行查询，返回DataTable对象-----------------------

        public static DataTable GetTableProc(string procName)
        {
            return (GetTable(procName, null, CommandType.StoredProcedure));
        }
        public static DataTable GetTableProc(string procName, OracleParameter[] pas)
        {
            return (GetTable(procName, pas, CommandType.StoredProcedure));
        }
        public static DataTable GetTableSQL(string strSQL)
        {
            return GetTableSQL(strSQL, null);
        }
        public static DataTable GetTableSQL(string strSQL, OracleParameter[] pas)
        {
            return GetTable(strSQL, pas, CommandType.Text);
        }
        /// <summary>
        /// 执行查询，返回DataTable对象
        /// </summary>
        /// <param name="strSQL">sql语句</param>
        /// <param name="pas">参数数组</param>
        /// <param name="cmdtype">Command类型</param>
        /// <returns>DataTable对象</returns>
        public static DataTable GetTable(string strSQL, OracleParameter[] pas, CommandType cmdtype)
        {
            DataTable dt = new DataTable(); ;
            using (OracleConnection conn = new OracleConnection(strConn))
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
        public static DataSet getDataSetProc(string procName)
        {
            return (GetDataSet(procName, null, CommandType.StoredProcedure));
        }
        public static DataSet getDataSetProc(string procName, OracleParameter[] pas)
        {
            return (GetDataSet(procName, pas, CommandType.StoredProcedure));
        }


        public static DataSet GetDataSetSQL(string strSQL)
        {
            return GetDataSetSQL(strSQL, null);
        }

        public static DataSet GetDataSetSQL(string strSQL, OracleParameter[] pas)
        {
            return GetDataSet(strSQL, pas, CommandType.Text);
        }
        /// <summary>
        /// 执行查询，返回DataSet对象
        /// </summary>
        /// <param name="strSQL">sql语句</param>
        /// <param name="pas">参数数组</param>
        /// <param name="cmdtype">Command类型</param>
        /// <returns>DataSet对象</returns>
        public static DataSet GetDataSet(string strSQL, OracleParameter[] pas, CommandType cmdtype)
        {
            DataSet dt = new DataSet();
            using (OracleConnection conn = new OracleConnection(strConn))
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
        
        #region 执行非查询存储过程和SQL语句-----------------------------

        public static int ExcuteProc(string ProcName)
        {
            return ExcuteSQL(ProcName, null, CommandType.StoredProcedure);
        }

        public static int ExcuteProc(string ProcName, OracleParameter[] pars)
        {
            return ExcuteSQL(ProcName, pars, CommandType.StoredProcedure);
        }

        public static int ExcuteSQL(string strSQL)
        {
            return ExcuteSQL(strSQL, null);
        }

        public static int ExcuteSQL(string strSQL, OracleParameter[] paras)
        {
            return ExcuteSQL(strSQL, paras, CommandType.Text);
        }

        /// 执行非查询存储过程和SQL语句
        /// 增、删、改
        /// </summary>
        /// <param name="strSQL">要执行的SQL语句</param>
        /// <param name="paras">参数列表，没有参数填入null</param>
        /// <param name="cmdType">Command类型</param>
        /// <returns>返回影响行数</returns>
        public static int ExcuteSQL(string strSQL, OracleParameter[] paras, CommandType cmdType)
        {
            int i = 0;
            using (OracleConnection conn = new OracleConnection(strConn))
            {
                OracleCommand cmd = new OracleCommand(strSQL, conn);
                cmd.CommandType = cmdType;
                if (paras != null)
                {
                    cmd.Parameters.AddRange(paras);
                }
                conn.Open();
                i = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return i;

        }


        #endregion
        
        #region 执行查询返回第一行，第一列---------------------------------

        public static int ExcuteScalarSQL(string strSQL)
        {
            return ExcuteScalarSQL(strSQL, null);
        }

        public static int ExcuteScalarSQL(string strSQL, OracleParameter[] paras)
        {
            return ExcuteScalarSQL(strSQL, paras, CommandType.Text);
        }
        public static int ExcuteScalarProc(string strSQL, OracleParameter[] paras)
        {
            return ExcuteScalarSQL(strSQL, paras, CommandType.StoredProcedure);
        }
        /// <summary>
        /// 执行SQL语句，返回第一行，第一列
        /// </summary>
        /// <param name="strSQL">要执行的SQL语句</param>
        /// <param name="paras">参数列表，没有参数填入null</param>
        /// <returns>返回影响行数</returns>
        public static int ExcuteScalarSQL(string strSQL, OracleParameter[] paras, CommandType cmdType)
        {
            int i = 0;
            using (OracleConnection conn = new OracleConnection(strConn))
            {
                OracleCommand cmd = new OracleCommand(strSQL, conn);
                cmd.CommandType = cmdType;
                if (paras != null)
                {
                    cmd.Parameters.AddRange(paras);
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
        /// <param name="ProcName"></param>
        /// <returns></returns>
        public static object GetObjectByProc(string ProcName)
        {
            return GetObjectByProc(ProcName, null);
        }
        /// <summary>
        /// 调用带参数的存储过程获取单个值
        /// </summary>
        /// <param name="ProcName"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static object GetObjectByProc(string ProcName, OracleParameter[] paras)
        {
            return GetObject(ProcName, paras, CommandType.StoredProcedure);
        }
        /// <summary>
        /// 根据sql语句获取单个值
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static object GetObject(string strSQL)
        {
            return GetObject(strSQL, null);
        }
        /// <summary>
        /// 根据sql语句 和 参数数组获取单个值
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static object GetObject(string strSQL, OracleParameter[] paras)
        {
            return GetObject(strSQL, paras, CommandType.Text);
        }

        /// <summary>
        /// 执行SQL语句，返回首行首列
        /// </summary>
        /// <param name="strSQL">要执行的SQL语句</param>
        /// <param name="paras">参数列表，没有参数填入null</param>
        /// <returns>返回的首行首列</returns>
        public static object GetObject(string strSQL, OracleParameter[] paras, CommandType cmdtype)
        {
            object o = null;
            using (OracleConnection conn = new OracleConnection(strConn))
            {
                OracleCommand cmd = new OracleCommand(strSQL, conn);
                cmd.CommandType = cmdtype;
                if (paras != null)
                {
                    cmd.Parameters.AddRange(paras);

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
        /// <param name="procName">存储过程名称</param>
        /// <returns>DataReader对象</returns>
        public static OracleDataReader GetReaderByProc(string procName)
        {
            return GetReaderByProc(procName, null);
        }
        /// <summary>
        /// 调用带有参数的存储过程，返回DataReader对象
        /// </summary>
        /// <param name="procName">存储过程名</param>
        /// <param name="paras">参数数组</param>
        /// <returns>DataReader对象</returns>
        public static OracleDataReader GetReaderByProc(string procName, OracleParameter[] paras)
        {
            return GetReader(procName, paras, CommandType.StoredProcedure);
        }
        /// <summary>
        /// 根据sql语句返回DataReader对象
        /// </summary>
        /// <param name="strSQL">sql语句</param>
        /// <returns>DataReader对象</returns>
        public static OracleDataReader GetReader(string strSQL)
        {
            return GetReader(strSQL, null);
        }
        /// <summary>
        /// 根据sql语句和参数返回DataReader对象
        /// </summary>
        /// <param name="strSQL">sql语句</param>
        /// <param name="paras">参数数组</param>
        /// <returns>DataReader对象</returns>
        public static OracleDataReader GetReader(string strSQL, OracleParameter[] paras)
        {
            return GetReader(strSQL, paras, CommandType.Text);
        }
        /// <summary>
        /// 查询SQL语句获取DataReader
        /// </summary>
        /// <param name="strSQL">查询的SQL语句</param>
        /// <param name="paras">参数列表，没有参数填入null</param>
        /// <returns>查询到的DataReader（关闭该对象的时候，自动关闭连接）</returns>
        public static OracleDataReader GetReader(string strSQL, OracleParameter[] paras, CommandType cmdtype)
        {
            OracleDataReader sqldr = null;
            OracleConnection conn = new OracleConnection(strConn);
            OracleCommand cmd = new OracleCommand(strSQL, conn);
            cmd.CommandType = cmdtype;
            if (paras != null)
            {
                cmd.Parameters.AddRange(paras);
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
