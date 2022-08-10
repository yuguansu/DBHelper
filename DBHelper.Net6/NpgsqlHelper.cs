using Npgsql;
using System.Data;
using System.Text;

namespace DBHelper
{
    /// <summary>
    /// Helper class that makes it easier to work with the provider.
    /// </summary>
    public static class NpgsqlHelper
    {
        enum CharClass : byte
        {
            None,
            Quote,
            Backslash
        }

        private static string stringOfBackslashChars = "\u005c\u00a5\u0160\u20a9\u2216\ufe68\uff3c";
        private static string stringOfQuoteChars = "\u0022\u0027\u0060\u00b4\u02b9\u02ba\u02bb\u02bc\u02c8\u02ca\u02cb\u02d9\u0300\u0301\u2018\u2019\u201a\u2032\u2035\u275b\u275c\uff07";

        private static CharClass[] charClassArray = makeCharClassArray();

        #region ExecuteNonQuery

        /// <summary>
        /// Executes a single command against a Npgsql database.  The <see cref="NpgsqlConnection"/> is assumed to be
        /// open when the method is called and remains open after the method completes.
        /// </summary>
        /// <param name="connection"><see cref="NpgsqlConnection"/> object to use</param>
        /// <param name="commandText">Npgsql command to be executed</param>
        /// <param name="commandParameters">Array of <see cref="NpgsqlParameter"/> objects to use with the command.</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(this NpgsqlConnection connection, string commandText, params NpgsqlParameter[] commandParameters)
        {
            //create a command and prepare it for execution
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = commandText;
            cmd.CommandType = CommandType.Text;

            if (commandParameters != null)
                foreach (NpgsqlParameter p in commandParameters)
                    cmd.Parameters.Add(p);

            int result = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();

            return result;
        }

        /// <summary>
        /// Executes a single command against a Npgsql database.  A new <see cref="NpgsqlConnection"/> is created
        /// using the <see cref="NpgsqlConnection.ConnectionString"/> given.
        /// </summary>
        /// <param name="connectionString"><see cref="NpgsqlConnection.ConnectionString"/> to use</param>
        /// <param name="commandText">Npgsql command to be executed</param>
        /// <param name="parms">Array of <see cref="NpgsqlParameter"/> objects to use with the command.</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connectionString, string commandText, params NpgsqlParameter[] parms)
        {
            //create & open a NpgsqlConnection, and dispose of it after we are done.
            using (NpgsqlConnection cn = new NpgsqlConnection(connectionString))
            {
                cn.Open();

                //call the overload that takes a connection in place of the connection string
                return ExecuteNonQuery(cn, commandText, parms);
            }
        }
        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// Executes a single Npgsql command and returns the first row of the resultset.  A new NpgsqlConnection object
        /// is created, opened, and closed during this method.
        /// </summary>
        /// <param name="connectionString">Settings to be used for the connection</param>
        /// <param name="commandText">Command to execute</param>
        /// <param name="parms">Parameters to use for the command</param>
        /// <returns>DataRow containing the first row of the resultset</returns>
        public static DataRow ExecuteDataRow(string connectionString, string commandText, params NpgsqlParameter[] parms)
        {
            DataSet ds = ExecuteDataset(connectionString, commandText, parms);
            if (ds == null) return null;
            if (ds.Tables.Count == 0) return null;
            if (ds.Tables[0].Rows.Count == 0) return null;
            return ds.Tables[0].Rows[0];
        }

        /// <summary>
        /// Executes a single Npgsql command and returns the resultset in a <see cref="DataSet"/>.  
        /// A new NpgsqlConnection object is created, opened, and closed during this method.
        /// </summary>
        /// <param name="connectionString">Settings to be used for the connection</param>
        /// <param name="commandText">Command to execute</param>
        /// <returns><see cref="DataSet"/> containing the resultset</returns>
        public static DataSet ExecuteDataset(string connectionString, string commandText)
        {
            //pass through the call providing null for the set of NpgsqlParameters
            return ExecuteDataset(connectionString, commandText, (NpgsqlParameter[])null);
        }

