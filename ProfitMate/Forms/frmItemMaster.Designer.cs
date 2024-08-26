namespace ProfitMate.Forms
{
    partial class frmItemMaster
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
            this.txt_item_name = new System.Windows.Forms.TextBox();
            this.btn_delete = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btn_edit = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btn_save = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btn_new = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btn_cancel = new Guna.UI2.WinForms.Guna2GradientButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_no = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_cost = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_id = new System.Windows.Forms.TextBox();
            this.dgData = new System.Windows.Forms.DataGridView();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Namemnbmh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.darfd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fst = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.feyg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sdwt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txt_item_type = new System.Windows.Forms.ComboBox();
            this.txt_unit = new System.Windows.Forms.ComboBox();
            this.txt_itm_search = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgData)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_item_name
            // 
            this.txt_item_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_item_name.Location = new System.Drawing.Point(262, 10);
            this.txt_item_name.Name = "txt_item_name";
            this.txt_item_name.Size = new System.Drawing.Size(362, 26);
            this.txt_item_name.TabIndex = 1;
            this.txt_item_name.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_item_name.Click += new System.EventHandler(this.txt_item_name_Click);
            this.txt_item_name.TextChanged += new System.EventHandler(this.txt_item_name_TextChanged);
            this.txt_item_name.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_item_name_KeyDown);
            // 
            // btn_delete
            // 
            this.btn_delete.BorderThickness = 1;
            this.btn_delete.CheckedState.Parent = this.btn_delete;
            this.btn_delete.CustomImages.Parent = this.btn_delete;
            this.btn_delete.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btn_delete.FillColor2 = System.Drawing.Color.Red;
            this.btn_delete.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btn_delete.ForeColor = System.Drawing.Color.White;
            this.btn_delete.HoverState.Parent = this.btn_delete;
            this.btn_delete.Location = new System.Drawing.Point(569, 42);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.ShadowDecoration.Parent = this.btn_delete;
            this.btn_delete.Size = new System.Drawing.Size(55, 26);
            this.btn_delete.TabIndex = 9;
            this.btn_delete.Text = "DELETE";
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // btn_edit
            // 
            this.btn_edit.BorderThickness = 1;
            this.btn_edit.CheckedState.Parent = this.btn_edit;
            this.btn_edit.CustomImages.Parent = this.btn_edit;
            this.btn_edit.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btn_edit.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btn_edit.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btn_edit.ForeColor = System.Drawing.Color.Black;
            this.btn_edit.HoverState.Parent = this.btn_edit;
            this.btn_edit.Location = new System.Drawing.Point(447, 74);
            this.btn_edit.Name = "btn_edit";
            this.btn_edit.ShadowDecoration.Parent = this.btn_edit;
            this.btn_edit.Size = new System.Drawing.Size(55, 26);
            this.btn_edit.TabIndex = 11;
            this.btn_edit.Text = "EDIT";
            this.btn_edit.Click += new System.EventHandler(this.btn_edit_Click);
            // 
            // btn_save
            // 
            this.btn_save.BorderThickness = 1;
            this.btn_save.CheckedState.Parent = this.btn_save;
            this.btn_save.CustomImages.Parent = this.btn_save;
            this.btn_save.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btn_save.FillColor2 = System.Drawing.Color.Blue;
            this.btn_save.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btn_save.ForeColor = System.Drawing.Color.White;
            this.btn_save.HoverState.Parent = this.btn_save;
            this.btn_save.Location = new System.Drawing.Point(508, 74);
            this.btn_save.Name = "btn_save";
            this.btn_save.ShadowDecoration.Parent = this.btn_save;
            this.btn_save.Size = new System.Drawing.Size(116, 26);
            this.btn_save.TabIndex = 12;
            this.btn_save.Text = "SAVE";
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            this.btn_save.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btn_save_KeyDown);
            // 
            // btn_new
            // 
            this.btn_new.BorderThickness = 1;
            this.btn_new.CheckedState.Parent = this.btn_new;
            this.btn_new.CustomImages.Parent = this.btn_new;
            this.btn_new.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btn_new.FillColor2 = System.Drawing.Color.Green;
            this.btn_new.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btn_new.ForeColor = System.Drawing.Color.Black;
            this.btn_new.HoverState.Parent = this.btn_new;
            this.btn_new.Location = new System.Drawing.Point(447, 42);
            this.btn_new.Name = "btn_new";
            this.btn_new.ShadowDecoration.Parent = this.btn_new;
            this.btn_new.Size = new System.Drawing.Size(55, 26);
            this.btn_new.TabIndex = 15;
            this.btn_new.Text = "NEW";
            this.btn_new.Click += new System.EventHandler(this.btn_new_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.BorderThickness = 1;
            this.btn_cancel.CheckedState.Parent = this.btn_cancel;
            this.btn_cancel.CustomImages.Parent = this.btn_cancel;
            this.btn_cancel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btn_cancel.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btn_cancel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btn_cancel.ForeColor = System.Drawing.Color.Black;
            this.btn_cancel.HoverState.Parent = this.btn_cancel;
            this.btn_cancel.Location = new System.Drawing.Point(508, 42);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.ShadowDecoration.Parent = this.btn_cancel;
            this.btn_cancel.Size = new System.Drawing.Size(55, 26);
            this.btn_cancel.TabIndex = 14;
            this.btn_cancel.Text = "CANCEL";
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(186, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 26);
            this.label3.TabIndex = 16;
            this.label3.Text = "Item Name";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(12, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 26);
            this.label1.TabIndex = 17;
            this.label1.Text = "Unit";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(186, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 26);
            this.label2.TabIndex = 19;
            this.label2.Text = "Cost";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_no
            // 
            this.txt_no.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_no.Location = new System.Drawing.Point(262, 74);
            this.txt_no.Name = "txt_no";
            this.txt_no.Size = new System.Drawing.Size(179, 26);
            this.txt_no.TabIndex = 5;
            this.txt_no.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_no.Click += new System.EventHandler(this.txt_no_Click);
            this.txt_no.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_no_KeyDown);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(12, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 26);
            this.label5.TabIndex = 23;
            this.label5.Text = "Item Type";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_cost
            // 
            this.txt_cost.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_cost.Location = new System.Drawing.Point(262, 42);
            this.txt_cost.Name = "txt_cost";
            this.txt_cost.Size = new System.Drawing.Size(179, 26);
            this.txt_cost.TabIndex = 3;
            this.txt_cost.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_cost.Click += new System.EventHandler(this.txt_cost_Click);
            this.txt_cost.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_cost_KeyDown);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(186, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 26);
            this.label6.TabIndex = 27;
            this.label6.Text = "No";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_id
            // 
            this.txt_id.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_id.Location = new System.Drawing.Point(12, 10);
            this.txt_id.Name = "txt_id";
            this.txt_id.ReadOnly = true;
            this.txt_id.Size = new System.Drawing.Size(168, 26);
            this.txt_id.TabIndex = 28;
            this.txt_id.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dgData
            // 
            this.dgData.AllowUserToAddRows = false;
            this.dgData.AllowUserToDeleteRows = false;
            this.dgData.AllowUserToResizeColumns = false;
            this.dgData.AllowUserToResizeRows = false;
            this.dgData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Code,
            this.Namemnbmh,
            this.darfd,
            this.fst,
            this.feyg,
            this.sdwt});
            this.dgData.Location = new System.Drawing.Point(12, 149);
            this.dgData.Name = "dgData";
            this.dgData.RowHeadersVisible = false;
            this.dgData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgData.Size = new System.Drawing.Size(612, 499);
            this.dgData.TabIndex = 29;
            this.dgData.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgData_CellClick);
            // 
            // Code
            // 
            this.Code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Code.DataPropertyName = "Item_code";
            this.Code.HeaderText = "Code";
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            this.Code.Width = 57;
            // 
            // Namemnbmh
            // 
            this.Namemnbmh.DataPropertyName = "Item_Name";
            this.Namemnbmh.HeaderText = "Name";
            this.Namemnbmh.Name = "Namemnbmh";
            this.Namemnbmh.ReadOnly = true;
            // 
            // darfd
            // 
            this.darfd.DataPropertyName = "Unit";
            this.darfd.HeaderText = "Unit";
            this.darfd.Name = "darfd";
            this.darfd.ReadOnly = true;
            // 
            // fst
            // 
            this.fst.DataPropertyName = "Cost";
            this.fst.HeaderText = "Cost";
            this.fst.Name = "fst";
            this.fst.ReadOnly = true;
            // 
            // feyg
            // 
            this.feyg.DataPropertyName = "ItemType";
            this.feyg.HeaderText = "Type";
            this.feyg.Name = "feyg";
            this.feyg.ReadOnly = true;
            // 
            // sdwt
            // 
            this.sdwt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.sdwt.DataPropertyName = "Kid";
            this.sdwt.HeaderText = "Kid No";
            this.sdwt.Name = "sdwt";
            this.sdwt.ReadOnly = true;
            this.sdwt.Width = 64;
            // 
            // txt_item_type
            // 
            this.txt_item_type.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_item_type.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.txt_item_type.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_item_type.FormattingEnabled = true;
            this.txt_item_type.Items.AddRange(new object[] {
            "BOT",
            "KOT",
            "SOT",
            "HOT",
            "SOT",
            "AOT"});
            this.txt_item_type.Location = new System.Drawing.Point(99, 74);
            this.txt_item_type.Name = "txt_item_type";
            this.txt_item_type.Size = new System.Drawing.Size(81, 26);
            this.txt_item_type.TabIndex = 4;
            this.txt_item_type.Text = "KOT";
            this.txt_item_type.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_item_type_KeyDown);
            // 
            // txt_unit
            // 
            this.txt_unit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_unit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.txt_unit.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_unit.FormattingEnabled = true;
            this.txt_unit.Items.AddRange(new object[] {
            "NOS",
            "ML",
            "KG",
            "GRM"});
            this.txt_unit.Location = new System.Drawing.Point(99, 42);
            this.txt_unit.Name = "txt_unit";
            this.txt_unit.Size = new System.Drawing.Size(81, 26);
            this.txt_unit.TabIndex = 2;
            this.txt_unit.Text = "NOS";
            this.txt_unit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_unit_KeyDown);
            // 
            // txt_itm_search
            // 
            this.txt_itm_search.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_itm_search.Location = new System.Drawing.Point(99, 112);
            this.txt_itm_search.Name = "txt_itm_search";
            this.txt_itm_search.Size = new System.Drawing.Size(525, 26);
            this.txt_itm_search.TabIndex = 30;
            this.txt_itm_search.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_itm_search.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(12, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 26);
            this.label4.TabIndex = 31;
            this.label4.Text = "Item Search";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmItemMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 660);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_itm_search);
            this.Controls.Add(this.txt_unit);
            this.Controls.Add(this.txt_item_type);
            this.Controls.Add(this.dgData);
            this.Controls.Add(this.txt_id);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_cost);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_no);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_new);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.btn_edit);
            this.Controls.Add(this.btn_delete);
            this.Controls.Add(this.txt_item_name);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmItemMaster";
            this.Text = "Item Master Mini";
            this.Load += new System.EventHandler(this.frmItemMaster_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txt_item_name;
        private Guna.UI2.WinForms.Guna2GradientButton btn_delete;
        private Guna.UI2.WinForms.Guna2GradientButton btn_edit;
        private Guna.UI2.WinForms.Guna2GradientButton btn_save;
        private Guna.UI2.WinForms.Guna2GradientButton btn_new;
        private Guna.UI2.WinForms.Guna2GradientButton btn_cancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_no;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_cost;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_id;
        private System.Windows.Forms.DataGridView dgData;
        private System.Windows.Forms.ComboBox txt_item_type;
        private System.Windows.Forms.ComboBox txt_unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Namemnbmh;
        private System.Windows.Forms.DataGridViewTextBoxColumn darfd;
        private System.Windows.Forms.DataGridViewTextBoxColumn fst;
        private System.Windows.Forms.DataGridViewTextBoxColumn feyg;
        private System.Windows.Forms.DataGridViewTextBoxColumn sdwt;
        private System.Windows.Forms.TextBox txt_itm_search;
        private System.Windows.Forms.Label label4;
    }
}