using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using DBHelper;

namespace TestClient_SqlSeverAlone
{
    public partial class Form1 : Form
    {
        SqlConnectionStringBuilder consb = new SqlConnectionStringBuilder();
        //SqlConnection("server=172.17.200.168;database=EAP;uid=mes_dev;pwd=mes_dev");
        SqlConnection con = new SqlConnection();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (btnConnect.Text == "连接")
            {
                try
                {
                    consb.DataSource = "172.17.200.168";
                    consb.InitialCatalog = "EAP";
                    consb.UserID = "mes_dev";
                    consb.Password = "mes_dev";
                    con = new SqlConnection(consb.ToString());

                    txtLog.Text += "SQL Server connect OK." + Environment.NewLine;
                    btnConnect.Text = "断开";
                }
                catch (Exception ex)
                {
                    txtLog.Text += ex.Message + Environment.NewLine;
                }
            }
            else
            {
                try
                {
                    con.Close();
                    txtLog.Text += "SQL Server disconnect OK." + Environment.NewLine;
                    btnConnect.Text = "连接";
                }
                catch (Exception ex)
                {
                    txtLog.Text += ex.Message + Environment.NewLine;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                string sql = "select * from ees_user";
                SqlCommand cmd = new SqlCommand(sql,con);
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);//执行查询并加载数据到DataTable中
                dgv.DataSource = dt;
            }
            catch (Exception ex)
            {
                txtLog.Text += ex.Message + Environment.NewLine;
            }
            

        }

        private void btnProcedure_Click(object sender, EventArgs e)
        {

        }

        private void btnSelectHelper_Click(object sender, EventArgs e)
        {
            string sql = "select * from ees_ss_user";
            DataSet ds = new DataSet();
            ds = SqlServerHelper.ExecuteSQLQuery(sql);
            if (ds != null)
            {
                dgv.DataSource = ds.Tables[0];
            }
        }

        private void btnProcHelper_Click(object sender, EventArgs e)
        {
            try
            {
                //PROC_EES_S_PDLINE_SELECT
                DataSet ds = new DataSet();
                ds = SqlServerHelper.ExecuteProcedureQuery("PROC_EES_S_PDLINE_SELECT");
                if (ds != null)
                {
                    dgv.DataSource = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