        /// <summary>
        /// Executes a single Npgsql command and returns the resultset in a <see cref="DataSet"/>.  
        /// A new NpgsqlConnection object is created, opened, and closed during this method.
        /// </summary>
        /// <param name="connectionString">Settings to be used for the connection</param>
        /// <param name="commandText">Command to execute</param>
        /// <param name="commandParameters">Parameters to use for the command</param>
        /// <returns><see cref="DataSet"/> containing the resultset</returns>
        public static DataSet ExecuteDataset(string connectionString, string commandText, params NpgsqlParameter[] commandParameters)
        {
            //create & open a NpgsqlConnection, and dispose of it after we are done.
            using (NpgsqlConnection cn = new NpgsqlConnection(connectionString))
            {
                cn.Open();

                //call the overload that takes a connection in place of the connection string
                return ExecuteDataset(cn, commandText, commandParameters);
            }
        }

        /// <summary>
        /// Executes a single Npgsql command and returns the resultset in a <see cref="DataSet"/>.  
        /// The state of the <see cref="NpgsqlConnection"/> object remains unchanged after execution
        /// of this method.
        /// </summary>
        /// <param name="connection"><see cref="NpgsqlConnection"/> object to use</param>
        /// <param name="commandText">Command to execute</param>
        /// <returns><see cref="DataSet"/> containing the resultset</returns>
        public static DataSet ExecuteDataset(this NpgsqlConnection connection, string commandText)
        {
            //pass through the call providing null for the set of NpgsqlParameters
            return ExecuteDataset(connection, commandText, (NpgsqlParameter[])null);
        }

        /// <summary>
        /// Executes a single Npgsql command and returns the resultset in a <see cref="DataSet"/>.  
        /// The state of the <see cref="NpgsqlConnection"/> object remains unchanged after execution
        /// of this method.
        /// </summary>
        /// <param name="connection"><see cref="NpgsqlConnection"/> object to use</param>
        /// <param name="commandText">Command to execute</param>
        /// <param name="commandParameters">Parameters to use for the command</param>
        /// <returns><see cref="DataSet"/> containing the resultset</returns>
        public static DataSet ExecuteDataset(this NpgsqlConnection connection, string commandText, params NpgsqlParameter[] commandParameters)
        {
            //create a command and prepare it for execution
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = commandText;
            cmd.CommandType = CommandType.Text;

            if (commandParameters != null)
                foreach (NpgsqlParameter p in commandParameters)
                    cmd.Parameters.Add(p);

            //create the DataAdapter & DataSet
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            //fill the DataSet using default values for DataTable names, etc.
            da.Fill(ds);

            // detach the NpgsqlParameters from the command object, so they can be used again.			
            cmd.Parameters.Clear();

            //return the dataset
            return ds;
        }

        /// <summary>
        /// Updates the given table with data from the given <see cref="DataSet"/>
        /// </summary>
        /// <param name="connectionString">Settings to use for the update</param>
        /// <param name="commandText">Command text to use for the update</param>
        /// <param name="ds"><see cref="DataSet"/> containing the new data to use in the update</param>
        /// <param name="tablename">Tablename in the dataset to update</param>
        public static void UpdateDataSet(string connectionString, string commandText, DataSet ds, string tablename)
        {
            NpgsqlConnection cn = new NpgsqlConnection(connectionString);
            cn.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(commandText, cn);
            NpgsqlCommandBuilder cb = new NpgsqlCommandBuilder(da);
            cb.ToString();
            da.Update(ds, tablename);
            cn.Close();
        }

        #endregion

        #region ExecuteDataReader

