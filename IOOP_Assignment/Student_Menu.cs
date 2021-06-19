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
    public partial class Student_Menu : Form
    {
        public Student_Menu()
        {
            InitializeComponent();
        }

        private void Student_Menu_Shown(object sender, EventArgs e)
        {
            Reservation res = new Reservation();
            List<Reservation> resList = res.getResData();   //returns resList as List of Objects
            if (resList != null)
            {
                foreach (var ress in resList)    //Fills the table with data
                {
                    resDataGridView.Rows.Add(
                        new object[]
                        {
                            "",
                            ress.RoomType,
                            ress.RoomNumber,
                            ress.NumStudents,
                            ress.Date,
                            ress.Time,
                            ress.Duration,
                        });
                }
                foreach (var ress in resList)    //Fills the table with data
                {
                    tableReservationEdit.Rows.Add(
                        new object[]
                        {
                            ress.ResID,
                            ress.RoomType,
                            ress.RoomNumber,
                            ress.NumStudents,
                            ress.Date,
                            ress.Time,
                            ress.Duration,
                        });
                }
            }
            formatTables();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString()))
            {
                con.Open();
                using (SqlCommand cmd4 = new SqlCommand("DELETE FROM [dbo].RequestStatus where " +
                    "StudentID=@StudID", con))
                {
                    cmd4.Parameters.AddWithValue("@StudID", User.tpNumber);
                    cmd4.ExecuteNonQuery();
                }
            }
        }

        private void Student_Menu_Load(object sender, EventArgs e)
        {
            checkNotification(User.tpNumber);
            fillProfileDetails();

            //Converts DatePicker to TimePicker
            timeReserve.Format = DateTimePickerFormat.Custom;
            timeReserve.CustomFormat = "HH:mm tt";
            timeReserve.ShowUpDown = true;
            newTimeRes.Format = DateTimePickerFormat.Custom;
            newTimeRes.CustomFormat = "HH:mm tt";
            newTimeRes.ShowUpDown = true;
        }

        private void shapeClose_Click(object sender, EventArgs e)
        {
            this.Close();   //Close application
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            bunifuPages2.SetPage(0);    //Redirect to Dashboard
        }

        private void btnRoomRes_Click(object sender, EventArgs e)
        {
            bunifuPages2.SetPage(1);    //Redirect to Room Reservation
            dateReserve.Value = DateTime.Now.AddDays(2);
            dateReserve.MinDate = DateTime.Now.AddDays(2); //Limiting Reservation date
        }
        private void btnEditRes_Click(object sender, EventArgs e)
        {
            bunifuPages2.SetPage(2);    //Redirect to Reservation Modification
        }

        private void btnPreviewCancel_Click(object sender, EventArgs e)
        {
            bunifuPages2.SetPage(1);
        }
        private void shapeMinimize_Click(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Minimized;   //Minimise Form
            }
        }
        private void resDataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //Auto Increment first column
            this.resDataGridView.Rows[e.RowIndex].Cells["colResID"].Value = (e.RowIndex + 1).ToString();
        }

        private void chkboxDate_CheckedChanged(object sender, EventArgs e)
        {
            dateNewReserved.Enabled = chkboxDate.Checked;
            dateNewReserved.Value = DateTime.Now.AddDays(2);
            dateNewReserved.MinDate = DateTime.Now.AddDays(2);
        }

        private void chkboxRoom_CheckedChanged(object sender, EventArgs e)
        {
            comboNewRoom.Enabled = chkboxRoom.Checked;
            comboNewStudent.Enabled = chkboxRoom.Checked;
            if (chkboxRoom.Checked)
            {
                bunifuSnackbar1.Show(this, "You must select both Room Type and Number of Students!",
                    Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 3000);
            }
        }

        private void chkboxTime_CheckedChanged(object sender, EventArgs e)
        {
            newTimeRes.Enabled = chkboxTime.Checked;
        }

        private void chkboxDuration_CheckedChanged(object sender, EventArgs e)
        {
            comboNewDuration.Enabled = chkboxDuration.Checked;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            comboDuration.SelectedItem = null;
            comboDuration.Text = "Select Duration";
            comboRoom.SelectedItem = null;
            comboRoom.Text = "Select Room Type";
            comboStudentNo.SelectedItem = null;
            comboStudentNo.Text = "Select Number of Students";
            dateReserve.ResetText();
            timeReserve.ResetText();
        }

        private void comboRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboRoom.SelectedIndex)
            {
                case 0:
                    comboStudentNo.Items.Clear();
                    for (int i = 1; i <= 10; i++)
                    {
                        comboStudentNo.Items.Add(i);
                    }
                    break;
                case 1:
                    comboStudentNo.Items.Clear();
                    for (int i = 1; i <= 8; i++)
                    {
                        comboStudentNo.Items.Add(i);
                    }
                    break;
                case 2:
                    comboStudentNo.Items.Clear();
                    for (int i = 1; i <= 4; i++)
                    {
                        comboStudentNo.Items.Add(i);
                    }
                    break;
                case 3:
                    comboStudentNo.Items.Clear();
                    for (int i = 1; i <= 2; i++)
                    {
                        comboStudentNo.Items.Add(i);
                    }
                    break;
            }
        }
        private void formatTables()
        {
            //Bunifu DataGridView Style Formatting
            resDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resDataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colResID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colRoomType"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colRoomNumber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colStudentNum"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colTime"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colDuration"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            tableReservationEdit.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableReservationEdit.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableReservationEdit.Columns["colResIDEdit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableReservationEdit.Columns["colRoomTypeEdit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableReservationEdit.Columns["colRoomNumEdit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableReservationEdit.Columns["colNumStudentsEdit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableReservationEdit.Columns["colDateEdit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableReservationEdit.Columns["colTimeEdit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableReservationEdit.Columns["colDurationEdit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            tableAvailableRoom.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableAvailableRoom.Columns["columnAvailableRoomType"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableAvailableRoom.Columns["columnAvailableRoom"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void checkNotification(string studID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString()))
            {
                con.Open();
                int notificationCount;
                using (SqlCommand cmd = new SqlCommand("select count(*) from [dbo].RequestStatus where StudentID='"
                    + studID + "'", con))
                {
                    notificationCount = Convert.ToInt32(cmd.ExecuteScalar());
                }
                if (notificationCount != 0)
                {
                    SqlCommand cmd2 = new SqlCommand("select [Request Status], format(Date, 'dd/MM/yyyy') as Date from " +
                        "[dbo].RequestStatus where StudentID='" + studID + "'", con);
                    using (SqlDataReader reader = cmd2.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string date = Convert.ToString(reader["Date"]);
                            string message = "The Reservation Change Request on " + date + " was " +
                                Convert.ToString(reader["Request Status"]);
                            bunifuSnackbar1.Show(this, message, Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 10000,
                                "Notification", Bunifu.UI.WinForms.BunifuSnackbar.Positions.MiddleCenter);
                        }
                    }
                }
            }
        }

        private void btnEditReserve_Click(object sender, EventArgs e)
        {
            int row = tableReservationEdit.CurrentRow.Index;
            DateTime dt = DateTime.ParseExact(Convert.ToString(tableReservationEdit[4, row].Value),
                "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string datee = dt.ToString("MM/dd/yyyy");

            Reservation req = new Reservation
            {
                ResID = Convert.ToInt32(tableReservationEdit[0, row].Value),
                RoomType = Convert.ToString(tableReservationEdit[1, row].Value),
                RoomNumber = Convert.ToString(tableReservationEdit[2, row].Value),
                NumStudents = Convert.ToInt32(tableReservationEdit[3, row].Value),
                Time = Convert.ToString(tableReservationEdit[5, row].Value),
                Duration = Convert.ToString(tableReservationEdit[6, row].Value),

            };

            lblReservedDate1.Text = datee;
            lblReservedDuration1.Text = req.Duration;
            lblReservedTime1.Text = req.Time;
            lblReservedRoom1.Text = req.RoomType;
            lblReservedStudent1.Text = req.NumStudents.ToString();


            bunifuPages2.SetPage(4);    //Redirect to Edit Form Page
        }

        private void cbkboxSecurityCancel_CheckedChanged(object sender, EventArgs e)
        {
            btnCancelReserve.Enabled = cbkboxSecurityCancel.Checked;
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (comboDuration.SelectedIndex != -1
                && comboRoom.SelectedIndex != -1
                && comboStudentNo.SelectedIndex != -1)
            {
                string roomtype = comboRoom.GetItemText(comboRoom.SelectedItem);
                lblPreviewDate1.Text = dateReserve.Value.ToShortDateString();
                lblPreviewTime1.Text = timeReserve.Value.ToShortTimeString();
                lblPreviewDuration1.Text = comboDuration.GetItemText(comboDuration.SelectedItem);
                lblPreviewRoomType1.Text = roomtype;
                lblPreviewStudent1.Text = comboStudentNo.GetItemText(comboStudentNo.SelectedItem);
                Reservation res = new Reservation
                {
                    RoomType = roomtype,
                    Date = dateReserve.Value.ToString("MM/dd/yyyy")
                };

                Reservation.assignedRoom = res.assignRoom(res);
                lblPreviewRoomNumber1.Text = Reservation.assignedRoom;
                lblRoomType.Text = roomtype;

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString()))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("select Capacity from [dbo].[Room] where [Room Type]=@roomtype", con))
                    {
                        cmd.Parameters.AddWithValue("@roomtype", roomtype);
                        lblCapacity.Text = cmd.ExecuteScalar().ToString();
                    }

                }
                bunifuPages2.SetPage(3);    //Redirect to Reservation Preview

            }
            else
            {
                bunifuSnackbar1.Show(this, "All fields are required to fill, Please make sure all fields are filled.",
                    Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000);
            }
        }

        private void btnPreviewConfirm_Click(object sender, EventArgs e)
        {
            lblPreviewRoomNumber1.Text = Reservation.assignedRoom;
            
            Reservation res = new Reservation
            {
                Date = dateReserve.Value.ToShortDateString(),
                Time = timeReserve.Value.ToShortTimeString(),
                Duration = comboDuration.GetItemText(comboDuration.SelectedItem),
                RoomType = comboRoom.GetItemText(comboRoom.SelectedItem),
                NumStudents = Convert.ToInt32(comboStudentNo.GetItemText(comboStudentNo.SelectedItem)),
                StudentID = User.tpNumber,
            };

            int result = res.addReservation(res);
            if (result != 0)
            {
                bunifuSnackbar1.Show(this, "You have successfully booked a slot for the Discussion Room! To modify the details, " +
                "Click on 'Edit Reservation' on the side panel.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 3000);
            }
            else
            {
                bunifuSnackbar1.Show(this, "Something went wrong while trying to make a reservation. Please contact the system" +
                    "administrator for support.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000);
            }
        }

        private string[] getRoomStatus(string[] roomType, string d)
        {
            int reservedRoom;
            int counter = 0;
            int[] reservedNum = new int[4];
            string[] roomStatus = { "Unavailable", "Unavailable" , "Unavailable" , "Unavailable" };
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString()))
            {
                con.Open();
                foreach (string i in roomType)
                {
                    using (SqlCommand cmd = new SqlCommand("select count(*) from [dbo].Reservation where [Room Type]='" +
                    i + "' and Date='" + d + "'", con))
                    {
                        reservedRoom = Convert.ToInt32(cmd.ExecuteScalar());
                        reservedNum[counter] = reservedRoom;
                    }
                    counter += 1;
                }
            }
            if (reservedNum[0] < 10) roomStatus[0] = "Available";
            if (reservedNum[1] < 8) roomStatus[1] = "Available";
            if (reservedNum[2] < 4) roomStatus[2] = "Available";
            if (reservedNum[3] < 2) roomStatus[3] = "Available";

            return roomStatus;
        }
        private void dateReserve_ValueChanged(object sender, EventArgs e)
        {
            tableAvailableRoom.Rows.Clear();
            comboRoom.Items.Clear();
            string[] roomType = { "Amber", "Blackthorn", "Cedar", "Daphne" };
            string date = dateReserve.Value.ToString("MM/dd/yyyy");
            string[] roomStatus = getRoomStatus(roomType, date);

            for (int i = 0; i <= 3; i++)
            {
                tableAvailableRoom.Rows.Add(
                    new object[]
                    {
                        roomType[i],
                        roomStatus[i],
                    });
                if (roomStatus[i] == "Available") comboRoom.Items.Add(roomType[i]);
            }
        }

        private void comboNewRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboNewRoom.SelectedIndex)
            {
                case 0:
                    comboNewStudent.Items.Clear();
                    for (int i = 1; i <= 10; i++)
                    {
                        comboNewStudent.Items.Add(i);
                    }
                    break;
                case 1:
                    comboNewStudent.Items.Clear();
                    for (int i = 1; i <= 8; i++)
                    {
                        comboNewStudent.Items.Add(i);
                    }
                    break;
                case 2:
                    comboNewStudent.Items.Clear();
                    for (int i = 1; i <= 4; i++)
                    {
                        comboNewStudent.Items.Add(i);
                    }
                    break;
                case 3:
                    comboNewStudent.Items.Clear();
                    for (int i = 1; i <= 2; i++)
                    {
                        comboNewStudent.Items.Add(i);
                    }
                    break;
            }
        }

        private void btnEditConfirm_Click(object sender, EventArgs e)
        {
            if (chkboxDate.Checked == false
                && chkboxRoom.Checked == false
                && chkboxTime.Checked == false
                && chkboxDuration.Checked == false)
            {
                bunifuSnackbar1.Show(this, "No changes made, redirecting to Edit Page.",
                    Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000);

                bunifuPages2.SetPage(2);
            }
            else
            {
                int roomvalidate = 0;   //Use as count to validate Room and Student combobox
                int durationvalidate = 0; //Use as count to validate Duration combobox

                if (chkboxRoom.Checked)
                {
                    if (comboNewRoom.SelectedIndex != -1
                        && comboNewStudent.SelectedIndex != -1)
                    {
                        roomvalidate = 1;
                    }
                }
                else
                {
                    roomvalidate = 1;
                }

                if (chkboxDuration.Checked)
                {
                    if (comboNewRoom.SelectedIndex != -1)
                    {
                        durationvalidate = 1;
                    }
                }
                else
                {
                    durationvalidate = 1;
                }

                if (roomvalidate == 1
                    && durationvalidate == 1)
                {
                    int row = tableReservationEdit.CurrentRow.Index;
                    DateTime dt = DateTime.ParseExact(Convert.ToString(tableReservationEdit[4, row].Value),
                        "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string date = dt.ToString("MM/dd/yyyy");

                    Request req = new Request
                    {
                        ReservationID = Convert.ToInt32(tableReservationEdit[0, row].Value),
                        RoomType = Convert.ToString(tableReservationEdit[1, row].Value),
                        NumStudents = Convert.ToInt32(tableReservationEdit[3, row].Value),
                        Date = date,
                        Time = Convert.ToString(tableReservationEdit[5, row].Value),
                        Duration = Convert.ToString(tableReservationEdit[6, row].Value),
                    };

                    if (chkboxDate.Checked) req.Date = dateNewReserved.Value.ToShortDateString();
                    if (chkboxRoom.Checked)
                    {
                        req.RoomType = comboNewRoom.GetItemText(comboNewRoom.SelectedItem);
                        req.NumStudents = Convert.ToInt32(comboNewStudent.GetItemText(comboNewStudent.SelectedItem));
                    }
                    if (chkboxTime.Checked) req.Time = newTimeRes.Value.ToShortTimeString();
                    if (chkboxDuration.Checked) req.Duration = comboNewDuration.GetItemText(comboNewDuration.SelectedItem);

                    int result = req.addRequest(req);
                    if (result != 0)
                    {
                        bunifuSnackbar1.Show(this, "You have successfully requested for a change on your reservation. Librarians will " +
                            "check on it soon.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 3000);
                    }
                    else
                    {
                        bunifuSnackbar1.Show(this, "Something went wrong while trying to make a request. Please contact the system" +
                            "administrator for support.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000);
                    }
                }
                else
                {
                    bunifuSnackbar1.Show(this, "Please make sure ticked field is filled.",
                        Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000);
                }
            }
        }

        private void btnEditCancel_Click(object sender, EventArgs e)
        {
            bunifuPages2.SetPage(0);
        }

        private void btnCancelReserve_Click(object sender, EventArgs e)
        {
            if (cbkboxSecurityCancel.Checked)
            {
                int row = tableReservationEdit.CurrentRow.Index;
                Request delreq = new Request
                {
                    ReservationID = Convert.ToInt32(tableReservationEdit[0, row].Value),
                };

                int result = delreq.deleteReservation(delreq);
                if (result != 0)
                {
                    bunifuSnackbar1.Show(this, "You have successfully cancelled your reservation.",
                        Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 3000);
                    tableReservationEdit.Rows.RemoveAt(row);
                    resDataGridView.Rows.RemoveAt(row);
                }
                else
                {
                    bunifuSnackbar1.Show(this, "Something went wrong while attempting to make a request. Please contact the " +
                        "system administrator for support.", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000);
                }
            }
        }

        private void fillProfileDetails()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("select Name from [dbo].[User] where Email='" +
                    User.loginEmail + "'", con))
                {
                    User.name = cmd.ExecuteScalar().ToString();
                }
                lblName.Text = User.name;
                lblName2.Text = User.name;
                lblName3.Text = User.name;
                using (SqlCommand cmd2 = new SqlCommand("select TPNumber from [dbo].[User]  where Email='"
                    + User.loginEmail + "'", con))
                {
                    User.tpNumber = cmd2.ExecuteScalar().ToString();
                }
                int activeReservation;
                using (SqlCommand cmd3 = new SqlCommand("select count(StudentID) from [dbo].[Reservation] where " +
                    "StudentID ='" + User.tpNumber + "' and Date >= getDate()", con))
                {
                    activeReservation = Convert.ToInt32(cmd3.ExecuteScalar().ToString());
                }
                lblHi.Text = "Hi " + User.name + ",";
                lblActiveRes.Text = activeReservation.ToString();
            }
        }
    }
}
