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
    public partial class Student_Menu : Form
    {
        //Declare connection string
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString());


        public Student_Menu()
        {
            InitializeComponent();
            timeReserve.Format = DateTimePickerFormat.Custom;
            timeReserve.CustomFormat = "HH:mm tt";
            timeReserve.ShowUpDown = true;
            newTimeRes.Format = DateTimePickerFormat.Custom;
            newTimeRes.CustomFormat = "HH:mm tt";
            newTimeRes.ShowUpDown = true;
        }

        private void Student_Menu_Shown(object sender, EventArgs e)
        {
            //Reservation List Table (Dashboard)
            con.Open();
            SqlCommand cmd = new SqlCommand("select count(StudentID) from [dbo].[Reservation] where StudentID ='"
                + User.tpNumber + "'", con);
            int activeReservation = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            SqlCommand cmd2 = new SqlCommand("Select [Room Type], [Room Number], [Number of Students], " +
                "format(Date, 'dd/MM/yyyy') as Date, " +
                "CONVERT(varchar(10), CAST(Time as Time),0) as Time, " +
                "Duration from [dbo].[Reservation] where StudentID='" + User.tpNumber + "'", con);
            if (activeReservation != 0)
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd2);
                DataTable dt = new DataTable(); //Creates a DataTable in the memory
                da.Fill(dt); //Fills the DataTable with the result from SQL Query
                con.Close();
                List<Reservation> resList = new List<Reservation>();
                resList = (from DataRow dr in dt.Rows
                           select new Reservation()
                           {
                               roomType = dr["Room Type"].ToString(),
                               roomNumber = dr["Room Number"].ToString(),
                               date = dr["Date"].ToString(),
                               time = dr["Time"].ToString(),
                               numStudents = Convert.ToInt32(dr["Number of Students"]),
                               duration = dr["Duration"].ToString(),
                           }).ToList(); //LINQ returns a list of object

                foreach (var res in resList) //Fills the table with data
                {
                    resDataGridView.Rows.Add(
                        new object[]
                        {
                            "",
                            res.roomType,
                            res.roomNumber,
                            res.numStudents,
                            res.date,
                            res.time,
                            res.duration,
                        });
                }
                foreach (var res in resList) //Fills the table with data
                {
                    tableReservationEdit.Rows.Add(
                        new object[]
                        {
                            "",
                            res.roomType,
                            res.roomNumber,
                            res.numStudents,
                            res.date,
                            res.time,
                            res.duration,
                        });
                }
            }
            resDataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colResID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colRoomType"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colRoomNumber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colStudentNum"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colTime"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colDuration"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            tableReservationEdit.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableReservationEdit.Columns["colResIDEdit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableReservationEdit.Columns["colRoomTypeEdit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableReservationEdit.Columns["colRoomNumEdit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableReservationEdit.Columns["colNumStudentsEdit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableReservationEdit.Columns["colDateEdit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableReservationEdit.Columns["colTimeEdit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableReservationEdit.Columns["colDurationEdit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }

        private void Student_Menu_Load(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select Name from [dbo].[User] where Email='" + User.loginEmail + "'", con);
            User.name = cmd.ExecuteScalar().ToString();
            lblName.Text = User.name;
            lblName2.Text = User.name;
            lblName3.Text = User.name;
            SqlCommand cmd2 = new SqlCommand("select TPNumber from [dbo].[User]  where Email='" 
                + User.loginEmail + "'", con);
            User.tpNumber = cmd2.ExecuteScalar().ToString();
            SqlCommand cmd3 = new SqlCommand("select count(StudentID) from [dbo].[Reservation] where StudentID ='"
                + User.tpNumber + "'", con);
            int activeReservation = Convert.ToInt32(cmd3.ExecuteScalar().ToString());
            lblHi.Text = "Hi " + User.name + ",";
            lblActiveRes.Text = activeReservation.ToString();
            con.Close();
        }

        private void shapeClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            bunifuPages2.SetPage(0);
        }

        private void btnRoomRes_Click(object sender, EventArgs e)
        {
            bunifuPages2.SetPage(1);
        }
        private void btnEditRes_Click(object sender, EventArgs e)
        {
            bunifuPages2.SetPage(2);
        }
        private void shapeMinimize_Click(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }
        private void resDataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            this.resDataGridView.Rows[e.RowIndex].Cells["colResID"].Value = (e.RowIndex + 1).ToString();
            this.tableReservationEdit.Rows[e.RowIndex].Cells["colResIDEdit"].Value = (e.RowIndex + 1).ToString();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            bunifuPages2.PageIndex = 3;
        }

        private void chkboxDate_CheckedChanged(object sender, EventArgs e)
        {
            dateNewReserved.Enabled = chkboxDate.Checked;
        }

        private void chkboxRoom_CheckedChanged(object sender, EventArgs e)
        {
            comboNewRoom.Enabled = chkboxRoom.Checked;
        }

        private void chkboxStudent_CheckedChanged(object sender, EventArgs e)
        {
            txtNewStudent.Enabled = chkboxStudent.Checked;
        }

        private void chkboxTime_CheckedChanged(object sender, EventArgs e)
        {
            newTimeRes.Enabled = chkboxTime.Checked;
        }

        private void chkboxDuration_CheckedChanged(object sender, EventArgs e)
        {
            comboNewDuration.Enabled = chkboxDuration.Checked;
        }
    }
}