        /// <summary>
        /// Executes a single command against a Npgsql database, possibly inside an existing transaction.
        /// </summary>
        /// <param name="connection"><see cref="NpgsqlConnection"/> object to use for the command</param>
        /// <param name="transaction"><see cref="NpgsqlTransaction"/> object to use for the command</param>
        /// <param name="commandText">Command text to use</param>
        /// <param name="commandParameters">Array of <see cref="NpgsqlParameter"/> objects to use with the command</param>
        /// <param name="ExternalConn">True if the connection should be preserved, false if not</param>
        /// <returns><see cref="NpgsqlDataReader"/> object ready to read the results of the command</returns>
        private static NpgsqlDataReader ExecuteReader(this NpgsqlConnection connection, NpgsqlTransaction transaction, string commandText, NpgsqlParameter[] commandParameters, bool ExternalConn)
        {
            //create a command and prepare it for execution
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = connection;
            cmd.Transaction = transaction;
            cmd.CommandText = commandText;
            cmd.CommandType = CommandType.Text;

            if (commandParameters != null)
                foreach (NpgsqlParameter p in commandParameters)
                    cmd.Parameters.Add(p);

            //create a reader
            NpgsqlDataReader dr;

            // call ExecuteReader with the appropriate CommandBehavior
            if (ExternalConn)
            {
                dr = cmd.ExecuteReader();
            }
            else
            {
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }

            // detach the NpgsqlParameters from the command object, so they can be used again.
            cmd.Parameters.Clear();

            return dr;
        }

        /// <summary>
        /// Executes a single command against a Npgsql database.
        /// </summary>
        /// <param name="connectionString">Settings to use for this command</param>
        /// <param name="commandText">Command text to use</param>
        /// <returns><see cref="NpgsqlDataReader"/> object ready to read the results of the command</returns>
        public static NpgsqlDataReader ExecuteReader(string connectionString, string commandText)
        {
            //pass through the call providing null for the set of NpgsqlParameters
            return ExecuteReader(connectionString, commandText, (NpgsqlParameter[])null);
        }

        /// <summary>
        /// Executes a single command against a Npgsql database.
        /// </summary>
        /// <param name="connection"><see cref="NpgsqlConnection"/> object to use for the command</param>
        /// <param name="commandText">Command text to use</param>
        /// <returns><see cref="NpgsqlDataReader"/> object ready to read the results of the command</returns>
        public static NpgsqlDataReader ExecuteReader(this NpgsqlConnection connection, string commandText)
        {
            //pass through the call providing null for the set of NpgsqlParameters
            return ExecuteReader(connection, null, commandText, (NpgsqlParameter[])null, true);
        }

        /// <summary>
        /// Executes a single command against a Npgsql database.
        /// </summary>
        /// <param name="connectionString">Settings to use for this command</param>
        /// <param name="commandText">Command text to use</param>
        /// <param name="commandParameters">Array of <see cref="NpgsqlParameter"/> objects to use with the command</param>
        /// <returns><see cref="NpgsqlDataReader"/> object ready to read the results of the command</returns>
        public static NpgsqlDataReader ExecuteReader(string connectionString, string commandText, params NpgsqlParameter[] commandParameters)
        {
            //create & open a NpgsqlConnection
            NpgsqlConnection cn = new NpgsqlConnection(connectionString);
            cn.Open();

            //call the private overload that takes an internally owned connection in place of the connection string
            return ExecuteReader(cn, null, commandText, commandParameters, false);
        }

        /// <summary>
        /// Executes a single command against a Npgsql database.
        /// </summary>
        /// <param name="connection">Connection to use for the command</param>
        /// <param name="commandText">Command text to use</param>
        /// <param name="commandParameters">Array of <see cref="NpgsqlParameter"/> objects to use with the command</param>
        /// <returns><see cref="NpgsqlDataReader"/> object ready to read the results of the command</returns>
        public static NpgsqlDataReader ExecuteReader(this NpgsqlConnection connection, string commandText, params NpgsqlParameter[] commandParameters)
        {
            //call the private overload that takes an internally owned connection in place of the connection string
            return ExecuteReader(connection, null, commandText, commandParameters, true);
        }


        #endregion

        #region ExecuteScalar

