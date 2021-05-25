using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IOOP_Assignment
{
    public partial class Librarian_Menu : Form
    {
        public Librarian_Menu()
        {
            InitializeComponent();
            customiseDesign();
        }
        private void customiseDesign()
        {
            panelReportSubMenu.Visible = false;
        }
        private void hideSubMenu()
        {
            if (panelReportSubMenu.Visible == true)
                panelReportSubMenu.Visible = false;
        }
        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideSubMenu();
                subMenu.Visible = true;
            }
            else
            {
                subMenu.Visible = false;
            }
        }

        private void reportBtn_Click(object sender, EventArgs e)
        {
            showSubMenu(panelReportSubMenu);
        }


        private void monthlyReportButton_Click(object sender, EventArgs e)
        {
            //Code...
            hideSubMenu();
        }

        private void dailyReportBtn_Click(object sender, EventArgs e)
        {
            //Code...
            hideSubMenu();
        }
    }
}
