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
using MySql.Data.MySqlClient;
using ProfitMate;

namespace ProfitMate.Forms
{
    public partial class frmLogin : Form
    {
        Connection connection;
        public frmLogin()
        {
            InitializeComponent();
            connection = new Connection();
            //Program.AddOpenForm(this);
        }

        private void saveuser_settigns()
        {
            Properties.Settings.Default.LogUsername = txtUser.Text.ToString();
            Properties.Settings.Default.Save();
        }

        private void loaduser_settigns()
        {
            if (Properties.Settings.Default.LogUsername != "")
            {
                txtUser.Text = Properties.Settings.Default.LogUsername;
            }
        }

        private void prove_login(string username, string password)
        {
            if (username == null && password == null)
            {

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

                    string query = "select App_name,Us_is_Cashier,Us_pass from [User_info] where Us_name=@username";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@username", username);
                    //cmd.Parameters.AddWithValue("@password", password);
                    SqlDataReader reader = cmd.ExecuteReader();

                    string App_name;
                    bool lvl = true;
                    string Pass = "";
                    string asciiPass = string.Empty;
                    foreach (char c in password)
                    {
                        asciiPass += ((int)c).ToString();
                    }

                    asciiPass = asciiPass.Trim();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            App_name = reader["App_name"].ToString();
                            lvl = Convert.ToBoolean(reader["Us_is_Cashier"]);
                            Pass = reader["Us_pass"].ToString();                           
                        }
              
                        if (asciiPass == Pass)
                        {
                            if (rem_user.Checked)
                            {
                                saveuser_settigns();
                            }
                            MDI mdi = new MDI(txtUser.Text, lvl);
                            mdi.Show();
                            this.Close();
                        } else
                        {
                            MessageBox.Show("Unknown User or Invalid credentials! ", "Access Declined !", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            Properties.Settings.Default.LogUsername = "";
                            Properties.Settings.Default.Save();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unknown User or Invalid credentials! ", "Access Declined !", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        Properties.Settings.Default.LogUsername = "";
                        Properties.Settings.Default.Save();
                    }

                    if (con.State != ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            prove_login(txtUser.Text, txtPass.Text);
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            //Splash splash = new Splash();
            //splash.Show();
            loaduser_settigns();
        }

        private void txtUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPass.Focus();
            }
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                prove_login(txtUser.Text, txtPass.Text);
            }
        }

        private void frmLogin_Activated(object sender, EventArgs e)
        {
            if (txtUser.Text != "")
            {
                txtPass.Focus();
            }
            else
            {
                txtUser.Focus();
            }
        }
    }
}