        /// <summary>
        /// Execute a single command against a Npgsql database.
        /// </summary>
        /// <param name="connectionString">Settings to use for the update</param>
        /// <param name="commandText">Command text to use for the update</param>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        public static object ExecuteScalar(string connectionString, string commandText)
        {
            //pass through the call providing null for the set of NpgsqlParameters
            return ExecuteScalar(connectionString, commandText, (NpgsqlParameter[])null);
        }

        /// <summary>
        /// Execute a single command against a Npgsql database.
        /// </summary>
        /// <param name="connectionString">Settings to use for the command</param>
        /// <param name="commandText">Command text to use for the command</param>
        /// <param name="commandParameters">Parameters to use for the command</param>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        public static object ExecuteScalar(string connectionString, string commandText, params NpgsqlParameter[] commandParameters)
        {
            //create & open a NpgsqlConnection, and dispose of it after we are done.
            using (NpgsqlConnection cn = new NpgsqlConnection(connectionString))
            {
                cn.Open();

                //call the overload that takes a connection in place of the connection string
                return ExecuteScalar(cn, commandText, commandParameters);
            }
        }

        /// <summary>
        /// Execute a single command against a Npgsql database.
        /// </summary>
        /// <param name="connection"><see cref="NpgsqlConnection"/> object to use</param>
        /// <param name="commandText">Command text to use for the command</param>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        public static object ExecuteScalar(this NpgsqlConnection connection, string commandText)
        {
            //pass through the call providing null for the set of NpgsqlParameters
            return ExecuteScalar(connection, commandText, (NpgsqlParameter[])null);
        }

        /// <summary>
        /// Execute a single command against a Npgsql database.
        /// </summary>
        /// <param name="connection"><see cref="NpgsqlConnection"/> object to use</param>
        /// <param name="commandText">Command text to use for the command</param>
        /// <param name="commandParameters">Parameters to use for the command</param>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        public static object ExecuteScalar(this NpgsqlConnection connection, string commandText, params NpgsqlParameter[] commandParameters)
        {
            //create a command and prepare it for execution
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = commandText;
            cmd.CommandType = CommandType.Text;

            if (commandParameters != null)
                foreach (NpgsqlParameter p in commandParameters)
                    cmd.Parameters.Add(p);

            //execute the command & return the results
            object retval = cmd.ExecuteScalar();

            // detach the NpgsqlParameters from the command object, so they can be used again.
            cmd.Parameters.Clear();
            return retval;

        }

        #endregion

        #region Utility methods
        private static CharClass[] makeCharClassArray()
        {

            CharClass[] a = new CharClass[65536];
            foreach (char c in stringOfBackslashChars)
            {
                a[c] = CharClass.Backslash;
            }
            foreach (char c in stringOfQuoteChars)
            {
                a[c] = CharClass.Quote;
            }
            return a;
        }

        private static bool needsQuoting(string s)
        {
            foreach (char c in s)
            {
                if (charClassArray[c] != CharClass.None)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Escapes the string.
        /// </summary>
        /// <param name="value">The string to escape</param>
        /// <returns>The string with all quotes escaped.</returns>
        public static string EscapeString(string value)
        {
            if (!needsQuoting(value))
                return value;

            StringBuilder sb = new StringBuilder();

            foreach (char c in value)
            {
                CharClass charClass = charClassArray[c];
                if (charClass != CharClass.None)
                {
                    sb.Append("\\");
                }
                sb.Append(c);
            }
            return sb.ToString();
        }

        public static string DoubleQuoteString(string value)
        {
            if (!needsQuoting(value))
                return value;

            StringBuilder sb = new StringBuilder();
            foreach (char c in value)
            {
                CharClass charClass = charClassArray[c];
                if (charClass == CharClass.Quote)
                    sb.Append(c);
                else if (charClass == CharClass.Backslash)
                    sb.Append("\\");
                sb.Append(c);
            }
            return sb.ToString();
        }

        #endregion
    }
}

