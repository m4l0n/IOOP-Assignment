﻿using LiveCharts;
using LiveCharts.Wpf;
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
            DateTime Now = DateTime.Now;
            int year = int.Parse(Now.ToString("yyyy"));
            string month = Now.ToString("MMMM");
            int min_year = year - 20;
            int max_year = year + 20;
            int item_year = min_year;
            while (item_year < max_year)
            {
                ddmYear.Items.Add(item_year.ToString());
                item_year += 1;
            }
            ddmMonth.SelectedIndex = ddmMonth.FindStringExact(month);
            ddmYear.SelectedIndex = 20;
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
            bunifuPages2.PageIndex = 3;
            //Code...
            hideSubMenu();
        }

        private void dailyReportBtn_Click(object sender, EventArgs e)
        {
            bunifuPages2.PageIndex = 2;
            //Code...
            hideSubMenu();
        }

        private void bunifuPictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void bunifuLabel10_Click(object sender, EventArgs e)
        {

        }

        private void changesPage_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            bunifuPages2.PageIndex = 0;
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            bunifuPages2.PageIndex = 1;
        }

        private void bunifuShapes1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuShapes2_Click(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void monthlyReportPage_Click(object sender, EventArgs e)
        {

        }

        private void bunifuCheckBox1_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
        {

        }

        private void totalUsersInfoButton_Click(object sender, EventArgs e)
        {
            bunifuSnackbar1.Show(this, "This card shows the total users registered in the system.",
                        Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 4000);
        }

        private void resInfoButton_Click(object sender, EventArgs e)
        {
            bunifuSnackbar1.Show(this, "This card shows the total reservations being made today.",
                Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 4000);
        }

        private void requestInfoButton_Click(object sender, EventArgs e)
        {
            bunifuSnackbar1.Show(this, "This card shows the total unattended requests for reservation changes.",
                Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 4000);
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {

        }

        private void Librarian_Menu_Load(object sender, EventArgs e)
        {
           using (ReservationSystemEntities db = new ReservationSystemEntities())
            {
                var data = db.GetPastReservation();
                LineSeries line = new LineSeries() { DataLabels = true, Values = new ChartValues<int>(), LabelPoint
                = point => point.Y.ToString() };
                Axis ax = new Axis();
                ax.Labels = new List<string>();
                foreach (var x in data)
                {
                    line.Values.Add(x.TotalReservation.Value);
                    ax.Labels.Add(x.Date.ToString());
                }
                resChart.Series.Add(line);
                resChart.AxisX.Add(ax);
                resChart.AxisY.Add(
                    new Axis
                    {
                        LabelFormatter = value => value.ToString()
                    });
            }
        }
    }
}
