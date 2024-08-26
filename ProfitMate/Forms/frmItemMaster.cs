using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProfitMate.Forms
{
    public partial class frmItemMaster : Form
    {
        Connection connection;

        public frmItemMaster()
        {
            InitializeComponent();
            connection = new Connection();
        }

        bool frmlvl = true;

        public frmItemMaster(string user, bool lvl)
        {
            InitializeComponent();
            connection = new Connection();
            frmlvl = lvl;
        }

        private void user_rights_manage()
        {
            if (frmlvl) 
            {
                btn_delete.Enabled = false;
            }
        }

        int mode = 0;

        private async Task<string> GenerateItemIdAsync(string itemName)
        {
            if (string.IsNullOrEmpty(itemName))
                return string.Empty;

            // Get the first letter of the item name and convert it to uppercase
            char firstLetter = char.ToUpper(itemName[0]);
            string newItemId = string.Empty;

            // Use your existing connection method
            using (SqlConnection con = connection.my_conn())
            {
                await con.OpenAsync();

                // SQL query to get the max Item_code starting with the first letter
                string query = "SELECT MAX(Item_code) FROM tblMstItem WHERE Item_code LIKE @prefix + '%'";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@prefix", firstLetter.ToString());

                    // Execute the query asynchronously and get the result
                    object result = await cmd.ExecuteScalarAsync();
                    if (result != DBNull.Value && result != null)
                    {
                        string maxItemId = result.ToString();
                        // Extract the numeric part from the max Item_code
                        int maxNumericPart = int.Parse(maxItemId.Substring(1));

                        // Increment the numeric part by one
                        int newNumericPart = maxNumericPart + 1;

                        // Format the new Item_code
                        newItemId = $"{firstLetter}{newNumericPart.ToString("D9")}";
                    }
                    else
                    {
                        // If no matching Item_code is found, start with "A000000001"
                        newItemId = $"{firstLetter}000000001";
                    }
                }
            }

            return newItemId;
        }


        //private string GenerateItemId(string itemName)
        //{
        //    if (string.IsNullOrEmpty(itemName))
        //        return string.Empty;

        //    // Get the first letter of the item name and convert it to uppercase
        //    char firstLetter = char.ToUpper(itemName[0]);

        //    // Generate a random number between 100000000 and 999999999
        //    Random random = new Random();
        //    int randomNumber = random.Next(100000000, 1000000000); // upper bound is exclusive, so we use 1000000000

        //    // Combine the first letter and the random/ number to form the ID
        //    string itemId = $"{firstLetter}{randomNumber}";

        //    return itemId;
        //}

        void LoadToGrid()
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
                    // Create the select query with a LIKE clause and a parameter
                    string selectQuery = "SELECT * FROM tblMstItem WHERE Item_Name LIKE @ItemName";

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(selectQuery, con);

                    // Add the parameter for the LIKE clause, allowing for partial matches
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@ItemName", "%" + txt_itm_search.Text + "%");

                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    dgData.AutoGenerateColumns = false;
                    dgData.DataSource = dataTable;
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

        private void get_next_Kid()
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
                    // SQL query to get the maximum Kid from the table, or 0 if the table is empty
                    string selectQuery = "SELECT ISNULL(MAX(Kid), 0) + 1 FROM tblMstItem";

                    SqlCommand cmd = new SqlCommand(selectQuery, con);

                    // Execute the query and get the next Kid
                    object result = cmd.ExecuteScalar();

                    // Set the result to the txt_no textbox
                    txt_no.Text = result.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching next Kid: " + ex.Message);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Process Unsuccessful.");
            }
        }



        void Unlock_for_new()
        {
            txt_itm_search.Text = "";
            txt_itm_search.Enabled = false;
            txt_item_name.Enabled = true;
            txt_unit.Enabled = true;
            txt_cost.Enabled = true;
            txt_item_type.Enabled = true;
            txt_no.Enabled = true;
        }

        void Clear()
        {
            txt_id.Text = "";
            txt_item_name.Text = "";
            //txt_unit.Text = "";
            txt_cost.Text = "";
            //txt_item_type.Text = string.Empty;
            txt_no.Text = "";
            user_rights_manage();
        }

        void Bnlock()
        {
            txt_itm_search.Text = "";
            txt_itm_search.Enabled = true;
            txt_item_name.Enabled = false;
            txt_unit.Enabled = false;
            txt_cost.Enabled = false;
            txt_item_type.Enabled = false;
            txt_no.Enabled = false;
            user_rights_manage();
        }

        private async void txt_item_name_TextChanged(object sender, EventArgs e)
        {
            if (mode == 1)
            {
                txt_id.Text = await GenerateItemIdAsync(txt_item_name.Text);
            }
        }

        private void frmItemMaster_Load(object sender, EventArgs e)
        {
            LoadToGrid();
            Bnlock();
            btn_edit.Enabled = true;
            btn_delete.Enabled = true;
            btn_new.Enabled = true;
            btn_save.Enabled = false;
            btn_cancel.Enabled = false;
            user_rights_manage();
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            mode = 1;
            Unlock_for_new();
            Clear();
            btn_edit.Enabled = false;
            btn_delete.Enabled = false;
            btn_new.Enabled = false;
            btn_save.Enabled = true;
            btn_cancel.Enabled = true;
            txt_item_name.Focus();
            user_rights_manage();
            get_next_Kid();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Bnlock();
            btn_edit.Enabled = true;
            btn_delete.Enabled = true;
            btn_new.Enabled = true;
            btn_save.Enabled = false;
            btn_cancel.Enabled = false;
            user_rights_manage();
            Clear();
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            mode = 2;
            Unlock_for_new();
            btn_edit.Enabled = false;
            btn_delete.Enabled = false;
            btn_new.Enabled = false;
            btn_save.Enabled = true;
            btn_cancel.Enabled = true;
        }

        private void dgData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dgData.RowCount)
                {
                    txt_id.Text = dgData.Rows[e.RowIndex].Cells[0].Value.ToString();
                    txt_item_name.Text = dgData.Rows[e.RowIndex].Cells[1].Value.ToString();
                    txt_unit.Text = dgData.Rows[e.RowIndex].Cells[2].Value.ToString();
                    txt_cost.Text = dgData.Rows[e.RowIndex].Cells[3].Value.ToString();
                    txt_item_type.Text = dgData.Rows[e.RowIndex].Cells[4].Value.ToString();
                    txt_no.Text = dgData.Rows[e.RowIndex].Cells[5].Value.ToString();
                }
            }
            catch (Exception)
            {

            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = connection.my_conn())
                {
                    // Ensure the connection is open
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    // Check if we're in insert or update mode
                    if (mode == 1) // Insert mode
                    {
                        // Check if the Kid already exists for insert
                        if (IsKidExists(con, txt_no.Text))
                        {
                            int nextAvailableKid = getNextAvailableKid(con);
                            MessageBox.Show($"This Kid number is already assigned to another item. " +
                                            $"Please use the next available Kid: {nextAvailableKid}.",
                                            "Kid Already Exists",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);
                            return;
                        }

                        // Insert operation
                        string insert = @"INSERT INTO tblMstItem (Item_code, Item_Name, Unit, Cost, QOH, ItemType, OrderID, Kid) 
                                  VALUES (@Item_code, @Item_Name, @Unit, @Cost, @QOH, @ItemType, @OrderType, @Kid)";

                        using (SqlCommand cmd = new SqlCommand(insert, con))
                        {
                            cmd.Parameters.AddWithValue("@Item_code", txt_id.Text);
                            cmd.Parameters.AddWithValue("@Item_Name", txt_item_name.Text);
                            cmd.Parameters.AddWithValue("@Unit", txt_unit.Text);
                            cmd.Parameters.AddWithValue("@Cost", decimal.Parse(txt_cost.Text));
                            cmd.Parameters.AddWithValue("@QOH", 0.00);
                            cmd.Parameters.AddWithValue("@ItemType", txt_item_type.Text);
                            cmd.Parameters.AddWithValue("@OrderType", "9999");
                            cmd.Parameters.AddWithValue("@Kid", txt_no.Text);

                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Item added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Clear();
                    }
                    else if (mode == 2) // Update mode
                    {
                        // Fetch the original Kid for the current item being updated
                        string originalKid = GetOriginalKid(con, txt_id.Text);

                        // If the Kid was changed by the user, check for conflicts
                        if (txt_no.Text != originalKid && IsKidExists(con, txt_no.Text))
                        {
                            int nextAvailableKid = getNextAvailableKid(con);
                            MessageBox.Show($"This Kid number is already assigned to another item. " +
                                            $"Please use the next available Kid: {nextAvailableKid}.",
                                            "Kid Already Exists",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);
                            return;
                        }

                        // Update operation
                        string update = @"UPDATE tblMstItem 
                                  SET Item_Name = @Item_Name, 
                                      Unit = @Unit, 
                                      Cost = @Cost, 
                                      ItemType = @ItemType, 
                                      Kid = @Kid 
                                  WHERE Item_code = @Item_code";

                        using (SqlCommand cmd = new SqlCommand(update, con))
                        {
                            cmd.Parameters.AddWithValue("@Item_code", txt_id.Text);
                            cmd.Parameters.AddWithValue("@Item_Name", txt_item_name.Text);
                            cmd.Parameters.AddWithValue("@Unit", txt_unit.Text);
                            cmd.Parameters.AddWithValue("@Cost", decimal.Parse(txt_cost.Text));
                            cmd.Parameters.AddWithValue("@ItemType", txt_item_type.Text);
                            cmd.Parameters.AddWithValue("@Kid", txt_no.Text);

                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Item updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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
            LoadToGrid();
        }

        private bool IsKidExists(SqlConnection con, string kid)
        {
            string query = "SELECT COUNT(*) FROM tblMstItem WHERE Kid = @Kid";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Kid", kid);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        private string GetOriginalKid(SqlConnection con, string itemCode)
        {
            string query = "SELECT Kid FROM tblMstItem WHERE Item_code = @Item_code";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Item_code", itemCode);
                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : string.Empty;
            }
        }

        private int getNextAvailableKid(SqlConnection con)
        {
            string query = "SELECT ISNULL(MAX(Kid), 0) + 1 FROM tblMstItem";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                return (int)cmd.ExecuteScalar();
            }
        }


        private void btn_delete_Click(object sender, EventArgs e)
        {
            // Confirm delete action with the user
            DialogResult result = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection con = connection.my_conn())
                    {
                        // Ensure the connection is open
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        // Delete operation
                        string delete = @"DELETE FROM tblMstItem WHERE Item_code = @Item_code";

                        using (SqlCommand cmd = new SqlCommand(delete, con))
                        {
                            cmd.Parameters.AddWithValue("@Item_code", txt_id.Text);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Item deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Clear();
                            }
                            else
                            {
                                MessageBox.Show("No item found with the given Item Code.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
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

            LoadToGrid();
        }

        //keydown-------------------------------------------------------------------------------

        private void txt_item_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txt_item_name.Text != "")
                {
                    txt_unit.Focus();
                }
            }
        }

        private void txt_unit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txt_unit.Text != "")
                {
                    txt_cost.Focus();
                }
            }
        }

        private void txt_cost_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txt_cost.Text != "")
                {
                    txt_item_type.Focus();
                }
            }
        }

        private void txt_item_type_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txt_item_type.Text != "")
                {
                    txt_no.Focus();
                }
            }
        }

        private void txt_no_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txt_no.Text != "")
                {
                    btn_save.Focus();
                }
            }
        }

        private void btn_save_KeyDown(object sender, KeyEventArgs e)
        {
            if(txt_no.Text == "" || txt_unit.Text == "" || txt_cost.Text == "" || txt_item_type.Text == "" || txt_no.Text == "" || txt_item_name.Text == "")
            {
                MessageBox.Show("Some Fields Are Missing !", "Stop !", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }else
            {
                btn_save.PerformClick();
                txt_item_name.Focus();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadToGrid();
        }

        private void txt_item_name_Click(object sender, EventArgs e)
        {
            txt_item_name.SelectAll();
        }

        private void txt_cost_Click(object sender, EventArgs e)
        {
            txt_cost.SelectAll();
        }

        private void txt_no_Click(object sender, EventArgs e)
        {
            txt_no.SelectAll();
        }
    }
}
