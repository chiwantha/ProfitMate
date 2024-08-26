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
    public partial class frmReportViewer : Form
    {
        public frmReportViewer()
        {
            InitializeComponent();
        }

        public frmReportViewer(ReportDocument rpt)
        {
            InitializeComponent();
            rpt.Refresh();
            crystalReportViewer1.ReportSource = rpt;
        }
    }
}
