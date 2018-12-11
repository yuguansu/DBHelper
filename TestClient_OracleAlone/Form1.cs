using System;
using System.Windows.Forms;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace TestClientStandalone
{
    public partial class Form1 : Form
    {
        public static string strConn = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)"
            + "(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=MES)));"
            + "User Id=SJ;Password=SJ; ";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            Timer t1 = new Timer();
            t1.Start();
            using (OracleConnection oraConn = new OracleConnection(strConn))
            {
                try
                {
                    string sSQL = txtSQL.Text;

                    oraConn.Open();
                    using (OracleCommand oraCmd = oraConn.CreateCommand())
                    {
                        oraCmd.CommandText = sSQL;
                        OracleDataAdapter oraAdapter = new OracleDataAdapter(oraCmd);
                        DataSet ds = new DataSet();
                        oraAdapter.Fill(ds);
                        dgvData.DataSource = ds.Tables[0];
                    }
                    oraConn.Close();
                }
                catch (Exception ex)
                {
                    rtxMsg.AppendText(ex.Message + Environment.NewLine);
                }
                finally
                {
                    oraConn.Dispose();
                }
            }
            t1.Stop();
            rtxMsg.AppendText(t1.ToString() + Environment.NewLine);
        }
    }
}
