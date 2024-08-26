using Org.BouncyCastle.Crypto.Macs;
using ProfitMate;
using System;
using System.Linq;
using System.Windows.Forms;

namespace ProfitMate
{
    internal static class Program
    {

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Show the Splash form
            var splashForm = new Forms.Splash();
            splashForm.Show();

            Application.Run();
        }
        
    }
}
