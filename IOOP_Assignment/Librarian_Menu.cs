﻿using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IOOP_Assignment
{
    public partial class Librarian_Menu : Form
    {
        //Declare connection string
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString());
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
        private void btnDashboardPage_Click(object sender, EventArgs e)
        {
            bunifuPages2.PageIndex = 0;
        }

        private void btnChangesPage_Click(object sender, EventArgs e)
        {
            bunifuPages2.PageIndex = 1;
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

        private void Librarian_Menu_Load(object sender, EventArgs e)
        {
            showGraph();
            formatTables();

            //Dropdown for monthly utilization report
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
           
            //Displays at Overview
            string tdy_date = Now.ToString("M/d/yyyy");
            con.Open();
            SqlCommand sql = new SqlCommand($"Select count(Date) from [dbo].[Reservation] where Date='{tdy_date}'", con);
            lblReservationTdy.Text = sql.ExecuteScalar().ToString();
            SqlCommand sql2 = new SqlCommand("Select count(TPNumber) from [dbo].[User] where role='Student'", con);
            lblTotalUsers.Text = sql2.ExecuteScalar().ToString();
            SqlCommand sql3 = new SqlCommand("Select count(RequestID) from [dbo].[Request]", con);
            lblUnattendReq.Text = sql3.ExecuteScalar().ToString();
            //Username
            SqlCommand sql4 = new SqlCommand($"Select Name from [dbo].[User] where Email='{User.loginEmail}'", con);
            User.name = sql4.ExecuteScalar().ToString();
            lblNameL1.Text = User.name;
            lblNameL2.Text = User.name;
            lblNameL3.Text = User.name;
            lblNameL4.Text = User.name;
            con.Close();

            //Display Request Data in Request Table
            Request reqData = new Request();
            List<Request> reqList = reqData.getReqData();
            if (reqList != null)
            {
                foreach (var req in reqList)
                {
                    requestDataGridView.Rows.Add(
                        new object[]
                        {
                            req.StudentID,
                            req.RoomType,
                            req.Date,
                            req.Time,
                            req.NumStudents,
                            req.Duration,
                            req.ReservationID,
                        });
                }
            }
        }

        private void showGraph()
        {
            using (ReservationSystemEntities db = new ReservationSystemEntities())
            {
                var data = db.GetPastReservation();
                LineSeries line = new LineSeries()
                {
                    DataLabels = true,
                    Values = new ChartValues<int>(),
                    LabelPoint
                = point => point.Y.ToString()
                };
                Axis ax = new Axis();
                ax.Labels = new List<string>();
                foreach (var x in data)
                {
                    line.Values.Add(x.TotalReservation.Value);
                    string dates = x.Date.ToString().Replace("12:00:00 AM", "");
                    ax.Labels.Add(dates);
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

        private void btnSearchDaily_Click(object sender, EventArgs e)
        {
            dailyReportTable.Rows.Clear();
            string date = dailyReportDatePicker.Value.ToString("MM/dd/yyyy");
            string roomType = radDaphne.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked).Text;
            Reservation dailyReport = new Reservation();
            List<Reservation> resList = dailyReport.getDailyReport(date, roomType);
            if (resList != null)
            {
                foreach (var res in resList)    //Fills the table with data
                {
                    dailyReportTable.Rows.Add(
                        new object[]
                        {
                            res.ResID,
                            res.RoomType,
                            res.RoomNumber,
                            res.NumStudents,
                            res.Date,
                            res.Time,
                            res.Duration,
                            res.StudentID,
                        });
                }
            }
        }
        private void formatTables()
        {
            //Bunifu DataGridView Style Formatting
            dailyReportTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dailyReportTable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dailyReportTable.Columns["colResID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dailyReportTable.Columns["colRoomType"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dailyReportTable.Columns["colRoomNum"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dailyReportTable.Columns["colNumStudents"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dailyReportTable.Columns["colDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dailyReportTable.Columns["colTime"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dailyReportTable.Columns["colDuration"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dailyReportTable.Columns["colStudentID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            requestDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            requestDataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            requestDataGridView.Columns["colStudID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            requestDataGridView.Columns["colNewRoom"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            requestDataGridView.Columns["colNewDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            requestDataGridView.Columns["colNewTime"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            requestDataGridView.Columns["colNewStudentNum"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            requestDataGridView.Columns["colNewDuration"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            requestDataGridView.Columns["colNewResID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

    }
}
