using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
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
        }
        /// <summary>
        ///  customiseDesign(), hideSubMenu() & showSubMenu() shows and hide the Report panel buttons at the side menu.
        /// </summary>
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
            else subMenu.Visible = false;
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
            hideSubMenu();
        }

        private void dailyReportBtn_Click(object sender, EventArgs e)
        {
            bunifuPages2.PageIndex = 2;
            hideSubMenu();
        }

        private void bunifuShapes1_Click(object sender, EventArgs e)
        {
            this.Close();   //Close button
        }

        private void bunifuShapes2_Click(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
                this.WindowState = FormWindowState.Minimized;   //Minimise button
        }
        /// <summary>
        /// Following three methods shows a Snackbar with a message regarding each Cards in dashboard
        /// </summary>
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
            customiseDesign();
            showGraph();
            formatTables();
            updateTable();
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
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString()))
            {
                fillProfileDetails(tdy_date);

                //Columns for Monthly Utilization Review
                SqlDataAdapter sql5 = new SqlDataAdapter($"select [Room Number], Count(Date) as [Number of Reservations], " +
                    $"replace(replace(replace(concat(cast(sum(Cast(replace(replace(replace(replace(duration,'30 minutes','.5')," +
                    $"'hours',''),'hour',''),' ','') as float)) as varchar),' hours'),'0.5 hours','30 minutes'),'.5 hours'," +
                    $"'hours 30 minutes'),'1 hours','1 hour') as [Total Duration Used] from [dbo].[Reservation] where [Room Type]=''" +
                    $" group by [Room Number]", con);
                SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(sql5);
                DataTable dataTable = new DataTable();
                sql5.Fill(dataTable);
                monthlyUtilizationDataGridView.DataSource = dataTable;
            }
        }
        /// <summary>
        /// This method obtains values and displays Line Graph on Dashboard
        /// </summary>
        private void showGraph()
        {
            using (ReservationSystemEntities db = new ReservationSystemEntities())
            {
                var data = db.GetPastReservation();
                LineSeries line = new LineSeries()
                {
                    DataLabels = true,
                    Values = new ChartValues<int>(),
                    LabelPoint = point => point.Y.ToString()
                };
                Axis ax = new Axis
                {
                    Labels = new List<string>()
                };
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
        /// <summary>
        /// This method updates the DataGridView, fills the table with data
        /// </summary>
        private void updateTable()
        {
            requestDataGridView.Rows.Clear();
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
                            req.RequestID,
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
        /// <summary>
        /// This method format the style of the DataGridViews
        /// </summary>
        private void formatTables()
        {
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
            requestDataGridView.Columns["colReqID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            requestDataGridView.Columns["colStudID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            requestDataGridView.Columns["colNewRoom"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            requestDataGridView.Columns["colNewDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            requestDataGridView.Columns["colNewTime"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            requestDataGridView.Columns["colNewStudentNum"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            requestDataGridView.Columns["colNewDuration"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            requestDataGridView.Columns["colNewResID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void requestDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in requestDataGridView.SelectedRows)
            {
                //Get values from the table
                string roomType = row.Cells[2].Value.ToString();
                string date = row.Cells[3].Value.ToString();
                string time = row.Cells[4].Value.ToString();
                bunifuLabel35.Text = "Status of Room Type " + roomType + " on " + date + " " + time + " is";
                DateTime dt = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string datee = dt.ToString("MM/dd/yyyy");
                string status = getRoomStatus(roomType, datee);
                if (status == "Available")
                {
                    lblStatus.ForeColor = Color.Lime;
                    lblStatus.Text = "AVAILABLE";
                }
                else
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "UNAVAILABLE";
                }
            }
        }
        /// <summary>
        /// This method gets the room status (Available/Unavailable) on a selected date
        /// </summary>
        /// <param name="rt">string roomType</param>
        /// <param name="d">string date</param>
        /// <returns>The status of the selected roomType on a selected Date</returns>
        private string getRoomStatus(string rt, string d)
        {
            int reservedRoom;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("select count(*) from [dbo].Reservation where [Room Type]='" +
                    rt + "' and Date='" + d + "'", con))
                {
                    reservedRoom = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            string status = "Unavailable";
            switch (rt)
            {
                case "Amber":
                    if (reservedRoom < 5) status = "Available";
                    break;
                case "Blackthorn":
                    if (reservedRoom < 1) status = "Available";
                    break;
                case "Cedar":
                    if (reservedRoom < 6) status = "Available";
                    break;
                case "Daphne":
                    if (reservedRoom < 5) status = "Available";
                    break;
                default:
                    status = "Unavailable";
                    break;
            }
            return status;
        }

        private void btnAcceptReq_Click(object sender, EventArgs e)
        {
            int row = requestDataGridView.CurrentRow.Index;
            DateTime dt = DateTime.ParseExact(Convert.ToString(requestDataGridView[3, row].Value),
                "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string datee = dt.ToString("MM/dd/yyyy");
            Request req = new Request
            {
                RequestID = Convert.ToInt32(requestDataGridView[0, row].Value),
                StudentID = Convert.ToString(requestDataGridView[1, row].Value),
                RoomType = Convert.ToString(requestDataGridView[2, row].Value),
                Date = datee,
                Time = Convert.ToString(requestDataGridView[4, row].Value),
                NumStudents = Convert.ToInt32(requestDataGridView[5, row].Value),
                Duration = Convert.ToString(requestDataGridView[6, row].Value),
                ReservationID = Convert.ToInt32(requestDataGridView[7, row].Value),
            };
            string assignedRoom = req.assignRoom(req);
            string query = "update [dbo].Reservation set [Room Type]='" + req.RoomType + "', Date='"
                + req.Date + "', Time='" + req.Time + "', [Number of Students]='" + req.NumStudents + "'," +
                "Duration='" + req.Duration + "', [Room Number]='" + assignedRoom + "' where ReservationID='" +
                req.ReservationID + "'";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.ExecuteNonQuery(); //Updates Reservation Table
                }
                using (SqlCommand cmd2 = new SqlCommand("insert into [dbo].RequestStatus values ('APPROVED','" +
                    req.StudentID + "','" + req.Date + "')", con))
                {
                    cmd2.ExecuteNonQuery();
                }
                using (SqlCommand cmd3 = new SqlCommand("delete from [dbo].Request where RequestID='" + req.RequestID + "'", con))
                {
                    cmd3.ExecuteNonQuery();
                }   
            }
            bunifuSnackbar1.Show(this, "Request is Accepted.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 4000);
            //Update Label and Table after Request Approval
            int unattendedReq = Convert.ToInt32(lblUnattendReq.Text);
            lblUnattendReq.Text = (unattendedReq -= 1).ToString();
            updateTable();
        }

        private void btnDenyReq_Click(object sender, EventArgs e)
        {
            int row = requestDataGridView.CurrentRow.Index;
            DateTime dt = DateTime.ParseExact(Convert.ToString(requestDataGridView[3, row].Value),
                "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string datee = dt.ToString("MM/dd/yyyy");
            Request req = new Request
            {
                RequestID = Convert.ToInt32(requestDataGridView[0, row].Value),
                StudentID = Convert.ToString(requestDataGridView[1, row].Value),
                Date = datee,
            };
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("insert into [dbo].RequestStatus values ('REJECTED','" +
                req.StudentID + "','" + req.Date + "')", con))
                {
                    cmd.ExecuteNonQuery();
                }
                using (SqlCommand cmd2 = new SqlCommand("delete from[dbo].Request where RequestID='" + req.RequestID + "'", con))
                {
                    cmd2.ExecuteNonQuery();
                }   
            }
            bunifuSnackbar1.Show(this, "Request is Denied.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 4000);
            //Update Label and Table after Request Rejection
            int unattendedReq = Convert.ToInt32(lblUnattendReq.Text);
            lblUnattendReq.Text = (unattendedReq -= 1).ToString();
            updateTable();
        }

        private void btnSearchMonthly_Click(object sender, EventArgs e)
        {
            monthlyUtilizationDataGridView.DataSource = null;
            string month_name = ddmMonth.SelectedItem.ToString();
            int year1 = Int32.Parse(ddmYear.SelectedItem.ToString());
            int month1 = ddmMonth.FindStringExact(month_name) + 1;
            int month2 = month1 + 1;
            int year2 = year1;

            if (month2 == 13)
            {
                month2 = 1;
                year2 = year1 + 1;
            }
            string date1 = $"{month1}-1-{year1}";
            string date2 = $"{month2}-1-{year2}";
            string roomtype = "";

            if (cbmAmber.Checked == true)
                roomtype += "[Room Type]='Amber'";
            if (cbmBlackThorn.Checked == true)
            {
                if (roomtype != "") roomtype += " or ";
                roomtype += "[Room Type]='BlackThorn'";
            }
            if (cbmCedar.Checked == true)
            {
                if (roomtype != "") roomtype += " or ";
                roomtype += "[Room Type]='Cedar'";
            }
            if (cbmDaphne.Checked == true)
            {
                if (roomtype != "") roomtype += " or ";
                roomtype += "[Room Type]='Daphne'";
            }
            if (roomtype == "")
                roomtype += "[Room Type]=''";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString()))
            {
                con.Open();
                SqlDataAdapter sql = new SqlDataAdapter($"select [Room Number], Count(Date) as [Number of Reservations], " +
                    $"replace(replace(replace(concat(cast(sum(Cast(replace(replace(replace(replace(duration,'30 minutes','.5')" +
                    $",'hours',''),'hour',''),' ','') as float)) as varchar),' hours'),'0.5 hours','30 minutes')," +
                    $"'.5 hours','hours 30 minutes'),'1 hours','1 hour') as [Total Duration Used] from [dbo].[Reservation] " +
                    $"where date between '{date1}' and '{date2}' and {roomtype} group by [Room Number]", con);
                SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(sql);
                DataTable dataTable = new DataTable();
                sql.Fill(dataTable);
                monthlyUtilizationDataGridView.DataSource = dataTable;
            }
        }
        private void lblAmber_Click(object sender, EventArgs e)
        {
            if (cbmAmber.Checked) cbmAmber.Checked = false;
            else cbmAmber.Checked = true;
        }

        private void lblBThorn_Click(object sender, EventArgs e)
        {
            if (cbmBlackThorn.Checked) cbmBlackThorn.Checked = false;
            else cbmBlackThorn.Checked = true;
        }
        private void lblCedar_Click(object sender, EventArgs e)
        {
            if (cbmCedar.Checked) cbmCedar.Checked = false;
            else cbmCedar.Checked = true;
        }
        private void lblDaphne_Click(object sender, EventArgs e)
        {
            if (cbmDaphne.Checked) cbmDaphne.Checked = false;
            else cbmDaphne.Checked = true;
        }
        /// <summary>
        /// This method obtains user's profile details and display on respective labels
        /// </summary>
        /// <param name="tdy_date">The current date, used to display the Reservations on the day itself</param>
        private void fillProfileDetails(string tdy_date)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString()))
            {
                con.Open();
                using (SqlCommand sql = new SqlCommand($"Select count(Date) from [dbo].[Reservation] " +
                    $"where Date='{tdy_date}'", con))
                {
                    lblReservationTdy.Text = sql.ExecuteScalar().ToString();
                }
                using (SqlCommand sql2 = new SqlCommand("Select count(TPNumber) from [dbo].[User] where role='Student'", con))
                {
                    lblTotalUsers.Text = sql2.ExecuteScalar().ToString();
                }
                using (SqlCommand sql3 = new SqlCommand("Select count(RequestID) from [dbo].[Request]", con))
                {
                    lblUnattendReq.Text = sql3.ExecuteScalar().ToString();
                }
                //Username
                using (SqlCommand sql4 = new SqlCommand($"Select Name from [dbo].[User] where Email='{User.loginEmail}'", con))
                {
                    User.name = sql4.ExecuteScalar().ToString();
                }
                lblNameL1.Text = User.name;
                lblNameL2.Text = User.name;
                lblNameL3.Text = User.name;
                lblNameL4.Text = User.name;
            }
        }
    }
}
