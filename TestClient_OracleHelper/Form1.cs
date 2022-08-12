using System;
using System.Data;
using System.Windows.Forms;
using DBHelper;

namespace TestClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        LogHelper.LogHelperUtils log = new LogHelper.LogHelperUtils("TestClient", "OracleHelper");
        int sLogType = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            log.WriteLogStart();
            log.WriteLog("[Form load]");
            log.WriteLogEnd();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            log.WriteLogStart();
            string sSQL = "select sysdate from dual";
            log.WriteLog("[ExecuteSelectSQL]["+sSQL+"]");

            try
            {
                DataTable dtTemp = OracleHelper.GetTableSQL("connectionString",sSQL);
                dgvData.DataSource = dtTemp;
                LogLocal(rtxLog, "\nconnect finish ", 2);
            }
            catch (Exception ex)
            {
                log.WriteLog(ex.Message);
                LogLocal(rtxLog, "\nconnect error ", 0);
            }
            
            log.WriteLogEnd();
        }
        
        /// <summary>
        /// 记录LOG到richtextbox控件中
        /// </summary>
        /// <param name="rtx">richtextbox</param>
        /// <param name="sMsg">message</param>
        /// <param name="sType">0=error,red;1=warning,yellow;2=normal,balck;3=test,blue;other=normal,black;</param>
        private void LogLocal(RichTextBox rtx,string sMsg,int sType)
        {
            switch (sType)
            {
                case 0://erorr ,red
                    rtx.SelectionColor = System.Drawing.Color.Red;
                    break;
                case 1://warning ,yellow
                    rtx.SelectionColor = System.Drawing.Color.Orange;
                    break;
                case 2://normal ,black
                    rtx.SelectionColor = System.Drawing.Color.Black;
                    break;
                case 3://test ,blue
                    rtx.SelectionColor = System.Drawing.Color.Blue;
                    break;
                default:
                    rtx.SelectionColor = System.Drawing.Color.Black;
                    break;
            }
            rtx.AppendText(DateTime.Now.ToString() + " : " + sMsg + Environment.NewLine);
        }
    }
}
