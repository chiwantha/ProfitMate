using ProfitMate.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;


namespace ProfitMate
{
    public partial class MDI : Form
    {
        Connection connection;
        public MDI()
        {
            InitializeComponent();
            connection = new Connection();
        }

        bool frmlvl = true;

        public MDI(string user, bool lvl)
        {
            InitializeComponent();
            if (lvl)
            {
                user = user + ", CASHIER";
            } else
            {
                user = user + ", ADMINISTRATOR MODE";
            }
            frmlvl = lvl;
            usernamestrip.Text = user;
        }

        private void MDI_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch
            {
                Environment.Exit(0);
            }
        }

        private void MDI_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch
            {
                Environment.Exit(0);
            }
        }

        private Form currentForm;
        public void SwapForms(Form newForm)
        {
            btnPannel.Visible = false;
            pictureBox1.Visible = false;
            top.Visible = false;

            // If there is a current form, close it
            if (currentForm != null)
            {
                currentForm.Close();
            }

            // Set up properties for the newForm
            currentForm = newForm;
            newForm.TopLevel = false;
            newForm.Parent = MainPannel;
            newForm.Dock = DockStyle.Fill;
            newForm.Show();
        }

        private void errMessage(string location)
        {
            MessageBox.Show("Please Login to Main Software to Use " + location, "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmItemMaster frmItemMaster = new frmItemMaster(usernamestrip.Text, frmlvl);
            frmItemMaster.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmReportLog frmReportLog = new frmReportLog();
            frmReportLog.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmMPR frmMPR = new frmMPR(frmlvl);
            SwapForms(frmMPR);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            errMessage("Invoice");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            errMessage("Database");
        }

        private void mSTITEMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmItemMaster frmItemMaster = new frmItemMaster(usernamestrip.Text, frmlvl);
            frmItemMaster.ShowDialog();
        }

        private void mPRRepLOGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReportLog frmReportLog = new frmReportLog();
            frmReportLog.ShowDialog();
        }

        private void dATABASEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Comming Soon ! ", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void hOMEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentForm != null)
            {
                currentForm.Close();
            }
            btnPannel.Visible = true;
            pictureBox1.Visible = true;
            top.Visible = true;
        }

        private void aBOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Comming Soon ! ", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void lOGSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReportLog frmReportLog = new frmReportLog();
            frmReportLog.ShowDialog();
        }

        private void pURCHESINGREPORTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMPR frmMPR = new frmMPR(frmlvl);
            SwapForms(frmMPR);
        }

        private void rESTARTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void mONTHLYPURCHESINGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMPR_MONTHLY frmMPR_MONTHLY = new frmMPR_MONTHLY();
            frmMPR_MONTHLY.ShowDialog(this);
        }
    }
}
