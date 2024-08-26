namespace ProfitMate.Forms
{
    partial class frmReportLog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rptView = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.Loadbtn = new Guna.UI2.WinForms.Guna2GradientButton();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rptView
            // 
            this.rptView.ActiveViewIndex = -1;
            this.rptView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rptView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rptView.Cursor = System.Windows.Forms.Cursors.Default;
            this.rptView.Location = new System.Drawing.Point(12, 50);
            this.rptView.Name = "rptView";
            this.rptView.ShowCloseButton = false;
            this.rptView.ShowCopyButton = false;
            this.rptView.ShowGotoPageButton = false;
            this.rptView.ShowGroupTreeButton = false;
            this.rptView.ShowLogo = false;
            this.rptView.ShowParameterPanelButton = false;
            this.rptView.ShowTextSearchButton = false;
            this.rptView.Size = new System.Drawing.Size(874, 715);
            this.rptView.TabIndex = 0;
            this.rptView.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFrom.Location = new System.Drawing.Point(95, 12);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(301, 26);
            this.dtpFrom.TabIndex = 3;
            // 
            // Loadbtn
            // 
            this.Loadbtn.BorderThickness = 1;
            this.Loadbtn.CheckedState.Parent = this.Loadbtn;
            this.Loadbtn.CustomImages.Parent = this.Loadbtn;
            this.Loadbtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.Loadbtn.FillColor2 = System.Drawing.Color.Green;
            this.Loadbtn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Loadbtn.ForeColor = System.Drawing.Color.Black;
            this.Loadbtn.HoverState.Parent = this.Loadbtn;
            this.Loadbtn.Location = new System.Drawing.Point(799, 12);
            this.Loadbtn.Name = "Loadbtn";
            this.Loadbtn.ShadowDecoration.Parent = this.Loadbtn;
            this.Loadbtn.Size = new System.Drawing.Size(87, 26);
            this.Loadbtn.TabIndex = 4;
            this.Loadbtn.Text = "LOAD";
            this.Loadbtn.Click += new System.EventHandler(this.Load_Click);
            // 
            // dtpTo
            // 
            this.dtpTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpTo.Location = new System.Drawing.Point(492, 12);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(301, 26);
            this.dtpTo.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(402, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 26);
            this.label3.TabIndex = 9;
            this.label3.Text = "To";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 26);
            this.label1.TabIndex = 10;
            this.label1.Text = "From";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmReportLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 777);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.Loadbtn);
            this.Controls.Add(this.rptView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmReportLog";
            this.Text = "frmReportLog";
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer rptView;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private Guna.UI2.WinForms.Guna2GradientButton Loadbtn;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
    }
}