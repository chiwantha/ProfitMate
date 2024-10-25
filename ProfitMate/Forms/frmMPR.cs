using SAPBusinessObjects.WPF.Viewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using CrystalDecisions.CrystalReports.Engine;
using System.Diagnostics.Metrics;

namespace ProfitMate.Forms
{
    public partial class frmMPR : Form
    {
        Connection connection;
        public frmMPR()
        {
            InitializeComponent();
            connection = new Connection();
        }

        bool frmlvl = true;
        bool once = false;
        bool bil = false;
        decimal OLD_BF = 0;
        decimal NEW_BF = 0;

        public frmMPR(bool lvl)
        {
            InitializeComponent();
            connection = new Connection();
            set_user_rights(lvl);
            frmlvl = lvl;
        }

        private void set_user_rights(bool lvl)
        {
            if (lvl)
            {
                //grpStaff.Enabled = false;
                //txt_p_float.Enabled = false;
                //txt_m_float.Enabled = false;
                txt_Available_cash.Enabled = false;
                cbxAvailableCashAutoManual.Visible = false;
                //txt_real_card_sale.Enabled = false;
                //txt_OnHand.Enabled = false;
                //txt_Bank.Enabled = false;
                //AdminCtrl.Visible = true;
                USER_END.Visible = true;
            }
        }

        private void backdate_billing()
        {
            guna2GroupBox3.Enabled = false;
            guna2GroupBox1.Enabled = false;
            grpStaff.Enabled = false;
            grpStaff.Enabled = false;
            txt_p_float.Enabled = false;
            txt_m_float.Enabled = false;
            txt_Available_cash.Enabled = false;
            txt_real_card_sale.Enabled = false;
            txt_OnHand.Enabled = false;
            txt_Bank.Enabled = false;
            //AdminCtrl.Visible = true;
        }

        private void sameday_billing()
        {
            guna2GroupBox3.Enabled = true;
            guna2GroupBox1.Enabled = true;
            grpStaff.Enabled = true;
            grpStaff.Enabled = true;
            txt_p_float.Enabled = true;
            txt_m_float.Enabled = true;
            txt_Available_cash.Enabled = false;
            txt_real_card_sale.Enabled = true;
            txt_OnHand.Enabled = true;
            txt_Bank.Enabled = true;
        }

