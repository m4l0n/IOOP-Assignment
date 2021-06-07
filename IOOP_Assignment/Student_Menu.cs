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

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myCS"].ToString());


        public Student_Menu()
        {
            InitializeComponent();
        }

        private void Student_Menu_Shown(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select count(StudentID) from [dbo].[Reservation] where StudentID ='"
                + User.tpNumber + "'", con);
            int activeReservation = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            SqlCommand cmd2 = new SqlCommand("Select [Room Type], [Room Number], [Number of Students], Date, " +
                "convert(varchar(5),Time,108) as Time, " +
                "Duration from [dbo].[Reservation] where StudentID='" + User.tpNumber + "'", con);
            if (activeReservation != 0)
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd2);
                DataTable dt = new DataTable();
                da.Fill(dt);
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
                           }).ToList();

                foreach (var res in resList)
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
            }

            resDataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colResID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colRoomType"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colRoomNumber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colStudentNum"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colTime"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            resDataGridView.Columns["colDuration"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void Student_Menu_Load(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select Name from [dbo].[User] where Email='" + User.loginEmail + "'", con);
            User.name = cmd.ExecuteScalar().ToString();
            lblName.Text = User.name;
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
        }
    }
}
