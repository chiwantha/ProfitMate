using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;


namespace ProfitMate.Forms
{
    public partial class frmReportLog : Form
    {
        Connection connection;
        public frmReportLog()
        {
            InitializeComponent();
            connection = new Connection();
        }

        private void createReport()
        {
            ReportDocument rpt = new ReportDocument();
            rpt.Load(Application.StartupPath + "\\Reports\\BuiltLog.RPT");

            CrystalDecisions.Shared.TableLogOnInfo logOnInfo = rpt.Database.Tables[0].LogOnInfo;
            logOnInfo.ConnectionInfo.ServerName = connection.Get_path();
            logOnInfo.ConnectionInfo.DatabaseName = "HMS";
            logOnInfo.ConnectionInfo.UserID = "sa";
            logOnInfo.ConnectionInfo.Password = "";
            foreach (CrystalDecisions.CrystalReports.Engine.Table table in rpt.Database.Tables)
            {
                table.ApplyLogOnInfo(logOnInfo);
            }

            // Format the date strings to match the format used in your Crystal Reports
            string fromDate = dtpFrom.Value.ToString("yyyy-MM-dd");
            string toDate = dtpTo.Value.ToString("yyyy-MM-dd");

            // Set the record selection formula to select records between the two dates
            rpt.RecordSelectionFormula = "{MPR_Report_Logs.Built_Date} >= Date('" + fromDate + "') AND {MPR_Report_Logs.Built_Date} <= Date('" + toDate + "')";

            rptView.ReportSource = rpt;
        }


        private void frmReportLog_Load(object sender, EventArgs e)
        {

        }

        private void Load_Click(object sender, EventArgs e)
        {
            createReport();
        }
    }
}