        private void DeleteRecord(int id, string specify, string tableName)
        {
            try
            {
                using (SqlConnection con = connection.my_conn())
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    string deleteQuery = "DELETE FROM " + tableName + " WHERE " + specify + " = @id";

                    using (SqlCommand cmd = new SqlCommand(deleteQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while deleting record: " + ex.Message);
            }
        }

        private void DeleteCreditRecode(int id)
        {
            try
            {
                // Show confirmation dialog
                DialogResult result = MessageBox.Show(
                    "You can't remove a single credit item. If you remove one, it will remove the whole credit bill. Are you sure you want to proceed?",
                    "Confirm Remove",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                // Check the user's response
                if (result == DialogResult.Yes)
                {
                    using (SqlConnection con = connection.my_conn())
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        string removeQuery = "UPDATE TblPurchesingHed SET payment_state = @payment_state, credit_date = NULL WHERE id = @id";

                        using (SqlCommand cmd = new SqlCommand(removeQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@payment_state", 0);
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while removing record: " + ex.Message);
            }
        }

        private void Decide_Paied(int id)
        {
            try
            {
                using (SqlConnection con = connection.my_conn())
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    string updateQuery = "UPDATE TblPurchesingHed SET payment_state = @payment_state, credit_date = @creditDate WHERE id = @id";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@payment_state", 1);
                        cmd.Parameters.AddWithValue("@creditDate", dtpReportDate.Value.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@id", id);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while updating record: " + ex.Message);
            }
        }



        private void CalculateTotalExpenses()
        {
            decimal totalExpenses = 0;

            if (!string.IsNullOrEmpty(txtPurchesing_Total.Text))
            {
                totalExpenses += Convert.ToDecimal(txtPurchesing_Total.Text);
            }

            if (!string.IsNullOrEmpty(txtCancelled_bills_Total.Text))
            {
                totalExpenses += Convert.ToDecimal(txtCancelled_bills_Total.Text);
            }

            if (!string.IsNullOrEmpty(txtStaff_Advance_Total.Text))
            {
                totalExpenses += Convert.ToDecimal(txtStaff_Advance_Total.Text);
            }

            txtTotal_Expenses.Text = totalExpenses.ToString("N2");
        }

        private void CalculateMissingCardExpence()
        {
            if (bal_after_expences.Text == "")
            {
                return;
            }

            decimal hms_card_sale = 0;
            decimal real_card_sale = 0;
            decimal balance = 0;
            decimal missing_expence = 0;
            decimal balance_1 = Convert.ToDecimal(bal_after_expences.Text);
            decimal balance_2 = 0;

            // Try parsing txt_hms_card_sale
            if (decimal.TryParse(txt_hms_card_sale.Text, out hms_card_sale))
            {
                // Check if txt_real_card_sale is not empty and parse it
                if (!string.IsNullOrEmpty(txt_real_card_sale.Text) && decimal.TryParse(txt_real_card_sale.Text, out real_card_sale))
                {
                    // Calculate balance and missing expense
                    if (real_card_sale > hms_card_sale)
                    {
                        balance = real_card_sale - hms_card_sale;
                        missing_expence = balance * 3 / 100;
                        balance_2 = balance_1 - real_card_sale + missing_expence;
                    }
                    else
                    {
                        balance_2 = balance_1 - real_card_sale;
                    }
                }
                else
                {
                    // Handle the case when txt_real_card_sale is null or empty
                    balance = 0;
                    missing_expence = 0;
                }

                // Update the UI
                txt_card_sales_difference.Text = balance.ToString("N2");
                txt_card_missing_expense.Text = missing_expence.ToString("N2");
                bal_after_card_expences.Text = balance_2.ToString("N2");
            }
            else
            {
                // Handle the case when txt_hms_card_sale is null or empty
                txt_card_sales_difference.Text = "0.00";
                txt_card_missing_expense.Text = "0.00";
            }
        }

        private void Calculate_1st_Balance()
        {
            decimal hms_sale = 0;
            decimal hms_expenses = 0;
            decimal float_p = 0;
            decimal float_m = 0;
            decimal balance_1 = 0;

            // Method to convert text to decimal, handling null or empty values as 0.00
            decimal ConvertToDecimal(TextBox textBox)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = "0.00";
                    return 0.00m;
                }
                return Convert.ToDecimal(textBox.Text);
            }

            // Convert values from text boxes
            hms_sale = ConvertToDecimal(txt_hms__CardCashIncome);
            hms_expenses = ConvertToDecimal(txtTotal_Expenses);
            float_p = ConvertToDecimal(txt_p_float);
            float_m = ConvertToDecimal(txt_m_float);

            // Calculate balance
            balance_1 = (hms_sale + float_p - hms_expenses) - float_m;

            // Set the balance to the label
            bal_after_expences.Text = balance_1.ToString("N2");
        }

        private void CalculateBalanceFoword()
        {
            // Initialize all decimal variables to 0.00
            decimal balance_2 = 0.00m;
            decimal availableAmount = 0.00m;
            decimal cashExsShort = 0.00m;
            decimal onHand = 0.00m;
            decimal total = 0.00m;
            decimal bank = 0.00m;
            decimal balanceForward = 0.00m;

            // Convert text inputs to decimals, handling null or empty inputs as 0.00
            balance_2 = string.IsNullOrWhiteSpace(bal_after_card_expences.Text) ? 0.00m : Convert.ToDecimal(bal_after_card_expences.Text);
            availableAmount = string.IsNullOrWhiteSpace(txt_Available_cash.Text) ? 0.00m : Convert.ToDecimal(txt_Available_cash.Text);
            onHand = string.IsNullOrWhiteSpace(txt_OnHand.Text) ? 0.00m : Convert.ToDecimal(txt_OnHand.Text);
            bank = string.IsNullOrWhiteSpace(txt_Bank.Text) ? 0.00m : Convert.ToDecimal(txt_Bank.Text);

            // Calculate CashExsShort
            cashExsShort = (balance_2 <= 0) ? 0 : availableAmount - balance_2;

            // Calculate total
            total = (cashExsShort <= 0) ? balance_2 + onHand : balance_2 + cashExsShort + onHand;

            // Calculate balance forward
            balanceForward = total - bank;

            // Update text boxes with formatted values
            txt_Cash_ExorShort.Text = cashExsShort.ToString("N2");
            txt_FinalTotal.Text = total.ToString("N2");
            txt_Balance_Foword.Text = balanceForward.ToString("N2");
        }



        private async Task<string> get_Unit()
        {
            string Unit = "";

            try
            {
                SqlConnection con = connection.my_conn();
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                try
                {
                    // SQL Query with parameter
                    string selectQuery = @"SELECT Unit FROM tblMstItem WHERE Kid = @item_kid";

                    SqlCommand cmd = new SqlCommand(selectQuery, con);
                    cmd.Parameters.AddWithValue("@item_kid", cbPurchesingDescription.SelectedValue);

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            Unit = reader["Unit"].ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show("No Unit found.");
                    }

                    reader.Close(); // Close the reader after processing

                    if (con.State == ConnectionState.Open)
                    {
                        con.Close(); // Close the connection
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // Handle potential connection exceptions
            }

            return Unit; // Return the Unit string value
        }

        private async void Calculate_item_cost()
        {
            string Unit = await get_Unit();
            Unit = Unit.Trim();
            Unit = Unit.ToUpper();

            if (!string.IsNullOrEmpty(txtQuantity.Text) && txtQuantity.Text != "Qt" &&
                !string.IsNullOrEmpty(txtPurchesingAmount.Text) && txtPurchesingAmount.Text != "Amount")
            {
                try
                {
                    decimal quantity = Convert.ToDecimal(txtQuantity.Text); // Allow for decimal quantities
                    decimal purchasingAmount = Convert.ToDecimal(txtPurchesingAmount.Text);
                    decimal costPerItem = 0;

                    if (quantity > 0)
                    {
                        switch (Unit.ToUpper()) // Handle units case-insensitively
                        {
                            case "NOS":
                                costPerItem = purchasingAmount / quantity; // Simple division for NOS (Number of items)
                                break;

                            case "KG":
                                costPerItem = purchasingAmount / quantity; // Already in Kg, no additional conversion
                                break;

                            case "GRM":
                                costPerItem = purchasingAmount / (quantity * 1000); // Convert Kg to Grams (1000g = 1Kg)
                                break;

                            case "LT":
                                costPerItem = purchasingAmount / quantity; // Already in Liters, no additional conversion
                                break;

                            case "ML":
                                costPerItem = purchasingAmount / (quantity * 1000); // Convert Liters to Milliliters (1000ml = 1L)
                                break;

                            default:
                                MessageBox.Show("Unknown Unit.", "Unit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                        }

                        txtCost.Text = costPerItem.ToString("F2"); // Format to 2 decimal places
                    }
                    else
                    {
                        MessageBox.Show("Quantity must be greater than zero.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (FormatException)
                {
                    if (txtQuantity.Text != "." || txtQuantity.Text != "0.")
                    {
                        MessageBox.Show("Invalid number format. Please check the input.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void CreateReport()
        {
            ReportDocument rpt = new ReportDocument();
            rpt.Load(Application.StartupPath + "\\Reports\\MPR_.RPT");

            CrystalDecisions.Shared.TableLogOnInfo logOnInfo = rpt.Database.Tables[0].LogOnInfo;
            logOnInfo.ConnectionInfo.ServerName = connection.Get_path();
            logOnInfo.ConnectionInfo.DatabaseName = "HMS";
            logOnInfo.ConnectionInfo.UserID = "sa";
            logOnInfo.ConnectionInfo.Password = "";
            foreach (CrystalDecisions.CrystalReports.Engine.Table table in rpt.Database.Tables) 
            {
                table.ApplyLogOnInfo(logOnInfo);
            }

            // Set the date for the main report
            rpt.RecordSelectionFormula = "{TblMPR_Report.date} = Date(" +
                dtpReportDate.Value.Year + ", " +
                dtpReportDate.Value.Month + ", " +
                dtpReportDate.Value.Day + ")";

            // Set the date for the "Purchesing" subreport
            ReportDocument subReportPurchesing = rpt.Subreports["Purchesing"];
            subReportPurchesing.RecordSelectionFormula = "{Vw_Purchesing.date} = Date(" +
                dtpReportDate.Value.Year + ", " +
                dtpReportDate.Value.Month + ", " +
                dtpReportDate.Value.Day + ") OR " +
                "{Vw_Purchesing.credit_date} = Date(" +
                dtpReportDate.Value.Year + ", " +
                dtpReportDate.Value.Month + ", " +
                dtpReportDate.Value.Day + ")";

            // Set the date for the "Cancelled_Bills" subreport
            ReportDocument subReportCancelledBills = rpt.Subreports["Cancelled_Bills"];
            subReportCancelledBills.RecordSelectionFormula = "DateValue({TblManualClancelBills.Date}) = Date(" +
                dtpReportDate.Value.Year + ", " +
                dtpReportDate.Value.Month + ", " +
                dtpReportDate.Value.Day + ")";

            // Set the date for the "Staff_Expences" subreport
            ReportDocument subReportStaffExpences = rpt.Subreports["Staff_Expences"];
            subReportStaffExpences.RecordSelectionFormula = "{TblStaffExpences.date} = Date(" +
                dtpReportDate.Value.Year + ", " +
                dtpReportDate.Value.Month + ", " +
                dtpReportDate.Value.Day + ")";

            frmReportViewer frmReportViewer = new frmReportViewer(rpt);
            frmReportViewer.Show();

        }

        private void update_cost(string kid, string cost)
        {
            if (string.IsNullOrWhiteSpace(kid) || string.IsNullOrWhiteSpace(cost))
            {
                MessageBox.Show("Kid and Cost cannot be empty.");
                return;
            }

            try
            {
                using (SqlConnection con = connection.my_conn())
                {
                    con.Open();

                    string updateQuery = "UPDATE tblMstItem SET Cost=@cost WHERE Kid=@kid";
                    using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.Add("@kid", SqlDbType.VarChar).Value = kid;
                        cmd.Parameters.Add("@cost", SqlDbType.Decimal).Value = decimal.Parse(cost);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Task ResetFields()
        {
            txt_hms__CardCashIncome.Text = "0.00";
            txt_p_float.Text = "40000.00";
            txtTotal_Expenses.Text = "0.00";
            txt_m_float.Text = "40000.00";
            bal_after_expences.Text = "0.00";
            txt_hms_card_sale.Text = "0.00";
            txt_real_card_sale.Text = "0.00";
            txt_card_sales_difference.Text = "0.00";
            txt_card_missing_expense.Text = "0.00";
            bal_after_card_expences.Text = "0.00";
            txt_Available_cash.Text = "0.00";
            txt_Cash_ExorShort.Text = "0.00";
            txt_OnHand.Text = "0.00";
            txt_FinalTotal.Text = "0.00";
            txt_Bank.Text = "0.00";
            txt_Balance_Foword.Text = "0.00";
            return Task.CompletedTask;
        }

        //------------------------------------------------------------------------------------------------Load funcs

        /*
        private async Task LoadAvailableCashAsync(string date)
        {
            try
            {
                using (SqlConnection con = connection.my_conn())
                {
                    if (con.State != ConnectionState.Open)
                    {
                        await con.OpenAsync();
                    }

                    try
                    {
                        // SQL Query to get the last CashDrawer amount for the given date
                        string selectQuery = @"
                    SELECT TOP 1 
                        CAST([CashDrawer] AS DECIMAL(18, 2)) AS LastCashDrawer
                    FROM 
                        [HMS].[dbo].[TblTrnCashier]
                    WHERE 
                        CONVERT(VARCHAR(10), [SOTime], 120) = @Date
                    ORDER BY 
                        [SOTime] DESC";

                        using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@Date", date);

                            SqlDataReader reader = await cmd.ExecuteReaderAsync();

                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    // Get the LastCashDrawer value
                                    decimal lastCashDrawer = reader.GetDecimal(reader.GetOrdinal("LastCashDrawer"));

                                    lastCashDrawer = lastCashDrawer - 4000;
                                    // Set the value to the txt_Available_cash textbox
                                    txt_Available_cash.Text = lastCashDrawer.ToString("F2");
                                }
                            }
                            else
                            {
                                // If no data is found, set to 0.00
                                txt_Available_cash.Text = "0.00";
                            }

                            reader.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("An error occurred while retrieving data: " + e.Message);
                    }

                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred while connecting to the database.");
            }
        }
        */

        private async Task LoadAvailableCashAsync(string date)
        {
            try
            {
                using (SqlConnection con = connection.my_conn())
                {
                    if (con.State != ConnectionState.Open)
                    {
                        await con.OpenAsync();
                    }

                    try
                    {
                        string selectQuery = "";
                        bool applyAdjustment = false; // Flag to determine if the -4000 adjustment should be applied

                        // If cbxAvailableCashAutoManual is checked, fetch data from TblTrnCashier
                        if (!cbxAvailableCashAutoManual.Checked)
                        {
                            selectQuery = @"
                    SELECT TOP 1 
                        CAST([CashDrawer] AS DECIMAL(18, 2)) AS LastCashDrawer
                    FROM 
                        [HMS].[dbo].[TblTrnCashier]
                    WHERE 
                        CONVERT(VARCHAR(10), [SOTime], 120) = @Date
                    ORDER BY 
                        [SOTime] DESC";

                            applyAdjustment = true; // Set the flag to apply the -4000 adjustment
                        }
                        // If cbxAvailableCashAutoManual is not checked, fetch data from TblMPR_Report
                        else
                        {
                            selectQuery = @"
                    SELECT TOP 1 
                        CAST([available_cash] AS DECIMAL(18, 2)) AS LastCashDrawer
                    FROM 
                        [HMS].[dbo].[TblMPR_Report]
                    WHERE 
                        CONVERT(VARCHAR(10), [date], 120) = @Date
                    ORDER BY 
                        [date] DESC";

                            applyAdjustment = false; // Do not apply the adjustment for TblMPR_Report
                        }

                        using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@Date", date);

                            SqlDataReader reader = await cmd.ExecuteReaderAsync();

                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    // Get the LastCashDrawer value
                                    decimal lastCashDrawer = reader.GetDecimal(reader.GetOrdinal("LastCashDrawer"));

                                    // Apply -4000 adjustment only if data is fetched from TblTrnCashier
                                    if (applyAdjustment)
                                    {
                                        lastCashDrawer -= 4000;
                                    }

                                    // Set the value to the txt_Available_cash textbox
                                    txt_Available_cash.Text = lastCashDrawer.ToString("F2");
                                }
                            }
                            else
                            {
                                // If no data is found, set to 0.00
                                txt_Available_cash.Text = "0.00";
                            }

                            reader.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("An error occurred while retrieving data: " + e.Message);
                    }

                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred while connecting to the database.");
            }
        }



        private async Task Hms_Data_Load(string date)
        {
            try
            {
                SqlConnection con = connection.my_conn();
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                try
                {
                    // Corrected SQL Query with SUM and proper aliases
                    string selectQuery = @"
                        SELECT 
                            SUM([Cash Amount]) AS TotalCash, 
                            SUM([Card Amount]) AS TotalCard, 
                            SUM(Total) AS TotalAmount
                        FROM 
                            Vw_profitmate_support 
                        WHERE 
                            CONVERT(VARCHAR(10), [Invoice Date], 120) = @Date";

                    SqlCommand cmd = new SqlCommand(selectQuery, con);
                    cmd.Parameters.AddWithValue("@Date", date);

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            // Use the aliases to retrieve the data
                            decimal totalCash = reader.GetDecimal(reader.GetOrdinal("TotalCash"));
                            decimal totalCard = reader.GetDecimal(reader.GetOrdinal("TotalCard"));
                            decimal totalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount"));

                            // Assuming you want to display the combined cash and card income
                            txt_hms__CardCashIncome.Text = totalAmount.ToString("F2");
                            txt_hms_card_sale.Text = totalCard.ToString("F2");
                            //lbl_TotalAmount.Text = totalAmount.ToString(); // If you have a label for the total
                        }
                    }
                    else
                    {
                        MessageBox.Show("No records found.");
                    }

                    reader.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Process Unsuccessful!");
            }
        } //card cash incomes -----

        private async Task GetLastMprCashForward()
        {
            try
            {
                using (SqlConnection con = connection.my_conn())
                {
                    await con.OpenAsync();

                    try
                    {
                        // Prepare the SQL query with date-only comparison using CONVERT
                        string selectQuery = @"
                    SELECT TOP 1 balance_foword 
                    FROM TblMPR_Report 
                    WHERE CONVERT(VARCHAR(10), [date], 120) < CONVERT(VARCHAR(10), @ReportDate, 120)
                    ORDER BY report_created_date DESC";

                        using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                        {
                            // Convert the report date to 'yyyy-MM-dd' format for comparison
                            string formattedDate = dtpReportDate.Value.Date.ToString("yyyy-MM-dd");

                            // Add the parameter value for the report date
                            cmd.Parameters.AddWithValue("@ReportDate", formattedDate);

                            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                            {
                                if (reader.HasRows)
                                {
                                    while (await reader.ReadAsync())
                                    {
                                        decimal onhand = reader.GetDecimal(reader.GetOrdinal("balance_foword"));
                                        txt_OnHand.Text = onhand.ToString("F2");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("No Last Balance Forward found. Please Add Manually!");
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error retrieving data: " + e.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Process Unsuccessful: " + ex.Message);
            }
        }

        private async Task GetReportDataWhichAlreadyHave()
        {
            try
            {
                using (SqlConnection con = connection.my_conn())
                {
                    await con.OpenAsync();

                    try
                    {
                        // Check if data exists for the given date
                        string checkQuery = @"
                SELECT COUNT(*) 
                FROM TblMPR_Report 
                WHERE date = @ReportDate";

                        using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                        {
                            checkCmd.Parameters.AddWithValue("@ReportDate", dtpReportDate.Value.Date);

                            int count = (int)await checkCmd.ExecuteScalarAsync();

                            if (count > 0)
                            {
                                // Prompt user to load data
                                DialogResult result = MessageBox.Show("Data already exists for this date. Do you want to load it?", "Load Data", MessageBoxButtons.YesNo);

                                if (result == DialogResult.Yes)
                                {
                                    // Load the data
                                    string selectQuery = @"
                            SELECT cashier_name, sale, flotP, expences, flotM, balance_1, hms_card_sale, real_card_sale, difference, fee, balance_2, ac_state, available_cash, cash_ex_sh, on_hand, bank, balance_foword
                            FROM TblMPR_Report 
                            WHERE date = @ReportDate";

                                    using (SqlCommand selectCmd = new SqlCommand(selectQuery, con))
                                    {
                                        selectCmd.Parameters.AddWithValue("@ReportDate", dtpReportDate.Value.Date);

                                        using (SqlDataReader reader = await selectCmd.ExecuteReaderAsync())
                                        {
                                            if (reader.HasRows)
                                            {
                                                while (await reader.ReadAsync())
                                                {
                                                    if (reader["ac_state"] != DBNull.Value && Convert.ToBoolean(reader["ac_state"]))
                                                    {
                                                        cbxAvailableCashAutoManual.Checked = true;
                                                    }
                                                    else if (reader["ac_state"] == DBNull.Value || !Convert.ToBoolean(reader["ac_state"]))
                                                    {
                                                        cbxAvailableCashAutoManual.Checked = false;
                                                    }

                                                    // Ensure that each value is parsed as a decimal and formatted to 2 decimal places ("F2")
                                                    txtCashierName.Text = reader["cashier_name"].ToString(); // No formatting needed for string

                                                    if (decimal.TryParse(reader["flotP"].ToString(), out decimal flotP))
                                                        txt_p_float.Text = flotP.ToString("N2");

                                                    if (decimal.TryParse(reader["flotM"].ToString(), out decimal flotM))
                                                        txt_m_float.Text = flotM.ToString("N2");

                                                    if (decimal.TryParse(reader["balance_1"].ToString(), out decimal balance1))
                                                        bal_after_expences.Text = balance1.ToString("N2");

                                                    if (decimal.TryParse(reader["real_card_sale"].ToString(), out decimal realCardSale))
                                                        txt_real_card_sale.Text = realCardSale.ToString("N2");

                                                    if (decimal.TryParse(reader["difference"].ToString(), out decimal difference))
                                                        txt_card_sales_difference.Text = difference.ToString("N2");

                                                    if (decimal.TryParse(reader["fee"].ToString(), out decimal fee))
                                                        txt_card_missing_expense.Text = fee.ToString("N2");

                                                    if (decimal.TryParse(reader["balance_2"].ToString(), out decimal balance2))
                                                        bal_after_card_expences.Text = balance2.ToString("N2");

                                                    if (decimal.TryParse(reader["cash_ex_sh"].ToString(), out decimal cashExSh))
                                                        txt_Cash_ExorShort.Text = cashExSh.ToString("N2");

                                                    if (decimal.TryParse(reader["on_hand"].ToString(), out decimal onHand))
                                                        txt_OnHand.Text = onHand.ToString("N2");

                                                    if (decimal.TryParse(reader["bank"].ToString(), out decimal bank))
                                                        txt_Bank.Text = bank.ToString("N2");

                                                    //txt_Balance_Foword.Text = reader["balance_foword"].ToString();
                                                    OLD_BF = Convert.ToDecimal(reader["balance_foword"]);
                                                    //txt_FinalTotal.Text = reader["balance_foword"].ToString();

                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("No data found for the selected date.");
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                cbxAvailableCashAutoManual.Checked = false;
                                MessageBox.Show("No data available for the selected date.");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error retrieving data: " + e.Message);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Process Unsuccessful!");
            }
        }

        private async void purchesing_item_master()
        {
            try
            {
                using (SqlConnection con = connection.my_conn())
                {
                    // Open the connection asynchronously
                    await con.OpenAsync();

                    try
                    {
                        // Prepare the SQL query with parameterized search for Kid
                        string selectQuery = "SELECT Kid, Item_code, item_name FROM tblMstItem WHERE Kid LIKE @Kid";
                        using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                        {
                            // Add the parameter value for Kid
                            cmd.Parameters.AddWithValue("@Kid", "%" + txtPurchesingId.Text.Trim() + "%");

                            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                            {
                                DataTable dataTable = new DataTable();

                                // Fill the DataTable on a separate thread to avoid blocking the UI
                                await Task.Run(() => dataAdapter.Fill(dataTable));

                                // Bind the results to the ComboBox
                                cbPurchesingDescription.DataSource = dataTable;
                                cbPurchesingDescription.ValueMember = "Kid";
                                cbPurchesingDescription.DisplayMember = "item_name";
                                if (!once)
                                {
                                    cbPurchesingDescription.Text = "";
                                }
                                once = true;
                                if (txtPurchesingId.Text == "")
                                {
                                    cbPurchesingDescription.Text = "";
                                }

                                if (cbPurchesingDescription.Text != null && cbPurchesingDescription.ValueMember != null &&
                                     cbPurchesingDescription.ValueMember != "")
                                {
                                    txt_purchesing_unit.Text = await get_Unit();
                                } else
                                {
                                    txt_purchesing_unit.Text = "-";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error during database operation: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening the connection: " + ex.Message);
            }
        }

        private void frmMPR_Load(object sender, EventArgs e)
        {
            purchesing_item_master();
            cbPurchesingDescription.Text = "";
            disable_purchesing_inputs();
        }

        private void frmMPR_Activated(object sender, EventArgs e)
        {
            cbPurchesingDescription.Text = "";
        }

        private async Task purchesing_bill_no()
        {
            try
            {
                using (SqlConnection con = connection.my_conn())
                {
                    // Open the connection asynchronously
                    await con.OpenAsync();

                    try
                    {
                        // Prepare the SQL query with date-only comparison using CONVERT
                        string selectQuery = @"
                    SELECT id, bill_no, cash_credit, supplier 
                    FROM TblPurchesingHed 
                    WHERE CONVERT(VARCHAR(10), [date], 120) = CONVERT(VARCHAR(10), @date, 120)";

                        using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                        {
                            // Convert the selected date to the same format for comparison
                            string formattedDate = dtpReportDate.Value.Date.ToString("yyyy-MM-dd");

                            // Add the parameter value for the date only as a string
                            cmd.Parameters.AddWithValue("@date", formattedDate);

                            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                            {
                                DataTable dataTable = new DataTable();

                                // Fill the DataTable asynchronously to avoid blocking the UI
                                await Task.Run(() => dataAdapter.Fill(dataTable));

                                // Bind the results to the ComboBox
                                cb_txt_p_bill_no.DataSource = dataTable;
                                cb_txt_p_bill_no.ValueMember = "id";
                                cb_txt_p_bill_no.DisplayMember = "bill_no";

                                // Also bind supplier to the supplier ComboBox (hidden for now)
                                cb_txt_p_bill_supplier.DataSource = dataTable.Copy();
                                cb_txt_p_bill_supplier.ValueMember = "id";
                                cb_txt_p_bill_supplier.DisplayMember = "supplier";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error during database operation: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening the connection: " + ex.Message);
            }
        }

        private async Task purchesing_bill_supplier()
        {
            try
            {
                using (SqlConnection con = connection.my_conn())
                {
                    // Open the connection asynchronously
                    await con.OpenAsync();

                    try
                    {
                        // Prepare the SQL query with date-only comparison using CONVERT
                        string selectQuery = @"
                    SELECT id, supplier, bill_no, cash_credit 
                    FROM TblPurchesingHed 
                    WHERE CONVERT(VARCHAR(10), [date], 120) = CONVERT(VARCHAR(10), @date, 120)";

                        using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                        {
                            // Add the parameter value for the date only
                            // Ensure the date parameter is formatted as a string in 'YYYY-MM-DD'
                            cmd.Parameters.AddWithValue("@date", dtpReportDate.Value.Date.ToString("yyyy-MM-dd"));

                            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                            {
                                DataTable dataTable = new DataTable();

                                // Fill the DataTable asynchronously to avoid blocking the UI
                                await Task.Run(() => dataAdapter.Fill(dataTable));

                                // Bind the results to the ComboBox
                                cb_txt_p_bill_supplier.DataSource = dataTable;
                                cb_txt_p_bill_supplier.ValueMember = "id";
                                cb_txt_p_bill_supplier.DisplayMember = "supplier";

                                // Also bind bill_no to the bill_no ComboBox (hidden for now)
                                cb_txt_p_bill_no.DataSource = dataTable.Copy();
                                cb_txt_p_bill_no.ValueMember = "id";
                                cb_txt_p_bill_no.DisplayMember = "bill_no";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error during database operation: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening the connection: " + ex.Message);
            }
        }

        private async Task UpdateBalances(string targetDate, decimal differenceAmount)
        {
            try
            {
                using (SqlConnection con = connection.my_conn())
                {
                    // Open the connection asynchronously
                    await con.OpenAsync();

                    // Start a SQL transaction
                    using (SqlTransaction transaction = con.BeginTransaction())
                    {
                        try
                        {
                            // Check if future dates exist
                            string checkFutureDatesQuery = @"
                    SELECT COUNT(*) 
                    FROM TblMPR_Report 
                    WHERE CONVERT(VARCHAR(10), [date], 120) > @targetDate";

                            using (SqlCommand checkCmd = new SqlCommand(checkFutureDatesQuery, con, transaction))
                            {
                                checkCmd.Parameters.AddWithValue("@targetDate", targetDate);
                                int futureDateCount = (int)await checkCmd.ExecuteScalarAsync();

                                if (futureDateCount > 0)
                                {
                                    // If there are future dates, update ON_HAND and BALANCE_FORWARD for target date and future dates
                                    string updateQuery = @"
                            UPDATE TblMPR_Report
                            SET 
                                on_hand = on_hand + @differenceAmount,
                                balance_foword = balance_foword + @differenceAmount
                            WHERE 
                                CONVERT(VARCHAR(10), [date], 120) > @targetDate";

                                    using (SqlCommand cmd = new SqlCommand(updateQuery, con, transaction))
                                    {
                                        // Add parameters
                                        cmd.Parameters.AddWithValue("@differenceAmount", differenceAmount);
                                        cmd.Parameters.AddWithValue("@targetDate", targetDate);

                                        // Execute the update command
                                        await cmd.ExecuteNonQueryAsync();
                                    }
                                }
                                else
                                {
                                    // If no future dates are available, only update the target date
                                    string updateOnlyTargetDateQuery = @"
                            UPDATE TblMPR_Report
                            SET 
                                on_hand = on_hand + @differenceAmount,
                                balance_foword = balance_foword + @differenceAmount
                            WHERE 
                                CONVERT(VARCHAR(10), [date], 120) = @targetDate";

                                    using (SqlCommand cmd = new SqlCommand(updateOnlyTargetDateQuery, con, transaction))
                                    {
                                        // Add parameters
                                        cmd.Parameters.AddWithValue("@differenceAmount", differenceAmount);
                                        cmd.Parameters.AddWithValue("@targetDate", targetDate);

                                        // Execute the update command
                                        await cmd.ExecuteNonQueryAsync();
                                    }
                                }
                            }

                            // Commit the transaction
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            // Rollback the transaction in case of an error
                            transaction.Rollback();
                            MessageBox.Show("Error updating balances: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening the connection: " + ex.Message);
            }
        }



        //------------------------------------------------------------------------------------------------Cancelled Bill Section Handle

        private void clear_cancell_bill_inouts()
        {
            txt_cancelBill_no.Text = "Bill No";
            txt_cancellbill_description.Text = "Enter Discription";
            txt_cancessBill_amount.Text = "Amount";
        }

        private async void btnCanellBillAdd_Click(object sender, EventArgs e)
        {
            if (txt_cancelBill_no.Text == "" || txt_cancelBill_no.Text == "Bill No")
            {
                MessageBox.Show("Bill_No is Empty", "STOP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txt_cancellbill_description.Text == "" || txt_cancellbill_description.Text == "Enter Discription")
            {
                MessageBox.Show("cancell Bill Discription is Empty", "STOP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txt_cancessBill_amount.Text == "" || txt_cancessBill_amount.Text == "Amount")
            {
                MessageBox.Show("Amount is Empty", "STOP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    SqlConnection con = connection.my_conn();
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    string insert = "insert into TblManualClancelBills (bill_no, description, amount, date) " +
                        "values (@bill_no ,@description, @amount ,@date)";
                    SqlCommand cmd = new SqlCommand(insert, con);
                    cmd.Parameters.AddWithValue("@bill_no",txt_cancelBill_no.Text);
                    cmd.Parameters.AddWithValue("@description", txt_cancellbill_description.Text);
                    cmd.Parameters.AddWithValue("@amount", txt_cancessBill_amount.Text);
                    cmd.Parameters.AddWithValue("@date", dtpReportDate.Value.ToString("yyyy-MM-dd"));

                    cmd.ExecuteNonQuery();

                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                await CancelledBills_Load(dtpReportDate.Value.ToString("yyyy-MM-dd"));
                clear_cancell_bill_inouts();
                txt_cancelBill_no.Focus();
            }
        }

        private async Task CancelledBills_Load(string Date)
        {
            try
            {
                using (SqlConnection con = connection.my_conn())
                {
                    // Open the connection asynchronously
                    await con.OpenAsync();

                    try
                    {
                        // Prepare the SQL query with date-only comparison if necessary
                        string selectQuery = "SELECT * FROM TblManualClancelBills WHERE CONVERT(VARCHAR(10), [Date], 120) = CONVERT(VARCHAR(10), @Date, 120)";

                        using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                        {
                            // Add the parameter value for the date only
                            cmd.Parameters.AddWithValue("@Date", Date);

                            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                            {
                                DataTable dataTable = new DataTable();

                                // Fill the DataTable asynchronously to avoid blocking the UI
                                await Task.Run(() => dataAdapter.Fill(dataTable));

                                // Bind the DataTable to the DataGridView
                                dgCancelledBills.AutoGenerateColumns = false;
                                dgCancelledBills.DataSource = dataTable;

                                // Format the Amount column
                                dgCancelledBills.CellFormatting += (s, e) =>
                                {
                                    if (e.ColumnIndex == dgCancelledBills.Columns["dataGridViewTextBoxColumn4"].Index) // Replace "Amount" with the actual column name or index
                                    {
                                        if (e.Value != null && decimal.TryParse(e.Value.ToString(), out decimal amount))
                                        {
                                            e.Value = amount.ToString("N2");
                                            e.FormattingApplied = true;
                                        }
                                    }
                                };

                                // Calculate the sum of the Amount column
                                decimal sumPrice = dataTable.AsEnumerable()
                                    .Where(row => row.Field<decimal?>("Amount") != null)
                                    .Sum(row => row.Field<decimal>("Amount"));

                                // Display the total in the TextBox
                                txtCancelled_bills_Total.Text = sumPrice.ToString("0.00");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error during data operation: " + e.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Cancelled Bills is Unsuccessful: " + ex.Message);
            }
        }

        private void txt_cancelBill_no_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txt_cancelBill_no.Text != "" && txt_cancelBill_no.Text != "Bill No")
                {
                    txt_cancellbill_description.Focus();
                }
            }
        }

        private void txt_cancellbill_description_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txt_cancellbill_description.Text != "" && txt_cancellbill_description.Text != "Enter Description")
                {
                    txt_cancessBill_amount.Focus();
                }
            }
        }

        private void txt_cancessBill_amount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txt_cancessBill_amount.Text != "" && txt_cancessBill_amount.Text != "Amount")
                {
                    btnCanellBillAdd.Focus();
                }
            }
        }

        private void btnCanellBillAdd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txt_cancelBill_no.Text == "" && txt_cancelBill_no.Text == "Bill No")
                {
                    MessageBox.Show("Bill Number is Empty", "STOP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else if (txt_cancellbill_description.Text == "" && txt_cancellbill_description.Text == "Enter Description")
                {
                    MessageBox.Show("Bill Description is Empty", "STOP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else if (txt_cancessBill_amount.Text == "" && txt_cancessBill_amount.Text == "Amount")
                {
                    MessageBox.Show("Bill Amount is Empty", "STOP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else
                {
                    btnCanellBillAdd.PerformClick();
                    txt_cancelBill_no.Text = "";
                    txt_cancellbill_description.Text = "";
                    txt_cancessBill_amount.Text = "";
                    txt_cancelBill_no.Focus();
                }
            }
        }

        private async void dgCancelledBills_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is in the "Delete" button column
            if (dgCancelledBills.Columns[e.ColumnIndex].Name == "Remove" && e.RowIndex >= 0)
            {
                // Get the ID of the selected row
                int id = Convert.ToInt32(dgCancelledBills.Rows[e.RowIndex].Cells["idcb"].Value);
                
                DeleteRecord(id, "id", "TblManualClancelBills");

                // Optionally, refresh the DataGridView after deletion
                await CancelledBills_Load(dtpReportDate.Value.ToString("yyyy-MM-dd"));
            }
        }

        //------------------------------------------------------------------------------------------------Purchesing Section Handle

        private void Make_Build_Logs(string Cashier_Name)
        {
            try
            {
                SqlConnection con = connection.my_conn();
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                string insert = "insert into MPR_Report_Logs (Cashier_Name, Report_Date, Built_Date) " +
                    "values (@Cashier_Name ,@Report_Date, @Built_Date)";
                SqlCommand cmd = new SqlCommand(insert, con);
                cmd.Parameters.AddWithValue("@Cashier_Name", Cashier_Name);
                cmd.Parameters.AddWithValue("@Report_Date", dtpReportDate.Value);
                cmd.Parameters.AddWithValue("@Built_Date", DateTime.Now);

                cmd.ExecuteNonQuery();

                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}\nDetails: {ex.StackTrace}");
            }
        }

        private void clear_purchesing_inouts()
        {
            txtPurchesingId.Text = "";
            txtQuantity.Text = "Qt";
            txtCost.Text = "0.00";
            txtPurchesingAmount.Text = "Amount";
            txt_optional_disc.Text = "Note*";
        }

        private async void btnPurchesingAdd_Click(object sender, EventArgs e)
        {
            
            if (cbPurchesingDescription.Text == "")
            {
                MessageBox.Show("Item is Empty", "STOP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtQuantity.Text == "" || txtQuantity.Text == "Qt")
            {
                MessageBox.Show("Quantity is Empty", "STOP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtPurchesingAmount.Text == "" || txtPurchesingAmount.Text == "Amount")
            {
                MessageBox.Show("Amount is Empty", "STOP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    SqlConnection con = connection.my_conn();
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    string insert = "insert into TblPurchesing (hed_id ,Kid, note, quantity, cost, amount) " +
                        "values (@hed_id ,@Kid , @note, @quantity, @cost ,@amount)";
                    SqlCommand cmd = new SqlCommand(insert, con);
                    cmd.Parameters.AddWithValue("@hed_id", Convert.ToInt32(cb_txt_p_bill_no.SelectedValue));
                    cmd.Parameters.AddWithValue("@Kid", Convert.ToInt32(cbPurchesingDescription.SelectedValue));
                    cmd.Parameters.AddWithValue("@note", string.IsNullOrEmpty(txt_optional_disc.Text) || txt_optional_disc.Text == "Note*" ? (object)DBNull.Value : txt_optional_disc.Text);
                    cmd.Parameters.AddWithValue("@quantity", txtQuantity.Text);
                    cmd.Parameters.AddWithValue("@cost", txtCost.Text);
                    cmd.Parameters.AddWithValue("@amount", txtPurchesingAmount.Text);

                    cmd.ExecuteNonQuery();

                    update_cost(cbPurchesingDescription.SelectedValue.ToString(), txtCost.Text);

                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                await Purchesting_Load(dtpReportDate.Value.ToString("yyyy-MM-dd"));
                clear_purchesing_inouts();
                txtPurchesingId.Focus();
                once = false;
            }
        }

        private async Task Purchesting_Load(string Date)
        {
            try
            {
                using (SqlConnection con = connection.my_conn())
                {
                    await con.OpenAsync();

                    try
                    {
                        // Prepare the SQL query with date-only comparison using CONVERT
                        string selectQuery = @"
                    SELECT * 
                    FROM Vw_Purchesing 
                    WHERE CONVERT(VARCHAR(10), [date], 120) = CONVERT(VARCHAR(10), @Date, 120) 
                    OR CONVERT(VARCHAR(10), credit_date, 120) = CONVERT(VARCHAR(10), @Date, 120)";

                        using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                        {
                            // Convert the selected date to the same format for comparison
                            string formattedDate = DateTime.Parse(Date).ToString("yyyy-MM-dd");

                            // Add the parameter value for the date only
                            cmd.Parameters.AddWithValue("@Date", formattedDate);

                            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                            {
                                DataTable dataTable = new DataTable();

                                // Fill the DataTable asynchronously to avoid blocking the UI
                                await Task.Run(() => dataAdapter.Fill(dataTable));

                                // Bind the DataTable to the DataGridView
                                dgPurchesing.AutoGenerateColumns = false;
                                dgPurchesing.DataSource = dataTable;

                                dgPurchesing.CellFormatting += (s, e) =>
                                {
                                    if (e.ColumnIndex == dgPurchesing.Columns["dasd"].Index) // Replace "Amount" with the actual column name or index
                                    {
                                        if (e.Value != null && decimal.TryParse(e.Value.ToString(), out decimal amount))
                                        {
                                            e.Value = amount.ToString("N2");
                                            e.FormattingApplied = true;
                                        }
                                    }

                                    if (e.ColumnIndex == dgPurchesing.Columns["afer"].Index) // Replace "Amount" with the actual column name or index
                                    {
                                        if (e.Value != null && decimal.TryParse(e.Value.ToString(), out decimal amount))
                                        {
                                            e.Value = amount.ToString("N2");
                                            e.FormattingApplied = true;
                                        }
                                    }
                                };

                                // Format the date for comparison
                                string reportDateStr = dtpReportDate.Value.ToString("yyyy-MM-dd");

                                // Sum for records with null credit_date
                                decimal sumNullCreditDate = dataTable.AsEnumerable()
                                    .Where(row => row.IsNull("credit_date") && row.Field<bool>("payment_method"))
                                    .Sum(row => row.Field<decimal>("amount"));

                                // Sum for records with non-null credit_date matching the specified date
                                decimal sumNonNullCreditDate = dataTable.AsEnumerable()
                                    .Where(row => !row.IsNull("credit_date") &&
                                                  row.Field<bool>("payment_state") &&
                                                  ((DateTime)row["credit_date"]).ToString("yyyy-MM-dd") == reportDateStr)
                                    .Sum(row => row.Field<decimal>("amount"));

                                // Total sum
                                decimal totalSumPrice = sumNullCreditDate + sumNonNullCreditDate;

                                // Display the total sum in the TextBox
                                txtPurchesing_Total.Text = totalSumPrice.ToString("0.00");

                                // Handle any additional formatting if needed
                                dgPurchesing.CellFormatting += dgPurchesing_CellFormatting;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error during data operation: " + e.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Process Unsuccessful: " + ex.Message);
            }
        }

        private void dgPurchesing_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the column being formatted is the "cc" column (payment_method)
            if (dgPurchesing.Columns[e.ColumnIndex].Name == "cc")
            {
                var paymentMethod = dgPurchesing.Rows[e.RowIndex].Cells["cc"].Value;  // Access the payment_method (bit)
                var creditDate = dgPurchesing.Rows[e.RowIndex].Cells["cd_pays_on"].Value;  // Access the credit date (cd_pays_on)

                // Convert the payment_method to bool (bit) for handling (true for Cash, false for Credit)
                bool isCash = paymentMethod != DBNull.Value && (bool)paymentMethod; // payment_method == 1 means Cash, 0 means Credit

                // Define report date
                DateTime reportDate = dtpReportDate.Value.Date;

                // Handle formatting based on conditions
                if (creditDate == DBNull.Value)
                {
                    // If credit_date is null, assign based on paymentMethod
                    e.Value = isCash ? "Cash" : "Credit";
                }
                else
                {
                    // Convert creditDate to DateTime if not null
                    DateTime dateFromCell;
                    if (DateTime.TryParse(creditDate.ToString(), out dateFromCell))
                    {
                        DateTime dateOnlyFromCell = dateFromCell.Date;

                        // Assign value based on paymentMethod and date comparison
                        if (isCash && dateOnlyFromCell == reportDate)
                        {
                            e.Value = "Cash";
                        }
                        else if (!isCash && dateOnlyFromCell != reportDate)
                        {
                            e.Value = "Credit";
                        }
                        else if (!isCash && dateOnlyFromCell == reportDate)
                        {
                            e.Value = "Cash";
                        }
                    }
                }

                // Indicate that formatting has been applied
                e.FormattingApplied = true;
            }
        }

        private async void dgPurchesing_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is in the "Delete" button column
            if (dgPurchesing.Columns[e.ColumnIndex].Name == "Delete" && e.RowIndex >= 0)
            {
                // Get the ID of the selected row
                int id = Convert.ToInt32(dgPurchesing.Rows[e.RowIndex].Cells["id"].Value);
                int hed_id = Convert.ToInt32(dgPurchesing.Rows[e.RowIndex].Cells["Hed_Id"].Value);
                string credit_pays = dgPurchesing.Rows[e.RowIndex].Cells["cd_pays_on"].Value.ToString();

                // Call the method to delete the record
                if (credit_pays == "")
                {
                    DeleteRecord(id, "id", "TblPurchesing");
                } else if (credit_pays != "")
                {
                    DeleteCreditRecode(hed_id);
                }

                // Optionally, refresh the DataGridView after deletion
                await Purchesting_Load(dtpReportDate.Value.ToString("yyyy-MM-dd"));
            }
        }

        //------------------------------------------------------------------------------------------------credit Section Handle

        private void LoadCreditToGrid()
        {
            try
            {
                SqlConnection con = connection.my_conn();
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                try
                {
                    string selectQuery = "SELECT Hed_ID, supplier, payment_method, SUM(amount) AS amount,MAX(date) AS date " +
                        "FROM Vw_Purchesing WHERE payment_method = 0 AND payment_state = 0 GROUP BY Hed_ID, supplier, payment_method;";

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(selectQuery, con);
                    DataTable dataTable = new DataTable();

                    dataAdapter.Fill(dataTable);

                    DgPcredit.AutoGenerateColumns = false;
                    DgPcredit.DataSource = dataTable;

                    DgPcredit.CellFormatting += (s, e) =>
                    {
                        if (e.ColumnIndex == DgPcredit.Columns["fgjjj"].Index) // Replace "Amount" with the actual column name or index
                        {
                            if (e.Value != null && decimal.TryParse(e.Value.ToString(), out decimal amount))
                            {
                                e.Value = amount.ToString("N2");
                                e.FormattingApplied = true;
                            }
                        }
                    };
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Process Unsuccessful : ");
            }
        }

        private async void DgPcredit_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is in the "Delete" button column
            if (DgPcredit.Columns[e.ColumnIndex].Name == "Pay" && e.RowIndex >= 0)
            {
                try
                {
                    // Get the ID of the selected row
                    int id = Convert.ToInt32(DgPcredit.Rows[e.RowIndex].Cells["pcid"].Value);
                    Decide_Paied(id);
                    LoadCreditToGrid();
                    await Purchesting_Load(dtpReportDate.Value.ToString("yyyy-MM-dd"));
                }
                catch
                {
                    MessageBox.Show("Got Some Erros !", "STOP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        //------------------------------------------------------------------------------------------------Staff Section Handle

        private async void btnStaffAdvanceAdd_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = connection.my_conn();
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                string insert = "insert into TblStaffExpences (description, amount, date) " +
                    "values (@description,@amount,@date)";
                SqlCommand cmd = new SqlCommand(insert, con);
                cmd.Parameters.AddWithValue("@description", cbStaffDiscription.Text);
                cmd.Parameters.AddWithValue("@amount", txtStaffAmount.Text);
                cmd.Parameters.AddWithValue("@date", dtpReportDate.Value.ToString("yyyy-MM-dd"));

                cmd.ExecuteNonQuery();

                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                clear_purchesing_inouts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            cbStaffDiscription.Text = "Enter Discription";
            txtStaffAmount.Text = "Amount";
            cbStaffDiscription.Focus();
            await StaffExpencess_Load(dtpReportDate.Value.ToString("yyyy-MM-dd"));
        }

        private async Task StaffExpencess_Load(string Date)
        {
            try
            {
                using (SqlConnection con = connection.my_conn())
                {
                    // Open the connection asynchronously
                    await con.OpenAsync();

                    try
                    {
                        // Prepare the SQL query with date-only comparison using CONVERT
                        string selectQuery = @"
                    SELECT * 
                    FROM TblStaffExpences 
                    WHERE CONVERT(VARCHAR(10), date, 120) = CONVERT(VARCHAR(10), @Date, 120)";

                        using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                        {
                            // Add the parameter value for the date only
                            cmd.Parameters.AddWithValue("@Date", Date);

                            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                            {
                                DataTable dataTable = new DataTable();

                                // Fill the DataTable asynchronously to avoid blocking the UI
                                await Task.Run(() => dataAdapter.Fill(dataTable));

                                // Bind the DataTable to the DataGridView
                                dgStaffAdvance.AutoGenerateColumns = false;
                                dgStaffAdvance.DataSource = dataTable;

                                dgStaffAdvance.CellFormatting += (s, e) =>
                                {
                                    if (e.ColumnIndex == dgStaffAdvance.Columns["dataGridViewTextBoxColumn8"].Index) // Replace "Amount" with the actual column name or index
                                    {
                                        if (e.Value != null && decimal.TryParse(e.Value.ToString(), out decimal amount))
                                        {
                                            e.Value = amount.ToString("N2");
                                            e.FormattingApplied = true;
                                        }
                                    }
                                };

                                // Calculate the sum of the Amount column
                                decimal sumPrice = dataTable.AsEnumerable()
                                    .Where(row => row.Field<decimal?>("amount") != null)
                                    .Sum(row => row.Field<decimal>("amount"));

                                // Display the total sum in the TextBox
                                txtStaff_Advance_Total.Text = sumPrice.ToString("0.00");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error during data operation: " + e.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Process Unsuccessful: " + ex.Message);
            }
        }

        private async void dgStaffAdvance_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is in the "Delete" button column
            if (dgStaffAdvance.Columns[e.ColumnIndex].Name == "DeleteA" && e.RowIndex >= 0)
            {
                // Get the ID of the selected row
                int id = Convert.ToInt32(dgStaffAdvance.Rows[e.RowIndex].Cells["idA"].Value);

                // Call the method to delete the record
                DeleteRecord(id, "id", "TblStaffExpences");

                // Optionally, refresh the DataGridView after deletion
                await StaffExpencess_Load(dtpReportDate.Value.ToString("yyyy-MM-dd"));
            }
        }

        //------------------------------------------------------------------------------------------------New Report

        private async void btnNew_Click(object sender, EventArgs e)
        {
            OLD_BF = 0;
            if (bil)
            {
                MessageBox.Show("Please End the Bill First !", "WARNING !", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else if (txtCashierName.Text == "")
            {
                MessageBox.Show("Cashier Name Empty !", "STOP !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else
            {
                await ResetFields();

                if (frmlvl)
                {
                    if (dtpReportDate.Value.Date < DateTime.Now.Date)
                    {
                        backdate_billing();
                    }
                    else
                    {
                        sameday_billing();
                    }
                }
                
                cb_txt_p_bill_no.Text = "";
                cb_txt_p_bill_supplier.Text = "";

                await Hms_Data_Load(dtpReportDate.Value.ToString("yyyy-MM-dd"));
                await purchesing_bill_no();
                await purchesing_bill_supplier();
                await GetReportDataWhichAlreadyHave();
                if (!frmlvl)
                {
                    if (!cbxAvailableCashAutoManual.Checked)
                    {
                        await LoadAvailableCashAsync(dtpReportDate.Value.ToString("yyyy-MM-dd"));
                    } else
                    {
                        await LoadAvailableCashAsync(dtpReportDate.Value.ToString("yyyy-MM-dd"));
                    }
                } else
                {
                    await LoadAvailableCashAsync(dtpReportDate.Value.ToString("yyyy-MM-dd"));
                }
   
                
                await GetLastMprCashForward();
                try
                {
                    await CancelledBills_Load(dtpReportDate.Value.ToString("yyyy-MM-dd"));
                    await Purchesting_Load(dtpReportDate.Value.ToString("yyyy-MM-dd"));
                    await StaffExpencess_Load(dtpReportDate.Value.ToString("yyyy-MM-dd"));
                } catch {
                    MessageBox.Show("Purchesing, Manual Cancell, StaffExpences Error !", "Error !");
                }
                if (OLD_BF == 0)
                {
                    OLD_BF = Convert.ToDecimal(txt_Balance_Foword.Text);
                }
                txt_m_float.Text = "0";
                txt_m_float.Text = "40000.00";
                MessageBox.Show(OLD_BF.ToString());
            }
        }

        //------------------------------------------------------------------------------------------------Total Expences Get

        private void txtPurchesing_Total_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalExpenses();
        }

        private void txtCancelled_bills_Total_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalExpenses();
        }

        private void txtStaff_Advance_Total_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalExpenses();
        }

        private void lbl_real_card_sale_TextChanged(object sender, EventArgs e)
        {
            CalculateMissingCardExpence();
        }

        //------------------------------------------------------------------------------------------------first bal Get

        private void txt_hms__CardCashIncome_TextChanged(object sender, EventArgs e)
        {
            Calculate_1st_Balance();
        }

        private void txt_p_float_TextChanged(object sender, EventArgs e)
        {
            Calculate_1st_Balance();
        }

        private void txtTotal_Expenses_TextChanged(object sender, EventArgs e)
        {
            Calculate_1st_Balance();
        }

        private void txt_m_float_TextChanged(object sender, EventArgs e)
        {
            Calculate_1st_Balance();
        }

        private void bal_after_expences_TextChanged(object sender, EventArgs e)
        {
            CalculateMissingCardExpence();
        }

        //------------------------------------------------------------------------------------------------second bal Get

        private void txt_hms_card_sale_TextChanged(object sender, EventArgs e)
        {
            CalculateMissingCardExpence();
        }

        //------------------------------------------------------------------------------------------------balance foword Get

        private void bal_after_card_expences_TextChanged(object sender, EventArgs e)
        {
            CalculateBalanceFoword();
        }

        private void txt_Available_cash_TextChanged(object sender, EventArgs e)
        {
            CalculateBalanceFoword();
        }

        private void txt_OnHand_TextChanged(object sender, EventArgs e)
        {
            CalculateBalanceFoword();
        }

        private void txt_Bank_TextChanged(object sender, EventArgs e)
        {
            CalculateBalanceFoword();
        }

        //------------------------------------------------------------------------------------------------Genarate Report

        private async void btnBuild_Click(object sender, EventArgs e)
        {
            await Hms_Data_Load(dtpReportDate.Value.ToString("yyyy-MM-dd"));
            
            if (!frmlvl)
            {
                if (!cbxAvailableCashAutoManual.Checked)
                {
                    await LoadAvailableCashAsync(dtpReportDate.Value.ToString("yyyy-MM-dd"));
                }
            }else
            {
                await LoadAvailableCashAsync(dtpReportDate.Value.ToString("yyyy-MM-dd"));
            }

            //MessageBox.Show("wait");
            if ( bil ) {
                MessageBox.Show("Please End the Bill First !", "WARNING !", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            } else if (txt_Available_cash.Text == "" || txt_Available_cash.Text == "0" || txt_Available_cash.Text == "0.00" || txt_Available_cash.Text == "0.0000" ||
                txt_Available_cash.Text == "0.0")
            {
                MessageBox.Show("Please Enter the Cash Float !", "WARNING !", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            } else
            {
                try
                {
                    

                    using (SqlConnection con = connection.my_conn())
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            await con.OpenAsync();
                        }

                        string checkQuery = "SELECT COUNT(1) FROM TblMPR_Report WHERE date = @date";
                        using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                        {
                            checkCmd.Parameters.AddWithValue("@date", dtpReportDate.Value.ToString("yyyy-MM-dd"));

                            int recordExists = Convert.ToInt32(await checkCmd.ExecuteScalarAsync());

                            string query;
                            if (recordExists > 0)
                            {
                                // Prompt user for confirmation before proceeding with update
                                DialogResult result = MessageBox.Show("Are you sure you want to proceed with this update?",
                                                                      "Confirm Update",
                                                                      MessageBoxButtons.YesNo,
                                                                      MessageBoxIcon.Question);

                                if (result == DialogResult.No)
                                {
                                    return; // Exit the method if the user does not confirm
                                }

                                query = "UPDATE TblMPR_Report SET sale = @sale, cashier_name=@cashier_name, flotP = @flotP, expences = @expences, flotM = @flotM, " +
                                        "balance_1 = @balance_1, hms_card_sale = @hms_card_sale, real_card_sale = @real_card_sale, " +
                                        "difference = @difference, fee = @fee, balance_2 = @balance_2, available_cash = @available_cash, ac_state=@ac_state," +
                                        "cash_ex_sh = @cash_ex_sh, on_hand = @on_hand, total = @total, bank = @bank, " +
                                        "balance_foword = @balance_foword, report_created_date = @report_created_date " +
                                        "WHERE date = @date";
                            }
                            else
                            {
                                // Insert new record
                                query = "INSERT INTO TblMPR_Report (date, cashier_name, sale, flotP, expences, flotM, balance_1, hms_card_sale, real_card_sale, difference, fee, balance_2, " +
                                        "ac_state ,available_cash, cash_ex_sh, on_hand, total, bank, balance_foword, report_created_date) " +
                                        "VALUES (@date,@cashier_name, @sale, @flotP, @expences, @flotM, @balance_1, @hms_card_sale, @real_card_sale, @difference, @fee, @balance_2, " +
                                        "@ac_state ,@available_cash, @cash_ex_sh, @on_hand, @total, @bank, @balance_foword, @report_created_date)";
                            }

                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@date", dtpReportDate.Value.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@cashier_name", txtCashierName.Text);
                                cmd.Parameters.AddWithValue("@sale", string.IsNullOrEmpty(txt_hms__CardCashIncome.Text) ? "0.00" : txt_hms__CardCashIncome.Text);
                                cmd.Parameters.AddWithValue("@flotP", string.IsNullOrEmpty(txt_p_float.Text) ? "0.00" : txt_p_float.Text);
                                cmd.Parameters.AddWithValue("@expences", string.IsNullOrEmpty(txtTotal_Expenses.Text) ? "0.00" : txtTotal_Expenses.Text);
                                cmd.Parameters.AddWithValue("@flotM", string.IsNullOrEmpty(txt_m_float.Text) ? "0.00" : txt_m_float.Text);
                                cmd.Parameters.AddWithValue("@balance_1", string.IsNullOrEmpty(bal_after_expences.Text) ? "0.00" : bal_after_expences.Text);
                                cmd.Parameters.AddWithValue("@hms_card_sale", string.IsNullOrEmpty(txt_hms_card_sale.Text) ? "0.00" : txt_hms_card_sale.Text);
                                cmd.Parameters.AddWithValue("@real_card_sale", string.IsNullOrEmpty(txt_real_card_sale.Text) ? "0.00" : txt_real_card_sale.Text);
                                cmd.Parameters.AddWithValue("@difference", string.IsNullOrEmpty(txt_card_sales_difference.Text) ? "0.00" : txt_card_sales_difference.Text);
                                cmd.Parameters.AddWithValue("@fee", string.IsNullOrEmpty(txt_card_missing_expense.Text) ? "0.00" : txt_card_missing_expense.Text);
                                cmd.Parameters.AddWithValue("@balance_2", string.IsNullOrEmpty(bal_after_card_expences.Text) ? "0.00" : bal_after_card_expences.Text);
                                cmd.Parameters.AddWithValue("@available_cash", string.IsNullOrEmpty(txt_Available_cash.Text) ? "0.00" : txt_Available_cash.Text);
                                cmd.Parameters.AddWithValue("@ac_state", cbxAvailableCashAutoManual.Checked ? 1 : 0);
                                cmd.Parameters.AddWithValue("@cash_ex_sh", string.IsNullOrEmpty(txt_Cash_ExorShort.Text) ? "0.00" : txt_Cash_ExorShort.Text);
                                cmd.Parameters.AddWithValue("@on_hand", string.IsNullOrEmpty(txt_OnHand.Text) ? "0.00" : txt_OnHand.Text);
                                cmd.Parameters.AddWithValue("@total", string.IsNullOrEmpty(txt_FinalTotal.Text) ? "0.00" : txt_FinalTotal.Text);
                                cmd.Parameters.AddWithValue("@bank", string.IsNullOrEmpty(txt_Bank.Text) ? "0.00" : txt_Bank.Text);
                                cmd.Parameters.AddWithValue("@balance_foword", string.IsNullOrEmpty(txt_Balance_Foword.Text) ? "0.00" : txt_Balance_Foword.Text);
                                cmd.Parameters.AddWithValue("@report_created_date", DateTime.Now);

                                await cmd.ExecuteNonQueryAsync();

                            }
                        }

                        if (con.State == ConnectionState.Open)
                        {
                            con.Close();
                        }
                        await UpdateBalances(dtpReportDate.Value.ToString("yyyy-MM-dd"), NEW_BF - OLD_BF);
                        Make_Build_Logs(txtCashierName.Text);
                        CreateReport();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
        }

        //------------------------------------------------------------------------------------------------Purching key Downs !

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            Calculate_item_cost();
        }

        private void txtPurchesingAmount_TextChanged(object sender, EventArgs e)
        {
            Calculate_item_cost();
        }

        private void txtPurchesingId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (cbPurchesingDescription.Text != "" && txtPurchesingId.Text != "")
                {
                    txt_optional_disc.Focus();
                }
            }
        }

        private void cbPurchesingDescription_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (cbPurchesingDescription.Text != "")
                {
                    txt_optional_disc.Focus();
                }
            }
        }

        private void txt_optional_disc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtQuantity.Focus();
            }
        }

        private void txtQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtQuantity.Text != "Qt" && txtQuantity.Text != "") {
                    txtPurchesingAmount.Focus();
                }
            }
        }

        private void txtPurchesingAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtPurchesingAmount.Text != "Amount" && txtPurchesingAmount.Text != "")
                {
                    btnPurchesingAdd.Focus();
                }
            }
        }

        private void txtQuantity_Click(object sender, EventArgs e)
        {
            txtQuantity.SelectAll();
        }

        private void txtPurchesingAmount_Click(object sender, EventArgs e)
        {
            txtPurchesingAmount.SelectAll();
        }

        private void btnPurchesingAdd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (cbPurchesingDescription.Text == "")
                {
                    MessageBox.Show("Item is Empty", "STOP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else if (txtQuantity.Text == "")
                {
                    MessageBox.Show("Quantity is Empty", "STOP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else if (txtPurchesingAmount.Text == "")
                {
                    MessageBox.Show("Amount is Empty", "STOP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else if (cbPurchesingMethod.Text == "")
                {
                    MessageBox.Show("Method is Empty", "STOP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else
                {
                    btnPurchesingAdd.PerformClick();
                    txtPurchesingId.Focus();
                }
            }
        }

        private void txtPurchesingId_TextChanged(object sender, EventArgs e)
        {
            purchesing_item_master();
        }

        private async void cbPurchesingDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPurchesingDescription.Text != null && cbPurchesingDescription.ValueMember != null
                && cbPurchesingDescription.Text != "" && cbPurchesingDescription.ValueMember != "")
            {
                txt_purchesing_unit.Text = await get_Unit();
            }
            else
            {
                txt_purchesing_unit.Text = "-";
            }
        }

        private void cbStaffDiscription_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the Enter key was pressed
            if (e.KeyCode == Keys.Enter)
            {
                // Check if the description is not null or empty
                if (!string.IsNullOrEmpty(cbStaffDiscription.Text) && cbStaffDiscription.Text != "Enter Discription")
                {
                    // Move the focus to the amount text box
                    txtStaffAmount.Focus();
                }
                else
                {
                    //MessageBox.Show("Please enter a description.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void txtStaffAmount_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the Enter key was pressed
            if (e.KeyCode == Keys.Enter)
            {
                // Check if the amount is not null or empty
                if (!string.IsNullOrEmpty(txtStaffAmount.Text) && txtStaffAmount.Text != "Amount")
                {
                    // Move the focus to the add button
                    btnStaffAdvanceAdd.Focus();
                }
                else
                {
                    //MessageBox.Show("Please enter an amount.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnStaffAdvanceAdd_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the Enter key was pressed
            if (e.KeyCode == Keys.Enter)
            {
                // Check if both the description and amount are not null or empty before clicking
                if (!string.IsNullOrEmpty(cbStaffDiscription.Text) && !string.IsNullOrEmpty(txtStaffAmount.Text) && txtStaffAmount.Text != "Amount" && cbStaffDiscription.Text != "Enter Discription")
                {
                    // Perform the click event
                    btnStaffAdvanceAdd.PerformClick();

                    // Clear the inputs
                    cbStaffDiscription.Text = string.Empty;
                    txtStaffAmount.Text = string.Empty;

                    // Set focus back to the description for a new entry
                    cbStaffDiscription.Focus();
                }
                else
                {
                    MessageBox.Show("Please ensure both description and amount are entered.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 3)
            {
                LoadCreditToGrid();
                //MessageBox.Show("ok");
            }
        }

        //------------------------------------Purchesing BIl No --------------------------------------------------------------------------------

        private async void btn_end_bill_Click(object sender, EventArgs e)
        {
            await purchesing_bill_no();
            await purchesing_bill_supplier();
            clear_purchesing_inouts();
            disable_purchesing_inputs();
            bil = false;
        }

        private async void btn_create_bill_Click(object sender, EventArgs e)
        {
            // Validate input fields
            if (string.IsNullOrEmpty(cb_txt_p_bill_no.Text))
            {
                MessageBox.Show("Bill No is Empty", "STOP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(cb_txt_p_bill_supplier.Text))
            {
                MessageBox.Show("Supplier is Empty", "STOP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection con = connection.my_conn())
                {
                    // Ensure the connection is open
                    if (con.State != ConnectionState.Open)
                    {
                        await con.OpenAsync();
                    }

                    // Step 1: Check if a record exists for the selected bill number and date
                    string checkQuery = @"
                SELECT COUNT(*) 
                FROM TblPurchesingHed 
                WHERE bill_no = @bill_no 
                AND CONVERT(VARCHAR(10), date, 120) = CONVERT(VARCHAR(10), @date, 120) 
                AND cash_credit = @cash_credit";

                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                    {
                        // Add parameters for the bill number and date
                        checkCmd.Parameters.AddWithValue("@bill_no", cb_txt_p_bill_no.Text);
                        checkCmd.Parameters.AddWithValue("@date", dtpReportDate.Value.Date.ToString("yyyy-MM-dd"));
                        checkCmd.Parameters.AddWithValue("@cash_credit", cbPurchesingMethod.Text == "Cash" ? 1 : 0);

                        // Execute the scalar query to get the count of matching rows
                        int count = (int)await checkCmd.ExecuteScalarAsync();

                        if (count > 0)
                        {
                            // Step 2: If the bill already exists, show a message and enable the inputs for adding items
                            MessageBox.Show("This bill already exists. Please add items to the bill.", "Bill Exists", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            enable_purchesing_inputs();
                            bil = true;
                            return; // Exit the method, no need to create a new bill
                        }
                    }

                    // Step 3: If no matching bill was found, insert a new record
                    string insertQuery = @"
                INSERT INTO TblPurchesingHed (bill_no, supplier, date, cash_credit, payment_state) 
                VALUES (@bill_no, @supplier, @date, @cash_credit, @payment_state); 
                SELECT SCOPE_IDENTITY();";

                    int newBillId; // Variable to hold the newly inserted bill's ID

                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, con))
                    {
                        insertCmd.Parameters.AddWithValue("@bill_no", cb_txt_p_bill_no.Text);
                        insertCmd.Parameters.AddWithValue("@supplier", cb_txt_p_bill_supplier.Text);
                        insertCmd.Parameters.AddWithValue("@date", dtpReportDate.Value.Date);
                        insertCmd.Parameters.AddWithValue("@cash_credit", cbPurchesingMethod.Text == "Cash" ? 1 : 0);
                        insertCmd.Parameters.AddWithValue("@payment_state", cbPurchesingMethod.Text == "Cash" ? 1 : 0);

                        // Execute the insert command and retrieve the new bill ID
                        newBillId = Convert.ToInt32(await insertCmd.ExecuteScalarAsync());
                        MessageBox.Show("Bill opened successfully for " + cb_txt_p_bill_supplier.Text + ". Enter Bill Items now!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Enable inputs for adding items after the bill is created
                        enable_purchesing_inputs();
                    }

                    // Update the ComboBoxes and select the newly created bill
                    await purchesing_bill_no();
                    await purchesing_bill_supplier();

                    // Select the newly created bill in both ComboBoxes
                    cb_txt_p_bill_no.SelectedValue = newBillId;
                    cb_txt_p_bill_supplier.SelectedValue = newBillId;
                    bil = true;
                    txtPurchesingId.Focus();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void disable_purchesing_inputs()
        {
            txtPurchesingId.Enabled = false;
            cbPurchesingDescription.Enabled = false;
            txt_optional_disc.Enabled = false;
            txtQuantity.Enabled = false;
            txtCost.Enabled = false;
            txtPurchesingAmount.Enabled = false;
            btnPurchesingAdd.Enabled = false;

            cb_txt_p_bill_no.Enabled = true;
            cb_txt_p_bill_supplier.Enabled = true;
            cbPurchesingMethod.Enabled = true;
            btn_create_bill.Enabled = true;           
            btn_end_bill.Enabled = false;
        }

        private void enable_purchesing_inputs()
        {
            txtPurchesingId.Enabled = true;
            cbPurchesingDescription.Enabled = true;
            txt_optional_disc.Enabled = true;
            txtQuantity.Enabled = true;
            txtCost.Enabled = true;
            txtPurchesingAmount.Enabled = true;
            cbPurchesingMethod.Enabled = true;
            btnPurchesingAdd.Enabled = true;

            cb_txt_p_bill_no.Enabled = false;
            cb_txt_p_bill_supplier.Enabled = false;
            cbPurchesingMethod.Enabled = false;
            btn_create_bill.Enabled = false;
            btn_end_bill.Enabled = true;
        }

        private void cb_txt_p_bill_no_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ensure ComboBox has items before changing the SelectedIndex
            if (cb_txt_p_bill_no.Items.Count > 0 && cb_txt_p_bill_no.SelectedValue != null)
            {
                // Get the selected index and set the same index to supplier ComboBox
                int selectedIndex = cb_txt_p_bill_no.SelectedIndex;
                if (selectedIndex >= 0 && selectedIndex < cb_txt_p_bill_supplier.Items.Count)
                {
                    cb_txt_p_bill_supplier.SelectedIndex = selectedIndex;

                    // Set cash_credit based on some condition
                    SetCashCredit(Convert.ToInt32(cb_txt_p_bill_supplier.SelectedValue));
                }
            }
        }

        private void cb_txt_p_bill_supplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ensure ComboBox has items before changing the SelectedIndex
            if (cb_txt_p_bill_supplier.Items.Count > 0 && cb_txt_p_bill_supplier.SelectedValue != null)
            {
                // Get the selected index and set the same index to bill_no ComboBox
                int selectedIndex = cb_txt_p_bill_supplier.SelectedIndex;
                if (selectedIndex >= 0 && selectedIndex < cb_txt_p_bill_no.Items.Count)
                {
                    cb_txt_p_bill_no.SelectedIndex = selectedIndex;

                    // Set cash_credit based on some condition
                    SetCashCredit(Convert.ToInt32(cb_txt_p_bill_no.SelectedValue));
                }
            }
        }

        private void SetCashCredit(int selectedIndex)
        {
            // Replace with your actual method to get cash_credit from the database
            string cashCredit = GetCashCreditFromDatabase(selectedIndex);

            if (cashCredit == "True")
            {
                cbPurchesingMethod.Text = "Cash";
            }
            else if (cashCredit == "False")
            {
                cbPurchesingMethod.Text = "Credit";
            }
            else
            {
                // Handle unexpected values or errors
                cbPurchesingMethod.Text = "";
            }
        }

        private string GetCashCreditFromDatabase(int selectedIndex)
        {
            string cashCredit = string.Empty;

            using (SqlConnection con = connection.my_conn())
            {
                con.Open();
                string query = "SELECT cash_credit FROM TblPurchesingHed WHERE id = @SelectedIndex";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@SelectedIndex", selectedIndex);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        cashCredit = result.ToString();
                    }
                }
            }

            return cashCredit;
        }

        private void cb_txt_p_bill_no_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (cb_txt_p_bill_no.Text != "")
                {
                    cb_txt_p_bill_supplier.Focus();
                }
            }
        }

        private void cb_txt_p_bill_supplier_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (cb_txt_p_bill_supplier.Text != "")
                {
                    cbPurchesingMethod.Focus();
                }
            }
        }

        private void cbPurchesingMethod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (cbPurchesingMethod.Text != "")
                {
                    btn_create_bill.Focus();
                }
            }
        }

        private void btn_create_bill_KeyDown(object sender, KeyEventArgs e)
        {
            btn_create_bill.PerformClick();
        }

        private async void cbxAvailableCashAutoManual_CheckedChanged(object sender, EventArgs e)
        {
            if (!frmlvl)
            {
                if (!cbxAvailableCashAutoManual.Checked)
                {
                    await LoadAvailableCashAsync(dtpReportDate.Value.ToString("yyyy-MM-dd"));
                } else
                {
                    await LoadAvailableCashAsync(dtpReportDate.Value.ToString("yyyy-MM-dd"));
                }
            }
        }

        private void txt_Balance_Foword_TextChanged(object sender, EventArgs e)
        {
            NEW_BF = Convert.ToDecimal(txt_Balance_Foword.Text);
        }
    }
}
