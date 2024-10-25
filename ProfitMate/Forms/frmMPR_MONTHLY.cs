using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using CrystalDecisions.CrystalReports.Engine;

namespace ProfitMate.Forms
{
    public partial class frmMPR_MONTHLY : Form
    {
        Connection connection;

        public frmMPR_MONTHLY()
        {
            InitializeComponent();
            connection = new Connection();
        }

        private void createReport()
        {
            ReportDocument rpt = new ReportDocument();
            rpt.Load(Application.StartupPath + "\\Reports\\MPR_MONTHLY_.RPT");

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
            string year = dtp.Value.ToString("yyyy");
            string month = dtp.Value.ToString("MM");

            // Set the record selection formula to select records between the two dates
            rpt.RecordSelectionFormula = "{Vw_Monthly_Purchesing.Year} = "+ year + " AND {Vw_Monthly_Purchesing.Month} = " + month;

            rptView.ReportSource = rpt;
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            createReport();
        }
    }
}
