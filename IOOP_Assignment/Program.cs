using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IOOP_Assignment
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 f1 = new Form1();
            DialogResult dr = f1.ShowDialog();
            if (dr == DialogResult.OK && f1.UserRole == "Student")
            {
                Application.Run(new Student_Menu());
            }
            else if (dr == DialogResult.OK && f1.UserRole == "Librarian")
            {
                Application.Run(new Librarian_Menu());
            }
            else
            {
                MessageBox.Show("Role not found!");
            }
        }
    }
}
