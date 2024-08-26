using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;

namespace ProfitMate.Forms
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
        }
        static string subFolderPath = "k-chord";
        static RegistryKey BaseFolderPath = Registry.CurrentUser;
        string serialNumber = "#0094-0788-8066-7007-6129-4262#";

        private void GetDeviceSerialNumber()
        {
            lbls.Text = "`#`cc-mnh-mnd`#`";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Loading.Width += 4;

            if (Loading.Width == 150)
            {
                timer1.Stop();
                //Thread.Sleep(500);
                string registryPath = "Software\\" + subFolderPath;

                try
                {
                    using (RegistryKey key = BaseFolderPath.OpenSubKey(registryPath))
                    {
                        if (key != null)
                        {
                            string registryValue = key.GetValue("key") as string;

                            if (registryValue != null)
                            {
                                if (registryValue == serialNumber)
                                {
                                    timer1.Start();
                                    return;

                                }
                                else
                                {
                                    MessageBox.Show("Licesening Failed !", "Please Enter vaalid Product key !");
                                    keyPannel.Visible = true;
                                }
                            }
                            else
                            {
                                keyPannel.Visible = true;
                            }
                        }
                        else
                        {
                            keyPannel.Visible = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }

            }
            else if (Loading.Width == 286)
            {
                timer1.Stop();
                string registryPath = "Software\\" + subFolderPath;

                try
                {
                    using (RegistryKey key = BaseFolderPath.OpenSubKey(registryPath))
                    {
                        if (key != null)
                        {
                            string registryValue = key.GetValue("Path") as string;

                            if (registryValue != null)
                            {
                                timer1.Start();
                                return;
                            }
                            else
                            {
                                dbPannel.Visible = true;
                            }
                        }
                        else
                        {
                            dbPannel.Visible = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else if (Loading.Width == 454)
            {
                timer1.Stop();
                Thread.Sleep(800);
                timer1.Start();

            }
            else if (Loading.Width == 650)
            {
                timer1.Stop();
                Thread.Sleep(500);
                timer1.Start();
            }
            else if (Loading.Width >= 800)
            {
                timer1.Stop();
                Thread.Sleep(200);
                frmLogin login = new frmLogin();
                login.Show();
                this.Close();
            }
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            dbPannel.Visible = false;
            keyPannel.Visible = false;
            GetDeviceSerialNumber();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string registryPath = "Software\\" + subFolderPath;

            try
            {
                using (RegistryKey key = BaseFolderPath.CreateSubKey(registryPath))
                {
                    key.SetValue("Path", txtPath.Text);
                }

                dbPannel.Visible = false;
                timer1.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string registryPath = "Software\\" + subFolderPath;

            try
            {
                if (serialNumber == txtKey.Text)
                {
                    using (RegistryKey key = BaseFolderPath.CreateSubKey(registryPath))
                    {
                        key.SetValue("key", txtKey.Text);
                    }

                    keyPannel.Visible = false;
                    timer1.Start();
                }
                else
                {
                    MessageBox.Show("Invalid Key !", "License Key is Invalid. Please Re Enter !");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
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
    }

}
